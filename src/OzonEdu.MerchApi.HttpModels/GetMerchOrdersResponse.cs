using OzonEdu.MerchApi.HttpModels.ViewModels;

using System.Collections.Generic;

namespace OzonEdu.MerchApi.HttpModels
{
    public sealed class GetMerchOrdersResponse
    {
        public IReadOnlyCollection<MerchOrderViewModel> MerchOrders { get; set; }
    }
}