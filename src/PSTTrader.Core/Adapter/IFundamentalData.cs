using System;

using ProSecuritiesTrading.PSTTrader.Core.Base;

namespace ProSecuritiesTrading.PSTTrader.Core.Adapter
{
    public interface IFundamentalData
    {
        void SubscribeFundamentalData(ProSecuritiesTrading.PSTTrader.Core.Base.Instrument instrument);
        void UnsubscribeFundamentalData(ProSecuritiesTrading.PSTTrader.Core.Base.Instrument instrument);
    }
}
