using MediatR;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Commands.CreateMerchOrder
{
    public class CreateManualMerchOrderCommand : IRequest<MerchOrder>
    {
        public long EmployeeId { get; set; }
        public int ClothingSize { get; set; }
        public int MerchPackId { get; set; }
    }
}