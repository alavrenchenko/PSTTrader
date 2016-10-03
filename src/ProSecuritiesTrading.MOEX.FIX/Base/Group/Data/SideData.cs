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

namespace ProSecuritiesTrading.MOEX.FIX.Base.Group.Data
{
    /// <summary>
    /// Group Sides.
    /// </summary>
    public class SideData
    {
        public byte Side = 0;
        public string OrderID = null;
        public byte[] OrderIDBytes = null;
        public string ClOrdID = null;
        public byte[] ClOrdIDBytes = null;
        public string SecondaryClOrdID = null;
        public byte[] SecondaryClOrdIDBytes = null;

        public PartyData[] Parties = null;

        public string Account = null;
        public byte[] AccountBytes = null;
        public string TradingSessionID = null;
        public byte[] TradingSessionIDBytes = null;
        public string TradingSessionSubID = null;
        public byte[] TradingSessionSubIDBytes = null;

        public CommissionData CommissionData = null;

        /// <summary>
        /// Null: -1.0.
        /// </summary>
        public double GrossTradeAmt = -1.0;
        /// <summary>
        /// Null: -1.0.
        /// </summary>
        public double AccruedInterestAmt = -1.0;
        /// <summary>
        /// Null: -1.0.
        /// </summary>
        public double EndAccruedInterestAmt = -1.0;
        /// <summary>
        /// Null: -1.0.
        /// </summary>
        public double StartCash = -1.0;
        /// <summary>
        /// Null: -1.0.
        /// </summary>
        public double EndCash = -1.0;

        public MiscFeeData[] MiscFees = null;

        public string SettlInstID = null;
        public byte[] SettlInstIDBytes = null;
    }
}
