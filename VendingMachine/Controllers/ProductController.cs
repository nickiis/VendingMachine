using System.Web.Http;
using System.Net;
using VendingMachine.Models;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace VendingMachineRepository.Controllers
{
    [RoutePrefix("vendingMachine/product")]
    public class ProductController : ApiController
    {
        private IVendingMachineRepository _repository;

        public ProductController(IVendingMachineRepository repository)
        {
            _repository = repository;
        }

        [HttpPut]
        public IHttpActionResult Purchase(Product product)
        {
            bool bPurchased = false;
            IProduct inventoryProduct = new VendingMachineRepository.Models.Product(product.Id, 1, product.Price, product.Name);
            bool bExists = _repository.ProductExists(inventoryProduct);
            bool bHasFunds = _repository.Payment.Amount >= product.Price;
            if (bExists)
            {
                if (bHasFunds)
                {
                    bPurchased = _repository.DecreaseProductQuantity(inventoryProduct);
                }
            }

            if (!bExists)
            {
                return Content(HttpStatusCode.NotFound, "Product is not in inventory.");
            }
            else if (!bHasFunds)
            {
                return Content(HttpStatusCode.NotModified, "Funds are inadequate.");
            }
            else if (!bPurchased)
            {
                return Content(HttpStatusCode.NotModified, "Product has insufficient quantity.");
            }
            else
            {
                _repository.DebitFunds(product.Price);
                return Ok();
            }

        }

        [HttpGet]
        [ResponseType(typeof(IList<Product>))]
        public IHttpActionResult List()
        {
            IList productList = new List<Product>();
            foreach(IProduct product in _repository.Products)
            {
                productList.Add(new Product(product.Id, product.Quantity, product.Price, product.Name));
            }
            return Ok(productList);
        }
    }
}
