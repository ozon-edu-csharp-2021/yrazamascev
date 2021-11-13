using OzonEdu.MerchApi.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchApi.Domain.Contracts;

using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public interface IMerchPackRepository : IRepository<MerchPack>
    {
        Task<MerchPack> FindByType(MerchPackType merchPackType, CancellationToken cancellationToken);
    }
}