using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.HttpModels;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Services
{
    public interface IStockApiService
    {
        Task<List<StockItemResponse>> GetAll(CancellationToken cancellationToken);

        Task<bool> Reserve(List<SkuPack> skuPacks, CancellationToken cancellationToken);
    }

    public class StockApiService : IStockApiService
    {
        public Task<List<StockItemResponse>> GetAll(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Reserve(List<SkuPack> skuPacks, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}