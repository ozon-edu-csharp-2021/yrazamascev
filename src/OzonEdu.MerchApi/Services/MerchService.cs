using OzonEdu.MerchApi.Services.Interfaces;

using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Services
{
    public class MerchService : IMerchService
    {
        public async Task<bool> GetMerchOrders(string employeeEmail, CancellationToken token)
        {
            return true;
        }

        public async Task<bool> IssueMerch(string employeeEmail, CancellationToken token)
        {
            return true;
        }
    }
}