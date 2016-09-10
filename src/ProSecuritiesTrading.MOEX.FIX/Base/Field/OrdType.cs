using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class OrdType
    {
        public const int Tag = 40;
        public static readonly byte[] TagBytes;
        public const byte Value1 = 49;
        public const byte Value2 = 50;
        public const byte W = 87;

        static OrdType()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 52;
            TagBytes[1] = 48;
        }
    }
}
