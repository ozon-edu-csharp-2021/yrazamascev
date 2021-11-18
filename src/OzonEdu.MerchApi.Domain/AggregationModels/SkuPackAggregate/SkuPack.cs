using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate
{
    public class SkuPack : Entity
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