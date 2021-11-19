using OzonEdu.MerchApi.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Models;

using System;
using System.Collections.Generic;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate
{
    public class MerchOrder : Entity
    {
        public MerchPackType PackType { get; }

        public MerchOrderStatus Status { get; private set; }

        public MerchRequestType RequestType { get; }

        public DateAt InWorkAt { get; }

        public DateAt ReserveAt { get; private set; }

        public DateAt DoneAt { get; private set; }

        public long EmployeeId { get; }

        public IReadOnlyList<SkuPack> SkuPackCollection { get; }

        public MerchOrder(
            long id,
            MerchPackType packType,
            MerchOrderStatus status,
            MerchRequestType requestType,
            DateAt inWorkAt,
            DateAt reserveAt,
            DateAt doneAt,
            long employeeId,
            IReadOnlyList<SkuPack> skuPackCollection)
        {
            Id = id;
            PackType = packType;
            Status = status;
            RequestType = requestType;
            InWorkAt = inWorkAt;
            ReserveAt = reserveAt;
            DoneAt = doneAt;
            EmployeeId = employeeId;
            SkuPackCollection = skuPackCollection;
        }

        public MerchOrder(
            MerchPackType packType,
            MerchRequestType requestType,
            long employeeId,
            IReadOnlyList<SkuPack> skuPackCollection)
        {
            PackType = packType;
            Status = MerchOrderStatus.InWork;
            RequestType = requestType;
            InWorkAt = new DateAt(DateTimeOffset.UtcNow);
            EmployeeId = employeeId;
            SkuPackCollection = skuPackCollection;
        }

        public void Done()
        {
            if (Status.Equals(MerchOrderStatus.IsDone))
            {
                throw new MerchOrderStatusException($"Order in done. Change status unavailable");
            }

            DoneAt = new DateAt(DateTimeOffset.UtcNow);
            Status = MerchOrderStatus.IsDone;
        }

        public void Reserve()
        {
            if (Status.Equals(MerchOrderStatus.IsReserved))
            {
                throw new MerchOrderStatusException($"Order in reserve. Change status unavailable");
            }
            if (Status.Equals(MerchOrderStatus.IsDone))
            {
                throw new MerchOrderStatusException($"Order in done. Change status unavailable");
            }

            ReserveAt = new DateAt(DateTimeOffset.UtcNow);
            Status = MerchOrderStatus.IsReserved;
        }
    }
}