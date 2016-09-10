using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class PriceType
    {
        public const int Tag = 423;
        public static readonly byte[] TagBytes;
        public const byte Value1 = 49;
        public const byte Value2 = 50;
        public const byte Value9 = 57;

        static PriceType()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 52;
            TagBytes[1] = 50;
            TagBytes[2] = 51;
        }
    }
}
