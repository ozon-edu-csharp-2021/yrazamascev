using System.Collections.Generic;

namespace OzonEdu.MerchApi.HttpModels
{
    public sealed class GetMerchOrdersResponse
    {
        public IEnumerable<MerchOrderViewModel> MerchOrders { get; set; }
    }
}