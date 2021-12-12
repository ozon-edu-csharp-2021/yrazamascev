using Dapper;

using Grpc.Net.Client;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OzonEdu.MerchApi.Domain.AggregationModels.ItemPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Infrastructure.Configuration;
using OzonEdu.MerchApi.Domain.Infrastructure.Handlers.DomainEvent;
using OzonEdu.MerchApi.Domain.Infrastructure.MessageBroker;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Implementation;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.StockApi.Grpc;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConfiguration>(configuration);

            return services;
        }

        public static IServiceCollection AddDatabaseComponents(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConnectionOptions>(configuration.GetSection(nameof(DatabaseConnectionOptions)));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IChangeTracker, ChangeTracker>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddScoped<IItemPackRepository, ItemPackRepository>();
            services.AddScoped<IMerchOrderRepository, MerchOrderRepository>();
            services.AddScoped<IMerchPackRepository, MerchPackRepository>();
            services.AddScoped<ISkuPackRepository, SkuPackRepository>();

            return services;
        }

        public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStockGrpcServiceClient(configuration);

            return services;
        }

        public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConfiguration>(configuration);
            services.AddSingleton<IProducerBuilderWrapper, ProducerBuilderWrapper>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddMediatR(
                typeof(CreateMerchOrderCommandHandler).Assembly,
                typeof(GetMerchOrdersCommandHandler).Assembly);

            return services;
        }

        private static void AddStockGrpcServiceClient(this IServiceCollection services, IConfiguration configuration)
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
    }
}