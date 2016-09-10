using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class MaxPriceLevels
    {
        public const int Tag = 1090;
        public static readonly byte[] TagBytes;
        public const byte Value1 = 49;

        static MaxPriceLevels()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 49;
            TagBytes[1] = 48;
            TagBytes[2] = 57;
            TagBytes[2] = 48;
        }
    }
}
