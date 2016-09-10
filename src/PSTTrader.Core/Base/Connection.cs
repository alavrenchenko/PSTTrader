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
using System.Reflection;
using System.IO;

using ProSecuritiesTrading.PSTTrader.Core.Adapter;
using ProSecuritiesTrading.PSTTrader.Core.Data;

namespace ProSecuritiesTrading.PSTTrader.Core.Base
{
    public class Connection : System.Windows.DependencyObject
    {
        private IAdapter adapter;
        private IFundamentalData adapterFundamentalData;
        private IMarketData adapterMarketData;
        private IMarketDepth adapterMarketDepth;
        private IOrder adapterOrder;
        private ProSecuritiesTrading.PSTTrader.Core.Base.ConnectionStatus status = ProSecuritiesTrading.PSTTrader.Core.Base.ConnectionStatus.Disconnected;
        private ProSecuritiesTrading.PSTTrader.Core.Base.ConnectionSettings connectionSettings;
        private Currency[] currencies = new Currency[0];
        private Exchange[] exchanges = new Exchange[0];
        private Feature[] features = new Feature[0];
        private InstrumentType[] instrumentTypes = new InstrumentType[0];
        private ConnectionStatusEventHandler connectionStatus;
        private List<FundamentalData> fundamentalDataCollection;
        private List<MarketData> marketDataCollection;
        private List<MarketDepth> marketDepthCollection;
        private object processConnectionStatusLock = new object();
        private object connectLock = new object();
        private bool canConnect = true;

        public event ConnectionStatusEventHandler ConnectionStatus
        {
            add
            {
                this.connectionStatus += value;
            }
            remove
            {
                this.connectionStatus -= value;
            }
        }

        protected virtual void OnConnectionStatus(ConnectionStatusEventArgs e)
        {
            ConnectionStatusEventHandler handler = this.connectionStatus;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public Connection(string connectionName)
        {
            this.connectionSettings = Globals.ConnectionSettingsCollection.FirstOrDefault(findCollectionSettings => findCollectionSettings.Name == connectionName);

            if (this.connectionSettings == null)
            {
                // Error
            }

            this.fundamentalDataCollection = new List<FundamentalData>();
            this.marketDataCollection = new List<MarketData>();
            this.marketDepthCollection = new List<MarketDepth>();
        }

        public void Connect()
        {
            Task.Run(() =>
                {
                    lock (this.connectLock)
                    {
                        if ((this.canConnect == false) || (this.status != Base.ConnectionStatus.Disconnected))
                        {
                            return;
                        }

                        this.canConnect = false;
                        string path = string.Empty;
                        string assemblyName = null;
                        string typeName = string.Empty;

                        if (connectionSettings.Provider == Provider.MOEX_ASTS_FAST)
                        {
                            assemblyName = "ProSecuritiesTrading.MOEX.FAST";
                            typeName = "ProSecuritiesTrading.MOEX.FAST.ASTS.ASTSLoader";
                        }
                        else if (connectionSettings.Provider == Provider.MOEX_ASTS_FIX)
                        {
                            assemblyName = "ProSecuritiesTrading.MOEX.FIX";
                            typeName = "ProSecuritiesTrading.MOEX.FIX.ASTS.ASTSLoader";
                        }

                        if (assemblyName == null)
                        {
                            this.canConnect = true;
                            return;
                        }

                        ProSecuritiesTrading.PSTTrader.Core.Connection.ConnectionItem connectionItem = null;

                        lock (Globals.ConnectionItems)
                        {
                            connectionItem = Globals.ConnectionItems.FirstOrDefault(findConnectionItem => findConnectionItem.ConnectionName == this.Name);
                        }

                        if (connectionItem != null)
                        {
                            Dispatcher.BeginInvoke((Action)(() =>
                                {
                                    connectionItem.ButtonConnectDisconnectEnabled = false;
                                }));
                        }

                        Assembly assembly = null;

                        lock (Globals.ConnectionAssemblies)
                        {
                            assembly = Globals.ConnectionAssemblies.FirstOrDefault(findAssembly => findAssembly.GetName().Name == assemblyName);
                        }

                        try
                        {
                            if (assembly == null)
                            {
                                path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + assemblyName + ".dll";

                                if (File.Exists(path) == false)
                                {
                                    // Error
                                    this.canConnect = true;
                                    connectionItem.ButtonConnectDisconnectEnabled = true;
                                    return;
                                }

                                assembly = Assembly.LoadFrom(path);

                                Globals.ConnectionAssemblies.Add(assembly);
                            }

                            ILoader loader = (ILoader)assembly.CreateInstance(typeName);
                            this.adapter = loader.Create(this);

                            if (this.adapter is IFundamentalData)
                            {
                                this.adapterFundamentalData = (IFundamentalData)this.adapter;
                            }

                            if (this.adapter is IMarketData)
                            {
                                this.adapterMarketData = (IMarketData)this.adapter;
                            }

                            if (this.adapter is IMarketDepth)
                            {
                                this.adapterMarketDepth = (IMarketDepth)this.adapter;
                            }

                            if (this.adapter is IOrder)
                            {
                                this.adapterOrder = (IOrder)this.adapter;
                            }

                            this.adapter.Connect();
                        }
                        catch (Exception e)
                        {
                            // Error
                            this.canConnect = true;

                            Dispatcher.BeginInvoke((Action)(() =>
                                {
                                    connectionItem.ButtonConnectDisconnectEnabled = true;
                                }));

                            Output.OutputEventArgs.ProcessEventArgs(new Output.OutputEventArgs("ConnectionName: " + this.Name + "; Status: " + this.status + "; Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "; \nError: " + e.ToString()));
                        }
                    }
                });
        }

        public void Disconnect()
        {
            if ((this.status == Base.ConnectionStatus.Disconnected) || (this.status == Base.ConnectionStatus.Disconnecting))
            {
                return;
            }

            ProSecuritiesTrading.PSTTrader.Core.Connection.ConnectionItem connectionItem = Globals.ConnectionItems.FirstOrDefault(findConnectionItem => findConnectionItem.ConnectionName == this.Name);

            if (connectionItem != null)
            {
                connectionItem.ButtonConnectDisconnectEnabled = false;
            }

            try
            {
                this.adapter.Disconnect();
            }
            catch (Exception e)
            {
                // Error
                connectionItem.ButtonConnectDisconnectEnabled = true;

                Output.OutputEventArgs.ProcessEventArgs(new Output.OutputEventArgs("ConnectionName: " + this.Name + "; Status: " + this.status + "; Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "; \nError: " + e.ToString()));
            }
        }

        public void ProcessConnectionStatus(ConnectionStatusEventArgs e)
        {
            lock (this.processConnectionStatusLock)
            {
                ProSecuritiesTrading.PSTTrader.Core.Connection.ConnectionItem connectionItem = null;

                lock (Globals.ConnectionItems)
                {
                    connectionItem = Globals.ConnectionItems.FirstOrDefault(findConnectionItem => findConnectionItem.ConnectionName == this.Name);
                }

                Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (connectionItem != null)
                        {
                            connectionItem.Status = e.Status.ToString();

                            if (e.Status == Base.ConnectionStatus.Connected)
                            {
                                connectionItem.ButtonConnectDisconnectContent = "Disconnect";

                                if (connectionItem.ButtonConnectDisconnectEnabled == false)
                                {
                                    connectionItem.ButtonConnectDisconnectEnabled = true;
                                }
                            }
                            else if (e.Status == Base.ConnectionStatus.Disconnected)
                            {
                                try
                                {
                                    this.adapter.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    Output.OutputEventArgs.ProcessEventArgs(new Output.OutputEventArgs("ConnectionName: " + this.Name + "; Status: " + e.Status + "; Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "; \nError: " + ex.ToString()));
                                }

                                this.adapter = null;

                                if (this.adapterFundamentalData != null)
                                {
                                    this.adapterFundamentalData = null;
                                }

                                if (this.adapterMarketData != null)
                                {
                                    this.adapterMarketData = null;
                                }

                                if (this.adapterMarketDepth != null)
                                {
                                    this.adapterMarketDepth = null;
                                }

                                if (this.adapterOrder != null)
                                {
                                    this.adapterOrder = null;
                                }

                                connectionItem.ButtonConnectDisconnectContent = "Connect";

                                if (connectionItem.ButtonConnectDisconnectEnabled == false)
                                {
                                    connectionItem.ButtonConnectDisconnectEnabled = true;
                                }

                                this.canConnect = true;
                            }
                        }

                        this.status = e.Status;
                        OnConnectionStatus(e);

                        Output.OutputEventArgs.ProcessEventArgs(new Output.OutputEventArgs("ConnectionName: " + e.Connection.Name + "; Status: " + e.Status + "; Time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff")));
                    }));
            }
        }

        public void ProcessOrder(Order order)
        {
            switch (order.OrderState)
            {
                case OrderState.Initialized:
                    {
                        this.adapterOrder.Submit(order);
                        break;
                    }
                case OrderState.PendingSubmit:
                    {
                        break;
                    }
                case OrderState.Working:
                    {
                        break;
                    }
                case OrderState.PartFilled:
                    {
                        break;
                    }
                case OrderState.Filled:
                    {
                        break;
                    }
                case OrderState.PendingCancel:
                    {
                        this.adapterOrder.Cancel(order);
                        break;
                    }
                case OrderState.Cancelled:
                    {
                        break;
                    }
            }
        }

        public void ProcessOrderMassCancel(string account, string instrumentName, byte secboardType, byte orderSide)
        {
            this.adapterOrder.MassCancel(account, instrumentName, secboardType, orderSide);
        }

        public string Name
        {
            get
            {
                return this.connectionSettings.Name;
            }
        }

        public IAdapter Adapter
        {
            get
            {
                return this.adapter;
            }
        }

        public IFundamentalData AdapterFundamentalData
        {
            get
            {
                return this.adapterFundamentalData;
            }
        }

        public IMarketData AdapterMarketData
        {
            get
            {
                return this.adapterMarketData;
            }
        }

        public IMarketDepth AdapterMarketDepth
        {
            get
            {
                return this.adapterMarketDepth;
            }
        }

        public IOrder AdapterOrder
        {
            get
            {
                return this.adapterOrder;
            }
        }

        public ProSecuritiesTrading.PSTTrader.Core.Base.ConnectionStatus Status
        {
            get
            {
                return this.status;
            }
        }

        public ProSecuritiesTrading.PSTTrader.Core.Base.ConnectionSettings ConnectionSettings
        {
            get
            {
                return this.connectionSettings;
            }
        }

        public Currency[] Currencies
        {
            get
            {
                return currencies;
            }
            set
            {
                currencies = value;
            }
        }

        public Exchange[] Exchanges
        {
            get
            {
                return exchanges;
            }
            set
            {
                exchanges = value;
            }
        }

        public Feature[] Features
        {
            get
            {
                return features;
            }
            set
            {
                features = value;
            }
        }

        public InstrumentType[] InstrumentTypes
        {
            get
            {
                return instrumentTypes;
            }
            set
            {
                instrumentTypes = value;
            }
        }

        public List<FundamentalData> FundamentalDataCollection
        {
            get
            {
                return this.fundamentalDataCollection;
            }
        }

        public List<MarketData> MarketDataCollection
        {
            get
            {
                return this.marketDataCollection;
            }
        }

        public List<MarketDepth> MarketDepthCollection
        {
            get
            {
                return this.marketDepthCollection;
            }
        }
    }
}
