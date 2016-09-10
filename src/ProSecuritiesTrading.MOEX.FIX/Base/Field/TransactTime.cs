using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class TransactTime
    {
        public const int Tag = 60;
        public static readonly byte[] TagBytes;

        static TransactTime()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 54;
            TagBytes[1] = 48;
        }
    }
}
