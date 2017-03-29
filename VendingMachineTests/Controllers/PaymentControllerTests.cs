using Xunit;
using VendingMachineRepository;
using VendingMachineRepository.Controllers;
using VendingMachine.Models;
using System.Web.Http;
using System.Web.Http.Results;

namespace VendingMachineTests
{
    public class PaymentControllerTests
    {
        private IVendingMachineRepository _repository;

        public PaymentControllerTests()
        {
            _repository = new TestRepository.VendingMachineRepository();
        }

        [InlineData(eMoney.eNickel)]
        [InlineData(eMoney.eDime)]
        [InlineData(eMoney.eQuarter)]
        [InlineData(eMoney.eOneDollar)]
        [InlineData(eMoney.eFiveDollar)]
        [Theory]
        public void PaymentIsAmount(eMoney money)
        {
            var payment = new Payment(0);
            var paymentController = new PaymentController(_repository);
            var response = paymentController.Credit(money);
            Assert.Equal(PaymentAmount.Money[money], _repository.Payment.Amount);
        }

        [Fact]
        public void PaymentAllIsCorrect()
        {
            var payment = new Payment(0);
            var paymentController = new PaymentController(_repository);
            decimal amount = 0;
            IHttpActionResult response = null;
            foreach (var money in PaymentAmount.Money)
            {
                amount += money.Value;
                response = paymentController.Credit(money.Key);
            }

            Assert.Equal(amount, _repository.Payment.Amount);
        }

        [Fact]
        public void DebitNegativePaymentNotAllowed()
        {
            var payment = new Payment(0);
            var paymentController = new PaymentController(_repository);
            var response = paymentController.Debit(eMoney.eDime);

            var contentResult = response as NegotiatedContentResult<string>;

            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal("Negative payment is not allowed", contentResult.Content);
        }

        [Fact]
        public void DebitIsSuccessful()
        {
            var paymentController = new PaymentController(_repository);
            var payment = new Payment(0);
            var response = paymentController.Credit(eMoney.eOneDollar);
            Assert.Equal(PaymentAmount.Money[eMoney.eOneDollar], _repository.Payment.Amount);

            response = paymentController.Debit(eMoney.eOneDollar);

            Assert.Equal(0, _repository.Payment.Amount);
        }
    }
}
