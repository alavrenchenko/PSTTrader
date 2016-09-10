using System;

using ProSecuritiesTrading.PSTTrader.Core.Base;

namespace ProSecuritiesTrading.PSTTrader.Core.Adapter
{
    public interface IOrder
    {
        void Cancel(Order order);
        void Change(Order order);
        /// <param name="secboardType">Null: 255.</param>
        /// <param name="orderSide">Null: 2.</param>
        void MassCancel(string account, string instrumentName, byte secboardType, byte orderSide);
        void Submit(Order order);
    }
}
