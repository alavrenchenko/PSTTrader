using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class OrigTime
    {
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public const int Tag = 9412;
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public static readonly byte[] TagBytes;

        static OrigTime()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 57;
            TagBytes[1] = 52;
            TagBytes[2] = 49;
            TagBytes[3] = 50;
        }
    }
}
