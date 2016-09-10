using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class ExecID
    {
        public const int Tag = 17;
        public static readonly byte[] TagBytes;

        static ExecID()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 49;
            TagBytes[1] = 55;
        }
    }
}
