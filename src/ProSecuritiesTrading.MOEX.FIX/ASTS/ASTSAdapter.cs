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
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

using ProSecuritiesTrading.PSTTrader.Core.Base;
using ProSecuritiesTrading.PSTTrader.Core.Adapter;
using ProSecuritiesTrading.PSTTrader.Core.Output;
using ProSecuritiesTrading.MOEX.FIX.Client;
using ProSecuritiesTrading.MOEX.FIX.Base.Converter;
using ProSecuritiesTrading.MOEX.FIX.Base.Message;
using ProSecuritiesTrading.MOEX.FIX.Base.Group;
using ProSecuritiesTrading.MOEX.FIX.Base.Field;
using ProSecuritiesTrading.MOEX.FIX.Base.Service;
using ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS;

namespace ProSecuritiesTrading.MOEX.FIX.ASTS
{
    public class ASTSAdapter : IAdapter, IOrder
    {
        internal ProSecuritiesTrading.PSTTrader.Core.Base.Connection connection;
        private bool canConnect = true;
        private bool canDisconnect = false;
        internal ProSecuritiesTrading.MOEX.FIX.Base.Service.MFIXTransactional MFIXTransactional;
        private byte marketType;
        private bool disposed = false;
        private object onConnectedLock = new object();


        public ASTSAdapter(ProSecuritiesTrading.PSTTrader.Core.Base.Connection connection)
        {
            this.connection = connection;

            this.MFIXTransactional = new MFIXTransactional(this);
            this.marketType = ((ProSecuritiesTrading.MOEX.FIX.ASTS.ASTSSettings)this.connection.ConnectionSettings).MarketType;
        }

        public void ClientError(byte serviceId, ErrorCode errorCode, ConnectionStatus status)
        {
            lock (this.onConnectedLock)
            {
                if ((errorCode == ErrorCode.NotConnected) && (status == ConnectionStatus.Disconnected) && (this.MFIXTransactional.MFIXTrade.Status == ConnectionStatus.Disconnected) && (this.MFIXTransactional.MFIXTradeCapture.Status == ConnectionStatus.Disconnected) && (this.MFIXTransactional.MFIXDropCopy.Status == ConnectionStatus.Disconnected))
                {
                    if (this.canConnect == true)
                    {
                        return;
                    }

                    this.canConnect = true;
                    this.connection.ProcessConnectionStatus(new ConnectionStatusEventArgs(this.connection, ErrorCode.NotConnected, ConnectionStatus.Disconnected));
                }
            }
        }

        internal void OnConnected(bool connected)
        {
            lock (this.onConnectedLock)
            {
                if (connected == true)
                {
                    if (this.canDisconnect == true)
                    {
                        return;
                    }

                    if ((this.MFIXTransactional.MFIXTrade.Status != ConnectionStatus.Connected) && (this.MFIXTransactional.MFIXTradeCapture.Status != ConnectionStatus.Connected) && (this.MFIXTransactional.MFIXDropCopy.Status != ConnectionStatus.Connected))
                    {
                        return;
                    }

                    this.canDisconnect = true;
                    this.connection.ProcessConnectionStatus(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, ConnectionStatus.Connected));
                }
                else
                {
                    if (this.canConnect == true)
                    {
                        return;
                    }

                    if ((this.MFIXTransactional.MFIXTrade.Status != ConnectionStatus.Disconnected) || (this.MFIXTransactional.MFIXTradeCapture.Status != ConnectionStatus.Disconnected) || (this.MFIXTransactional.MFIXDropCopy.Status != ConnectionStatus.Disconnected))
                    {
                        return;
                    }

                    this.canConnect = true;
                    this.connection.ProcessConnectionStatus(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, ConnectionStatus.Disconnected));
                }
            }
        }

        public void Connect()
        {
            if ((this.canConnect == true) && (this.connection.Status == ConnectionStatus.Disconnected))
            {
                this.canConnect = false;
                this.connection.ProcessConnectionStatus(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, ConnectionStatus.Connecting));

                this.MFIXTransactional.MFIXTrade.ResetSeqNumFlag = ResetSeqNumFlag.Y;
                this.MFIXTransactional.MFIXTradeCapture.ResetSeqNumFlag = ResetSeqNumFlag.Y;
                this.MFIXTransactional.MFIXDropCopy.ResetSeqNumFlag = ResetSeqNumFlag.Y;

                this.MFIXTransactional.MFIXTrade.Connect();
                this.MFIXTransactional.MFIXTradeCapture.Connect();
                this.MFIXTransactional.MFIXDropCopy.Connect();

                //------------------------------------------------------------
                //for (int x = 0; x < 1000; x++)
                    //this.MFIXTransactional.MFIXTrade.NewOrderSingle();
                //this.MFIXTransactional.MFIXTrade.OrderMassCancelRequest();
                //------------------------------------------------------------
            }
        }

        public void Disconnect()
        {
            if ((this.canDisconnect == true) && (this.connection.Status != ConnectionStatus.Disconnected) && (this.connection.Status != ConnectionStatus.Disconnecting))
            {
                this.canDisconnect = false;
                this.connection.ProcessConnectionStatus(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, ConnectionStatus.Disconnecting));

                if (this.MFIXTransactional.TimerEnabled == true)
                {
                    this.MFIXTransactional.TimerEnabled = false;
                }

                this.MFIXTransactional.MFIXTrade.Disconnect();
                this.MFIXTransactional.MFIXTradeCapture.Disconnect();
                this.MFIXTransactional.MFIXDropCopy.Disconnect();
            }
        }
        
        public void Cancel(ProSecuritiesTrading.PSTTrader.Core.Base.Order order)
        {
        }

        public void Cancel(string origClOrdID, string orderID, byte orderSide)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            byte[] clOrdID = StringConverter.GetBytes(DateTime.UtcNow.Ticks.ToString());

            if ((origClOrdID != null) && (orderID != null))
            {
                origClOrdID = null;
            }

            byte[] origClOrdIDBytes = null;
            byte[] orderIDBytes = null;

            if (origClOrdID != null)
            {
                origClOrdIDBytes = StringConverter.GetBytes(origClOrdID);
            }
            else
            {
                orderIDBytes = StringConverter.GetBytes(orderID);
            }

            byte side = (orderSide == 0) ? Side.Value1 : Side.Value2;

            byte[] messageBytes = Base.Message.ASTS.OrderCancelRequest.UnsafeGetBytes(this.MFIXTransactional.MFIXTrade.Messages.Header, this.MFIXTransactional.MFIXTrade.Messages.NextClientMsgSeqNum, false, origClOrdIDBytes, orderIDBytes, clOrdID, side);
            
            stopwatch.Stop();

            this.MFIXTransactional.MFIXTrade.Messages.ClientMessages.Add(this.MFIXTransactional.MFIXTrade.Messages.CurrentClientMsgSeqNum, messageBytes);
            this.MFIXTransactional.MFIXTrade.Client.Send(messageBytes);

            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\nMFIXTrade, OrderCancelRequest:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Message: " + Encoding.UTF8.GetString(messageBytes) + "\n   Time: " + StringConverter.GetString(DateTimeConverter.GetBytes(DateTime.Now.Ticks)) + "\n"));
        }

        public void MassCancel(string account, string instrumentName, byte secboardType, byte orderSide)
        {
            if ((secboardType != 255) && (secboardType >= Base.Boards.BoardsBytes.Length))
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            byte[] clOrdID = StringConverter.GetBytes(DateTime.UtcNow.Ticks.ToString());
            byte[] secondaryClOrdID = null;
            byte massCancelRequestType = MassCancelRequestType.Value7;
            byte[] tradingSessionID = (secboardType < 255) ? Base.Boards.BoardsBytes[secboardType] : null;
            byte[] symbol = null;

            if (instrumentName != null)
            {
                massCancelRequestType = MassCancelRequestType.Value1;
                symbol = StringConverter.GetBytes(instrumentName);
            }

            byte product = 0;

            bool cfiCode = false;
            byte[] securityType = null;

            // Currency (FX) market
            if (this.marketType == 1)
            {
                cfiCode = true;
                securityType = SecurityType.SecurityTypeFXSPOTWithSOH;
            }

            byte side = 0;

            if (orderSide < 2)
            {
                side = (orderSide == 0) ? Side.Value1 : Side.Value2;
            }

            byte[] accountBytes = (account != null) ? StringConverter.GetBytes(account) : null;

            byte[] messageBytes = Base.Message.ASTS.OrderMassCancelRequest.UnsafeGetBytes(this.MFIXTransactional.MFIXTrade.Messages.Header, this.MFIXTransactional.MFIXTrade.Messages.NextClientMsgSeqNum, false, clOrdID, secondaryClOrdID, massCancelRequestType, tradingSessionID, symbol, product, cfiCode, securityType, side, accountBytes, this.MFIXTransactional.MFIXTrade.Messages.Parties);
            stopwatch.Stop();

            this.MFIXTransactional.MFIXTrade.Messages.ClientMessages.Add(this.MFIXTransactional.MFIXTrade.Messages.CurrentClientMsgSeqNum, messageBytes);
            this.MFIXTransactional.MFIXTrade.Client.Send(messageBytes);
            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\nMFIXTrade, OrderMassCancelRequest:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Message: " + Encoding.UTF8.GetString(messageBytes) + "\n   Time: " + StringConverter.GetString(DateTimeConverter.GetBytes(DateTime.Now.Ticks)) + "\n"));

        }

        public void Change(ProSecuritiesTrading.PSTTrader.Core.Base.Order order)
        {
        }

        //int ordID = 1;

        public void Submit(ProSecuritiesTrading.PSTTrader.Core.Base.Order order)
        {
            if (order.SECBOARDType >= Base.Boards.BoardsBytes.Length)
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            byte[] clOrdID = StringConverter.GetBytes(DateTime.UtcNow.Ticks.ToString());
            //byte[] clOrdID = StringConverter.GetBytes(ordID.ToString());
            //ordID++;
            byte[] account = StringConverter.GetBytes(order.Account);
            int maxFloor = -1;
            byte[] secondaryClOrdID = null;
            byte[] tradingSessionID = Base.Boards.BoardsBytes[order.SECBOARDType];
            byte[] symbol = StringConverter.GetBytes(order.InstrumentName);
            byte product = 0;

            bool cfiCode = false;
            byte[] securityType = null;

            // Currency (FX) market
            if (this.marketType == 1)
            {
                cfiCode = true;
                securityType = SecurityType.SecurityTypeFXSPOTWithSOH;
            }

            byte side = (order.OrderSide == OrderSide.Buy) ? Side.Value1 : Side.Value2;
            int orderQty = order.Quantity;
            double cashOrderQty = 0.0;
            byte ordType = OrdType.Value1;
            byte priceType = 0;
            double price = 0;

            if (order.OrderType == OrderType.Limit)
            {
                ordType = OrdType.Value2;
                price = order.Price;
            }

            byte tradeThruTime = 0;
            byte timeInForce = 255;
            long effectiveTime = 0;
            byte orderCapacity = 0;
            byte orderRestrictions = 0;
            byte maxPriceLevels = 0;

            stopwatch.Start();

            byte[] messageBytes = Base.Message.ASTS.NewOrderSingle.UnsafeGetBytes(this.MFIXTransactional.MFIXTrade.Messages.Header, this.MFIXTransactional.MFIXTrade.Messages.NextClientMsgSeqNum, false, clOrdID, this.MFIXTransactional.MFIXTrade.Messages.Parties, account, maxFloor, secondaryClOrdID, tradingSessionID, symbol, product, cfiCode, securityType, side, orderQty, cashOrderQty, ordType, priceType, price, tradeThruTime, timeInForce, effectiveTime, orderCapacity, orderRestrictions, maxPriceLevels);

            stopwatch.Stop();

            this.MFIXTransactional.MFIXTrade.Messages.ClientMessages.Add(this.MFIXTransactional.MFIXTrade.Messages.CurrentClientMsgSeqNum, messageBytes);
            this.MFIXTransactional.MFIXTrade.Client.Send(messageBytes);

            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\nMFIXTrade, NewOrderSingle:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Message: " + Encoding.UTF8.GetString(messageBytes) + "\n   Time: " + StringConverter.GetString(DateTimeConverter.GetBytes(DateTime.Now.Ticks)) + "\n"));
        }

        // Send data
        public void Send()
        {

        }

        ~ASTSAdapter()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.disposed = true;

                this.MFIXTransactional.Dispose();

                if (disposing)
                {

                }
            }
        }
    }
}
