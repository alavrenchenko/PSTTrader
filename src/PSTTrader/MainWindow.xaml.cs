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

using System.Reflection;
using System.IO;

using ProSecuritiesTrading.PSTTrader.Core.Base;
using ProSecuritiesTrading.PSTTrader.Core.Connection;
using ProSecuritiesTrading.PSTTrader.Core.Gui.OrderOperations;

namespace ProSecuritiesTrading.PSTTrader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            StartUpMainWindow();
        }

        private void StartUpMainWindow()
        {
            ConnectionSettings settings = new ProSecuritiesTrading.MOEX.FIX.ASTS.ASTSSettings(0);
            Globals.ConnectionSettingsCollection.Add(settings);
            Globals.ConnectionItems.Add(new ConnectionItem()
            {
                ConnectionName = settings.Name,
                Provider = settings.Provider.ToString(),
                Status = "Disconnected",
                ButtonSettingsTag = settings.Name,
                ButtonConnectDisconnectTag = settings.Name,
                ButtonConnectDisconnectContent = "Connect",
                ButtonConnectDisconnectEnabled = true
            });

            settings = new ProSecuritiesTrading.MOEX.FIX.ASTS.ASTSSettings(1);
            Globals.ConnectionSettingsCollection.Add(settings);
            Globals.ConnectionItems.Add(new ConnectionItem()
            {
                ConnectionName = settings.Name,
                Provider = settings.Provider.ToString(),
                Status = "Disconnected",
                ButtonSettingsTag = settings.Name,
                ButtonConnectDisconnectTag = settings.Name,
                ButtonConnectDisconnectContent = "Connect",
                ButtonConnectDisconnectEnabled = true
            });

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly assembly = null;

            for (int x = 0; x < assemblies.Length; x++)
            {
                assembly = assemblies[x];
                string assemblyName = assembly.GetName().Name;

                if ((assemblyName == "ProSecuritiesTrading.MOEX.FAST") || (assemblyName == "ProSecuritiesTrading.MOEX.FIX"))
                {
                    Globals.ConnectionAssemblies.Add(assembly);
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {

        }

        private void menuItemTimeAndSales_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuItemOutput_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.OfType<ProSecuritiesTrading.PSTTrader.Core.Output.OutputWindow>().Any(findOutputWindow => findOutputWindow.Name == "windowOutput") == false)
            {
                new ProSecuritiesTrading.PSTTrader.Core.Output.OutputWindow().Show();
            }
        }

        private void menuItemManagingConnections_Click(object sender, RoutedEventArgs e)
        {
            if (ManagingConnections.IsOpened == true)
            {
                return;
            }

            new ManagingConnections().Show();
            ManagingConnections.IsOpened = true;
        }

        private void menuItemOrderOperations_Click(object sender, RoutedEventArgs e)
        {
            new OrderOperationsWindow().Show();
        }

    }
}
