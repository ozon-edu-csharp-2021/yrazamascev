using MediatR;

namespace OzonEdu.MerchApi.Domain.Events
{
    public class EmployeeIssueMerchEvent : INotification
    {
        public string EmployeeEmail { get; set; }
    }
}