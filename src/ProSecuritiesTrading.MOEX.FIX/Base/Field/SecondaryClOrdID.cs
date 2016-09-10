using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class SecondaryClOrdID
    {
        public const int Tag = 526;
        public static readonly byte[] TagBytes;

        static SecondaryClOrdID()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 53;
            TagBytes[1] = 50;
            TagBytes[2] = 54;
        }
    }
}
