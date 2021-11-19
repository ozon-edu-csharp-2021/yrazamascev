using OzonEdu.MerchApi.Domain.Contracts;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate
{
    public interface IMerchOrderRepository : IRepository<MerchOrder>
    {
        Task<MerchOrder> Create(MerchOrder merchOrder, CancellationToken cancellationToken);

        Task<List<MerchOrder>> FindByEmployeeId(long employeeId, CancellationToken cancellationToken);

        Task<List<MerchOrder>> FindIssuedMerch(long employeeId, int merchPackId, CancellationToken cancellationToken = default);
    }
}