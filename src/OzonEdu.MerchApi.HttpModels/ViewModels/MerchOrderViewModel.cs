using System;
using System.Collections.Generic;

namespace OzonEdu.MerchApi.HttpModels.ViewModels
{
    public sealed class MerchOrderViewModel
    {
        public long Id { get; set; }

        public EnumerationViewModel PackType { get; set; }

        public EnumerationViewModel Status { get; set; }

        public EnumerationViewModel RequestType { get; set; }

        public DateTimeOffset InWorkAt { get; set; }

        public DateTimeOffset? DoneAt { get; set; }

        public string EmployeeEmail { get; set; }

        public IEnumerable<SkuPackViewModel> SkuPackCollection { get; set; }
    }
}