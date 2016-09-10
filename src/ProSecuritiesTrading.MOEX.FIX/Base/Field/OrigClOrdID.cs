using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class OrigClOrdID
    {
        public const int Tag = 41;
        public static readonly byte[] TagBytes;

        static OrigClOrdID()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 52;
            TagBytes[1] = 49;
        }
    }
}
