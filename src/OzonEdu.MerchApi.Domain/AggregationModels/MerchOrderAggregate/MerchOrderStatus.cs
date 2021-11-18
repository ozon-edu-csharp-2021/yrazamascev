using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate
{
    public class MerchOrderStatus : Enumeration
    {
        public static MerchOrderStatus InWork = new(10, nameof(InWork));
        public static MerchOrderStatus IsReserved = new(20, nameof(IsReserved));
        public static MerchOrderStatus IsDone = new(30, nameof(IsDone));

        public MerchOrderStatus(int id, string name) : base(id, name)
        {
        }
    }
}