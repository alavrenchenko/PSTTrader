using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class StipulationValue
    {
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public const int Tag = 6636;
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public static readonly byte[] TagBytes;
        public const byte Y = 89;
        public const byte N = 78;

        static StipulationValue()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 54;
            TagBytes[1] = 54;
            TagBytes[2] = 51;
            TagBytes[3] = 54;
        }
    }
}
