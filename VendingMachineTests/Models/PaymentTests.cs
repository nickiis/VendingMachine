using System;
using Xunit;
using VendingMachineRepository;
using VendingMachine.Models;

namespace VendingMachineTests.Models
{
    public class PaymentTests
    {
        [InlineData(eMoney.eNickel)]
        [InlineData(eMoney.eDime)]
        [InlineData(eMoney.eQuarter)]
        [InlineData(eMoney.eOneDollar)]
        [InlineData(eMoney.eFiveDollar)]
        [Theory]
        public void PaymentIsAmount(eMoney money)
        {
            var payment = new Payment(0);
            payment.Credit(money);
            Assert.Equal(PaymentAmount.Money[money], payment.Amount);
        }

        [Fact]
        public void PaymentAllIsCorrect()
        {
            var payment = new Payment(0);
            decimal amount = 0;
            foreach (var money in PaymentAmount.Money)
            {
                amount += money.Value;
                payment.Credit(money.Key);
            }

            Assert.Equal(amount, payment.Amount);
        }

        [Fact]
        public void DebitThrowsException()
        {
            bool exceptionThrown = false;
            var payment = new Payment(0);
            try
            {
                payment.Debit(eMoney.eDime);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                exceptionThrown = true;
                Assert.Contains("Debit amount must be greater than or equal to the current amount.", ex.Message);
            }

            Assert.True(exceptionThrown);
        }

        [Fact]
        public void DebitIsSuccessful()
        {
            eMoney amount = eMoney.eOneDollar;
            var payment = new Payment(PaymentAmount.Money[amount]);
            Assert.Equal(1, payment.Amount);
            payment.Debit(amount);
            Assert.Equal(0, payment.Amount);
        }
    }
}
