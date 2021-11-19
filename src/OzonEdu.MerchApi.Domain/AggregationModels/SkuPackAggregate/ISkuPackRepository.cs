using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.Contracts;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate
{
    public interface ISkuPackRepository : IRepository<SkuPack>
    {
        Task<List<SkuPack>> Create(MerchOrder merchOrder, CancellationToken cancellationToken);
    }
}