using CSharpCourse.Core.Lib.Enums;

using OzonEdu.MerchApi.Domain.Contracts;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate
{
    public interface IMerchOrderRepository : IRepository<MerchOrder>
    {
        Task<MerchOrder> Create(MerchOrder merchOrder, CancellationToken cancellationToken);

        Task<MerchOrder> Update(MerchOrder itemToUpdate, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<MerchOrder>> FindByEmployeeId(string employeeEmail, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<MerchOrder>> FindIssuedMerch(string employeeEmail, MerchType merchType, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<MerchOrder>> FindInWork(IReadOnlyCollection<long> skus, CancellationToken cancellationToken);
    }
}