using MediatR;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;
using OzonEdu.MerchApi.Domain.Infrastructure.Commands;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Handlers.DomainEvent
{
    public class GetMerchOrdersCommandHandler : IRequestHandler<GetMerchOrdersCommand, IReadOnlyCollection<MerchOrder>>
    {
        private readonly IMerchOrderRepository _merchOrderRepository;

        public GetMerchOrdersCommandHandler(IMerchOrderRepository stockItemRepository) => _merchOrderRepository = stockItemRepository;

        public async Task<IReadOnlyCollection<MerchOrder>> Handle(GetMerchOrdersCommand request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<MerchOrder> merchOrders = await _merchOrderRepository.FindByEmployeeId(request.EmployeeEmail, cancellationToken);

            return merchOrders;
        }
    }
}