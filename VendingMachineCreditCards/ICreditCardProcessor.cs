using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineCreditCards
{
    // NOTE: I would do a class factory and instantiate the correct processor for the type of card and or cc processor
    public interface ICreditCardProcessor
    {
        CreditCardResult ProcessCard(string cardNumber);
    }
}
