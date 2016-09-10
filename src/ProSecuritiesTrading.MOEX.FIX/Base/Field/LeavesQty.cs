using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class LeavesQty
    {
        public const int Tag = 151;
        public static readonly byte[] TagBytes;

        static LeavesQty()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 53;
            TagBytes[2] = 49;
        }
    }
}
