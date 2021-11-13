using OzonEdu.MerchApi.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchApi.Domain.AggregationModels.ItemPackAggregate;
using OzonEdu.MerchApi.Domain.Models;

using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchPack : Entity
    {
        public IReadOnlyList<ItemPack> ItemPackCollection { get; }

        public MerchPackType Type { get; }

        public MerchPack(MerchPackType type, IReadOnlyList<ItemPack> itemPackCollection)
        {
            Type = type;
            ItemPackCollection = itemPackCollection;
        }
    }
}