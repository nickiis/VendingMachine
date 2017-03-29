using System;

namespace VendingMachineRepository.Models
{
    public class Product : IProduct
    {
        public Product(int? id, int quantity, decimal price, string name)
        {
            if (price % PaymentAmount.Money[eMoney.eNickel] > 0)
            {
                throw new ArgumentOutOfRangeException("amount", "Price must be a valid amount.");
            }

            _id = id;
            _quantity = quantity;
            _price = price;
            _name = name;
        }

        private int? _id;

        public int? Id
        {
            get { return _id; }
            internal set { _id = value; }
        }

        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
        }

        private decimal _price;

        public decimal Price
        {
            get { return _price; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
        }
    }
}
