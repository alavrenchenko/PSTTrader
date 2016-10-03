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

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message
{
    public class MessageBytesCollection
    {
        // byte[][][]
        private byte[][] messageArray;
        private const int defaultCapacity = 4;
        private const int maxArrayLength = 2146435071;
        public int currentMsgSeqNum = 0;
        private object syncRoot = new object();

        /*
        // MsgType
        public const byte Logon = 0;
        public const byte Logout = 1;
        public const byte Heartbeat = 2;
        public const byte TestRequest = 3;

        
        public static readonly byte[] Logon;
        public static readonly byte[] Logout;
        public static readonly byte[] Heartbeat;
        public static readonly byte[] TestRequest;

        // Index
        public static readonly byte[] Index;

        static MessageBytesCollection()
        {
            // Index
            Index = new byte[1];
            Index[0] = 0;

            // MsgType
            Logon = new byte[1];
            Logon[0] = 0;

            Logout = new byte[1];
            Logout[0] = 1;

            Heartbeat = new byte[1];
            Heartbeat[0] = 2;

            TestRequest = new byte[1];
            TestRequest[0] = 3;
        }
        */

        public MessageBytesCollection()
        {
            //this.messageArray = new byte[defaultCapacity][][];
            this.messageArray = new byte[defaultCapacity][];
        }

        public MessageBytesCollection(int capacity)
        {
            if (capacity < defaultCapacity)
            {
                capacity = defaultCapacity;
            }
            else if (capacity > maxArrayLength)
            {
                capacity = maxArrayLength;
            }

            //this.messageArray = new byte[capacity][][];
            this.messageArray = new byte[capacity][];
        }

        // byte[][] message
        public void Add(int msgSeqNum, byte[] message)
        {
            lock (this.syncRoot)
            {
                if (msgSeqNum > this.messageArray.Length)
                {
                    UInt32 capacity = (UInt32)this.messageArray.Length * 2;

                    if ((capacity <= msgSeqNum) && (capacity < maxArrayLength))
                    {
                        do
                        {
                            capacity *= 2;
                        }
                        while ((capacity <= msgSeqNum) && (capacity < maxArrayLength));
                    }

                    if (capacity > maxArrayLength)
                    {
                        capacity = maxArrayLength;
                    }

                    if (capacity < msgSeqNum)
                    {
                        throw new ArgumentOutOfRangeException("capacity", "MsgSeqNum > " + maxArrayLength);
                    }

                    //byte[][][] newMessageArray = new byte[capacity][][];
                    byte[][] newMessageArray = new byte[capacity][];
                    Array.Copy(this.messageArray, 0, newMessageArray, 0, this.messageArray.Length);
                    this.messageArray = newMessageArray;
                }

                this.messageArray[msgSeqNum - 1] = message;
                this.currentMsgSeqNum = msgSeqNum;
            }
        }

        // byte[][]
        public byte[] this[int msgSeqNum]
        {
            get
            {
                if (msgSeqNum > this.currentMsgSeqNum)
                {
                    return null;
                }

                return this.messageArray[msgSeqNum - 1];
            }
            set
            {
                if (msgSeqNum > this.currentMsgSeqNum)
                {
                    return;
                }

                this.messageArray[msgSeqNum - 1] = value;
            }
        }

        public int CurrentMsgSeqNum
        {
            get
            {
                return this.currentMsgSeqNum;
            }
        }
    }
}
