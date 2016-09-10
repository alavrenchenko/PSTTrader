using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class TradingSessionID
    {
        public const int Tag = 336;
        public static readonly byte[] TagBytes;

        static TradingSessionID()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 51;
            TagBytes[1] = 51;
            TagBytes[2] = 54;
        }
    }
}
