using System;

namespace ProSecuritiesTrading.PSTTrader.Core.Adapter
{
    public interface IAdapter
    {
        void Connect();
        void Disconnect();
        void Dispose();
    }
}
