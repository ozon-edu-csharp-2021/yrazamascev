using CSharpCourse.Core.Lib.Enums;

using MediatR;

using OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Commands
{
    public class CreateMerchOrderCommand : IRequest<MerchOrder>
    {
        public string EmployeeEmail { get; set; }
        public ClothingSize ClothingSize { get; set; }
        public MerchType MerchType { get; set; }
        public MerchRequestType MerchRequestType { get; set; }
    }
}