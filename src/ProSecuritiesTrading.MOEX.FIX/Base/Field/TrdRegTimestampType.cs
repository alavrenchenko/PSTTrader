using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class TrdRegTimestampType
    {
        public const int Tag = 770;
        public static readonly byte[] TagBytes;
        /// <summary>
        /// Time In = 2.
        /// </summary>
        public const byte Value2 = 50;

        static TrdRegTimestampType()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 55;
            TagBytes[1] = 55;
            TagBytes[2] = 48;
        }
    }
}
