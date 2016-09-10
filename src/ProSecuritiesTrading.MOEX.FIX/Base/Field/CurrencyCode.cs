using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class CurrencyCode
    {
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public const int Tag = 6029;
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public static readonly byte[] TagBytes;

        static CurrencyCode()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 54;
            TagBytes[1] = 48;
            TagBytes[2] = 50;
            TagBytes[3] = 57;
        }
    }
}
