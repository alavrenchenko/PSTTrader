using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class OrigOrderID
    {
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public const int Tag = 9945;
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public static readonly byte[] TagBytes;

        static OrigOrderID()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 57;
            TagBytes[1] = 57;
            TagBytes[2] = 52;
            TagBytes[3] = 53;
        }
    }
}
