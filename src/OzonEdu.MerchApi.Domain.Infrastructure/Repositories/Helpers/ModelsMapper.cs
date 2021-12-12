using OzonEdu.MerchApi.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchApi.Domain.AggregationModels.ItemPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Infrastructure.Extensions;

using System.Linq;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Helpers
{
    public static class ModelsMapper
    {
        public static ItemPack ItemPackModelToEntity(Models.ItemPack model)
        {
            return model is null
                ? null
                : (new(
                new StockItem(model.StockItemId),
                new Quantity(model.Quantity)));
        }

        public static MerchOrder MerchOrderModelToEntity(Models.MerchOrder model)
        {
            return model is null
                ? null
                : (new(
                model.Id,
                new MerchPackType(model.PackTypeId, model.PackTypeName),
                new MerchOrderStatus(model.StatusId, model.StatusName),
                new MerchRequestType(model.RequestTypeId, model.RequestTypeName),
                new DateAt(model.InWorkAt),
                model.DoneAt is null ? null : new DateAt(model.DoneAt.Value),
                model.EmployeeEmail,
                model.SkuPackCollection.Map(model => SkuPackModelToEntity(model)).ToList()));
        }

        public static MerchPack MerchPackModelToEntity(Models.MerchPack model)
        {
            return model is null
                ? null
                : (new(
                new MerchPackType(model.PackTypeId, model.PackTypeName),
                model.ItemPackCollection.Map(model => ItemPackModelToEntity(model)).ToList()));
        }

        public static SkuPack SkuPackModelToEntity(Models.SkuPack model)
        {
            return model is null
                ? null
                : (new(
                new Sku(model.SkuId),
                new Quantity(model.Quantity)));
        }
    }
}