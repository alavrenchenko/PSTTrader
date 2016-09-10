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

namespace ProSecuritiesTrading.MOEX.FIX.Base.Converter
{
    public class ConverterBase
    {
        public static readonly byte[] NumeralsStringASCIIBytes;

        static ConverterBase()
        {
            NumeralsStringASCIIBytes = new byte[10];
            NumeralsStringASCIIBytes[0] = 48;
            NumeralsStringASCIIBytes[1] = 49;
            NumeralsStringASCIIBytes[2] = 50;
            NumeralsStringASCIIBytes[3] = 51;
            NumeralsStringASCIIBytes[4] = 52;
            NumeralsStringASCIIBytes[5] = 53;
            NumeralsStringASCIIBytes[6] = 54;
            NumeralsStringASCIIBytes[7] = 55;
            NumeralsStringASCIIBytes[8] = 56;
            NumeralsStringASCIIBytes[9] = 57;
        }
    }
}
