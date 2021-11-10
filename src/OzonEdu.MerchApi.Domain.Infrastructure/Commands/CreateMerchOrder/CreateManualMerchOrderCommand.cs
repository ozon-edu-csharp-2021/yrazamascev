using MediatR;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Commands.CreateMerchOrder
{
    public class CreateManualMerchOrderCommand : IRequest<int>
    {
        public long EmployeeId { get; set; }
        public int ClothingSize { get; init; }
        public int MerchPackId { get; set; }
    }
}