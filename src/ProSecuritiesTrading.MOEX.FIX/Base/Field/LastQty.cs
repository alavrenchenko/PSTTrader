using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class LastQty
    {
        public const int Tag = 32;
        public static readonly byte[] TagBytes;

        static LastQty()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 51;
            TagBytes[1] = 50;
        }
    }
}
