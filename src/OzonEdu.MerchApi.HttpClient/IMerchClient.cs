using OzonEdu.MerchApi.HttpModels;

using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Client
{
    public interface IMerchClient
    {
        Task<GetMerchOrdersResponse> GetMerchOrders(GetMerchOrdersRequest request, CancellationToken token);

        Task<IssueMerchResponse> IssueMerch(IssueMerchRequest request, CancellationToken token);
    }
}