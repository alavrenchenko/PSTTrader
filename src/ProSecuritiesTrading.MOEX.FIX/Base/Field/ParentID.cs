using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class ParentID
    {
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public const int Tag = 9580;
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public static readonly byte[] TagBytes;

        static ParentID()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 57;
            TagBytes[1] = 53;
            TagBytes[2] = 56;
            TagBytes[3] = 48;
        }
    }
}
