using OzonEdu.MerchApi.Domain.Models;

using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects
{
    public class Quantity : ValueObject
    {
        public int Value { get; }

        public Quantity(int value) => Value = value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}