using OzonEdu.MerchApi.Domain.AggregationModels.ItemPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Exceptions;

using Xunit;

namespace OzonEdu.MerchApi.Domain.Tests
{
    public class ItemPackTests
    {
        [Fact]
        public void CreateNegativeQuantitySuccess()
        {
            int value = -1;

            Assert.Throws<NegativeValueException>(() =>
            {
                ItemPack stockItem = new(
                    new StockItem(149568),
                    new Quantity(value));
            });
        }

        [Fact]
        public void DecreaseQuantityNegativeAndZeroValueSuccess()
        {
            ItemPack stockItem = new(
                new StockItem(149568),
                new Quantity(10));

            int value = -1;
            Assert.Throws<NegativeOrZeroValueException>(() => stockItem.DecreaseQuantity(value));

            value = 0;
            Assert.Throws<NegativeOrZeroValueException>(() => stockItem.DecreaseQuantity(value));
        }

        [Fact]
        public void IncreaseQuantityNegativeAndZeroValueSuccess()
        {
            ItemPack stockItem = new(
                new StockItem(149568),
                new Quantity(10));

            int value = -1;
            Assert.Throws<NegativeOrZeroValueException>(() => stockItem.IncreaseQuantity(value));

            value = 0;
            Assert.Throws<NegativeOrZeroValueException>(() => stockItem.IncreaseQuantity(value));
        }
    }
}