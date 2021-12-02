using OzonEdu.MerchApi.Domain.AggregationModels.ItemPackAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Implementation
{
    public class ItemPackRepository : IItemPackRepository
    {
        private readonly IQueryExecutor _queryExecutor;

        public ItemPackRepository(IQueryExecutor queryExecutor) => _queryExecutor = queryExecutor;
    }
}