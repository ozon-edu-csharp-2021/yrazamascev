using MediatR;

using OzonEdu.MerchApi.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchApi.Domain.AggregationModels.ItemPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Infrastructure.Commands.CreateMerchOrder;
using OzonEdu.MerchApi.Domain.Infrastructure.Services;
using OzonEdu.MerchApi.HttpModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Handlers.MerchOrderAggregate
{
    public class CreateManualMerchOrderCommandHandler : IRequestHandler<CreateManualMerchOrderCommand, MerchOrder>
    {
        private readonly IMerchOrderRepository _merchOrderRepository;
        private readonly ISkuPackRepository _skuPackRepository;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly IStockApiService _stockApiService;
        private readonly IEmailService _emailService;

        public CreateManualMerchOrderCommandHandler(
            IMerchOrderRepository stockItemRepository,
            ISkuPackRepository skuPackRepository,
            IMerchPackRepository merchPackRepository,
            IStockApiService stockApiService,
            IEmailService emailService)
        {
            _merchOrderRepository = stockItemRepository;
            _skuPackRepository = skuPackRepository;
            _merchPackRepository = merchPackRepository;
            _stockApiService = stockApiService;
            _emailService = emailService;
        }

        public async Task<MerchOrder> Handle(CreateManualMerchOrderCommand request, CancellationToken cancellationToken)
        {
            List<MerchOrder> merchOrders = await _merchOrderRepository
                .FindIssuedMerch(request.EmployeeId, request.MerchPackId, cancellationToken);

            if (merchOrders.Count > 0)
            {
                throw new Exception($"Merch has already been issued");
            }

            MerchPackType merchPackType = MerchPackType.GetAll<MerchPackType>().FirstOrDefault(m => m.Id == request.MerchPackId);

            if (merchPackType is null)
            {
                throw new Exception($"Merch pack type not found");
            }

            MerchPack merchPack = await _merchPackRepository.FindByType(merchPackType, cancellationToken);

            List<StockItemResponse> stockItems = await _stockApiService.GetAll(cancellationToken);

            stockItems = stockItems.Where(i =>
            merchPack.ItemPackCollection.Any(ip => ip.StockItem.Value == i.Id)
            && (i.ClothingSize is null || i.ClothingSize == request.ClothingSize)).ToList();

            bool isEnough = true;

            List<SkuPack> skuPacks = new();

            foreach (ItemPack itemPack in merchPack.ItemPackCollection)
            {
                StockItemResponse stockItem = stockItems.First(si => si.Id == itemPack.StockItem.Value);
                if (itemPack.Quantity.Value <= stockItem.Quantity)
                {
                    isEnough = false;
                }
                skuPacks.Add(new SkuPack(new Sku(stockItem.Sku), itemPack.Quantity));
            }

            MerchOrder merchOrder = new(
                merchPackType,
                MerchRequestType.Manual,
                request.EmployeeId,
                skuPacks);

            if (isEnough)
            {
                bool isReserved = await _stockApiService.Reserve(skuPacks, cancellationToken);
                if (isReserved)
                {
                    merchOrder.Reserve();
                    bool isSended = await _emailService.SendMail(request.EmployeeId, cancellationToken);
                    if (isSended)
                    {
                        merchOrder.Done();
                    }
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