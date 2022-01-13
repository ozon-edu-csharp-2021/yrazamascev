using MediatR;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;

using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Commands
{
    public class GetMerchOrdersCommand : IRequest<IReadOnlyCollection<MerchOrder>>
    {
        public string EmployeeEmail { get; set; }
    }
}