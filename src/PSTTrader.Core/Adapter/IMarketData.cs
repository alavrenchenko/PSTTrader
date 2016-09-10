using System;

using ProSecuritiesTrading.PSTTrader.Core.Base;

namespace ProSecuritiesTrading.PSTTrader.Core.Adapter
{
    public interface IMarketData
    {
        void SubscribeMarketData(ProSecuritiesTrading.PSTTrader.Core.Base.Instrument instrument);
        void UnsubscribeMarketData(ProSecuritiesTrading.PSTTrader.Core.Base.Instrument instrument);
    }
}
