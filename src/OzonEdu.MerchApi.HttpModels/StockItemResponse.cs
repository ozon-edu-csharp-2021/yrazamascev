namespace OzonEdu.MerchApi.HttpModels
{
    public class StockItemResponse
    {
        public long Id { get; set; }

        public long Sku { get; set; }

        public int? ClothingSize { get; set; }

        public int Quantity { get; set; }
    }
}