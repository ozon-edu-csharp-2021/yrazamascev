using MediatR;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Commands;
using OzonEdu.StockApi.Grpc;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Handlers.DomainEvent
{
    public class RestockingCommandHandler : IRequestHandler<RestockingCommand>
    {
        private readonly IMerchOrderRepository _merchOrderRepository;
        private readonly StockApiGrpc.StockApiGrpcClient _stockApiClient;

        public RestockingCommandHandler(
            IMerchOrderRepository stockItemRepository,
            StockApiGrpc.StockApiGrpcClient stockApiClient)
        {
            _merchOrderRepository = stockItemRepository;
            _stockApiClient = stockApiClient;
        }

        public async Task<Unit> Handle(RestockingCommand request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<MerchOrder> merchOrders = await _merchOrderRepository.FindInWork(request.Skus, cancellationToken);

            foreach (MerchOrder merchOrder in merchOrders.OrderBy(o => o.InWorkAt))
            {
                GiveOutItemsRequest stockRequest = new();

                foreach (SkuPack skuPack in merchOrder.SkuPackCollection)
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
                    await _merchOrderRepository.Update(merchOrder, cancellationToken);
                }
            }

            return Unit.Value;
        }
    }
}