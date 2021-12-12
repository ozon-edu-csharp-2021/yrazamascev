using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.Models;
using OzonEdu.MerchApi.HttpModels.ViewModels;
using OzonEdu.MerchApi.Infrastructure.Extensions;

namespace OzonEdu.MerchApi.HttpModels.Helpers
{
    public static class HttpModelsMapper
    {
        public static MerchOrderViewModel MerchOrderToViewModel(MerchOrder merchOrder)
        {
            return new()
            {
                Id = merchOrder.Id,
                PackType = EnumerationToViewModel(merchOrder.PackType),
                Status = EnumerationToViewModel(merchOrder.Status),
                RequestType = EnumerationToViewModel(merchOrder.RequestType),
                InWorkAt = merchOrder.InWorkAt.Value,
                DoneAt = merchOrder.DoneAt?.Value,
                EmployeeEmail = merchOrder.EmployeeEmail,
                SkuPackCollection = merchOrder.SkuPackCollection.Map(sp => SkuPackToViewModel(sp)),
            };
        }

        public static EnumerationViewModel EnumerationToViewModel(Enumeration enumeration)
        {
            return new()
            {
                Id = enumeration.Id,
                Name = enumeration.Name
            };
        }

        public static SkuPackViewModel SkuPackToViewModel(SkuPack skuPack)
        {
            return new()
            {
                SkuId = skuPack.Sku.Value,
                Quantity = skuPack.Quantity.Value
            };
        }
    }
}