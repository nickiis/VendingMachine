using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Controllers;

namespace VendingMachine.Tests
{
    [TestClass]
    public class PaymentControllerTest
    {
        [TestCase(eMoney.eNickel)]
        [TestCase(eMoney.eDime)]
        [TestCase(eMoney.eQuarter)]
        [TestCase(eMoney.eOneDollar)]
        [TestCase(eMoney.eFiveDollar)]
        [TestCase(eMoney.eTenDollar)]
        [TestCase(eMoney.eTwentyDollar)]
        public void PaymentIsAmount(eMoney money)
        {
            Payment payment = new Payment();
            payment.AddCredit(money);
            Assert.AreEqual(payment.Credit, payment.Money[money]);
        }

        [TestCase]
        public void PaymentAllIsCorrect()
        {
            Payment payment = new Payment();
            decimal amount = 0;
            foreach (var money in payment.Money)
            {
                amount += money.Value;
                payment.AddCredit(money.Key);
            }

            Assert.AreEqual(payment.Credit, amount);
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DebitThrowsException()
        {
            Payment payment = new Payment();
            payment.Debit(1);
        }

        [TestCase]
        public void DebitIsSuccessful()
        {
            Payment payment = new Payment();
            payment.AddCredit(eMoney.eOneDollar);
            Assert.AreEqual(payment.Credit, 1);
            payment.Debit(1);
            Assert.AreEqual(payment.Credit, 0);
        }
    }
}
