﻿using OzonEdu.MerchApi.Domain.Models;

using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects
{
    public class Sku : ValueObject
    {
        public long Value { get; }

        public Sku(long sku) => Value = sku;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}