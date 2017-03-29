using System;

namespace VendingMachineRepository.Models
{
    public class Payment : IPayment
    {
        public Payment(decimal amount)
        {
            if (!IsValidPayment(amount))
            {
                throw new ArgumentOutOfRangeException("amount", "Amount must be a valid number.");
            }
            _amount = amount;
        }

        private decimal _amount;

        public decimal Amount
        {
            get { return _amount; }
        }

        public bool SetPayment(decimal amount)
        {
            bool validAmount = IsValidPayment(amount);
            if (validAmount)
            {
                _amount = amount;
            }
            return validAmount;
        }

        private bool IsValidPayment(decimal amount)
        {
            return amount % PaymentAmount.Money[eMoney.eNickel] == 0;
        }
    }
}
