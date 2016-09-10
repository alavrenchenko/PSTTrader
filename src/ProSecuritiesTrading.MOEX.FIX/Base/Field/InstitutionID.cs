using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class InstitutionID
    {
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public const int Tag = 5155;
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public static readonly byte[] TagBytes;

        static InstitutionID()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 53;
            TagBytes[1] = 49;
            TagBytes[2] = 53;
            TagBytes[3] = 53;
        }
    }
}
