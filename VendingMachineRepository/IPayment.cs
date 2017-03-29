using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineRepository
{
    public interface IPayment
    {
        decimal Amount
        { get; }

        bool SetPayment(decimal amount);
    }
}
