using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class MiscFeeAmt
    {
        public const int Tag = 137;
        public static readonly byte[] TagBytes;

        static MiscFeeAmt()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 51;
            TagBytes[2] = 55;
        }
    }
}
