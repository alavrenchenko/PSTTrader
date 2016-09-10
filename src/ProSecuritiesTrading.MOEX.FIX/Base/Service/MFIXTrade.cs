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
using ProSecuritiesTrading.PSTTrader.Core.Output;
using ProSecuritiesTrading.MOEX.FIX.Base.Converter;
using ProSecuritiesTrading.MOEX.FIX.Base.Message;
using ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Service
{
    public class MFIXTrade : ServiceBase
    {
        public MFIXTrade(ProSecuritiesTrading.MOEX.FIX.ASTS.ASTSAdapter adapter)
            : base(adapter, 1, "MFIXTrade")
        {
            base.Session = new Session(this, 60);
            ProSecuritiesTrading.MOEX.FIX.ASTS.ASTSSettings settings = (ProSecuritiesTrading.MOEX.FIX.ASTS.ASTSSettings)adapter.connection.ConnectionSettings;
            base.Messages = new Message.Messages(new Group.Header(Field.BeginString.BeginStringBytes, settings.SenderCompID[0], settings.TargetCompID[0], 0, 0));

            base.Ip = settings.IpArray[0];
            base.Port = settings.PortArray[0];
        }

        public void NewOrderSingle()
        {
            
            byte[] clOrdID = StringConverter.GetBytes(Guid.NewGuid().ToString("N"));
            Group.Parties parties = null;
            byte[] account = StringConverter.GetBytes("account");
            int maxFloor = -1;
            byte[] secondaryClOrdID = null;
            byte[] tradingSessionID = new byte[] { 49, 50, 51, 52 };
            byte[] symbol = new byte[] { 49, 50, 51, 52 };
            byte product = 0;
            bool cfiCode = false;
            byte[] securityType = null;
            byte side = Field.Side.Value1;
            int orderQty = 10;
            double cashOrderQty = 0.0;
            byte ordType = Field.OrdType.Value2;
            byte priceType = 0;
            double price = 115.25;
            byte tradeThruTime = 0;
            byte timeInForce = 255;
            long effectiveTime = 0;
            byte orderCapacity = 0;
            byte orderRestrictions = 0;
            byte maxPriceLevels = 0;
            
            /*
            byte[] clOrdID = StringConverter.GetBytes(Guid.NewGuid().ToString("N"));
            Group.Parties parties = null;
            byte[] account = StringConverter.GetBytes("account");
            int maxFloor = 100;
            byte[] secondaryClOrdID = null;
            byte[] tradingSessionID = new byte[] { 49, 50, 51, 52 };
            byte[] symbol = new byte[] { 49, 50, 51, 52 };
            byte product = Field.Product.Value4;
            bool cfiCode = true;
            byte[] securityType = Field.SecurityType.SecurityTypeFXSPOTWithSOH;
            byte side = Field.Side.Value1;
            int orderQty = 1000;
            double cashOrderQty = 0.0;
            byte ordType = Field.OrdType.Value2;
            byte priceType = Field.PriceType.Value2;
            double price = 115.25;
            byte tradeThruTime = 0;
            byte timeInForce = Field.TimeInForce.Value0;
            long effectiveTime = DateTime.UtcNow.Ticks;
            byte orderCapacity = Field.OrderCapacity.P;
            byte orderRestrictions = Field.OrderRestrictions.Value5;
            byte maxPriceLevels = Field.MaxPriceLevels.Value1;
            */

            //int length = 0;
            //DateTime startTime = DateTime.Now;

            //for (int x = 0; x < 1000; x++)
            //{
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                byte[] messageBytes = Base.Message.ASTS.NewOrderSingle.UnsafeGetBytes(this.Messages.Header, this.Messages.NextClientMsgSeqNum, false, clOrdID, parties, account, maxFloor, secondaryClOrdID, tradingSessionID, symbol, product, cfiCode, securityType, side, orderQty, cashOrderQty, ordType, priceType, price, tradeThruTime, timeInForce, effectiveTime, orderCapacity, orderRestrictions, maxPriceLevels);

                //byte[] messageBytes = Base.Message.ASTS.NewOrderSingle.UnsafeGetBytes(this.Messages.Header, 1000, false, clOrdID, parties, account, maxFloor, secondaryClOrdID, tradingSessionID, symbol, product, cfiCode, securityType, side, orderQty, cashOrderQty, ordType, priceType, price, tradeThruTime, timeInForce, effectiveTime, orderCapacity, orderRestrictions, maxPriceLevels);

                //length = messageBytes.Length;
                stopwatch.Stop();

                this.Messages.ClientMessages.Add(this.Messages.CurrentClientMsgSeqNum, messageBytes);

                this.Client.Send(messageBytes);

                OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\nMFIXTrade, NewOrderSingle:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Message: " + Encoding.UTF8.GetString(messageBytes) + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
                //OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\nMFIXTrade, NewOrderSingle:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
                //OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\nMFIXTrade, NewOrderSingle:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Time: " + StringConverter.GetString(DateTimeConverter.GetBytes(DateTime.Now.Ticks)) + "\n"));
            //}

            //DateTime endTime = DateTime.Now;
            //OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\nMFIXTrade, NewOrderSingle:\n   Bytes:\n      Length: " + length.ToString() + "\n   StartTime: " + StringConverter.GetString(DateTimeConverter.GetBytes(startTime.Ticks)) + "\n   EndTime: " + StringConverter.GetString(DateTimeConverter.GetBytes(endTime.Ticks)) + "\n   Elapsed time, ticks: " + (endTime.Ticks - startTime.Ticks).ToString() + "\n"));

        }

        public void OrderMassCancelRequest()
        {
            
            byte[] clOrdID = StringConverter.GetBytes(Guid.NewGuid().ToString("N"));
            byte[] secondaryClOrdID = null;
            byte massCancelRequestType = Field.MassCancelRequestType.Value7;
            byte[] tradingSessionID = null;
            byte[] symbol = null;
            byte product = 0;
            bool cfiCode = false;
            byte[] securityType = null;
            byte side = 0;
            byte[] account = null;
            Group.Parties parties = null;
            
            /*
            byte[] clOrdID = StringConverter.GetBytes(Guid.NewGuid().ToString("N"));
            byte[] secondaryClOrdID = null;
            byte massCancelRequestType = Field.MassCancelRequestType.Value1;
            byte[] tradingSessionID = new byte[] { 49, 50, 51, 52 };
            byte[] symbol = new byte[] { 49, 50, 51, 52 };
            byte product = Field.Product.Value4;
            bool cfiCode = true;
            byte[] securityType = Field.SecurityType.SecurityTypeFXSPOTWithSOH;
            byte side = Field.Side.Value1;
            byte[] account = StringConverter.GetBytes("account");
            Group.Parties parties = null;
            */
            //DateTime startTime = DateTime.Now;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            byte[] messageBytes = Base.Message.ASTS.OrderMassCancelRequest.UnsafeGetBytes(this.Messages.Header, this.Messages.NextClientMsgSeqNum, false, clOrdID, secondaryClOrdID, massCancelRequestType, tradingSessionID, symbol, product, cfiCode, securityType, side, account, parties);
            
            stopwatch.Stop();
            //DateTime endTime = DateTime.Now;

            this.Messages.ClientMessages.Add(this.Messages.CurrentClientMsgSeqNum, messageBytes);
            this.Client.Send(messageBytes);

            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\nMFIXTrade, OrderMassCancelRequest:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Message: " + Encoding.UTF8.GetString(messageBytes) + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
            
            //OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\nMFIXTrade, OrderMassCancelRequest:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n   StartTime: " + StringConverter.GetString(DateTimeConverter.GetBytes(startTime.Ticks)) + "\n   EndTime: " + StringConverter.GetString(DateTimeConverter.GetBytes(endTime.Ticks)) + "\n   Elapsed time, ticks: " + (endTime.Ticks - startTime.Ticks).ToString() + "\n   Message: " + Encoding.UTF8.GetString(messageBytes) + "\n"));

        }

        internal override unsafe void ProcessingReceivedMessage(byte* pBytes)
        {
            /*
             * Сообщения сессионного уровня
             * A (Logon) = 65
             * 5 (Logout) = 53
             * 0 (Heartbeat) = 48
             * 1 (Test Request) = 49
             * 2 (Resend Request) = 50
             * 4 (Sequence Reset) = 52
             * 3 (Reject) = 51
             * 
             * Сервис MFIX Trade
             * h (Trading Session Status) = 104
             * 8 (Execution Report) = 56
             * r (Order Mass Cancel Report) = 114
             * 9 (Order Cancel Reject) = 57
             * 
             * */

            if ((*pBytes == 51) && (*(++pBytes) == 53) && (*(++pBytes) == 61)) // 35= (MsgType)
            {
                pBytes++;

                if (*(pBytes + 1) != 1)
                {
                    // Error
                    return;
                }

                switch (*pBytes)
                {
                    case 56: // 8 (Execution Report)
                        {

                            break;
                        }
                    case 57: // 9 (Order Cancel Reject)
                        {

                            break;
                        }
                    case 114: // r (Order Mass Cancel Report)
                        {

                            break;
                        }
                    case 104: // h (Trading Session Status)
                        {

                            break;
                        }
                    case 48: // 0 (Heartbeat)
                        {

                            break;
                        }
                    case 49: // 1 (Test Request)
                        {

                            break;
                        }
                    case 50: // 2 (Resend Request)
                        {

                            break;
                        }
                    case 52: // 4 (Sequence Reset)
                        {

                            break;
                        }
                    case 51: // 3 (Reject)
                        {

                            break;
                        }
                    case 65: // A (Logon)
                        {
                            LogonData logonData = Base.Message.ASTS.Logon.GetLogonData(pBytes);

                            base.Session.ExpectedMessageLogon = false;
                            base.Status = ConnectionStatus.Connected;
                            base.Adapter.OnConnected(true);

                            // Проверка порядковых номеров (MsgSeqNum (34))

                            break;
                        }
                    case 53: // 5 (Logout)
                        {

                            break;
                        }
                    default:
                        // Error
                        return;
                }

                return;
            }

            // Error
        }

        internal override void ProcessingReceivedMessage(byte[] bytes, int index)
        {
            /*
             * Сообщения сессионного уровня
             * A (Logon) = 65
             * 5 (Logout) = 53
             * 0 (Heartbeat) = 48
             * 1 (Test Request) = 49
             * 2 (Resend Request) = 50
             * 4 (Sequence Reset) = 52
             * 3 (Reject) = 51
             * 
             * Сервис MFIX Trade
             * h (Trading Session Status) = 104
             * 8 (Execution Report) = 56
             * r (Order Mass Cancel Report) = 114
             * 9 (Order Cancel Reject) = 57
             * 
             * */

            Stopwatch stopwatch = new Stopwatch();
            int msgSeqNum = 0;
            int x;

            if ((bytes[index++] == 51) && (bytes[index++] == 53) && (bytes[index++] == 61)) // 35= (MsgType)
            {
                if (bytes[index + 1] != 1)
                {
                    // Error
                    return;
                }

                switch (bytes[index])
                {
                    case 56: // 8 (Execution Report)
                        {
                            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Received:\nMFIXTrade, Execution Report:\n   Bytes:\n      Length: " + bytes.Length.ToString() + "\n   Message: " + Encoding.UTF8.GetString(bytes) + "\n   Time: " + StringConverter.GetString(DateTimeConverter.GetBytes(DateTime.Now.Ticks)) + "\n"));
                            
                            break;
                        }
                    case 57: // 9 (Order Cancel Reject)
                        {
                            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Received:\nMFIXTrade, Order Cancel Reject:\n   Bytes:\n      Length: " + bytes.Length.ToString() + "\n   Message: " + Encoding.UTF8.GetString(bytes) + "\n   Time: " + StringConverter.GetString(DateTimeConverter.GetBytes(DateTime.Now.Ticks)) + "\n"));

                            break;
                        }
                    case 114: // r (Order Mass Cancel Report)
                        {
                            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Received:\nMFIXTrade, Order Mass Cancel Report:\n   Bytes:\n      Length: " + bytes.Length.ToString() + "\n   Message: " + Encoding.UTF8.GetString(bytes) + "\n   Time: " + StringConverter.GetString(DateTimeConverter.GetBytes(DateTime.Now.Ticks)) + "\n"));

                            break;
                        }
                    case 104: // h (Trading Session Status)
                        {
                            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Received:\nMFIXTrade, Trading Session Status:\n   Bytes:\n      Length: " + bytes.Length.ToString() + "\n   Message: " + Encoding.UTF8.GetString(bytes) + "\n   Time: " + StringConverter.GetString(DateTimeConverter.GetBytes(DateTime.Now.Ticks)) + "\n"));

                            break;
                        }
                    case 48: // 0 (Heartbeat)
                        {
                            index += 2;

                            HeartbeatData heartbeatData = Base.Message.ASTS.Heartbeat.GetHeartbeatData(bytes, index);
                            base.Messages.ServerMessages.Add(heartbeatData.Header.MsgSeqNum, heartbeatData.MessageBytes);
                            msgSeqNum = heartbeatData.Header.MsgSeqNum;

                            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Received:\nMFIXTrade, Heartbeat:\n   Bytes:\n      Length: " + heartbeatData.MessageBytes.Length.ToString() + "\n   Header:\n      MsgType: " + heartbeatData.Header.MsgType + "\n      SenderCompID: " + heartbeatData.Header.SenderCompID + "\n      TargetCompID: " + heartbeatData.Header.TargetCompID.ToString() + "\n      MsgSeqNum: " + heartbeatData.Header.MsgSeqNum.ToString() + "\n      PossDupFlag: " + heartbeatData.Header.PossDupFlag.ToString() + "\n      PossResend: " + heartbeatData.Header.PossResend.ToString() + "\n      SendingTime: " + heartbeatData.Header.SendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n      OrigSendingTime: " + heartbeatData.Header.OrigSendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n   TestReqID: " + ((heartbeatData.TestReqID != null) ? heartbeatData.TestReqID : "null") + "\n   Trailer\n      CheckSum: " + heartbeatData.CheckSum.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));

                            if (base.Messages.HTRSent == false)
                            {
                                if (heartbeatData.TestReqIDBytes == null)
                                {
                                    base.Heartbeat(null);
                                }
                            }
                            else if (base.Messages.SentHeartbeatTime != DateTime.MinValue)
                            {
                                if (base.Messages.SentTestRequestTime == DateTime.MinValue)
                                {
                                    base.Messages.SentHeartbeatTime = DateTime.MinValue;
                                    base.Messages.HTRSent = false;
                                }
                                else if ((base.Messages.TestReqID != null) && (heartbeatData.TestReqIDBytes != null) && (Message.Messages.TestReqIDEquals(base.Messages.TestReqID, heartbeatData.TestReqIDBytes) == true))
                                {
                                    base.Messages.TestReqID = null;
                                    base.Messages.SentHeartbeatTime = DateTime.MinValue;
                                    base.Messages.SentTestRequestTime = DateTime.MinValue;
                                    base.Messages.HTRSent = false;
                                }
                            }

                            break;
                        }
                    case 49: // 1 (Test Request)
                        {
                            index += 2;

                            TestRequestData trData = Base.Message.ASTS.TestRequest.GetTestRequestData(bytes, index);
                            base.Messages.ServerMessages.Add(trData.Header.MsgSeqNum, trData.MessageBytes);
                            msgSeqNum = trData.Header.MsgSeqNum;

                            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Received:\nMFIXTrade, Test Request:\n   Bytes:\n      Length: " + trData.MessageBytes.Length.ToString() + "\n   Header:\n      MsgType: " + trData.Header.MsgType + "\n      SenderCompID: " + trData.Header.SenderCompID + "\n      TargetCompID: " + trData.Header.TargetCompID.ToString() + "\n      MsgSeqNum: " + trData.Header.MsgSeqNum.ToString() + "\n      PossDupFlag: " + trData.Header.PossDupFlag.ToString() + "\n      PossResend: " + trData.Header.PossResend.ToString() + "\n      SendingTime: " + trData.Header.SendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n      OrigSendingTime: " + trData.Header.OrigSendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n   TestReqID: " + ((trData.TestReqID != null) ? trData.TestReqID : "null") + "\n   Trailer\n      CheckSum: " + trData.CheckSum.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));

                            if (trData.TestReqIDBytes != null)
                            {
                                base.Heartbeat(trData.TestReqIDBytes);
                            }

                            break;
                        }
                    case 50: // 2 (Resend Request)
                        {
                            index += 2;

                            ResendRequestData rrData = Base.Message.ASTS.ResendRequest.GetResendRequestData(bytes, index);
                            base.Messages.ServerMessages.Add(rrData.Header.MsgSeqNum, rrData.MessageBytes);
                            msgSeqNum = rrData.Header.MsgSeqNum;

                            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Received:\nMFIXTrade, Resend Request:\n   Bytes:\n      Length: " + rrData.MessageBytes.Length.ToString() + "\n   Header:\n      MsgType: " + rrData.Header.MsgType + "\n      SenderCompID: " + rrData.Header.SenderCompID + "\n      TargetCompID: " + rrData.Header.TargetCompID.ToString() + "\n      MsgSeqNum: " + rrData.Header.MsgSeqNum.ToString() + "\n      PossDupFlag: " + rrData.Header.PossDupFlag.ToString() + "\n      PossResend: " + rrData.Header.PossResend.ToString() + "\n      SendingTime: " + rrData.Header.SendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n      OrigSendingTime: " + rrData.Header.OrigSendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n   BeginSeqNo: " + rrData.BeginSeqNo.ToString() + "\n   EndSeqNo: " + rrData.EndSeqNo.ToString() + "\n   Trailer\n      CheckSum: " + rrData.CheckSum.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));

                            // ...



                            break;
                        }
                    case 52: // 4 (Sequence Reset)
                        {
                            index += 2;

                            SequenceResetData srData = Base.Message.ASTS.SequenceReset.GetSequenceResetData(bytes, index);
                            base.Messages.ServerMessages.Add(srData.Header.MsgSeqNum, srData.MessageBytes);
                            msgSeqNum = srData.Header.MsgSeqNum;

                            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Received:\nMFIXTrade, Sequence Reset:\n   Bytes:\n      Length: " + srData.MessageBytes.Length.ToString() + "\n   Header:\n      MsgType: " + srData.Header.MsgType + "\n      SenderCompID: " + srData.Header.SenderCompID + "\n      TargetCompID: " + srData.Header.TargetCompID.ToString() + "\n      MsgSeqNum: " + srData.Header.MsgSeqNum.ToString() + "\n      PossDupFlag: " + srData.Header.PossDupFlag.ToString() + "\n      PossResend: " + srData.Header.PossResend.ToString() + "\n      SendingTime: " + srData.Header.SendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n      OrigSendingTime: " + srData.Header.OrigSendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n   GapFillFlag: " + srData.GapFillFlag.ToString() + "\n   NewSeqNo: " + srData.NewSeqNo.ToString() + "\n   Trailer\n      CheckSum: " + srData.CheckSum.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));

                            // ...


                            break;
                        }
                    case 51: // 3 (Reject)
                        {
                            index += 2;

                            RejectData rejectData = Base.Message.ASTS.Reject.GetRejectData(bytes, index);
                            base.Messages.ServerMessages.Add(rejectData.Header.MsgSeqNum, rejectData.MessageBytes);
                            msgSeqNum = rejectData.Header.MsgSeqNum;

                            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Received:\nMFIXTrade, Reject:\n   Bytes:\n      Length: " + rejectData.MessageBytes.Length.ToString() + "\n   Header:\n      MsgType: " + rejectData.Header.MsgType + "\n      SenderCompID: " + rejectData.Header.SenderCompID + "\n      TargetCompID: " + rejectData.Header.TargetCompID.ToString() + "\n      MsgSeqNum: " + rejectData.Header.MsgSeqNum.ToString() + "\n      PossDupFlag: " + rejectData.Header.PossDupFlag.ToString() + "\n      PossResend: " + rejectData.Header.PossResend.ToString() + "\n      SendingTime: " + rejectData.Header.SendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n      OrigSendingTime: " + rejectData.Header.OrigSendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n   RefSeqNum: " + rejectData.RefSeqNum.ToString() + "\n   RefTagID: " + rejectData.RefTagID.ToString() + "\n   RefMsgType: " + ((rejectData.RefMsgType != null) ? rejectData.RefMsgType : "null") + "\n   SessionRejectReason: " + rejectData.SessionRejectReason.ToString() + "\n   Text: " + ((rejectData.Text != null) ? rejectData.Text : "null") + "\n   Trailer\n      CheckSum: " + rejectData.CheckSum.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));

                            // ...




                            break;
                        }
                    case 65: // A (Logon)
                        {
                            index += 2;
                            
                            stopwatch.Start();
                            
                            LogonData logonData = Base.Message.ASTS.Logon.GetLogonData(bytes, index);

                            stopwatch.Stop();

                            base.Messages.ServerMessages.Add(logonData.Header.MsgSeqNum, logonData.LogonBytes);
                            msgSeqNum = logonData.Header.MsgSeqNum;

                            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Received:\nMFIXTrade, Logon:\n   Bytes:\n      Length: " + logonData.LogonBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Header:\n      MsgType: " + logonData.Header.MsgType + "\n      SenderCompID: " + logonData.Header.SenderCompID + "\n      TargetCompID: " + logonData.Header.TargetCompID.ToString() + "\n      MsgSeqNum: " + logonData.Header.MsgSeqNum.ToString() + "\n      PossDupFlag: " + logonData.Header.PossDupFlag.ToString() + "\n      PossResend: " + logonData.Header.PossResend.ToString() + "\n      SendingTime: " + logonData.Header.SendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n      OrigSendingTime: " + logonData.Header.OrigSendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n   EncryptMethod: " + logonData.EncryptMethod.ToString() + "\n   HeartBtInt: " + logonData.HeartBtInt.ToString() + "\n   ResetSeqNumFlag: " + logonData.ResetSeqNumFlag.ToString() + "\n   SessionStatus: " + logonData.SessionStatus.ToString() + "\n   CancelOnDisconnect: " + logonData.CancelOnDisconnect.ToString() + "\n   LanguageID: " + logonData.LanguageID.ToString() + "\n   Trailer\n      CheckSum: " + logonData.CheckSum.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));

                            base.Session.ExpectedMessageLogon = false;
                            base.Status = ConnectionStatus.Connected;
                            base.Adapter.OnConnected(true);

                            if (base.Adapter.MFIXTransactional.TimerEnabled == true)
                            {
                                base.Adapter.MFIXTransactional.TimerEnabled = false;
                            }

                            base.Adapter.MFIXTransactional.TimerEnabled = true;

                            break;
                        }
                    case 53: // 5 (Logout)
                        {
                            index += 2;

                            stopwatch.Start();

                            LogoutData logoutData = Base.Message.ASTS.Logout.GetLogoutData(bytes, index);

                            stopwatch.Stop();

                            base.Messages.ServerMessages.Add(logoutData.Header.MsgSeqNum, logoutData.LogoutBytes);
                            msgSeqNum = logoutData.Header.MsgSeqNum;

                            OutputEventArgs.ProcessEventArgs(new OutputEventArgs(base.Adapter.connection.ConnectionSettings.Name + ":\nReceived:\nMFIXTrade, Logout:\n   Bytes:\n      Length: " + logoutData.LogoutBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Header:\n      MsgType: " + logoutData.Header.MsgType + "\n      SenderCompID: " + logoutData.Header.SenderCompID + "\n      TargetCompID: " + logoutData.Header.TargetCompID.ToString() + "\n      MsgSeqNum: " + logoutData.Header.MsgSeqNum.ToString() + "\n      PossDupFlag: " + logoutData.Header.PossDupFlag.ToString() + "\n      PossResend: " + logoutData.Header.PossResend.ToString() + "\n      SendingTime: " + logoutData.Header.SendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n      OrigSendingTime: " + logoutData.Header.OrigSendingTime.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n   Text: " + ((logoutData.Text != null) ? logoutData.Text : "null") + "\n   Trailer\n      CheckSum: " + logoutData.CheckSum.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));

                            if (base.Session.ExpectedMessageLogout == true)
                            {
                                base.Session.ExpectedMessageLogout = false;
                                base.Client.DisconnectClient();
                            }
                            else
                            {
                                base.canDisconnect = false;
                                base.Status = ConnectionStatus.Disconnecting;
                                base.Logout(false);
                            }

                            break;
                        }
                    default:
                        // Error
                        return;
                }

                base.Messages.LastReceivedMessageTime = DateTime.Now;

                // Проверка порядковых номеров (MsgSeqNum (34))

                if (msgSeqNum > (base.Messages.CurrentServerMsgSeqNum + 1))
                {
                    int beginSeqNo = base.Messages.CurrentServerMsgSeqNum + 1;
                    int endSeqNo = msgSeqNum - 1;

                    lock (base.Messages.MissingMessages)
                    {
                        for (x = beginSeqNo; x <= endSeqNo; x++)
                        {
                            base.Messages.MissingMessages.Add(x);
                        }
                    }

                    base.ResendRequest(beginSeqNo, endSeqNo);
                }
                else if (msgSeqNum <= base.Messages.CurrentServerMsgSeqNum)
                {
                    return;
                }

                base.Messages.CurrentServerMsgSeqNum = msgSeqNum;



                return;
            }

            // Error
        }

    }
}
