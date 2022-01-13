using MediatR;

using Microsoft.Extensions.DependencyInjection;

using OzonEdu.MerchApi.Domain.Infrastructure.Configuration;
using OzonEdu.StockApi.HostedServices;

namespace OzonEdu.MerchApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup), typeof(DatabaseConnectionOptions));

            return services;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<StockConsumerHostedService>();

            return services;
        }
    }
}