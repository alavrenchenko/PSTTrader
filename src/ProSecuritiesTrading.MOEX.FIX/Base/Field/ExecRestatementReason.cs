using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class ExecRestatementReason
    {
        public const int Tag = 378;
        public static readonly byte[] TagBytes;
        // MOEX, ASTS.
        // Value97
        // Value98
        // Value100

        static ExecRestatementReason()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 51;
            TagBytes[1] = 55;
            TagBytes[2] = 56;
        }
    }
}
