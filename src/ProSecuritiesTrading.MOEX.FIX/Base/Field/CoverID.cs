using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class CoverID
    {
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public const int Tag = 7695;
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public static readonly byte[] TagBytes;
        public const byte Y = 89;
        public const byte N = 78;

        static CoverID()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 55;
            TagBytes[1] = 54;
            TagBytes[2] = 57;
            TagBytes[3] = 53;
        }
    }
}
