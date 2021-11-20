namespace OzonEdu.MerchApi.HttpModels
{
    public sealed class IssueMerchRequest
    {
        public long EmployeeId { get; set; }
        public int ClothingSize { get; set; }
        public int MerchPackId { get; set; }
    }
}