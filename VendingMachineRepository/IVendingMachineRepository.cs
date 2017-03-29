using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineRepository
{
    public interface IVendingMachineRepository
    {
        IPayment Payment
        { get; }

        IList<IProduct> Products
        { get; }

        bool DecreaseProductQuantity(IProduct product);

        bool DebitFunds(decimal amount);

        void CreditFunds(decimal amount);
    }
}
