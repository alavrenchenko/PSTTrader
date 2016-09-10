using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class NoTrdRegTimestamps
    {
        public const int Tag = 768;
        public static readonly byte[] TagBytes;

        static NoTrdRegTimestamps()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 55;
            TagBytes[1] = 54;
            TagBytes[2] = 56;
        }
    }
}
