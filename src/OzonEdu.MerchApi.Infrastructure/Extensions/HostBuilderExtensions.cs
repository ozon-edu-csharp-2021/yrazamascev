using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using OzonEdu.MerchApi.Infrastructure.Filters;
using OzonEdu.MerchApi.Infrastructure.Interceptors;
using OzonEdu.MerchApi.Infrastructure.StartupFilters;

using System;
using System.IO;
using System.Reflection;

namespace OzonEdu.MerchApi.Infrastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
        {
            return builder
                .ConfigureStartupFilter()
                .ConfigureHttp()
                .ConfigureGrpc()
                .ConfigureSwagger();
        }

        private static IHostBuilder ConfigureStartupFilter(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartupFilter, TerminalStartupFilter>();
            });

            return builder;
        }

        private static IHostBuilder ConfigureHttp(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
            });

            return builder;
        }

        private static IHostBuilder ConfigureGrpc(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddGrpc(options => options.Interceptors.Add<LoggingInterceptor>());
            });

            return builder;
        }

        private static IHostBuilder ConfigureSwagger(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "OzonEdu.MerchApi", Version = "v1" });
                    options.CustomSchemaIds(x => x.FullName);
                    string xmlFileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                    string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                    options.IncludeXmlComments(xmlFilePath);
                });
            });

            return builder;
        }
    }
}