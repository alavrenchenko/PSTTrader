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

namespace ProSecuritiesTrading.PSTTrader.Core.Base
{
    public class Instrument
    {
        private Currency currency;
        private Exchange exchange;
        private DateTime expiry;
        private string id;
        // Instrument settings, parameters
        private string name;
        private string description;
        private InstrumentType instrumentType;
        private OptionType optionType;
        private double strikePrice;
        private double tickSize;

        public Instrument()
        {
            this.currency = ProSecuritiesTrading.PSTTrader.Core.Base.Currency.RussiaRuble;
            this.exchange = ProSecuritiesTrading.PSTTrader.Core.Base.Exchange.Default;
            this.expiry = DateTime.MaxValue;
            this.id = Guid.NewGuid().ToString("N");
            this.name = string.Empty;
            this.description = string.Empty;
            this.instrumentType = ProSecuritiesTrading.PSTTrader.Core.Base.InstrumentType.Unknown;
            this.optionType = ProSecuritiesTrading.PSTTrader.Core.Base.OptionType.Unknown;
            this.strikePrice = 0.0;
            this.tickSize = 0.01;
        }

        public Exchange Exchange
        {
            get
            {
                return exchange;
            }
        }

        public DateTime Expiry
        {
            get
            {
                return expiry;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }
        }

        public string FullName
        {
            get
            {
                return (this.name + ((this.expiry != DateTime.MaxValue) ? (" " + this.expiry.ToString("MM-yy")) : ""));
            }
        }

        public override string ToString()
        {
            return (this.FullName + " " + this.exchange.ToString());
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
    }
}
