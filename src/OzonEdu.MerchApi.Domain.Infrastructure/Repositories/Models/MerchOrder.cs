using System;
using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.Infrastructure.Repositories.Models
{
    public sealed class MerchOrder
    {
        public long Id { get; set; }

        public int PackTypeId { get; set; }

        public string PackTypeName { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int RequestTypeId { get; set; }

        public string RequestTypeName { get; set; }

        public DateTimeOffset InWorkAt { get; set; }

        public DateTimeOffset? DoneAt { get; set; }

        public string EmployeeEmail { get; set; }

        public IEnumerable<SkuPack> SkuPackCollection { get; set; }
    }
}