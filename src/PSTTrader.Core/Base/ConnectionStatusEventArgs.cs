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
    public class ConnectionStatusEventArgs
    {
        private Connection connection;
        private ConnectionStatus status;
        private ConnectionStatus oldStatus;
        private ErrorCode errorCode;

        public ConnectionStatusEventArgs(Connection currentConnection, ErrorCode errorCode, ConnectionStatus status)
        {
            this.connection = currentConnection;
            this.errorCode = errorCode;
            this.status = status;
            this.oldStatus = this.connection.Status;
        }

        public override string ToString()
        {
            return connection.Name + ": Status=" + status.ToString();
        }

        public Connection Connection
        {
            get
            {
                return this.connection;
            }
        }

        public ConnectionStatus OldStatus
        {
            get
            {
                return oldStatus;
            }
        }

        public ConnectionStatus Status
        {
            get
            {
                return status;
            }
        }

        public ErrorCode Error
        {
            get
            {
                return this.errorCode;
            }
        }
    }
}
