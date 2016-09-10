using System;

using ProSecuritiesTrading.PSTTrader.Core.Base;

namespace ProSecuritiesTrading.PSTTrader.Core.Adapter
{
    public interface IMarketDepth
    {
        void SubscribeMarketDepth(ProSecuritiesTrading.PSTTrader.Core.Base.Instrument instrument);
        void UnsubscribeMarketDepth(ProSecuritiesTrading.PSTTrader.Core.Base.Instrument instrument);
    }
}
