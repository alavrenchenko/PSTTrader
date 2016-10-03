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
    public class TradeCaptureReportData
    {
        public byte[] MessageBytes;
        public HeaderData Header;

        public string TradeReportID = null;
        public byte[] TradeReportIDBytes = null;
        /// <summary>
        /// Null: -1.
        /// </summary>
        public int TrdType = -1;
        /// <summary>
        /// Null: -1.
        /// </summary>
        public int TrdSubType = -1;
        public string SecondaryTradeReportID = null;
        public byte[] SecondaryTradeReportIDBytes = null;
        /// <summary>
        /// Null: 0.
        /// </summary>
        public byte ExecType = 0;
        public string ExecID = null;
        public byte[] ExecIDBytes = null;
        /// <summary>
        /// Null: 0.
        /// </summary>
        public byte PreviouslyReported = 0;

        // <Instrument>
        public string Symbol = null;
        public byte[] SymbolBytes = null;
        /// <summary>
        /// Null: -1.
        /// </summary>
        public int Product = -1;
        public string CFICode = null;
        public byte[] CFICodeBytes = null;
        public string SecurityType = null;
        public byte[] SecurityTypeBytes = null;
        // </Instrument>

        public FinancingDetails FinancingDetails = null;

        // <YieldData>
        /// <summary>
        /// Null: -1.0.
        /// </summary>
        public double Yield = -1.0;
        // </YieldData>

        public UnderlyingInstrumentData[] UnderlyingInstruments = null;

        public int LastQty = 0;
        public double LastPx = 0.0;
        /// <summary>
        /// Null: -1.0.
        /// </summary>
        public double CalculatedCcyLastQty = -1.0;
        /// <summary>
        /// Null: DateTime.MinValue.
        /// </summary>
        public DateTime TradeDate = DateTime.MinValue;
        /// <summary>
        /// Null: DateTime.MinValue.
        /// </summary>
        public DateTime TransactTime = DateTime.MinValue;
        public string SettlType = null;
        public byte[] SettlTypeBytes = null;
        /// <summary>
        /// Null: DateTime.MinValue.
        /// </summary>
        public DateTime SettlDate = DateTime.MinValue;
        public string OptionSettlType = null;
        public byte[] OptionSettlTypeBytes = null;

        public SideData[] Sides = null;

        /// <summary>
        /// Null: -1.0.
        /// </summary>
        public double Price2 = -1.0;
        /// <summary>
        /// Null: -1.0.
        /// </summary>
        public double Price = -1.0;
        /// <summary>
        /// Null: 0.
        /// </summary>
        public byte PriceType = 0;
        public string InstitutionID = null;
        public byte[] InstitutionIDBytes = null;
        /// <summary>
        /// Null: 0.
        /// </summary>
        public byte TradeThruTime = 0;
        public string ClearingStatus = null;
        public byte[] ClearingStatusBytes = null;
        public string ClearingRefNo = null;
        public byte[] ClearingRefNoBytes = null;
        public string CurrencyCode = null;
        public byte[] CurrencyCodeBytes = null;
        public string ClientAccID = null;
        public byte[] ClientAccIDBytes = null;
        public string ParentID = null;
        public byte[] ParentIDBytes = null;
        public string ClearingHandlingType = null;
        public byte[] ClearingHandlingTypeBytes = null;
        /// <summary>
        /// Null: -1.
        /// </summary>
        public int OperationType = -1;
        /// <summary>
        /// Null: -1.0.
        /// </summary>
        public double Price1 = -1.0;
        /// <summary>
        /// Null: double.MinValue.
        /// </summary>
        public double CurrentRepoValue = double.MinValue;
        /// <summary>
        /// Null: double.MinValue.
        /// </summary>
        public double CurrentRepoBackValue = double.MinValue;
        /// <summary>
        /// Null: -1.
        /// </summary>
        public int TradeBalance = -1;
        /// <summary>
        /// Null: double.MinValue.
        /// </summary>
        public double TotalAmount = double.MinValue;
        /// <summary>
        /// Null: double.MinValue.
        /// </summary>
        public double LastCouponPaymentValue = double.MinValue;
        /// <summary>
        /// Null: DateTime.MinValue.
        /// </summary>
        public DateTime LastCouponPaymentDate = DateTime.MinValue;
        /// <summary>
        /// Null: double.MinValue.
        /// </summary>
        public double LastPrincipalPaymentValue = double.MinValue;
        /// <summary>
        /// Null: DateTime.MinValue.
        /// </summary>
        public DateTime LastPrincipalPaymentDate = DateTime.MinValue;
        /// <summary>
        /// Null: double.MinValue.
        /// </summary>
        public double ExpectedDiscount = double.MinValue;
        /// <summary>
        /// Null: -1.
        /// </summary>
        public int ExpectedQty = -1;
        /// <summary>
        /// Null: double.MinValue.
        /// </summary>
        public double ExpectedRepoValue = double.MinValue;
        /// <summary>
        /// Null: double.MinValue.
        /// </summary>
        public double ExpectedRepoBackValue = double.MinValue;
        /// <summary>
        /// Null: double.MinValue.
        /// </summary>
        public double ExpectedReturnValue = double.MinValue;
        /// <summary>
        /// Null: -1.
        /// </summary>
        public int PreciseBalance = -1;

        // <Trailer>
        public int CheckSum = -1;
        // </Trailer>

        /// <summary>
        /// NoError = 0, Sides = 1.
        /// </summary>
        public byte Error = 0;

        public TradeCaptureReportData(byte[] buffer, HeaderData header)
        {
            this.MessageBytes = buffer;
            this.Header = header;
        }
    }
}
