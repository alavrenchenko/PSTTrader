using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class MaxFloor
    {
        public const int Tag = 111;
        public static readonly byte[] TagBytes;

        static MaxFloor()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 49;
            TagBytes[2] = 49;
        }
    }
}
