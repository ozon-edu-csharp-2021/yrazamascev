using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges(CancellationToken cancellationToken = default);

        Task<bool> SaveEntities(CancellationToken cancellationToken = default);
    }
}