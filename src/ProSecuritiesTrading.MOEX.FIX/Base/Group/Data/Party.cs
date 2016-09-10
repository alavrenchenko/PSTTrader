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

using ProSecuritiesTrading.MOEX.FIX.Base;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Group.Data
{
    public class Party
    {
        /// <summary>
        /// With SOH.
        /// </summary>
        public readonly byte[] PartyBytes = null;
        public readonly string PartyID = string.Empty;
        public readonly byte PartyIDSource;
        public readonly int PartyRole;

        public Party(string partyID, byte partyIDSource, int partyRole)
        {
            this.PartyID = partyID;
            this.PartyIDSource = partyIDSource;
            this.PartyRole = partyRole;

            byte[] id = Converter.StringConverter.GetBytes(partyID);
            byte[] role;

            if (partyRole == 1)
            {
                role = Field.PartyRole.Value1Bytes;
            }
            else if (partyRole == 3)
            {
                role = Field.PartyRole.Value3Bytes;
            }
            else if (partyRole == 12)
            {
                role = Field.PartyRole.Value12Bytes;
            }
            else if (partyRole == 17)
            {
                role = Field.PartyRole.Value17Bytes;
            }
            else
            {
                this.PartyRole = -1;
                return;
            }

            // PartyID, PartyIDSource = 5 (Length), PartyRole, SOH
            byte[] bytes = new byte[id.Length + 4 + 5 + role.Length + 4 + 3];
            int index = 0;

            bytes[index++] = 52; // 4
            bytes[index++] = 52; // 4
            bytes[index++] = 56; // 8
            bytes[index++] = 61; // =
            Buffer.BlockCopy(id, 0, bytes, 0, id.Length);
            index += id.Length;
            bytes[index++] = Message.Messages.SOH;

            bytes[index++] = 52; // 4
            bytes[index++] = 52; // 4
            bytes[index++] = 55; // 7
            bytes[index++] = 61; // =
            bytes[index++] = this.PartyIDSource;
            bytes[index++] = Message.Messages.SOH;

            bytes[index++] = 52; // 4
            bytes[index++] = 53; // 5
            bytes[index++] = 50; // 2
            bytes[index++] = 61; // =
            bytes[index++] = role[0];

            if (role.Length == 2)
            {
                bytes[index++] = role[1];
            }

            bytes[index] = Message.Messages.SOH;

            this.PartyBytes = bytes;
        }
    }
}
