using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class OrdRejReason
    {
        public const int Tag = 103;
        public static readonly byte[] TagBytes;

        static OrdRejReason()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 48;
            TagBytes[2] = 51;
        }
    }
}
