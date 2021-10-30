using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Services.Interfaces
{
    public interface IMerchService
    {
        Task<bool> CheckWasIssuedMerch(long employeeId, CancellationToken token);

        Task<bool> IssueMerch(long employeeId, CancellationToken token);
    }
}