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
using System.Threading;
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

namespace ProSecuritiesTrading.PSTTrader.Core.Output
{
    /// <summary>
    /// Логика взаимодействия для OutputWindow.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {
        private StringBuilder sbOutput;
        private System.Threading.Timer timerOutput;
        private TimerCallback timerCallbackTimerOutput;

        public OutputWindow()
        {
            InitializeComponent();

            this.sbOutput = new StringBuilder();
            timerCallbackTimerOutput = new TimerCallback(OnTimerOutput);
            timerOutput = new Timer(timerCallbackTimerOutput, null, 1, 500);
        }

        private void Output_Loaded(object sender, RoutedEventArgs e)
        {
            OutputEventArgs.OutputEvent += OnOutput;
        }

        private void OnOutput(object sender, OutputEventArgs e)
        {
            try
            {
                this.sbOutput.Append(e.Message + "\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("OnOutput:\n" + ex.ToString());
            }
        }

        private void OnTimerOutput(object state)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                try
                {
                    if (this.sbOutput.Length > 0)
                    {
                        textBoxOutput.AppendText(this.sbOutput.ToString());

                        if (textBoxOutput.SelectionStart == textBoxOutput.Text.Length)
                            textBoxOutput.ScrollToEnd();

                        this.sbOutput.Clear();
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("OnTimerOutput:\n" + ex.ToString());
                    this.sbOutput.Clear();
                }
            }));
        }

        private void Output_Closed(object sender, EventArgs e)
        {
            OutputEventArgs.OutputEvent -= OnOutput;

            if (this.timerOutput != null)
            {
                this.timerOutput.Dispose();
                this.timerOutput = null;
            }
        }
    }
}
