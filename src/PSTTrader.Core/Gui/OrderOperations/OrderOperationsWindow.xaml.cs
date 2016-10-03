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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

using ProSecuritiesTrading.PSTTrader.Core.Base;
using ProSecuritiesTrading.PSTTrader.Core.Output;

namespace ProSecuritiesTrading.PSTTrader.Core.Gui.OrderOperations
{
    /// <summary>
    /// Логика взаимодействия для OrderOperationsWindow.xaml
    /// </summary>
    public partial class OrderOperationsWindow : Window
    {
        private ProSecuritiesTrading.PSTTrader.Core.Base.Connection connection;

        public OrderOperationsWindow()
        {
            InitializeComponent();

            lock (Globals.Connections)
            {
                Base.Connection connection2 = null;

                for (int x = 0; x < Globals.Connections.Count; x++)
                {
                    connection2 = Globals.Connections[x];

                    if (((connection2.Status == ConnectionStatus.Connected) || (connection2.Status == ConnectionStatus.ConnectionLost)) && (connection2.AdapterOrder != null) && (connection2.ConnectionSettings.Provider == Provider.MOEX_ASTS_FIX))
                    {
                        this.connection = connection2;
                        break;
                    }
                }
            }

            if (this.connection == null)
            {
                ButtonsEnable(false);
            }

            Globals.Connections.ConnectionStatus += Connections_ConnectionStatus;
        }

        private void Connections_ConnectionStatus(object sender, ConnectionStatusEventArgs e)
        {
            if (this.connection != null)
            {
                if ((this.connection == e.Connection) && ((e.Status == ConnectionStatus.Disconnected) || (e.Status == ConnectionStatus.Disconnecting)))
                {
                    this.connection = null;

                    ButtonsEnable(false);
                }
            }
            else if (((e.Status == ConnectionStatus.Connected) || (e.Status == ConnectionStatus.ConnectionLost)) && (e.Connection.AdapterOrder != null) && (e.Connection.ConnectionSettings.Provider == Provider.MOEX_ASTS_FIX))
            {
                this.connection = e.Connection;

                ButtonsEnable(true);
            }
        }

        private void ButtonsEnable(bool enable)
        {
            this.btnBuyMarket.IsEnabled = enable;
            this.btnSellMarket.IsEnabled = enable;
            this.btnBuy.IsEnabled = enable;
            this.btnSell.IsEnabled = enable;
            this.btnMassCancel.IsEnabled = enable;
            this.btnCancel.IsEnabled = enable;
        }

        private void btnBuyMarket_Click(object sender, RoutedEventArgs e)
        {
            SubmitBuySellMarket(OrderSide.Buy);
        }

        private void btnSellMarket_Click(object sender, RoutedEventArgs e)
        {
            SubmitBuySellMarket(OrderSide.Sell);
        }

        private void SubmitBuySellMarket(OrderSide orderSide)
        {
            if ((string.IsNullOrWhiteSpace(this.tbAccount.Text) == true) || (string.IsNullOrWhiteSpace(this.tbInstrument.Text) == true) || (string.IsNullOrWhiteSpace(this.tbTradingSessionID.Text) == true) || (string.IsNullOrWhiteSpace(this.tbQuantity.Text) == true))
            {
                return;
            }

            byte secboardType;
            int quantity;

            if (byte.TryParse(this.tbTradingSessionID.Text, out secboardType) == false)
            {
                return;
            }

            if (int.TryParse(this.tbQuantity.Text, out quantity) == false)
            {
                return;
            }

            this.connection.ProcessOrder(new Order(this.tbAccount.Text, this.tbInstrument.Text, secboardType, 0, quantity, OrderState.Initialized, OrderType.Market, orderSide));
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            SubmitBuySell(OrderSide.Buy);
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            SubmitBuySell(OrderSide.Sell);
        }

        private void SubmitBuySell(OrderSide orderSide)
        {
            if ((string.IsNullOrWhiteSpace(this.tbAccount.Text) == true) || (string.IsNullOrWhiteSpace(this.tbInstrument.Text) == true) || (string.IsNullOrWhiteSpace(this.tbPrice.Text) == true) || (string.IsNullOrWhiteSpace(this.tbTradingSessionID.Text) == true) || (string.IsNullOrWhiteSpace(this.tbQuantity.Text) == true))
            {
                return;
            }

            double price;
            byte secboardType;
            int quantity;

            if (double.TryParse(this.tbPrice.Text, out price) == false)
            {
                return;
            }

            if (byte.TryParse(this.tbTradingSessionID.Text, out secboardType) == false)
            {
                return;
            }

            if (int.TryParse(this.tbQuantity.Text, out quantity) == false)
            {
                return;
            }

            this.connection.ProcessOrder(new Order(this.tbAccount.Text, this.tbInstrument.Text, secboardType, price, quantity, OrderState.Initialized, OrderType.Limit, orderSide));
        }

        private void btnMassCancel_Click(object sender, RoutedEventArgs e)
        {
            string account = null;
            byte side = 2;

            string instrument = null;
            byte secboardType = 255;

            try
            {
                if ((this.rbMCT1.IsChecked == true) && (string.IsNullOrWhiteSpace(this.tbMCT1Instrument.Text) == false))
                {
                    instrument = this.tbMCT1Instrument.Text;
                }

                if ((this.cbMCAccount.IsChecked == true) && (string.IsNullOrWhiteSpace(this.tbMCAccount.Text) == false))
                {
                    account = this.tbMCAccount.Text;
                }

                if ((this.rbMCT1.IsChecked == true) && (byte.TryParse(this.tbTradingSessionID.Text, out secboardType) == false))
                {
                    return;
                }

                if (this.cbSide.IsChecked == true)
                {
                    side = (byte)((this.rbBuy.IsChecked == true) ? 0 : 1);
                }

                this.connection.ProcessOrderMassCancel(account, instrument, secboardType, side);
            }
            catch (Exception ex)
            {
                OutputEventArgs.ProcessEventArgs(new OutputEventArgs("btnMassCancel_Click, Error: " + ex.ToString() + "\n"));
            }
        }

        private void rbMCT1_Checked(object sender, RoutedEventArgs e)
        {
            this.labelMCT1Instrument.IsEnabled = true;
            this.tbMCT1Instrument.IsEnabled = true;
        }

        private void rbMCT1_Unchecked(object sender, RoutedEventArgs e)
        {
            this.labelMCT1Instrument.IsEnabled = false;
            this.tbMCT1Instrument.IsEnabled = false;
        }

        private void cbMCAccount_Checked(object sender, RoutedEventArgs e)
        {
            this.labelMCAccount.IsEnabled = true;
            this.tbMCAccount.IsEnabled = true;
        }

        private void cbMCAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            this.labelMCAccount.IsEnabled = false;
            this.tbMCAccount.IsEnabled = false;
        }

        private void cbSide_Checked(object sender, RoutedEventArgs e)
        {
            this.rbBuy.IsEnabled = true;
            this.rbSell.IsEnabled = true;
        }

        private void cbSide_Unchecked(object sender, RoutedEventArgs e)
        {
            this.rbBuy.IsEnabled = false;
            this.rbSell.IsEnabled = false;
        }

        private void OrderOperationsWindow_Closed(object sender, EventArgs e)
        {
            Globals.Connections.ConnectionStatus -= Connections_ConnectionStatus;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            string origClOrdID = null;
            string orderID = null;

            if (string.IsNullOrWhiteSpace(this.tbOrigClOrdID.Text) == false)
            {
                origClOrdID = this.tbOrigClOrdID.Text;
            }

            if (string.IsNullOrWhiteSpace(this.tbOrderID.Text) == false)
            {
                orderID = this.tbOrderID.Text;
            }

            if ((origClOrdID == null) && (orderID == null))
            {
                return;
            }

            this.connection.ProcessOrderCancel(origClOrdID, orderID, (byte)((this.rbCancelBuy.IsChecked == true) ? 0 : 1));
        }
    }
}
