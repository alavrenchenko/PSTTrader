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

using ProSecuritiesTrading.MOEX.FIX.Base.Field;
using ProSecuritiesTrading.MOEX.FIX.Base.Group;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message
{
    public class Messages
    {
        public const byte SOH = 1;
        public const byte CharacterEqual = 61;
        public readonly ProSecuritiesTrading.MOEX.FIX.Base.Group.Header Header;
        public ProSecuritiesTrading.MOEX.FIX.Base.Group.Parties Parties = null;
        private int clientMsgSeqNum = 0;
        private int serverMsgSeqNum = 0;

        /*
         * MessageBytesCollection:
         *  MsgType: (byte)
         *    Logon = 0
         *    Logout = 1
         *    Heartbeat = 2
         *    TestRequest = 3
         * */

        /*
        /// <summary>
        /// byte[][]: [0][0] = MsgType, [1] = MessageBytes
        /// </summary>
        
        /// <summary>
        /// object[]: 0 = MsgType (byte), 1 = MessageBytes (byte[])
        /// </summary>
        */
        public MessageBytesCollection ClientMessages;

        /*
        /// <summary>
        /// byte[][]: [0][0] = MsgType, [1][0] = Index, [2] = MessageBytes
        /// </summary>
        
        /// <summary>
        /// object[]: 0 = MsgType (byte), 1 = Index (byte), 2 = MessageBytes (byte[])
        /// </summary>
        */
        public MessageBytesCollection ServerMessages;

        public const string TimeFormat = "yyyyMMdd-HH:mm:ss.fff";
        public DateTime LastReceivedMessageTime = DateTime.MinValue;
        public List<int> MissingMessages;
        public byte[] TestReqID = null;
        public DateTime SentHeartbeatTime = DateTime.MinValue;
        public DateTime SentTestRequestTime = DateTime.MinValue;
        public bool HTRSent = false;

        /// <summary>
        /// 8=FIX.4.4|9=
        /// </summary>
        public static readonly byte[] BeginStringBodyLength;

        static Messages()
        {
            BeginStringBodyLength = new byte[BeginString.BeginStringBytes.Length + 3];
            Buffer.BlockCopy(BeginString.BeginStringBytes, 0, BeginStringBodyLength, 0, BeginString.BeginStringBytes.Length);
            int index = BeginString.BeginStringBytes.Length;
            BeginStringBodyLength[index++] = SOH;
            BeginStringBodyLength[index++] = 57; // 9
            BeginStringBodyLength[index] = 61; // =
        }

        public Messages(ProSecuritiesTrading.MOEX.FIX.Base.Group.Header header)
        {
            this.Header = header;

            this.ClientMessages = new MessageBytesCollection(1024);
            this.ServerMessages = new MessageBytesCollection(1024);
            this.MissingMessages = new List<int>();
        }

        public static byte[] GetValueBytes(byte[] buffer, int startIndex)
        {
            int index = startIndex;
            int length = buffer.Length;

            if (buffer[index] == Messages.SOH)
            {
                return new byte[0];
            }

            do
            {
                index++;
            }
            while ((index < length) && (buffer[index] != Messages.SOH));

            byte[] bytes = new byte[index - startIndex];
            int x = 0;

            do
            {
                bytes[x] = buffer[startIndex];
                x++;
                startIndex++;
            }
            while (startIndex < index);

            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pBytes">Destination.</param>
        public unsafe static void BytesCopy(byte* pBytes, byte* pSrcBytes, int length)
        {
            byte* dBytes = pBytes;
            byte* srcBytes = pSrcBytes;
            byte* pEnd = srcBytes + length;

#if WIN64
            int count = length / 8;

            for (int x = 0; x < count; x++)
            {
                *(Int64*)dBytes = *(Int64*)srcBytes;
                dBytes += 8;
                srcBytes += 8;
            }

            if ((length - (count * 8)) > 3)
            {
                *(Int32*)dBytes = *(Int32*)srcBytes;
                dBytes += 4;
                srcBytes += 4;
            }
#else
            int count = length / 4;

            for (int x = 0; x < count; x++)
            {
                *(Int32*)dBytes = *(Int32*)srcBytes;
                dBytes += 4;
                srcBytes += 4;
            }
#endif
            while (srcBytes < pEnd)
            {
                *dBytes = *srcBytes;
                dBytes++;
                srcBytes++;
            }
        }

        public unsafe static void SendingTimeCopy(byte* pBytes, byte* pSTVBytes, int length)
        {
            byte* dBytes = pBytes;
            byte* stvBytes = pSTVBytes;

            if (length == 21)
            {
                *((Decimal*)dBytes) = unchecked(*((Decimal*)stvBytes));
                dBytes += 16;
                stvBytes += 16;

                *((Int32*)dBytes) = unchecked(*((Int32*)stvBytes));
                dBytes += 4;
                stvBytes += 4;

                *dBytes = unchecked(*stvBytes);
                dBytes++;
            }
            else
            {
                byte* pEnd = stvBytes + length;
                int count = length / 4;

                for (int x = 0; x < count; x++)
                {
                    *((Int32*)dBytes) = unchecked(*((Int32*)stvBytes));
                    dBytes += 4;
                    stvBytes += 4;
                }

                while (stvBytes < pEnd)
                {
                    *dBytes = unchecked(*stvBytes);
                    dBytes++;
                    stvBytes++;
                }
            }
        }

        public unsafe static bool TestReqIDEquals(byte[] bytes1, byte[] bytes2)
        {
            if ((bytes1 == null) || (bytes2 == null))
            {
                return false;
            }

            int length = bytes1.Length;

            if (length != bytes2.Length)
            {
                return false;
            }

            fixed (byte* pBytes1 = bytes1, pBytes2 = bytes2)
            {
                byte* tridBytes1 = pBytes1;
                byte* tridBytes2 = pBytes2;

                if (length == 15)
                {
                    if ((*(Int64*)tridBytes1 != *(Int64*)tridBytes2) || (*(Int32*)(tridBytes1 + 8) != *(Int32*)(tridBytes2 + 8)) || (*(Int16*)(tridBytes1 + 12) != *(Int16*)(tridBytes2 + 12)) || (*(tridBytes1 + 14) != *(tridBytes2 + 14)))
                    {
                        return false;
                    }
                }
                else
                {
                    byte* pEnd = tridBytes1 + length;
                    int count = length / 4;

                    for (int x = 0; x < count; x++)
                    {
                        if (*(Int32*)tridBytes1 != *(Int32*)tridBytes2)
                        {
                            return false;
                        }

                        tridBytes1 += 4;
                        tridBytes2 += 4;
                    }

                    while (tridBytes1 < pEnd)
                    {
                        if (*tridBytes1 != *tridBytes2)
                        {
                            return false;
                        }

                        tridBytes1++;
                        tridBytes2++;
                    }
                }
            }

            return true;
        }

        public int CurrentClientMsgSeqNum
        {
            get
            {
                return this.clientMsgSeqNum;
            }
            set
            {
                this.clientMsgSeqNum = value;
            }
        }

        public int NextClientMsgSeqNum
        {
            get
            {
                return ++this.clientMsgSeqNum;
            }
            set
            {
                this.clientMsgSeqNum = value;
            }
        }

        public int CurrentServerMsgSeqNum
        {
            get
            {
                return this.serverMsgSeqNum;
            }
            set
            {
                this.serverMsgSeqNum = value;
            }
        }

        public int NextServerMsgSeqNum
        {
            get
            {
                return ++this.serverMsgSeqNum;
            }
            set
            {
                this.serverMsgSeqNum = value;
            }
        }
    }
}
