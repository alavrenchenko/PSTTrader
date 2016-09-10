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

namespace ProSecuritiesTrading.PSTTrader.Core.Base
{
    public enum Currency
    {
        RussiaRuble = 0,
        UsDollar = 1,
        Euro = 2,
        BritishPound = 3,
        JapaneseYen = 4,
        SwissFranc = 5,
        SwedishKrona = 6,
        ChinaYuan = 7,
        AustralianDollar = 8,
        CanadianDollar = 9,
        BrasilianReal = 10,
        HongKongDollar = 11,
        Unknown = 0x63,
    }
}
