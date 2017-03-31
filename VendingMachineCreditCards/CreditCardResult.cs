using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineCreditCards
{
    public class CreditCardResult
    {
        public CreditCardResult(bool success, string reasonForFailure)
        {
            Success = success;
            ReasonForFailure = reasonForFailure;
        }

        public bool Success { get; }

        public string ReasonForFailure { get; }
    }
}
