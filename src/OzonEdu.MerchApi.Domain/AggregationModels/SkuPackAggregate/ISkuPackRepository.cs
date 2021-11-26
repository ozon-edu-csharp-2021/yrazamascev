using OzonEdu.MerchApi.Domain.Contracts;

using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate
{
    public interface ISkuPackRepository : IRepository<SkuPack>
    {
        Task<SkuPack> Create(SkuPack skuPack, long merchOrderId, CancellationToken cancellationToken);
    }
}