using OzonEdu.MerchApi.Domain.Contracts;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate
{
    public interface IMerchOrderRepository : IRepository<MerchOrder>
    {
        Task<MerchOrder> Create(MerchOrder merchOrder, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<MerchOrder>> FindByEmployeeId(long employeeId, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<MerchOrder>> FindIssuedMerch(long employeeId, int merchPackId, CancellationToken cancellationToken = default);
    }
}