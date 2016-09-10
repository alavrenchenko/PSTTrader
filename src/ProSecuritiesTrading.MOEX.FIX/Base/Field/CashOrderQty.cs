using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class CashOrderQty
    {
        public const int Tag = 152;
        public static readonly byte[] TagBytes;

        static CashOrderQty()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 53;
            TagBytes[2] = 50;
        }
    }
}
