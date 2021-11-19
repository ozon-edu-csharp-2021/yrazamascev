namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Models
{
    public class SkuPack
    {
        public long Id { get; set; }

        public long MerchOrderId { get; set; }

        public long SkuId { get; set; }

        public int Quantity { get; set; }
    }
}