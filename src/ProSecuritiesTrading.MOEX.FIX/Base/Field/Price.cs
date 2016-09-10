using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class Price
    {
        public const int Tag = 44;
        public static readonly byte[] TagBytes;

        static Price()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 52;
            TagBytes[1] = 52;
        }
    }
}
