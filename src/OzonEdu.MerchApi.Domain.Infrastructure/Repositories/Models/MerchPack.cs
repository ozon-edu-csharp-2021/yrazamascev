using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Models
{
    public sealed class MerchPack
    {
        public long Id { get; set; }

        public int PackTypeId { get; set; }

        public string PackTypeName { get; set; }

        public IEnumerable<ItemPack> ItemPackCollection { get; set; }
    }
}