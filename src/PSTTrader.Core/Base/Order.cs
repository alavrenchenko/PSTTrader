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

namespace ProSecuritiesTrading.PSTTrader.Core.Base
{
    public class Order
    {
        public string Account;
        //public Instrument instrument;
        public string InstrumentName;
        public byte SECBOARDType;
        public double Price;
        public int Quantity;
        public OrderState OrderState;
        public OrderType OrderType;
        public OrderSide OrderSide;
        
        public Order(string account, string instrumentName, byte secboardType, double price, int quantity, OrderState orderState, OrderType orderType, OrderSide orderSide)
        {
            this.Account = account;
            this.InstrumentName = instrumentName;
            this.SECBOARDType = secboardType;
            this.Price = price;
            this.Quantity = quantity;
            this.OrderState = orderState;
            this.OrderType = orderType;
            this.OrderSide = orderSide;
        }
    }
}
