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

using ProSecuritiesTrading.PSTTrader.Core.Base;

namespace ProSecuritiesTrading.PSTTrader.Core.Data
{
    public class MarketDepthEventArgs : EventArgs
    {
        private MarketDataType marketDataType;
        private MarketDepth marketDepth;
        private Operation operation;
        private int position;
        private double price;
        private DateTime time;
        private long volume;
        private ErrorCode errorCode;

        public MarketDepthEventArgs(MarketDepth marketDepth, ErrorCode errorCode, int position, Operation operation, MarketDataType marketDataType, double price, long volume, DateTime time)
        {
            this.marketDepth = marketDepth;
            this.errorCode = errorCode;
            this.position = position;
            this.operation = operation;
            this.marketDataType = marketDataType;
            this.price = price;
            this.volume = volume;
            this.time = time;
        }

        public override string ToString()
        {
            return "Instrument='" + marketDepth.Instrument.FullName + " " + marketDepth.Instrument.Exchange.ToString() + "' Type=" + marketDataType.ToString() + " Operation=" + operation.ToString() + " Position=" + position.ToString() + " Price=" + price.ToString() + " Volume=" + volume.ToString() + " Time=" + time.ToString();
        }

        public MarketDataType MarketDataType
        {

            get
            {
                return marketDataType;
            }
        }

        public MarketDepth MarketDepth
        {

            get
            {
                return marketDepth;
            }
        }

        public Operation Operation
        {

            get
            {
                return operation;
            }

            set
            {
                operation = value;
            }
        }

        public int Position
        {

            get
            {
                return position;
            }
        }

        public double Price
        {

            get
            {
                return price;
            }
        }

        public DateTime Time
        {

            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        public long Volume
        {

            get
            {
                return volume;
            }
        }
    }
}
