using Google.Protobuf.WellKnownTypes;

using MediatR;

using OzonEdu.MerchApi.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchApi.Domain.AggregationModels.ItemPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Infrastructure.Commands;
using OzonEdu.StockApi.Grpc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Handlers.DomainEvent
{
    public class CreateMerchOrderCommandHandler : IRequestHandler<CreateMerchOrderCommand, MerchOrder>
    {
        private readonly IMerchOrderRepository _merchOrderRepository;
        private readonly ISkuPackRepository _skuPackRepository;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly StockApiGrpc.StockApiGrpcClient _stockApiClient;

        public CreateMerchOrderCommandHandler(
            IMerchOrderRepository stockItemRepository,
            ISkuPackRepository skuPackRepository,
            IMerchPackRepository merchPackRepository,
            StockApiGrpc.StockApiGrpcClient stockApiClient)
        {
            _merchOrderRepository = stockItemRepository;
            _skuPackRepository = skuPackRepository;
            _merchPackRepository = merchPackRepository;
            _stockApiClient = stockApiClient;
        }

        public async Task<MerchOrder> Handle(CreateMerchOrderCommand request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<MerchOrder> merchOrders = await _merchOrderRepository
                .FindIssuedMerch(request.EmployeeEmail, request.MerchType, cancellationToken);

            if (merchOrders.Count > 0)
            {
                throw new Exception($"Merch has already been issued");
            }

            MerchPackType merchPackType = Models.Enumeration.GetAll<MerchPackType>().FirstOrDefault(m => m.Id == (int)request.MerchType);

            if (merchPackType is null)
            {
                throw new Exception($"Merch pack type not found");
            }

            MerchPack merchPack = await _merchPackRepository.FindByType(merchPackType, cancellationToken);

            StockItemsResponse stockResponse = await _stockApiClient.GetAllStockItemsAsync(new Empty(), cancellationToken: cancellationToken);

            List<StockItemUnit> stockitems = stockResponse.Items.Where(i =>
                merchPack.ItemPackCollection.Any(ip => ip.StockItem.Value == i.ItemTypeId)
                && (i.SizeId is null || i.SizeId == (long)request.ClothingSize)).ToList();

            bool isEnough = true;

            List<SkuPack> skuPacks = new();
            foreach (ItemPack itemPack in merchPack.ItemPackCollection)
            {
                StockItemUnit stockItem = stockitems.First(si => si.ItemTypeId == itemPack.StockItem.Value);
                if (itemPack.Quantity.Value <= stockItem.Quantity)
                {
                    isEnough = false;
                }

                skuPacks.Add(new SkuPack(new Sku(stockItem.Sku), itemPack.Quantity));
            }

            MerchOrder merchOrder = new(
                merchPackType,
                request.MerchRequestType,
                request.EmployeeEmail,
                skuPacks);

            if (isEnough)
            {
                GiveOutItemsRequest stockRequest = new();

                foreach (SkuPack skuPack in skuPacks)
                {
                    stockRequest.Items.Add(new SkuQuantityItem()
                    {
                        Sku = skuPack.Sku.Value,
                        Quantity = skuPack.Quantity.Value
                    });
                }

                GiveOutItemsResponse response = await _stockApiClient.GiveOutItemsAsync(stockRequest, cancellationToken: cancellationToken);
                if (response.Result == GiveOutItemsResponse.Types.Result.Successful)
                {
                    merchOrder.Done();
                }
            }

            merchOrder = await _merchOrderRepository.Create(merchOrder, cancellationToken);
            foreach (SkuPack skuPack in merchOrder.SkuPackCollection)
            {
                await _skuPackRepository.Create(skuPack, merchOrder.Id, cancellationToken);
            }

            return merchOrder;
        }
    }
}