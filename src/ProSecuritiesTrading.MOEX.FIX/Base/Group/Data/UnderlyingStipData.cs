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

using ProSecuritiesTrading.MOEX.FIX.Base.Converter;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Group.Data
{
    public class UnderlyingStipData
    {
        public string UnderlyingStipType = null;
        public byte[] UnderlyingStipTypeBytes = null;
        public string UnderlyingStipValue = null;
        public byte[] UnderlyingStipValueBytes = null;

        public UnderlyingStipData(byte[] ustBytes, byte[] usvBytes)
        {
            this.UnderlyingStipTypeBytes = ustBytes;
            this.UnderlyingStipType = StringConverter.GetString(ustBytes);

            if (usvBytes == null)
            {
                return;
            }

            this.UnderlyingStipValueBytes = usvBytes;
            this.UnderlyingStipValue = StringConverter.GetString(usvBytes);
        }
    }
}
