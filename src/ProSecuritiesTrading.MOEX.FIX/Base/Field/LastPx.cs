using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class LastPx
    {
        public const int Tag = 31;
        public static readonly byte[] TagBytes;

        static LastPx()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 51;
            TagBytes[1] = 49;
        }
    }
}
