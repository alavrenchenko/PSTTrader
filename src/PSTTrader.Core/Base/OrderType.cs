using System;

namespace ProSecuritiesTrading.PSTTrader.Core.Base
{
    public enum OrderType : byte
    {
        Limit = 0,
        Market = 1,
        Stop = 2,
        StopLimit = 3,
        Unknown = 0x63
    }
}
