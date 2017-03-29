using System;
using System.Web.Http;
using System.Net;
using VendingMachine.Models;

namespace VendingMachineRepository.Controllers
{
    [RoutePrefix("vendingMachine/payment")]
    public class PaymentController : ApiController
    {
        private IVendingMachineRepository _repository;

        public PaymentController(IVendingMachineRepository repository)
        {
            _repository = repository;
        }

        [HttpPut]
        public IHttpActionResult Credit(eMoney money)
        {
            _repository.CreditFunds(PaymentAmount.Money[money]);

            return Ok();
        }

        [HttpPut]
        public IHttpActionResult Debit(eMoney money)
        {
            if (_repository.DebitFunds(PaymentAmount.Money[money]))
            { 
                return Ok();
            }
            else
            {
                return Content(HttpStatusCode.NotModified, "Negative payment is not allowed");
            }
        }
    }
}
