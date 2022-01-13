using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Services.Interfaces
{
    public interface IMerchService
    {
        Task<bool> GetMerchOrders(string employeeEmail, CancellationToken token);

        Task<bool> IssueMerch(string employeeEmail, CancellationToken token);
    }
}