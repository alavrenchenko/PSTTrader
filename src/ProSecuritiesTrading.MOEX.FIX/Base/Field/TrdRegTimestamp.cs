using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class TrdRegTimestamp
    {
        public const int Tag = 769;
        public static readonly byte[] TagBytes;

        static TrdRegTimestamp()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 55;
            TagBytes[1] = 54;
            TagBytes[2] = 57;
        }
    }
}
