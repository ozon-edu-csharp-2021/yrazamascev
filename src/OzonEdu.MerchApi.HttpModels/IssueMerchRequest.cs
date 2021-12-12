using CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchApi.HttpModels
{
    public sealed class IssueMerchRequest
    {
        public string EmployeeEmail { get; set; }
        public ClothingSize ClothingSize { get; set; }
        public MerchType MerchType { get; set; }
    }
}