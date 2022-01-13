using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;

using OzonEdu.MerchApi.Domain.Infrastructure.Extensions;
using OzonEdu.MerchApi.Extensions;
using OzonEdu.MerchApi.GrpcServices;
using OzonEdu.MerchApi.Infrastructure.Interceptors;
using OzonEdu.MerchApi.Services;
using OzonEdu.MerchApi.Services.Interfaces;

namespace OzonEdu.MerchApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MerchApiGrpsService>();
                endpoints.MapControllers();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomOptions(Configuration)
                .AddMediator()
                .AddHostedServices()
                .AddDatabaseComponents(Configuration)
                .AddRepositories()
                .AddExternalServices(Configuration)
                .AddKafkaServices(Configuration)
                .AddInfrastructureServices();

            services.AddSingleton<IMerchService, MerchService>();
            services.AddGrpc(options => options.Interceptors.Add<LoggingInterceptor>());

            services.AddSingleton<ITracer>(sp =>
            {
                string serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
                ILoggerFactory loggerFactory = sp.GetRequiredService<ILoggerFactory>();

                RemoteReporter reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(loggerFactory)
                    .WithSender(new UdpSender())
                    .Build();

                Tracer tracer = new Tracer.Builder(serviceName)
                    .WithSampler(new ConstSampler(true))
                    .WithReporter(reporter)
                    .Build();

                return tracer;
            });

            services.Configure<HttpHandlerDiagnosticOptions>(options =>
                options.OperationNameResolver = request =>
                    $"{request.Method.Method}: {request?.RequestUri?.AbsoluteUri}");
        }
    }
}