using System.Collections.Generic;

namespace VendingMachineRepository
{
    public enum eMoney { eNickel, eDime, eQuarter, eOneDollar, eFiveDollar, eTenDollar, eTwentyDollar };

    public class PaymentAmount
    {
        public static Dictionary<eMoney, decimal> Money = new Dictionary<eMoney, decimal>()
        {
            { eMoney.eNickel, 0.05M },
            { eMoney.eDime, 0.10M },
            { eMoney.eQuarter, 0.25M },
            { eMoney.eOneDollar, 1.00M },
            { eMoney.eFiveDollar, 5.00M },
            { eMoney.eTenDollar, 10.00M },
            { eMoney.eTwentyDollar, 20.00M }
        };
    }
}