using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class ClientAccID
    {
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public const int Tag = 7693;
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public static readonly byte[] TagBytes;

        static ClientAccID()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 55;
            TagBytes[1] = 54;
            TagBytes[2] = 57;
            TagBytes[3] = 51;
        }
    }
}
