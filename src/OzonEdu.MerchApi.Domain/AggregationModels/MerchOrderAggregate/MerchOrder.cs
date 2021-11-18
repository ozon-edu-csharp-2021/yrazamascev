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
        public DateAt DoneAt { get; private set; }

        public DateAt ReserveAt { get; private set; }

        public long EmployeeId { get; }

        public DateAt InWorkAt { get; }

        public IReadOnlyList<SkuPack> SkuPackCollection { get; }

        public MerchRequestType RequestType { get; }

        public MerchOrderStatus Status { get; private set; }

        public MerchPackType PackType { get; }

        public MerchOrder(long employee,
                          IReadOnlyList<SkuPack> skuPackCollection,
                          MerchRequestType requestType,
                          MerchPackType packType)
        {
            InWorkAt = new DateAt(DateTimeOffset.UtcNow);
            EmployeeId = employee;
            SkuPackCollection = skuPackCollection;
            RequestType = requestType;
            Status = MerchOrderStatus.InWork;
            PackType = packType;
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