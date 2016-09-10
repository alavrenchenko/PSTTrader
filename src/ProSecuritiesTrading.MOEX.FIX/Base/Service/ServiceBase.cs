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
    public abstract class ServiceBase
    {
        public string ServiceName = string.Empty;
        protected internal ProSecuritiesTrading.MOEX.FIX.ASTS.ASTSAdapter Adapter;
        internal ProSecuritiesTrading.MOEX.FIX.Client.Client Client;
        public byte Id;
        protected string Ip = string.Empty;
        protected int Port = 0;
        public ProSecuritiesTrading.MOEX.FIX.Base.Session Session;
        public ProSecuritiesTrading.MOEX.FIX.Base.Message.Messages Messages;
        private bool canConnect = true;
        protected internal bool canDisconnect = false;
        public ConnectionStatus Status = ConnectionStatus.Disconnected;
        public byte ResetSeqNumFlag = 0;
        private object disconnectLock = new object();

        public ServiceBase(ProSecuritiesTrading.MOEX.FIX.ASTS.ASTSAdapter adapter, byte id, string serviceName)
        {
            this.Adapter = adapter;
            this.Client = new Client.Client(this);
            this.Id = id;
            this.ServiceName = serviceName;
            this.Client.ClientError += Client_ClientError;
        }

        private void Client_ClientError(object sender, Client.ClientErrorEventArgs e)
        {
            bool disconnected = false;

            if (e.Exception is SocketException)
            {
                SocketException socketException = (SocketException)e.Exception;

                if (socketException.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    disconnected = true;
                }
                else if (socketException.SocketErrorCode == SocketError.ConnectionAborted)
                {
                    disconnected = true;
                }
                else if (socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    disconnected = true;
                }
                else if (socketException.SocketErrorCode == SocketError.Disconnecting)
                {

                }
                else if (socketException.SocketErrorCode == SocketError.HostDown)
                {

                }
                else if (socketException.SocketErrorCode == SocketError.IsConnected)
                {

                }
                else if (socketException.SocketErrorCode == SocketError.NotConnected)
                {
                    disconnected = true;
                }
                else if (socketException.SocketErrorCode == SocketError.NetworkDown)
                {

                }
                else if (socketException.SocketErrorCode == SocketError.Shutdown)
                {

                }
                else if (socketException.SocketErrorCode == SocketError.SocketError)
                {

                }
                else if (socketException.SocketErrorCode == SocketError.TimedOut)
                {

                }
                else
                {
                }

                OutputEventArgs.ProcessEventArgs(new OutputEventArgs(this.ServiceName + ", client_ClientError, SocketException:\n   NativeErrorCode: " + socketException.NativeErrorCode.ToString() + "\n   SocketErrorCode: " + socketException.SocketErrorCode.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff")));
            }
            else
            {
                OutputEventArgs.ProcessEventArgs(new OutputEventArgs(this.ServiceName + ", client_ClientError:\n   Exception: " + e.Exception.ToString() + "\n   Exception.Source: " + e.Exception.Source.ToString() + "\n   NameSource: " + e.NameSource.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff")));
            }

            if (disconnected == true)
            {
                lock (this.disconnectLock)
                {
                    if (this.canDisconnect == true)
                    {
                        this.canDisconnect = false;
                    }

                    if (this.canConnect == true)
                    {
                        return;
                    }

                    if (this.Session.ExpectedMessageLogon == true)
                    {
                        this.Session.ExpectedMessageLogon = false;
                    }
                    else if (this.Session.ExpectedMessageLogout == true)
                    {
                        this.Session.ExpectedMessageLogout = false;
                    }

                    this.canConnect = true;
                    this.Status = ConnectionStatus.Disconnected;
                    this.Adapter.ClientError(this.Id, ErrorCode.NotConnected, ConnectionStatus.Disconnected);
                }
            }
        }

        public void Connect()
        {
            if ((this.canConnect == true) && (this.Status == ConnectionStatus.Disconnected))
            {
                this.canConnect = false;
                this.Status = ConnectionStatus.Connecting;
                this.Client.ConnectClient(this.Ip, this.Port);

                //--------------------------------------
                //OnClientConnected(true);
                //this.Client.ConnectClient("127.0.0.1", 5000);
                //--------------------------------------
            }
        }

        public void Disconnect()
        {
            lock (this.disconnectLock)
            {
                if ((this.canDisconnect == true) && (this.Status != ConnectionStatus.Disconnected) && (this.Status != ConnectionStatus.Disconnecting))
                {
                    this.canDisconnect = false;
                    this.Status = ConnectionStatus.Disconnecting;
                    
                    if (this.Session.ExpectedMessageLogon == false)
                    {
                        Logout(true);
                    }
                    else
                    {
                        this.Session.ExpectedMessageLogon = false;
                        this.Client.DisconnectClient();
                    }
                    
                    //--------------------------------------
                    //OnClientConnected(false);
                    //--------------------------------------
                }
            }
        }

        /// <summary>
        /// Если сервер не отвечает или отсутствует подтверждающее сообщение Logout.
        /// </summary>
        /// <param name="reason">Сервер не отвечает = 0, Отсутствует подтверждающее сообщение Logout = 1.</param>
        public void DisconnectNow(byte reason)
        {
            lock (this.disconnectLock)
            {
                if ((this.Status == ConnectionStatus.Disconnected) || (reason > 1))
                {
                    return;
                }

                if (reason == 1)
                {
                    if (this.Session.ExpectedMessageLogout == false)
                    {
                        return;
                    }

                    this.Session.ExpectedMessageLogout = false;
                }

                this.Client.DisconnectClient();

                OutputEventArgs.ProcessEventArgs(new OutputEventArgs(this.ServiceName + ":\n   Error: " + ((reason == 0) ? "Сервер не отвечает." : "Отсутствует подтверждающее сообщение Logout.") + "\n   ConnectionStatus: " + this.Status.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
            }
        }

        public void OnClientConnected(bool connected)
        {
            if (connected == true)
            {
                this.canDisconnect = true;
                Logon();
            }
            else
            {
                lock (this.disconnectLock)
                {
                    if (this.canDisconnect == true)
                    {
                        this.canDisconnect = false;
                    }

                    if (this.canConnect == true)
                    {
                        return;
                    }

                    if (this.Session.ExpectedMessageLogon == true)
                    {
                        this.Session.ExpectedMessageLogon = false;
                    }
                    else if (this.Session.ExpectedMessageLogout == true)
                    {
                        this.Session.ExpectedMessageLogout = false;
                    }

                    this.canConnect = true;
                    this.Status = ConnectionStatus.Disconnected;
                    this.Adapter.OnConnected(false);

                    OutputEventArgs.ProcessEventArgs(new OutputEventArgs(this.ServiceName + ":\n   ConnectionStatus: " + this.Status.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
                }
            }
        }

        private void Logon()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            byte[] messageBytes = Base.Message.ASTS.Logon.UnsafeGetBytes(this.Messages.Header, this.Messages.NextClientMsgSeqNum, false, 48, this.Session.HeartBtIntBytes, this.ResetSeqNumFlag, this.Adapter.connection.ConnectionSettings.Password, null, 0, 0);

            stopwatch.Stop();
            /*
            byte[][] message = new byte[2][];
            message[0] = MessageBytesCollection.Logon;
            message[1] = messageBytes;
            

            object[] message = new object[2];
            message[0] = MessageBytesCollection.Logon;
            message[1] = messageBytes;
            this.Messages.ClientMessages.Add(this.Messages.CurrentClientMsgSeqNum, message);
            */

            this.Messages.ClientMessages.Add(this.Messages.CurrentClientMsgSeqNum, messageBytes);

            this.Client.Send(messageBytes);
            this.Session.ExpectedMessageLogon = true;

            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\n" + this.ServiceName + ", Logon:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Message: " + Encoding.ASCII.GetString(messageBytes) + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
            //OutputEventArgs.ProcessEventArgs(new OutputEventArgs(this.ServiceName + ", Logon:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));

            //--------------------------------------
            /*
            this.Session.ExpectedMessageLogon = false;
            this.Status = ConnectionStatus.Connected;
            this.Adapter.OnConnected(true);
            */
            //--------------------------------------
        }

        internal void Logout(bool initiating)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            byte[] messageBytes = Base.Message.ASTS.Logout.UnsafeGetBytes(this.Messages.Header, this.Messages.NextClientMsgSeqNum, false);

            stopwatch.Stop();

            this.Messages.ClientMessages.Add(this.Messages.CurrentClientMsgSeqNum, messageBytes);

            this.Client.Send(messageBytes);

            if (initiating == true)
            {
                this.Session.ExpectedMessageLogout = true;
            }

            OutputEventArgs.ProcessEventArgs(new OutputEventArgs(this.Adapter.connection.ConnectionSettings.Name + ":\nSent:\n" + this.ServiceName + ", Logout:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Message: " + Encoding.ASCII.GetString(messageBytes) + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
        }

        internal void Heartbeat(byte[] testReqID)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            byte[] messageBytes = Base.Message.ASTS.Heartbeat.UnsafeGetBytes(this.Messages.Header, this.Messages.NextClientMsgSeqNum, false, testReqID);

            stopwatch.Stop();

            this.Messages.ClientMessages.Add(this.Messages.CurrentClientMsgSeqNum, messageBytes);

            this.Client.Send(messageBytes);

            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\n" + this.ServiceName + ", Heartbeat:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Message: " + Encoding.UTF8.GetString(messageBytes) + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
            //OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\n" + this.ServiceName + ", Heartbeat:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
        }

        internal void TestRequest()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            byte[] testReqID = DateTimeConverter.CreateTestReqID();
            byte[] messageBytes = Base.Message.ASTS.TestRequest.UnsafeGetBytes(this.Messages.Header, this.Messages.NextClientMsgSeqNum, false, testReqID);
            this.Messages.TestReqID = testReqID;

            stopwatch.Stop();

            this.Messages.ClientMessages.Add(this.Messages.CurrentClientMsgSeqNum, messageBytes);

            this.Client.Send(messageBytes);

            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\n" + this.ServiceName + ", TestRequest:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Message: " + Encoding.ASCII.GetString(messageBytes) + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
            //OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\n" + this.ServiceName + ", TestRequest:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
        }

        protected internal void ResendRequest(int beginSeqNo, int endSeqNo)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            byte[] messageBytes = Base.Message.ASTS.ResendRequest.UnsafeGetBytes(this.Messages.Header, this.Messages.NextClientMsgSeqNum, false, beginSeqNo, endSeqNo);

            stopwatch.Stop();

            this.Messages.ClientMessages.Add(this.Messages.CurrentClientMsgSeqNum, messageBytes);

            this.Client.Send(messageBytes);

            OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\n" + this.ServiceName + ", ResendRequest:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Message: " + Encoding.ASCII.GetString(messageBytes) + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
            //OutputEventArgs.ProcessEventArgs(new OutputEventArgs("Sent:\n" + this.ServiceName + ", ResendRequest:\n   Bytes:\n      Length: " + messageBytes.Length.ToString() + "\n      Elapsed time, ticks: " + stopwatch.ElapsedTicks.ToString() + "\n   Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\n"));
        }

        internal abstract unsafe void ProcessingReceivedMessage(byte* pBytes);

        internal abstract void ProcessingReceivedMessage(byte[] bytes, int index);

        internal void Dispose()
        {
            this.Client.ClientError -= Client_ClientError;

            if (this.Session != null)
            {
                this.Session.Dispose();
            }
        }
    }
}
