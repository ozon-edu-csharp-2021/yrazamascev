using Dapper;

using Grpc.Net.Client;

using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;

using OzonEdu.MerchApi.Domain.AggregationModels.ItemPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Infrastructure.Configuration;
using OzonEdu.MerchApi.Domain.Infrastructure.Extensions;
using OzonEdu.MerchApi.Domain.Infrastructure.MessageBroker;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Implementation;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchApi.Domain.Infrastructure.Services;
using OzonEdu.MerchApi.Extensions;
using OzonEdu.MerchApi.GrpcServices;
using OzonEdu.MerchApi.Infrastructure.Interceptors;
using OzonEdu.MerchApi.Services;
using OzonEdu.MerchApi.Services.Interfaces;
using OzonEdu.StockApi.Grpc;

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
                .AddHostedServices();

            AddMediator(services);
            AddDatabaseComponents(services);
            AddRepositories(services);
            AddStockGrpcServiceClient(services, Configuration);

            services.AddSingleton<IMerchService, MerchService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IStockApiService, StockApiService>();
            services.AddInfrastructureServices();
            services.AddGrpc(options => options.Interceptors.Add<LoggingInterceptor>());

            services.AddSingleton<ITracer>(sp =>
            {
                string serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
                ILoggerFactory loggerFactory = sp.GetRequiredService<ILoggerFactory>();

                RemoteReporter reporter = new RemoteReporter
                    .Builder()
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
                options.OperationNameResolver =
                    request => $"{request.Method.Method}: {request?.RequestUri?.AbsoluteUri}");
        }

        private void AddMediator(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup), typeof(DatabaseConnectionOptions));
        }

        private void AddDatabaseComponents(IServiceCollection services)
        {
            services.Configure<DatabaseConnectionOptions>(Configuration.GetSection(nameof(DatabaseConnectionOptions)));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IChangeTracker, ChangeTracker>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();
        }

        private void AddRepositories(IServiceCollection services)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddScoped<IItemPackRepository, ItemPackRepository>();
            services.AddScoped<IMerchOrderRepository, MerchOrderRepository>();
            services.AddScoped<IMerchPackRepository, MerchPackRepository>();
            services.AddScoped<ISkuPackRepository, SkuPackRepository>();
        }

        private void AddStockGrpcServiceClient(IServiceCollection services, IConfiguration configuration)
        {
            string connectionAddress = configuration.GetSection(nameof(StockApiGrpcServiceConfiguration))
                .Get<StockApiGrpcServiceConfiguration>().ServerAddress;

            if (string.IsNullOrWhiteSpace(connectionAddress))
            {
                connectionAddress = configuration
                    .Get<StockApiGrpcServiceConfiguration>()
                    .ServerAddress;
            }

            services.AddScoped(opt =>
            {
                GrpcChannel channel = GrpcChannel.ForAddress(connectionAddress);

                return new StockApiGrpc.StockApiGrpcClient(channel);
            });
        }

        private void AddKafkaServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConfiguration>(configuration);
            services.AddSingleton<IProducerBuilderWrapper, ProducerBuilderWrapper>();
        }
    }
}