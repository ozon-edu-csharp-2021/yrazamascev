using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate
{
    public class SkuPack : Entity
    {
        public Sku Sku { get; }

        public Quantity Quantity { get; private set; }

        public SkuPack(Sku sku, Quantity quantity)
        {
            Sku = sku;
            Quantity = quantity;
        }
    }
}