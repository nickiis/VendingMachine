using System;
using VendingMachineRepository;

namespace VendingMachine.Models
{

    public class Payment
    {
        public Payment(decimal amount)
        {
            if (amount % PaymentAmount.Money[eMoney.eNickel] > 0)
            {
                throw new ArgumentOutOfRangeException("amount", "Amount must be a valid number.");
            }
            Amount = amount;
        }

        public decimal Amount
        { get; private set; }

        public void Credit(eMoney money)
        {
            Amount += PaymentAmount.Money[money];
        }

        public void Debit(eMoney money)
        {
            if (Amount >= PaymentAmount.Money[money])
            {
                Amount -= PaymentAmount.Money[money];
            }
            else
            {
                throw new ArgumentOutOfRangeException("money", "Debit amount must be greater than or equal to the current amount.");
            }
        }
    }
}