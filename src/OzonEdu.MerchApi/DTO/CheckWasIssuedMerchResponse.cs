namespace OzonEdu.MerchApi.DTO
{
    public sealed class CheckWasIssuedMerchResponse
    {
        public long EmployeeId { get; set; }
        public bool WasIssued { get; set; }
    }
}