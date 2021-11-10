using OzonEdu.MerchApi.Domain.Contracts;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate
{
    public interface IMerchOrderRepository : IRepository<MerchOrder>
    {
        Task<MerchOrder> FindIssuedMerchAsync(long employeeId, int merchPackId, CancellationToken cancellationToken = default);
        Task<List<MerchOrder>> FindByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken);
    }
}