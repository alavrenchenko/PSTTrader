using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class ClOrdID
    {
        public const int Tag = 11;
        public static readonly byte[] TagBytes;

        static ClOrdID()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 49;
            TagBytes[1] = 49;
        }
    }
}
