using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class OrderID
    {
        public const int Tag = 37;
        public static readonly byte[] TagBytes;

        static OrderID()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 51;
            TagBytes[1] = 55;
        }
    }
}
