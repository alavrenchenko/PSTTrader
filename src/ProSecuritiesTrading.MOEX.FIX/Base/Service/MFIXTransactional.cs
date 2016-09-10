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

namespace ProSecuritiesTrading.MOEX.FIX.Base.Service
{
    public class MFIXTransactional
    {
        internal ProSecuritiesTrading.MOEX.FIX.Base.Service.MFIXTrade MFIXTrade;
        internal ProSecuritiesTrading.MOEX.FIX.Base.Service.MFIXTradeCapture MFIXTradeCapture;
        internal ProSecuritiesTrading.MOEX.FIX.Base.Service.MFIXDropCopy MFIXDropCopy;
        private System.Threading.Timer timer;
        private TimerCallback timerCallbackTimer;
        private object onTimerLock = new object();
        private object timerEnabledLock = new object();

        public MFIXTransactional(ProSecuritiesTrading.MOEX.FIX.ASTS.ASTSAdapter adapter)
        {
            this.MFIXTrade = new MFIXTrade(adapter);
            this.MFIXTradeCapture = new MFIXTradeCapture(adapter);
            this.MFIXDropCopy = new MFIXDropCopy(adapter);
        }

        private void OnTimer(object state)
        {
            lock (this.onTimerLock)
            {
                this.MFIXTrade.Session.CheckConnectionStatus();
                this.MFIXTradeCapture.Session.CheckConnectionStatus();
                this.MFIXDropCopy.Session.CheckConnectionStatus();

                /*
                DateTime time = DateTime.Now;

                if (this.MFIXTrade.Status == PSTTrader.Core.Base.ConnectionStatus.Connected)
                {
                    if (time >= this.MFIXTrade.Messages.LastReceivedMessageTime.AddSeconds(this.MFIXTrade.Session.HeartBtInt))
                    {
                        if (this.MFIXTrade.Messages.SentHeartbeatTime == DateTime.MinValue)
                        {
                            this.MFIXTrade.Heartbeat(null);
                            this.MFIXTrade.Messages.SentHeartbeatTime = time;
                        }

                        else if ((time >= this.MFIXTrade.Messages.SentHeartbeatTime.AddSeconds(this.MFIXTrade.Session.HeartBtInt)) && (this.MFIXTrade.Messages.SentTestRequestTime == DateTime.MinValue))
                        {
                            this.MFIXTrade.TestRequest();
                            this.MFIXTrade.Messages.SentTestRequestTime = time;
                        }
                        else if (time >= this.MFIXTrade.Messages.SentTestRequestTime.AddSeconds(this.MFIXTrade.Session.HeartBtInt))
                        {
                            this.MFIXTrade.DisconnectNow(0);
                            this.MFIXTrade.Messages.SentHeartbeatTime = DateTime.MinValue;
                            this.MFIXTrade.Messages.SentTestRequestTime = DateTime.MinValue;
                        }
                    }
                    else
                    {
                        if (this.MFIXTrade.Messages.SentHeartbeatTime != DateTime.MinValue)
                        {
                            this.MFIXTrade.Messages.SentHeartbeatTime = DateTime.MinValue;
                        }

                        if (this.MFIXTrade.Messages.SentTestRequestTime != DateTime.MinValue)
                        {
                            this.MFIXTrade.Messages.SentTestRequestTime = DateTime.MinValue;
                        }
                    }
                }
                */
            }
        }

        public bool TimerEnabled
        {
            get
            {
                return (this.timer != null);
            }
            set
            {
                lock (this.timerEnabledLock)
                {
                    if (value == true)
                    {
                        if (this.timer != null)
                        {
                            return;
                        }

                        int period = 60;

                        if ((this.MFIXTrade.Status == PSTTrader.Core.Base.ConnectionStatus.Connected) && (this.MFIXTrade.Session.HeartBtInt < period))
                        {
                            period = this.MFIXTrade.Session.HeartBtInt;
                        }

                        if ((this.MFIXTradeCapture.Status == PSTTrader.Core.Base.ConnectionStatus.Connected) && (this.MFIXTradeCapture.Session.HeartBtInt < period))
                        {
                            period = this.MFIXTradeCapture.Session.HeartBtInt;
                        }

                        if ((this.MFIXDropCopy.Status == PSTTrader.Core.Base.ConnectionStatus.Connected) && (this.MFIXDropCopy.Session.HeartBtInt < period))
                        {
                            period = this.MFIXDropCopy.Session.HeartBtInt;
                        }

                        period *= 1000;

                        this.timerCallbackTimer = new TimerCallback(OnTimer);
                        this.timer = new Timer(this.timerCallbackTimer, null, period, period);

                    }
                    else
                    {
                        if (this.timer != null)
                        {
                            timer.Dispose();
                            timer = null;
                        }
                    }
                }
            }
        }

        internal void Dispose()
        {
            this.MFIXTrade.Dispose();
            this.MFIXTradeCapture.Dispose();
            this.MFIXDropCopy.Dispose();

            if (this.TimerEnabled == true)
            {
                this.TimerEnabled = false;
            }
        }
    }
}
