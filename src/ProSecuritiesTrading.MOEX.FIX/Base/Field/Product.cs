using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class Product
    {
        public const int Tag = 460;
        public static readonly byte[] TagBytes;
        public const byte Value4 = 52;

        static Product()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 52;
            TagBytes[1] = 54;
            TagBytes[2] = 48;
        }
    }
}
