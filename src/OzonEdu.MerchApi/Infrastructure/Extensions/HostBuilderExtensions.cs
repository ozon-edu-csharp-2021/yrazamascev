﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using OzonEdu.MerchApi.Infrastructure.Filters;
using OzonEdu.MerchApi.Infrastructure.StartupFilters;

namespace OzonEdu.MerchApi.Infrastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartupFilter, TerminalStartupFilter>();
                services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();

                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "OzonEdu.MerchApi", Version = "v1" });
                });

                services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
            });

            return builder;
        }
    }
}