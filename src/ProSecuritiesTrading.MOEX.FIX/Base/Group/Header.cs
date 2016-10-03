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
using ProSecuritiesTrading.MOEX.FIX.Base.Message;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Group
{
    public class Header
    {
        // Tag = 8, String, FIX.4.4
        public readonly byte[] BeginString;

        public readonly byte[] BodyLength = null;
        public readonly byte[] MsgType = null;
        public readonly byte[] SenderCompID;
        public readonly byte[] TargetCompID;
        public readonly byte[] MsgSeqNum = null;
        public byte PossDupFlag = 0;
        public byte PossResend = 0;
        public readonly byte[] SendingTime = null;
        public readonly byte[] OrigSendingTime = null;
        public readonly byte[] SenderAndTargetCompIDWithSOH;

        public Header(byte[] beginString, string senderCompID, string targetCompID, byte possDupFlag, byte possResend)
        {
            this.BeginString = new byte[beginString.Length];
            Buffer.BlockCopy(beginString, 0, this.BeginString, 0, beginString.Length);
            this.SenderCompID = StringConverter.GetBytes("49=" + senderCompID);
            this.TargetCompID = StringConverter.GetBytes("56=" + targetCompID);
            this.PossDupFlag = possDupFlag;
            this.PossResend = possResend;

            int index = 0;
            int x;
            this.SenderAndTargetCompIDWithSOH = new byte[this.SenderCompID.Length + this.TargetCompID.Length + 2];

            if (this.SenderCompID.Length < 5)
            {
                for (x = 0; x < this.SenderCompID.Length; x++)
                {
                    this.SenderAndTargetCompIDWithSOH[index] = this.SenderCompID[x];
                    index++;
                }
            }
            else
            {
                Buffer.BlockCopy(this.SenderCompID, 0, this.SenderAndTargetCompIDWithSOH, index, this.SenderCompID.Length);
                index += this.SenderCompID.Length;
            }

            this.SenderAndTargetCompIDWithSOH[index] = Messages.SOH;
            index++;

            if (this.TargetCompID.Length < 5)
            {
                for (x = 0; x < this.TargetCompID.Length; x++)
                {
                    this.SenderAndTargetCompIDWithSOH[index] = this.TargetCompID[x];
                    index++;
                }
            }
            else
            {
                Buffer.BlockCopy(this.TargetCompID, 0, this.SenderAndTargetCompIDWithSOH, index, this.TargetCompID.Length);
                index += this.TargetCompID.Length;
            }

            this.SenderAndTargetCompIDWithSOH[index] = Messages.SOH;
        }

        public static void GetBytes()
        {

        }
    }
}
