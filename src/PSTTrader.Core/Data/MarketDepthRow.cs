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

namespace ProSecuritiesTrading.PSTTrader.Core.Data
{
    public class MarketDepthRow
    {
        private double price;
        private DateTime time;
        private long volume;

        internal MarketDepthRow(double price, long volume, DateTime time)
        {
            this.price = price;
            this.volume = volume;
            this.time = time;
        }

        public override string ToString()
        {
            return "Price=" + price.ToString() + " Volume=" + volume.ToString() + " Time=" + time.ToString();
        }

        public double Price
        {

            get
            {
                return price;
            }

            set
            {
                price = value;
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

            set
            {
                volume = value;
            }
        }
    }
}
