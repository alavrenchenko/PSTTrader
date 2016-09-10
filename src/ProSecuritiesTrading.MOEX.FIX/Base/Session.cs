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
using System.Threading;

namespace ProSecuritiesTrading.MOEX.FIX.Base
{
    public class Session
    {
        //private ProSecuritiesTrading.MOEX.FIX.Base.Service.IService service;
        private ProSecuritiesTrading.MOEX.FIX.Base.Service.ServiceBase service;
        public int HeartBtInt;
        public byte[] HeartBtIntBytes;
        private bool expectedMessageLogon = false;
        private bool expectedMessageLogout = false;
        private System.Threading.Timer timer;
        private TimerCallback timerCallbackTimer;

        public Session(ProSecuritiesTrading.MOEX.FIX.Base.Service.ServiceBase service, int heartBtInt)
        {
            this.service = service;

            this.HeartBtInt = Math.Max(0, heartBtInt);
            this.HeartBtIntBytes = ProSecuritiesTrading.MOEX.FIX.Base.Field.HeartBtInt.GetBytes(HeartBtInt);
        }

        private void OnTimer(object state)
        {
            if (this.expectedMessageLogon == true)
            {
                this.service.Disconnect();
            }
            else if (this.expectedMessageLogout == true)
            {
                this.service.DisconnectNow(1);
            }
        }

        internal void OnClientDisconnecting()
        {
            if (this.expectedMessageLogon == true)
            {
                this.ExpectedMessageLogon = false;
            }
            else if (this.expectedMessageLogout == true)
            {
                this.ExpectedMessageLogout = false;
            }
        }

        internal void CheckConnectionStatus()
        {
            DateTime time = DateTime.Now;

            if (this.service.Status == PSTTrader.Core.Base.ConnectionStatus.Connected)
            {
                if ((time >= this.service.Messages.LastReceivedMessageTime.AddSeconds(this.HeartBtInt)) || (this.service.Messages.HTRSent == true))
                {
                    if (this.service.Messages.SentHeartbeatTime == DateTime.MinValue)
                    {
                        this.service.Heartbeat(null);
                        this.service.Messages.SentHeartbeatTime = time;
                        this.service.Messages.HTRSent = true;
                    }
                    else if ((time >= this.service.Messages.SentHeartbeatTime.AddSeconds(this.HeartBtInt)) && (this.service.Messages.SentTestRequestTime == DateTime.MinValue))
                    {
                        this.service.TestRequest();
                        this.service.Messages.SentTestRequestTime = time;
                    }
                    else if (time >= this.service.Messages.SentTestRequestTime.AddSeconds(this.HeartBtInt))
                    {
                        this.service.DisconnectNow(0);
                        this.service.Messages.SentHeartbeatTime = DateTime.MinValue;
                        this.service.Messages.SentTestRequestTime = DateTime.MinValue;
                    }
                }
                else
                {
                    if (this.service.Messages.SentHeartbeatTime != DateTime.MinValue)
                    {
                        this.service.Messages.SentHeartbeatTime = DateTime.MinValue;
                    }

                    if (this.service.Messages.SentTestRequestTime != DateTime.MinValue)
                    {
                        this.service.Messages.SentTestRequestTime = DateTime.MinValue;
                    }
                }
            }
        }

        public bool ExpectedMessageLogon
        {
            get
            {
                return this.expectedMessageLogon;
            }
            set
            {
                if (this.expectedMessageLogon != value)
                {
                    this.expectedMessageLogon = value;

                    if (value == true)
                    {
                        if (this.timer != null)
                        {
                            return;
                        }

                        this.timerCallbackTimer = new TimerCallback(OnTimer);
                        this.timer = new Timer(this.timerCallbackTimer, null, 30000, 30000);
                    }
                    else
                    {
                        if (this.timer != null)
                        {
                            this.timer.Dispose();
                            this.timer = null;
                        }
                    }
                }
            }
        }

        public bool ExpectedMessageLogout
        {
            get
            {
                return this.expectedMessageLogout;
            }
            set
            {
                if (this.expectedMessageLogout != value)
                {
                    this.expectedMessageLogout = value;

                    if (value == true)
                    {
                        if (this.timer != null)
                        {
                            return;
                        }

                        this.timerCallbackTimer = new TimerCallback(OnTimer);
                        this.timer = new Timer(this.timerCallbackTimer, null, 30000, 30000);
                    }
                    else
                    {
                        if (this.timer != null)
                        {
                            this.timer.Dispose();
                            this.timer = null;
                        }
                    }
                }
            }
        }

        internal void Dispose()
        {
            if (this.expectedMessageLogon == true)
            {
                this.ExpectedMessageLogon = false;
            }
        }
    }
}
