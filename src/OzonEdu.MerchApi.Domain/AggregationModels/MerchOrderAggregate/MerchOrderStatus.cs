using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate
{
    public class MerchOrderStatus : Enumeration
    {
        public static MerchOrderStatus InWork = new(10, nameof(InWork));
        public static MerchOrderStatus IsDone = new(20, nameof(IsDone));

        public MerchOrderStatus(int id, string name) : base(id, name)
        {
        }
    }
}