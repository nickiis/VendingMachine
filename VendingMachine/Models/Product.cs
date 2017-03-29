using System;
using VendingMachineRepository;

namespace VendingMachine.Models
{
    public class Product
    {
        public Product(int? id, decimal price, string name)
        {
            if (price % PaymentAmount.Money[eMoney.eNickel] > 0)
            {
                throw new ArgumentOutOfRangeException("amount", "Price must be a valid amount.");
            }

            Id = id;
            Price = price;
            Name = name;
        }

        public int? Id
        { get; private set; }

        public decimal Price
        { get; private set; }

        public string Name
        { get; private set; }
    }
}