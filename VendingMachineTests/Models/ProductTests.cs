using Xunit;
using System;
using VendingMachine.Models;

namespace VendingMachineTests
{
    public class ProductTests
    {
        [Fact]
        public void PriceIsInvalid()
        {
            bool exceptionThrown = false;
            try
            {
                var product = new Product(null, .01M, "invalid price");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.Contains("Price must be a valid amount.", ex.Message);
                exceptionThrown = true;
            }

            Assert.True(exceptionThrown);
        }

        [Fact]
        public void ProductConstructor()
        {
            string name = "new product";
            var product = new Product(null, 1, name);
            Assert.Null(product.Id);
            Assert.Equal(1, product.Price);
            Assert.Equal(name, product.Name);
        }
    }
}
