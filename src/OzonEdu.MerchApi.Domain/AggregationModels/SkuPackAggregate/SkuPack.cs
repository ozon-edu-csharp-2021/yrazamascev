using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate
{
    public class SkuPack
    {
        public Quantity Quantity { get; private set; }
        public Sku Sku { get; }

        public SkuPack(Sku sku, Quantity quantity)
        {
            Sku = sku;
            Quantity = quantity;
        }
    }
}