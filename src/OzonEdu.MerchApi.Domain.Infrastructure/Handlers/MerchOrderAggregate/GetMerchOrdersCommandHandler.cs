using MediatR;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Commands.GetMerchOrders;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Handlers.MerchOrderAggregate
{
    public class GetMerchOrdersCommandHandler : IRequestHandler<GetMerchOrdersCommand, List<MerchOrder>>
    {
        private readonly IMerchOrderRepository _merchOrderRepository;

        public GetMerchOrdersCommandHandler(IMerchOrderRepository stockItemRepository) => _merchOrderRepository = stockItemRepository;

        public async Task<List<MerchOrder>> Handle(GetMerchOrdersCommand request, CancellationToken cancellationToken)
        {
            List<MerchOrder> merchOrders = await _merchOrderRepository.FindByEmployeeIdAsync(request.EmployeeId, cancellationToken);

            return merchOrders;
        }
    }
}