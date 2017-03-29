using System.Web.Http;
using System.Net;
using VendingMachine.Models;

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
            bool bHasFunds = _repository.Payment.Amount >= product.Price;
            if (bHasFunds)
            {
                bPurchased = _repository.DecreaseProductQuantity(new VendingMachineRepository.Models.Product(product.Id, 1, product.Price, product.Name));
            }

            if (!bHasFunds)
            {
                return Content(HttpStatusCode.NotModified, "Funds are inadequate.");
            }
            else if (!bPurchased)
            {
                return Content(HttpStatusCode.NotModified, "Product is not in inventory.");
            }
            else
            { 
                _repository.DebitFunds(product.Price);
                return Ok();
            }
        }

        [HttpGet]
        public IHttpActionResult List()
        {
            return Ok();
        }
    }
}
