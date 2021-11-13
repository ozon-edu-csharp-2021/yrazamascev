using MediatR;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;

using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Commands.GetMerchOrders
{
    public class GetMerchOrdersCommand : IRequest<List<MerchOrder>>
    {
        public long EmployeeId { get; set; }
    }
}