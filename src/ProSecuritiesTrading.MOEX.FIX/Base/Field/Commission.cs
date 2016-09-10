using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class Commission
    {
        public const int Tag = 12;
        public static readonly byte[] TagBytes;

        static Commission()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 49;
            TagBytes[1] = 50;
        }
    }
}
