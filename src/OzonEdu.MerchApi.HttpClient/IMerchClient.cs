using OzonEdu.MerchApi.DTO;

using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Client
{
    public interface IMerchClient
    {
        Task<CheckWasIssuedMerchResponse> CheckWasIssuedMerch(CheckWasIssuedMerchRequest request, CancellationToken token);
        Task<IssueMerchResponse> IssueMerch(IssueMerchRequest request, CancellationToken token);
    }
}