using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class AvgPx
    {
        public const int Tag = 6;
        public static readonly byte[] TagBytes;

        static AvgPx()
        {
            TagBytes = new byte[1];
            TagBytes[0] = 54;
        }
    }
}
