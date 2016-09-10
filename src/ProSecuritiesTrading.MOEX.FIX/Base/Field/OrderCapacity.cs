using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class OrderCapacity
    {
        public const int Tag = 528;
        public static readonly byte[] TagBytes;
        public const byte P = 80;

        static OrderCapacity()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 53;
            TagBytes[1] = 50;
            TagBytes[2] = 56;
        }
    }
}
