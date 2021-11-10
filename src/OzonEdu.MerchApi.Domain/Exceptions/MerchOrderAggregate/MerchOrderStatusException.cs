using System;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchOrderAggregate
{
    internal class MerchOrderStatusException : Exception
    {
        public MerchOrderStatusException(string message) : base(message)
        {
        }

        public MerchOrderStatusException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}