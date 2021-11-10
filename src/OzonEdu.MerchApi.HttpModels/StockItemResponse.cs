namespace OzonEdu.MerchApi.HttpModels
{
    public class StockItemResponse
    {
        public long Id { get; set; }

        public long Sku { get; init; }

        public int? ClothingSize { get; init; }

        public int Quantity { get; init; }
    }
}