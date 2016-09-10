using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class Symbol
    {
        public const int Tag = 55;
        public static readonly byte[] TagBytes;

        static Symbol()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 53;
            TagBytes[1] = 53;
        }
    }
}
