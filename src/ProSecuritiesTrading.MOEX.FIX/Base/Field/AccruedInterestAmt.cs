using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class AccruedInterestAmt
    {
        public const int Tag = 159;
        public static readonly byte[] TagBytes;

        static AccruedInterestAmt()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 53;
            TagBytes[2] = 57;
        }
    }
}
