using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class OrderQty
    {
        public const int Tag = 38;
        public static readonly byte[] TagBytes;

        static OrderQty()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 51;
            TagBytes[1] = 56;
        }
    }
}
