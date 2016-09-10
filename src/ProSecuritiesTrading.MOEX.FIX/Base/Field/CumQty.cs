using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class CumQty
    {
        public const int Tag = 14;
        public static readonly byte[] TagBytes;

        static CumQty()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 49;
            TagBytes[1] = 52;
        }
    }
}
