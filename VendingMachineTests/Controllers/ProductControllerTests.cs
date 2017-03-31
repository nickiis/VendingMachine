using System.Web.Http.Results;
using Xunit;
using VendingMachineRepository;
using VendingMachineRepository.Controllers;
using VendingMachine.Models;
using System.Collections.Generic;
using System.Collections;

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
        public void PurchaseSucceeds()
        {
            _repository.CreditFunds(100);
            var productController = new ProductController(_repository);
            _repository.Products.Add(new VendingMachineRepository.Models.Product(null, 1, 1, "new product"));
            var response = productController.Purchase(new Product(null, 1, 1, "new product"));

            Assert.Equal(0, _repository.Products[1].Quantity);
        }

        [Fact]
        public void PurchaseFailsWithInadequateQuantity()
        {
            _repository.CreditFunds(100);
            var productController = new ProductController(_repository);
            _repository.Products.Add(new VendingMachineRepository.Models.Product(null, 1, 1, "new product"));
            var response = productController.Purchase(new Product(null, 2, 1, "new product"));
            var contentResult = response as NegotiatedContentResult<string>;
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal("Product has insufficient quantity.", contentResult.Content);
        }

        [Fact]
        public void PurchaseFailsWithNoInventory()
        {
            _repository.CreditFunds(100);
            var productController = new ProductController(_repository);
            var response = productController.Purchase(new Product(null, 1, 1, "new product"));
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
            var response = productController.Purchase(new Product(null, 1, 1, "new product"));
            var contentResult = response as NegotiatedContentResult<string>;
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal("Funds are inadequate.", contentResult.Content);
        }

        [Fact]
        public void ProductListEmpty()
        {
            var productController = new ProductController(_repository);
            var response = productController.List();
            // TODO: This is failing
            //var contentResult = response as OkNegotiatedContentResult<IList<Product>>;
            var contentResult = response as OkNegotiatedContentResult<IList>;
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal(0, contentResult.Content.Count);
        }

        [Fact]
        public void ProductListHasOne()
        {
            _repository.Products.Add(new VendingMachineRepository.Models.Product(null, 1, 1, "new product"));
            var productController = new ProductController(_repository);
            var response = productController.List();
            // TODO: This is failing
            //var contentResult = response as OkNegotiatedContentResult<IList<Product>>;
            var contentResult = response as OkNegotiatedContentResult<IList>;
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal(1, contentResult.Content.Count);
        }
    }
}
