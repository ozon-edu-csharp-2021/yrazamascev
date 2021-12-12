using OzonEdu.MerchApi.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchApi.Domain.AggregationModels.SkuPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Events;
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

        public DateAt DoneAt { get; private set; }

        public string EmployeeEmail { get; }

        public IReadOnlyList<SkuPack> SkuPackCollection { get; }

        public MerchOrder(
            long id,
            MerchPackType packType,
            MerchOrderStatus status,
            MerchRequestType requestType,
            DateAt inWorkAt,
            DateAt doneAt,
            string employeeEmail,
            IReadOnlyList<SkuPack> skuPackCollection)
        {
            Id = id;
            PackType = packType;
            Status = status;
            RequestType = requestType;
            InWorkAt = inWorkAt;
            DoneAt = doneAt;
            EmployeeEmail = employeeEmail;
            SkuPackCollection = skuPackCollection;
        }

        public MerchOrder(
            MerchPackType packType,
            MerchRequestType requestType,
            string employeeEmail,
            IReadOnlyList<SkuPack> skuPackCollection)
        {
            PackType = packType;
            Status = MerchOrderStatus.InWork;
            RequestType = requestType;
            InWorkAt = new DateAt(DateTimeOffset.UtcNow);
            EmployeeEmail = employeeEmail;
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

            AddDomainEvent(new EmployeeIssueMerchEvent()
            {
                EmployeeEmail = EmployeeEmail
            });
        }
    }
}