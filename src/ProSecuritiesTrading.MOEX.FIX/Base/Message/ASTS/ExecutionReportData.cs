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

using ProSecuritiesTrading.MOEX.FIX.Base.Group;
using ProSecuritiesTrading.MOEX.FIX.Base.Group.Data;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class ExecutionReportData
    {
        public byte[] MessageBytes;
        public HeaderData Header;

        public string OrderID = null;
        public byte[] OrderIDBytes = null;
        public string SecondaryClOrdID = null;
        public byte[] SecondaryClOrdIDBytes = null;
        public string ClOrdID = null;
        public byte[] ClOrdIDBytes = null;
        public string OrigClOrdID = null;
        public byte[] OrigClOrdIDBytes = null;

        public PartyData[] Parties = null;

        public string ExecID = null;
        public byte[] ExecIDBytes = null;
        public byte ExecType = 0;
        public byte OrdStatus = 0;
        public byte WorkingIndicator = 0;
        public int OrdRejReason = -1;
        public int ExecRestatementReason = -1;
        public string Account = null;
        public byte[] AccountBytes = null;
        public DateTime SettlDate = DateTime.MinValue;
        public DateTime OptionSettlDate = DateTime.MinValue;
        public string OptionSettlType = null;
        public byte[] OptionSettlTypeBytes = null;

        // <Instrument>
        public string Symbol = null;
        public byte[] SymbolBytes = null;
        public int Product = -1;
        public string CFICode = null;
        public byte[] CFICodeBytes = null;
        public string SecurityType = null;
        public byte[] SecurityTypeBytes = null;
        // </Instrument>

        public byte Side = 0;

        // <OrderQtyData>
        public int OrderQty = 0;
        public double CashOrderQty = 0.0;
        // </OrderQtyData>

        public byte OrdType = 0;
        public byte PriceType = 0;
        public double Price = 0.0;
        public byte TimeInForce = 0;
        public DateTime EffectiveTime = DateTime.MinValue;
        public byte TradeThruTime = 0;
        public byte OrderCapacity = 0;
        public byte OrderRestrictions = 0;
        public int LastQty = 0;
        public double LastPx = 0.0;
        public string TradingSessionID = null;
        public byte[] TradingSessionIDBytes = null;
        public string TradingSessionSubID = null;
        public byte[] TradingSessionSubIDBytes = null;
        public int LeavesQty = -1;
        public int CumQty = 0;
        public double AvgPx = 0.0;
        public DateTime TransactTime = DateTime.MinValue;
        public int OrigTime = -1;
        public string CurrencyCode = null;
        public byte[] CurrencyCodeBytes = null;
        public string InstitutionID = null;
        public byte[] InstitutionIDBytes = null;
        public byte StipulationValue = 0;
        public byte CoverID = 0;
        public string ClientAccID = null;
        public byte[] ClientAccIDBytes = null;
        public DateTime TradeIssueDate = DateTime.MinValue;
        public string OrigOrderID = null;
        public byte[] OrigOrderIDBytes = null;
        public string ParentID = null;
        public byte[] ParentIDBytes = null;

        public CommissionData CommissionData = null;

        // <YieldData>
        public double Yield = -1.0;
        // </YieldData>

        public double AccruedInterestAmt = -1.0;
        public string Text = null;

        public MiscFeeData[] MiscFees = null;

        public TrdRegTimestampData[] TrdRegTimestamps = null;

        public int MaxPriceLevels = -1;
        public string MDEntryID = null;
        public byte[] MDEntryIDBytes = null;

        // <Trailer>
        public int CheckSum = -1;
        // </Trailer>

        public ExecutionReportData(byte[] buffer, HeaderData header)
        {
            this.MessageBytes = buffer;
            this.Header = header;
        }
    }
}
