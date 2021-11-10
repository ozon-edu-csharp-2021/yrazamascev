using OzonEdu.MerchApi.Domain.Models;

using System;
using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects
{
    public class DateAt : ValueObject
    {
        public DateTimeOffset Value { get; }

        public DateAt(DateTimeOffset value) => Value = value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}