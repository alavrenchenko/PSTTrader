using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class Side
    {
        public const int Tag = 54;
        public static readonly byte[] TagBytes;
        public const byte Value1 = 49;
        public const byte Value2 = 50;

        static Side()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 53;
            TagBytes[1] = 52;
        }
    }
}
