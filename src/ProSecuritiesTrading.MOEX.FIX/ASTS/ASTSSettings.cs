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

using ProSecuritiesTrading.PSTTrader.Core.Base;

namespace ProSecuritiesTrading.MOEX.FIX.ASTS
{
    public class ASTSSettings : ConnectionSettings
    {
        public string[] SenderCompID;

        /*
         * TargetCompID, IpArray, PortArray:
         * UAT Equities or FX MFIX Trade = 0
         * UAT Equities or FX MFIX Trade Capture = 1
         * UAT Equities or FX Drop Copy = 2
         * */

        public string[] TargetCompID;
        public string[] IpArray;
        public int[] PortArray;

        /// <summary>
        /// </summary>
        /// <param name="marketType">Stock market = 0, Currency (FX) market = 1.</param>
        public ASTSSettings(byte marketType)
        {
            base.Provider = PSTTrader.Core.Base.Provider.MOEX_ASTS_FIX;
            base.Ip = "";
            base.Port = 0;

            if (marketType == 0)
            {
                base.Name = "MOEX_ASTS_FIX: Stock market";

                this.SenderCompID = new string[0];
                this.TargetCompID = new string[3];
                this.IpArray = new string[3];
                this.PortArray = new int[3];
                base.Password = "";
            }
            else
            {
                base.Name = "MOEX_ASTS_FIX: Currency (FX) market";

                this.SenderCompID = new string[0];
                this.TargetCompID = new string[3];
                this.IpArray = new string[3];
                this.PortArray = new int[3];
                base.Password = "";
            }
        }
    }
}
