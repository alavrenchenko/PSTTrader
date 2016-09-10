using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class SettlDate
    {
        public const int Tag = 64;
        public static readonly byte[] TagBytes;

        static SettlDate()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 54;
            TagBytes[1] = 52;
        }
    }
}
