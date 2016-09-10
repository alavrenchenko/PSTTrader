using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class MDEntryID
    {
        public const int Tag = 278;
        public static readonly byte[] TagBytes;

        static MDEntryID()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 50;
            TagBytes[1] = 55;
            TagBytes[2] = 56;
        }
    }
}
