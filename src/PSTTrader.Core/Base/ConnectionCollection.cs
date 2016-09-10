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

namespace ProSecuritiesTrading.PSTTrader.Core.Base
{
    public class ConnectionCollection
    {
        private List<Connection> connections = new List<Connection>();
        private ConnectionStatusEventHandler connectionStatus;

        public event ConnectionStatusEventHandler ConnectionStatus
        {
            add
            {
                connectionStatus += value;
            }
            remove
            {
                connectionStatus -= value;
            }
        }

        public void Add(Connection connection)
        {
            this.connections.Add(connection);
            connection.ConnectionStatus += OnConnectionStatus;
        }

        internal void OnConnectionStatus(object sender, ConnectionStatusEventArgs e)
        {
            ConnectionStatusEventHandler handler = connectionStatus;

            if (handler != null)
            {
                handler(sender, e);
            }

            if (e.Status == Base.ConnectionStatus.Disconnected)
            {
                Globals.Connections.Remove(e.Connection);
            }
        }

        public bool Contains(Connection item)
        {
            lock (this.connections)
            {
                return this.connections.Contains(item);
            }
        }

        public Connection GetConnectionByName(string name)
        {
            Connection connection = null;

            lock (this.connections)
            {
                for (int x = 0; x < this.connections.Count; x++)
                {
                    connection = this.connections[x];

                    if (connection.Name == name)
                    {
                        return connection;
                    }
                }
            }

            return null;
        }

        public int IndexOf(Connection item)
        {
            lock (this.connections)
            {
                return this.connections.IndexOf(item);
            }
        }

        public void Remove(Connection connection)
        {
            this.connections.Remove(connection);
            connection.ConnectionStatus -= OnConnectionStatus;
        }

        public int Count
        {
            get
            {
                return this.connections.Count;
            }
        }

        public Connection this[int index]
        {
            get
            {
                return this.connections[index];
            }
        }
    }
}
