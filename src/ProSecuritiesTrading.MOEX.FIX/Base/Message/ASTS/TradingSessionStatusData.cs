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

using ProSecuritiesTrading.MOEX.FIX.Base.Group;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class TradingSessionStatusData
    {
        public byte[] MessageBytes;
        public HeaderData Header;

        public string TradingSessionID = null;
        public byte[] TradingSessionIDBytes = null;
        /// <summary>
        /// Null: 0.
        /// </summary>
        public byte UnsolicitedIndicator = 0;
        /// <summary>
        /// Null: -1.
        /// </summary>
        public int TradSesStatus = -1;
        public string Text = null;

        // <Trailer>
        public int CheckSum = -1;
        // </Trailer>

        public TradingSessionStatusData(byte[] buffer, HeaderData header)
        {
            this.MessageBytes = buffer;
            this.Header = header;
        }
    }
}
