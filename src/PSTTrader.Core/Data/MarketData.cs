/*
   Copyright (C) 2016 Alexey Lavrenchenko (http://prosecuritiestrading.com/)

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProSecuritiesTrading.PSTTrader.Core.Base;

namespace ProSecuritiesTrading.PSTTrader.Core.Data
{
    public class MarketData
    {
        private ProSecuritiesTrading.PSTTrader.Core.Base.Connection connection;
        private ProSecuritiesTrading.PSTTrader.Core.Base.Instrument instrument;
        internal MarketDataItemEventHandler marketDataItemEventHandler;

        public event MarketDataItemEventHandler MarketDataItem
        {
            add
            {
                marketDataItemEventHandler += value;
            }
            remove
            {
                marketDataItemEventHandler -= value;

                if (marketDataItemEventHandler == null)
                {
                    this.connection.MarketDataCollection.RemoveAll(removeMarketData => removeMarketData.Instrument == this.instrument);
                    this.connection.AdapterMarketData.UnsubscribeMarketData(this.instrument);
                }
            }
        }

        protected virtual void OnMarketDataItem(MarketDataEventArgs e)
        {
            MarketDataItemEventHandler handler = marketDataItemEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public MarketData(ProSecuritiesTrading.PSTTrader.Core.Base.Connection connection, ProSecuritiesTrading.PSTTrader.Core.Base.Instrument instrument)
        {
            this.connection = connection;
            this.instrument = instrument;

            this.connection.MarketDataCollection.Add(this);
            this.connection.AdapterMarketData.SubscribeMarketData(this.instrument);
        }

        public void ProcessingMarketData(MarketDataEventArgs e, bool initialize)
        {

        }

        public ProSecuritiesTrading.PSTTrader.Core.Base.Connection Connection
        {
            get
            {
                return this.connection;
            }
        }

        public ProSecuritiesTrading.PSTTrader.Core.Base.Instrument Instrument
        {
            get
            {
                return this.instrument;
            }
        }
    }
}
