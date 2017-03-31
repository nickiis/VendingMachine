using Xunit;
using VendingMachineRepository;
using VendingMachineRepository.Controllers;
using VendingMachine.Models;
using System.Web.Http;
using System.Web.Http.Results;
using VendingMachineCreditCards;

namespace VendingMachineTests
{
    public class PaymentControllerTests
    {
        private IVendingMachineRepository _repository;
        private ICreditCardProcessor _ccProcessor;

        public PaymentControllerTests()
        {
            _repository = new TestRepository.VendingMachineRepository();
            _ccProcessor = new CreditCard.TestCreditCardProcessor();
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
            var paymentController = new PaymentController(_repository, _ccProcessor);
            var response = paymentController.CreditCash(money);
            Assert.Equal(PaymentAmount.Money[money], _repository.Payment.Amount);
        }

        [Fact]
        public void PaymentAllIsCorrect()
        {
            var payment = new Payment(0);
            var paymentController = new PaymentController(_repository, _ccProcessor);
            decimal amount = 0;
            IHttpActionResult response = null;
            foreach (var money in PaymentAmount.Money)
            {
                amount += money.Value;
                response = paymentController.CreditCash(money.Key);
            }

            Assert.Equal(amount, _repository.Payment.Amount);
        }

        [Fact]
        public void DebitNegativePaymentNotAllowed()
        {
            var payment = new Payment(0);
            var paymentController = new PaymentController(_repository, _ccProcessor);
            var response = paymentController.DebitCash(eMoney.eDime);

            var contentResult = response as NegotiatedContentResult<string>;

            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal("Negative payment is not allowed", contentResult.Content);
        }

        [Fact]
        public void DebitIsSuccessful()
        {
            var paymentController = new PaymentController(_repository, _ccProcessor);
            var payment = new Payment(0);
            var response = paymentController.CreditCash(eMoney.eOneDollar);
            Assert.Equal(PaymentAmount.Money[eMoney.eOneDollar], _repository.Payment.Amount);

            response = paymentController.DebitCash(eMoney.eOneDollar);

            Assert.Equal(0, _repository.Payment.Amount);
        }

        [Fact]
        public void CreditCardSuccessful()
        {
            var paymentController = new PaymentController(_repository, _ccProcessor);
            var response = paymentController.CreditWithCard("2121212121212121", 20);
            Assert.Equal(20, _repository.Payment.Amount);
        }

        [Fact]
        public void CreditCardInvalidNumber()
        {
            var paymentController = new PaymentController(_repository, _ccProcessor);
            var response = paymentController.CreditWithCard("2121212b21212121", 20);
            Assert.Equal(0, _repository.Payment.Amount);
            var contentResult = response as NegotiatedContentResult<string>;

            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal("Invalid credit card format.", contentResult.Content);
        }

        [Fact]
        public void CreditCardNumberTooLong()
        {
            var paymentController = new PaymentController(_repository, _ccProcessor);
            var response = paymentController.CreditWithCard("21212121212121212", 20);
            Assert.Equal(0, _repository.Payment.Amount);
            var contentResult = response as NegotiatedContentResult<string>;

            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal("Invalid credit card format.", contentResult.Content);
        }

        [Fact]
        public void CreditCardNumberTooShort()
        {
            var paymentController = new PaymentController(_repository, _ccProcessor);
            var response = paymentController.CreditWithCard("212121212121212", 20);
            Assert.Equal(0, _repository.Payment.Amount);
            var contentResult = response as NegotiatedContentResult<string>;

            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal("Invalid credit card format.", contentResult.Content);
        }

        [Fact]
        public void CreditCardNoNumber()
        {
            var paymentController = new PaymentController(_repository, _ccProcessor);
            var response = paymentController.CreditWithCard("", 20);
            Assert.Equal(0, _repository.Payment.Amount);
            var contentResult = response as NegotiatedContentResult<string>;

            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal("Invalid credit card format.", contentResult.Content);
        }

        [Fact]
        public void CreditCardNullNumber()
        {
            var paymentController = new PaymentController(_repository, _ccProcessor);
            var response = paymentController.CreditWithCard(null, 20);
            Assert.Equal(0, _repository.Payment.Amount);
            var contentResult = response as NegotiatedContentResult<string>;

            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal("Invalid credit card format.", contentResult.Content);
        }

        [Fact]
        public void RefundSuccess()
        {
            _repository.CreditFunds(100);
            var productController = new PaymentController(_repository, _ccProcessor);
            var response = productController.Refund();
            var contentResult = response as OkNegotiatedContentResult<decimal>;
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal(100, contentResult.Content);
        }
    }
}
