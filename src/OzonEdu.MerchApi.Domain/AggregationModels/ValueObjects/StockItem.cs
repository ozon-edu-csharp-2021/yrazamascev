using OzonEdu.MerchApi.Domain.Models;

using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects
{
    public class StockItem : ValueObject
    {
        public long Value { get; }

        public StockItem(long sku) => Value = sku;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}