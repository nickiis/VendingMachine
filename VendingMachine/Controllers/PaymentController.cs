using System.Web.Http;
using System.Net;
using VendingMachineCreditCards;

namespace VendingMachineRepository.Controllers
{
    [RoutePrefix("vendingMachine/payment")]
    public class PaymentController : ApiController
    {
        private IVendingMachineRepository _repository;
        private ICreditCardProcessor _creditCardProcessor;

        public PaymentController(IVendingMachineRepository repository, ICreditCardProcessor creditCardProcessor)
        {
            _repository = repository;
            _creditCardProcessor = creditCardProcessor;
        }

        [HttpPut]
        public IHttpActionResult CreditCash(eMoney money)
        {
            _repository.CreditFunds(PaymentAmount.Money[money]);

            return Ok();
        }

        [HttpPut]
        public IHttpActionResult DebitCash(eMoney money)
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

        [HttpPut]
        public IHttpActionResult CreditWithCard(string cardNumber, decimal money)
        {
            CreditCardResult result = _creditCardProcessor.ProcessCard(cardNumber);
            if (!result.Success)
            {
                return Content(HttpStatusCode.NotModified, result.ReasonForFailure);
            }
            else
            {
                _repository.CreditFunds(money);
                return Ok();
            }
        }

        [HttpPut]
        public IHttpActionResult Refund()
        {
            decimal amount = _repository.Payment.Amount;
            _repository.DebitFunds(amount);
            return Ok(amount);
        }
    }
}
