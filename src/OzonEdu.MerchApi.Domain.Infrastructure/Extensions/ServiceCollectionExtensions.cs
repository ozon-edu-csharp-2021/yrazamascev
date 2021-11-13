using MediatR;

using Microsoft.Extensions.DependencyInjection;

using OzonEdu.MerchApi.Domain.Infrastructure.Handlers.MerchOrderAggregate;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddMediatR(
                typeof(CreateManualMerchOrderCommandHandler).Assembly,
                typeof(GetMerchOrdersCommandHandler).Assembly);

            return services;
        }

        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}