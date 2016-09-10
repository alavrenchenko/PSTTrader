using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class NoMiscFees
    {
        public const int Tag = 136;
        public static readonly byte[] TagBytes;

        static NoMiscFees()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 51;
            TagBytes[2] = 54;
        }
    }
}
