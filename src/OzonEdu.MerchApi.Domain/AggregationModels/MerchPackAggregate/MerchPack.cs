using OzonEdu.MerchApi.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchApi.Domain.AggregationModels.ItemPackAggregate;
using OzonEdu.MerchApi.Domain.Models;

using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchPack : Entity
    {
        public IReadOnlyList<ItemPack> ItemPackCollection { get; }

        public MerchPackType PackType { get; }

        public MerchPack(MerchPackType packType, IReadOnlyList<ItemPack> itemPackCollection)
        {
            PackType = packType;
            ItemPackCollection = itemPackCollection;
        }
    }
}