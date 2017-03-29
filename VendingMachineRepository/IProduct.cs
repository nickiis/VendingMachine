using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineRepository
{
    public interface IProduct
    {
        int? Id
        { get; }

        int Quantity
        { get; }

        decimal Price
        { get; }

        string Name
        { get; }
    }
}
