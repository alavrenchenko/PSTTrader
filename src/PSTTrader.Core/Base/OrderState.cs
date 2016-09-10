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
    public enum OrderState : byte
    {
        Accepted = 0,
        Cancelled = 1,
        Filled = 2,
        Initialized = 3,
        PartFilled = 4,
        PendingCancel = 5,
        PendingChange = 6,
        PendingSubmit = 7,
        Rejected = 8,
        Working = 9,
        Unknown = 10
    }
}
