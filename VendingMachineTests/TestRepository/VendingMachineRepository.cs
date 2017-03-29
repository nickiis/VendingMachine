using System;
using System.Collections.Generic;
using VendingMachineRepository;
using VendingMachineRepository.Models;

namespace VendingMachineTests.TestRepository
{
    public class VendingMachineRepository : IVendingMachineRepository
    {
        public VendingMachineRepository()
        {
            _products = new List<IProduct>();
            _payment = new Payment(0);
        }

        private Payment _payment;

        public IPayment Payment
        {
            get { return _payment; }
        }

        private IList<IProduct> _products;

        public IList<IProduct> Products
        {
            get { return _products; }
        }

        public bool DecreaseProductQuantity(IProduct product)
        {
            bool bFound = false;
            foreach(IProduct inventoryProduct in _products)
            {
                if (inventoryProduct.Id == product.Id && inventoryProduct.Quantity >= product.Quantity)
                {
                    _products.Remove(inventoryProduct);
                    if (inventoryProduct.Quantity > product.Quantity)
                    {
                        _products.Add(new Product(inventoryProduct.Id, inventoryProduct.Quantity - product.Quantity, inventoryProduct.Price, inventoryProduct.Name));
                    }
                    bFound = true;
                    break;
                }
            }
            return bFound;
        }

        public bool DebitFunds(decimal amount)
        {
            bool bPaymentMade = false;
            if (_payment.Amount >= amount)
            {
                _payment = new Payment(_payment.Amount - amount);
                bPaymentMade = true;
            }
            return bPaymentMade;
        }

        public void CreditFunds(decimal amount)
        {
            _payment = new Payment(_payment.Amount + amount);
        }
    }
}
