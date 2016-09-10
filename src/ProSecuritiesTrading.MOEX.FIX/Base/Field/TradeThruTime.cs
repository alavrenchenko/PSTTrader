using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class TradeThruTime
    {
        public const int Tag = 5202;
        public static readonly byte[] TagBytes;
        public const byte C = 67;

        static TradeThruTime()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 53;
            TagBytes[1] = 50;
            TagBytes[2] = 48;
            TagBytes[2] = 50;
        }
    }
}
