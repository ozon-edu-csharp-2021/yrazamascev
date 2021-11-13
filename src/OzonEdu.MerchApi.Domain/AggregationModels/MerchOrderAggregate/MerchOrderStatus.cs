using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate
{
    public class MerchOrderStatus : Enumeration
    {
        public static MerchOrderStatus InWork = new(1, nameof(InWork));
        public static MerchOrderStatus IsReserved = new(2, nameof(IsReserved));
        public static MerchOrderStatus IsDone = new(3, nameof(IsDone));

        public MerchOrderStatus(int id, string name) : base(id, name)
        {
        }
    }
}