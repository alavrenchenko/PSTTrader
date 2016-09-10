using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class Account
    {
        public const int Tag = 1;
        public static readonly byte[] TagBytes;

        static Account()
        {
            TagBytes = new byte[1];
            TagBytes[0] = 49;
        }
    }
}
