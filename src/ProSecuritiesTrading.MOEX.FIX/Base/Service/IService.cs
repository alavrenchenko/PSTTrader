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

namespace ProSecuritiesTrading.MOEX.FIX.Base.Service
{
    public interface IService
    {
        void Connect();

        void Disconnect();

        /// <summary>
        /// Если сервер не отвечает или отсутствует подтверждающее сообщение Logout.
        /// </summary>
        /// <param name="reason">Сервер не отвечает = 0, Отсутствует подтверждающее сообщение Logout = 1.</param>
        void DisconnectNow(byte reason);

        void OnClientConnected(bool connected);

        void Heartbeat(byte[] testReqID);

        void TestRequest();

        unsafe void ProcessingReceivedMessage(byte* pBytes);

        void ProcessingReceivedMessage(byte[] bytes, int index);
    }
}
