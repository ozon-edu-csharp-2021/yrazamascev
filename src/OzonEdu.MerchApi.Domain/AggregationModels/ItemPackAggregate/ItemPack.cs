using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Exceptions;
using OzonEdu.MerchApi.Domain.Exceptions.ItemPackAggregate;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.ItemPackAggregate
{
    public class ItemPack : Entity
    {
        public Quantity Quantity { get; private set; }
        public StockItem StockItem { get; }

        public ItemPack(StockItem stockItem, Quantity quantity)
        {
            StockItem = stockItem;
            SetQuantity(quantity);
        }

        public void DecreaseQuantity(int valueToDecrease)
        {
            if (valueToDecrease <= 0)
            {
                throw new NegativeOrZeroValueException($"{nameof(valueToDecrease)} value must be positive");
            }
            if (Quantity.Value - valueToDecrease < 1)
            {
                throw new ItemPackQuantityException($"Quantity must be greater than zero");
            }

            Quantity = new Quantity(Quantity.Value - valueToDecrease);
        }

        public void IncreaseQuantity(int valueToIncrease)
        {
            if (valueToIncrease <= 0)
            {
                throw new NegativeOrZeroValueException($"{nameof(valueToIncrease)} value must be positive");
            }

            Quantity = new Quantity(Quantity.Value + valueToIncrease);
        }

        private void SetQuantity(Quantity value)
        {
            if (value.Value < 0)
            {
                throw new NegativeValueException($"{nameof(value)} value is negative");
            }

            Quantity = value;
        }
    }
}