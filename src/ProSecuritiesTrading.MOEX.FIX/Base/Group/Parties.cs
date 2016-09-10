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

using ProSecuritiesTrading.MOEX.FIX.Base.Group.Data;
using ProSecuritiesTrading.MOEX.FIX.Base.Message;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Group
{
    public class Parties
    {
        private List<Party> parties;
        private int noPartyID = 0;
        public byte[] PartiesBytes = null;

        public Parties()
        {
            this.parties = new List<Party>();
        }

        public void Add(Party party, bool updateBytes)
        {
            lock (this.parties)
            {
                this.parties.Add(party);
            }

            if (updateBytes == true)
            {
                UpdateBytes();
            }
        }

        public void UpdateBytes()
        {
            lock (this.parties)
            {
                if (this.parties.Count == 0)
                {
                    if (this.PartiesBytes != null)
                    {
                        this.PartiesBytes = null;
                    }

                    return;
                }

                int count = 0;
                int length = 0;
                Party party = null;
                int x;

                for (x = 0; x < this.parties.Count; x++)
                {
                    party = this.parties[x];

                    if (party.PartyBytes == null)
                    {
                        continue;
                    }

                    count++;
                    length += party.PartyBytes.Length;
                }

                if (length == 0)
                {
                    if (this.PartiesBytes != null)
                    {
                        this.PartiesBytes = null;
                    }

                    return;
                }

                byte[] noPartyIDValue = null;

                if (count > 9)
                {
                    noPartyIDValue = Converter.StringConverter.FormatUInt32(count);
                }

                byte[] bytes = new byte[(((noPartyIDValue != null) ? noPartyIDValue.Length : 1) + 4) + length + 1];

                unsafe
                {
                    fixed (byte* pBytes = bytes)
                    {
                        byte* dBytes = pBytes;

                        *dBytes = 52; // 4
                        dBytes++;
                        *dBytes = 53; // 5
                        dBytes++;
                        *dBytes = 51; // 3
                        dBytes++;
                        *dBytes = 61; // =
                        dBytes++;

                        if (noPartyIDValue == null)
                        {
                            *dBytes = Converter.StringConverter.NumeralsStringASCIIBytes[count];
                            dBytes++;
                        }
                        else
                        {
                            for (x = 0; x < noPartyIDValue.Length; x++)
                            {
                                *dBytes = noPartyIDValue[x];
                                dBytes++;
                            }
                        }

                        *dBytes = Messages.SOH;
                        dBytes++;

                        for (x = 0; x < this.parties.Count; x++)
                        {
                            party = this.parties[x];

                            if (party.PartyBytes == null)
                            {
                                continue;
                            }

                            fixed (byte* pPartyBytes = party.PartyBytes)
                            {
                                byte* partyBytes = pPartyBytes;
                                int length2 = party.PartyBytes.Length;
                                byte* pEnd = partyBytes + length2;
                                int count2 = length2 / 8;

                                for (int i = 0; i < count2; i++)
                                {
                                    *(Int64*)dBytes = *(Int64*)partyBytes;
                                    dBytes += 8;
                                    partyBytes += 8;
                                }

                                if ((length2 - (count2 * 8)) > 3)
                                {
                                    *(Int32*)dBytes = *(Int32*)partyBytes;
                                    dBytes += 4;
                                    partyBytes += 4;
                                }

                                while (partyBytes < pEnd)
                                {
                                    *dBytes = *partyBytes;
                                    dBytes++;
                                    partyBytes++;
                                }
                            }
                        }
                    }
                }

                this.noPartyID = count;
                this.PartiesBytes = bytes;
            }
        }

        public unsafe static void PartiesCopy(byte* pBytes)
        {
        }

        public Party this[string partyID]
        {
            get
            {
                lock (this.parties)
                {
                    if (this.parties.Count == 0)
                    {
                        return null;
                    }

                    return this.parties.FirstOrDefault(findParty => findParty.PartyID == partyID);
                }
            }
        }

        public List<Party> PartiesCollection
        {
            get
            {
                return this.parties;
            }
        }

        public int NoPartyID
        {
            get
            {
                return this.noPartyID;
            }
        }
    }
}
