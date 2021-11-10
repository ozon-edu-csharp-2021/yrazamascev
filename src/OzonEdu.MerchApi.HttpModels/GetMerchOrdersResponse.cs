using System.Collections.Generic;

namespace OzonEdu.MerchApi.HttpModels
{
    public sealed class GetMerchOrdersResponse
    {
        public List<MerchOrderViewModel> MerchOrders { get; set; }
    }
}