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
using System.Windows;

namespace ProSecuritiesTrading.PSTTrader.Core.Connection
{
    public class ConnectionItem : DependencyObject
    {
        public string Provider { get; set; }
        public string ConnectionName { get; set; }
        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(ConnectionItem));

        public string ButtonSettingsTag
        {
            get { return (string)GetValue(ButtonSettingsTagProperty); }
            set { SetValue(ButtonSettingsTagProperty, value); }
        }

        public static readonly DependencyProperty ButtonSettingsTagProperty = DependencyProperty.Register("ButtonSettingsTag", typeof(string), typeof(ConnectionItem));

        public string ButtonConnectDisconnectTag
        {
            get { return (string)GetValue(ButtonConnectDisconnectTagProperty); }
            set { SetValue(ButtonConnectDisconnectTagProperty, value); }
        }

        public static readonly DependencyProperty ButtonConnectDisconnectTagProperty = DependencyProperty.Register("ButtonConnectDisconnectTag", typeof(string), typeof(ConnectionItem));

        public string ButtonConnectDisconnectContent
        {
            get { return (string)GetValue(ButtonConnectDisconnectContentProperty); }
            set { SetValue(ButtonConnectDisconnectContentProperty, value); }
        }

        public static readonly DependencyProperty ButtonConnectDisconnectContentProperty = DependencyProperty.Register("ButtonConnectDisconnectContent", typeof(string), typeof(ConnectionItem));

        public bool ButtonConnectDisconnectEnabled
        {
            get { return (bool)GetValue(ButtonConnectDisconnectEnabledProperty); }
            set { SetValue(ButtonConnectDisconnectEnabledProperty, value); }
        }

        public static readonly DependencyProperty ButtonConnectDisconnectEnabledProperty = DependencyProperty.Register("ButtonConnectDisconnectEnabled", typeof(bool), typeof(ConnectionItem));
    }
}
