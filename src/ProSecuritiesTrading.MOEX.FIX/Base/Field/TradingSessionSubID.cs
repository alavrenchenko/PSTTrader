using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class TradingSessionSubID
    {
        public const int Tag = 625;
        public static readonly byte[] TagBytes;

        static TradingSessionSubID()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 54;
            TagBytes[1] = 50;
            TagBytes[2] = 53;
        }
    }
}
