using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class TimeInForce
    {
        public const int Tag = 59;
        public static readonly byte[] TagBytes;
        public const byte Value0 = 48;
        public const byte Value3 = 51;
        public const byte Value4 = 52;

        static TimeInForce()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 53;
            TagBytes[1] = 57;
        }
    }
}
