using System.Web.Http.Results;
using Xunit;
using VendingMachineRepository;
using VendingMachineRepository.Controllers;
using VendingMachine.Models;

namespace VendingMachineTests.Controllers
{
    public class ProductControllerTests
    {
        private IVendingMachineRepository _repository;

        public ProductControllerTests()
        {
            _repository = new TestRepository.VendingMachineRepository();
        }

        [Fact]
        public void PurchaseFailsWithNoInventory()
        {
            _repository.CreditFunds(100);
            var productController = new ProductController(_repository);
            var response = productController.Purchase(new Product(null, 1, "new product"));
            var contentResult = response as NegotiatedContentResult<string>;
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal("Product is not in inventory.", contentResult.Content);
        }

        [Fact]
        public void PurchaseFailsWithInadequateFunds()
        {
            var productController = new ProductController(_repository);
            _repository.Products.Add(new VendingMachineRepository.Models.Product(null, 1, 1, "new product"));
            var response = productController.Purchase(new Product(null, 1, "new product"));
            var contentResult = response as NegotiatedContentResult<string>;
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal("Funds are inadequate.", contentResult.Content);
        }
    }
}
