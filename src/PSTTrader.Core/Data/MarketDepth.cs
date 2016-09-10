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
    public class MarketDepth
    {
        private ProSecuritiesTrading.PSTTrader.Core.Base.Connection connection;
        private ProSecuritiesTrading.PSTTrader.Core.Base.Instrument instrument;
        private List<MarketDepthRow> ask;
        private List<MarketDepthRow> bid;
        internal MarketDepthItemEventHandler marketDepthItemHandler;

        public event MarketDepthItemEventHandler MarketDepthItem
        {
            add
            {
                marketDepthItemHandler += value;

                if (marketDepthItemHandler.GetInvocationList().Count() > 1)
                {
                    for (int x = 0; x < ask.Count; x++)
                        value(this, new MarketDepthEventArgs(this, ErrorCode.NoError, x, Operation.Insert, MarketDataType.Ask, ask[x].Price, ask[x].Volume, ask[x].Time));

                    for (int x = 0; x < bid.Count; x++)
                        value(this, new MarketDepthEventArgs(this, ErrorCode.NoError, x, Operation.Insert, MarketDataType.Bid, bid[x].Price, bid[x].Volume, bid[x].Time));
                }
            }
            remove
            {
                marketDepthItemHandler -= value;

                if (marketDepthItemHandler == null)
                {
                    this.connection.MarketDepthCollection.RemoveAll(removeMarketDepth => removeMarketDepth.Instrument == this.instrument);
                    this.connection.AdapterMarketDepth.UnsubscribeMarketDepth(this.instrument);
                }
            }
        }

        protected void OnMarketDepthItem(MarketDepthEventArgs e)
        {
            MarketDepthItemEventHandler handler = marketDepthItemHandler;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public MarketDepth(ProSecuritiesTrading.PSTTrader.Core.Base.Connection connection, ProSecuritiesTrading.PSTTrader.Core.Base.Instrument instrument)
        {
            this.connection = connection;
            this.instrument = instrument;

            this.ask = new List<MarketDepthRow>();
            this.bid = new List<MarketDepthRow>();

            this.connection.MarketDepthCollection.Add(this);
            this.connection.AdapterMarketDepth.SubscribeMarketDepth(this.instrument);
        }

        public void ProcessingMarketDepth(MarketDataType marketDataType, double price, long volume, DateTime time, bool initialize)
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
