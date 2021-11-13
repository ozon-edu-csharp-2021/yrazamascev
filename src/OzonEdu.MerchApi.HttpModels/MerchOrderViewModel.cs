using System;

namespace OzonEdu.MerchApi.HttpModels
{
    public sealed class MerchOrderViewModel
    {
        public DateTimeOffset DoneAt { get; set; }

        public DateTimeOffset ReserveAt { get; set; }

        public long EmployeeId { get; set; }

        public DateTimeOffset InWorkAt { get; }

        public string RequestType { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }
    }
}