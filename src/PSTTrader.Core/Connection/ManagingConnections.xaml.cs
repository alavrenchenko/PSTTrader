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

using ProSecuritiesTrading.PSTTrader.Core.Base;

namespace ProSecuritiesTrading.PSTTrader.Core.Connection
{
    /// <summary>
    /// Логика взаимодействия для ManagingConnections.xaml
    /// </summary>
    public partial class ManagingConnections : Window
    {
        private static bool isOpened = false;

        public ManagingConnections()
        {
            InitializeComponent();

            this.listViewConnections.ItemsSource = Globals.ConnectionItems;
        }

        private void buttonButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Tag == null)
            {
                return;
            }

            ConnectionItem connectionItem = Globals.ConnectionItems.FirstOrDefault(findConnectionItem => findConnectionItem.ConnectionName == button.Tag.ToString());

            if (connectionItem == null)
            {
                return;
            }

            MessageBox.Show("Settings " + connectionItem.ConnectionName);
        }

        private void buttonButtonConnectDisconnect_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Tag == null)
            {
                return;
            }

            string connectionName = button.Tag.ToString();
            ProSecuritiesTrading.PSTTrader.Core.Base.Connection connection = Globals.Connections.GetConnectionByName(connectionName);

            if (connection == null)
            {
                connection = new Base.Connection(connectionName);
                Globals.Connections.Add(connection);
                connection.Connect();
            }
            else
            {
                if ((connection.Status != ConnectionStatus.Disconnected) && (connection.Status != ConnectionStatus.Disconnecting))
                {
                    connection.Disconnect();
                }
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        public static bool IsOpened
        {
            get
            {
                return isOpened;
            }
            set
            {
                isOpened = value;
            }
        }

        private void ManagingConnections_Closed(object sender, EventArgs e)
        {
            isOpened = false;
        }
    }
}
