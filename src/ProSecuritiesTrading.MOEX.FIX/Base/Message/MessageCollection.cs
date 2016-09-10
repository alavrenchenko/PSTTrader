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

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message
{
    public class MessageCollection<T> where T : class
    {
        private T[] messageArray;
        private const int defaultCapacity = 4;
        private const int maxArrayLength = 2146435071;
        public int currentMsgSeqNum = 0;
        private object syncRoot = new object();

        public MessageCollection()
        {
            this.messageArray = new T[defaultCapacity];
        }

        public MessageCollection(int capacity)
        {
            if (capacity < defaultCapacity)
            {
                capacity = defaultCapacity;
            }
            else if (capacity > maxArrayLength)
            {
                capacity = maxArrayLength;
            }

            this.messageArray = new T[capacity];
        }

        public void Add(int msgSeqNum, T message)
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
                        throw new ArgumentOutOfRangeException("capacity", "MsgSeqNum >" + maxArrayLength);
                    }

                    T[] newMessageArray = new T[capacity];
                    Array.Copy(this.messageArray, 0, newMessageArray, 0, this.messageArray.Length);
                    this.messageArray = newMessageArray;
                }

                this.messageArray[msgSeqNum - 1] = message;
                this.currentMsgSeqNum = msgSeqNum;
            }
        }

        public T this[int msgSeqNum]
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
