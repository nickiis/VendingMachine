using VendingMachineCreditCards;

namespace VendingMachineTests.CreditCard
{
    class TestCreditCardProcessor : ICreditCardProcessor
    {
        public CreditCardResult ProcessCard(string cardNumber)
        {
            if (cardNumber != null && cardNumber.Length == 16 && IsDigitsOnly(cardNumber))
            {
                return new CreditCardResult(true, null);
            }
            else
            {
                return new CreditCardResult(false, "Invalid credit card format.");
            }
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}
