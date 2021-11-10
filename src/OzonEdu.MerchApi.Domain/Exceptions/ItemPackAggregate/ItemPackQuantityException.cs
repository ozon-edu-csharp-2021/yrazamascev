using System;

namespace OzonEdu.MerchApi.Domain.Exceptions.ItemPackAggregate
{
    public class ItemPackQuantityException : Exception
    {
        public ItemPackQuantityException(string message) : base(message)
        {
        }

        public ItemPackQuantityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}