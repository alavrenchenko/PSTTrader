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
using System.Text;

using ProSecuritiesTrading.PSTTrader.Core.Output;
using ProSecuritiesTrading.MOEX.FIX.Base.Field;
using ProSecuritiesTrading.MOEX.FIX.Base.Group;
using ProSecuritiesTrading.MOEX.FIX.Base.Converter;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class Reject
    {
        // 35=3
        public static readonly byte[] MsgTypeBytes;

        static Reject()
        {
            int length = MsgType.TagBytes.Length;
            MsgTypeBytes = new byte[length + 2];

            for (int x = 0; x < length; x++)
            {
                MsgTypeBytes[x] = MsgType.TagBytes[x];
            }

            MsgTypeBytes[length] = 61; // =
            MsgTypeBytes[length + 1] = 51; // 3
        }

        public static RejectData GetRejectData(byte[] buffer, int index)
        {
            HeaderData headerData = new HeaderData();
            headerData.MsgType = "3";

            RejectData messageData = new RejectData(buffer, headerData);

            int bufferLength = buffer.Length;
            bool boolValue = true;
            byte[] valueBytes = null;
            byte byteValue;

            while (index < bufferLength)
            {
                if ((buffer[index] == 61) && (boolValue == true)) // =
                {
                    boolValue = false;
                    index++;
                    continue;
                }
                else if (buffer[index] == Messages.SOH)
                {
                    if (boolValue == false)
                    {
                        boolValue = true;
                    }

                    index++;
                    continue;
                }

                if (boolValue == false)
                {
                    index++;
                    continue;
                }

                switch (buffer[index])
                {
                    case 52: // 4
                        {
                            index++;

                            if (buffer[index + 1] != 61)
                            {
                                // Error
                                boolValue = false;
                                break;
                            }

                            if (buffer[index] == 57) // 9 (SenderCompID (49))
                            {
                                index += 2;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                headerData.SenderCompID = StringConverter.GetString(valueBytes);
                                index += valueBytes.Length;
                            }
                            else if (buffer[index] == 51) // 3 (PossDupFlag (43))
                            {
                                index += 2;

                                if (buffer[index + 1] != Messages.SOH)
                                {
                                    if (buffer[index] != Messages.SOH)
                                    {
                                        boolValue = false;
                                    }

                                    break;
                                }

                                headerData.PossDupFlag = buffer[index];
                            }
                            else if (buffer[index] == 53) // 5 (RefSeqNum (45))
                            {
                                index += 2;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                Int32 refSeqNum = 0;

                                if (IntConverter.ParsePositiveInt32(valueBytes, out refSeqNum) == true)
                                {
                                    messageData.RefSeqNum = refSeqNum;
                                }


                                headerData.SenderCompID = StringConverter.GetString(valueBytes);
                                index += valueBytes.Length;
                            }

                            break;
                        }
                    case 53: // 5
                        {
                            index++;

                            if (buffer[index + 1] != 61)
                            {
                                // Error
                                boolValue = false;
                                break;
                            }

                            if (buffer[index] == 54) // 6 (TargetCompID (56))
                            {
                                index += 2;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                headerData.TargetCompID = StringConverter.GetString(valueBytes);
                                index += valueBytes.Length;
                            }
                            else if (buffer[index] == 50) // 2 (SendingTime (52))
                            {
                                index += 2;

                                valueBytes = Messages.GetValueBytes(buffer, index);
                                Int64 ticks = 0;

                                if (DateTimeConverter.ParseDateTime(valueBytes, out ticks) == true)
                                {
                                    headerData.SendingTime = new DateTime(ticks);
                                }

                                index += valueBytes.Length;
                            }
                            else if (buffer[index] == 56) // 8 (Text (58))
                            {
                                index += 2;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                messageData.Text = Encoding.UTF8.GetString(valueBytes);
                                index += valueBytes.Length;
                            }

                            break;
                        }
                    case 51: // 3
                        {
                            index++;

                            if (buffer[index + 1] == 61)
                            {
                                if (buffer[index] == 52) // 4 (MsgSeqNum (34))
                                {
                                    index += 2;

                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 msgSeqNumValue = 0;

                                    if (IntConverter.ParsePositiveInt32(valueBytes, out msgSeqNumValue) == true)
                                    {
                                        headerData.MsgSeqNum = msgSeqNumValue;
                                    }

                                    index += valueBytes.Length;
                                }
                            }
                            else if (buffer[index + 2] == 61)
                            {
                                if (buffer[index] == 55) // 7
                                {
                                    index++;

                                    if (buffer[index] == 49) // 1
                                    {
                                        index++;
                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                        Int32 refTagIDValue = 0;

                                        if (IntConverter.ParsePositiveInt32(valueBytes, out refTagIDValue) == true)
                                        {
                                            messageData.RefTagID = refTagIDValue;
                                        }

                                        index += valueBytes.Length;
                                    }
                                    else if (buffer[index] == 50) // 2
                                    {
                                        index++;
                                        valueBytes = Messages.GetValueBytes(buffer, index);

                                        if (valueBytes.Length <= 0)
                                        {
                                            break;
                                        }

                                        char[] chars = new char[valueBytes.Length];
                                        chars[0] = (char)valueBytes[0];

                                        if (valueBytes.Length == 2)
                                        {
                                            chars[1] = (char)valueBytes[1];
                                        }

                                        messageData.RefMsgType = new String(chars);
                                        messageData.RefMsgTypeBytes = valueBytes;

                                        index += valueBytes.Length;
                                    }
                                    else if (buffer[index] == 51) // 3
                                    {
                                        index++;
                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                        Int32 sessionRejectReason = 0;

                                        if (IntConverter.ParsePositiveInt32(valueBytes, out sessionRejectReason) == true)
                                        {
                                            messageData.SessionRejectReason = sessionRejectReason;
                                        }

                                        index += valueBytes.Length;
                                    }
                                }
                                else
                                {
                                    // Error
                                    boolValue = false;
                                }
                            }
                            else
                            {
                                // Error
                                boolValue = false;
                            }

                            break;
                        }
                    case 57: // 9
                        {
                            index++;

                            if (buffer[index + 1] != 61)
                            {
                                // Error
                                boolValue = false;
                                break;
                            }

                            if (buffer[index] == 55) // 7 (PossResend (97))
                            {
                                index += 2;

                                if (buffer[index + 1] != Messages.SOH)
                                {
                                    if (buffer[index] != Messages.SOH)
                                    {
                                        boolValue = false;
                                    }

                                    break;
                                }

                                headerData.PossResend = buffer[index];
                            }

                            break;
                        }
                    case 49: // 1
                        {
                            index++;

                            if (buffer[index] == 50) // 2
                            {
                                index++;

                                if (buffer[index + 1] != 61)
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }

                                if (buffer[index] == 50) // 2 (OrigSendingTime (122))
                                {
                                    index += 2;

                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int64 ticks = 0;

                                    if (DateTimeConverter.ParseDateTime(valueBytes, out ticks) == true)
                                    {
                                        headerData.SendingTime = new DateTime(ticks);
                                    }

                                    index += valueBytes.Length;
                                }
                            }
                            else if (buffer[index] == 48) // 0 CheckSum (10)
                            {
                                index++;

                                if (buffer[index] != 61)
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }

                                index++;

                                valueBytes = Messages.GetValueBytes(buffer, index);
                                Int32 checkSumValue = 0;

                                if (IntConverter.ParseInt32(valueBytes, out checkSumValue) == true)
                                {
                                    messageData.CheckSum = checkSumValue;
                                }

                                index += valueBytes.Length;
                            }

                            break;
                        }
                    default:
                        boolValue = false;
                        index++;
                        continue;
                }

                index++;
            }

            return messageData;
        }

        /// <summary>
        /// Сборка: x64, Оптимизированный код.
        /// </summary>
        public unsafe static byte[] UnsafeGetBytes(ProSecuritiesTrading.MOEX.FIX.Base.Group.Header header, int msgSeqNum, bool origSendingTime, int refSeqNum, int refTagID, byte[] refMsgType, int sessionRejectReason, byte[] text)
        {
            /*
             * <Header>
             *  BeginString = 0
             *  BodyLength = 1
             *  MsgType = 2
             *  SenderCompID = 3
             *  TargetCompID = 4
             *  MsgSeqNum = 5
             *  PossDupFlag = 6
             *  PossResend = 7
             *  SendingTime = 8
             *  OrigSendingTime = 9
             * </Header>
             *  Text = 10
             * <Trailer>
             *  CheckSum = 11
             * </Trailer>
             * */

            int index = 0;
            int x;
            byte[] msgSeqNumValue = StringConverter.FormatUInt32(msgSeqNum);
            int msgSeqNumLength = msgSeqNumValue.Length + 3;

            byte[] sendingTimeValue = DateTimeConverter.GetBytes(DateTime.UtcNow.Ticks);

            byte[] refSeqNumValue = StringConverter.FormatUInt32(refSeqNum);
            byte[] refTagIDValue = (refTagID > -1) ? StringConverter.FormatUInt32(refTagID) : null;
            byte[] srrValue = (sessionRejectReason > -1) ? StringConverter.FormatUInt32(sessionRejectReason) : null;

            // TagValue.Length without SOH.
            // ((TagValue) ? Length : 0) with SOH.
            int bodyLengthValue = MsgTypeBytes.Length + header.SenderAndTargetCompIDWithSOH.Length + msgSeqNumLength + ((header.PossDupFlag > 0) ? 5 : 0) + ((header.PossResend > 0) ? 5 : 0) + (sendingTimeValue.Length + 3) + ((origSendingTime == true) ? (sendingTimeValue.Length + 5) : 0) + (refSeqNumValue.Length + 3) + ((refTagIDValue != null) ? (refTagIDValue.Length + 5) : 0) + ((refMsgType != null) ? (refMsgType.Length + 5) : 0) + ((srrValue != null) ? (srrValue.Length + 5) : 0) + ((text != null) ? (text.Length + 4) : 0) + 4;
            byte[] bodyLengthValueBytes = StringConverter.FormatUInt32(bodyLengthValue);

            byte[] bytes = new byte[BeginString.BeginStringBytes.Length + bodyLengthValueBytes.Length + 2 + bodyLengthValue + CheckSum.WithSOHLength + 2];

            fixed (byte* pBytes = bytes)
            {
                byte* dBytes = pBytes;
                byte* pEnd;
                int length;
                int count;

                // <Header>

                fixed (byte* pBSBytes = header.BeginString)
                {
                    byte* bsBytes = pBSBytes;
                    length = header.BeginString.Length;

                    if (length == 9)
                    {
                        *((Int64*)dBytes) = unchecked(*((Int64*)bsBytes));
                        dBytes += 8;
                        bsBytes += 8;

                        *dBytes = unchecked(*bsBytes);
                        dBytes++;
                    }
                    else
                    {
                        pEnd = bsBytes + length;
                        count = length / 4;

                        for (x = 0; x < count; x++)
                        {
                            *((Int32*)dBytes) = unchecked(*((Int32*)bsBytes));
                            dBytes += 4;
                            bsBytes += 4;
                        }

                        while (bsBytes < pEnd)
                        {
                            *dBytes = unchecked(*bsBytes);
                            dBytes++;
                            bsBytes++;
                        }
                    }

                    index += length;
                }

                *dBytes = Messages.SOH;
                dBytes++;
                index++;

                *dBytes = 57; // 9
                dBytes++;
                *dBytes = 61; // =
                dBytes++;
                index += 2;

                fixed (byte* pBLVBytes = bodyLengthValueBytes)
                {
                    byte* blvBytes = pBLVBytes;
                    length = bodyLengthValueBytes.Length;
                    pEnd = blvBytes + length;

                    if (length > 3)
                    {
                        count = length / 4;

                        for (x = 0; x < count; x++)
                        {
                            *((Int32*)dBytes) = unchecked(*((Int32*)blvBytes));
                            dBytes += 4;
                            blvBytes += 4;
                        }
                    }

                    while (blvBytes < pEnd)
                    {
                        *dBytes = unchecked(*blvBytes);
                        dBytes++;
                        blvBytes++;
                    }

                    index += length;
                }

                *dBytes = Messages.SOH;
                dBytes++;
                index++;

                *dBytes = 51; // 3
                dBytes++;
                *dBytes = 53; // 5
                dBytes++;
                *dBytes = 61; // =
                dBytes++;
                *dBytes = 51; // 3
                dBytes++;

                *dBytes = Messages.SOH;
                dBytes++;
                index += 5;

                fixed (byte* pSTCIDSBytes = header.SenderAndTargetCompIDWithSOH)
                {
                    byte* stcidsBytes = pSTCIDSBytes;
                    length = header.SenderAndTargetCompIDWithSOH.Length;
                    pEnd = stcidsBytes + length;

                    count = length / 4;

                    for (x = 0; x < count; x++)
                    {
                        *((Int32*)dBytes) = unchecked(*((Int32*)stcidsBytes));
                        dBytes += 4;
                        stcidsBytes += 4;
                    }

                    while (stcidsBytes < pEnd)
                    {
                        *dBytes = unchecked(*stcidsBytes);
                        dBytes++;
                        stcidsBytes++;
                    }

                    index += length;
                }

                *dBytes = 51; // 3
                dBytes++;
                *dBytes = 52; // 4
                dBytes++;
                *dBytes = 61; // =
                dBytes++;
                index += 3;

                fixed (byte* pMSNVBytes = msgSeqNumValue)
                {
                    byte* msnvBytes = pMSNVBytes;
                    length = msgSeqNumValue.Length;
                    pEnd = msnvBytes + length;

                    if (length > 3)
                    {
                        count = length / 4;

                        for (x = 0; x < count; x++)
                        {
                            *((Int32*)dBytes) = unchecked(*((Int32*)msnvBytes));
                            dBytes += 4;
                            msnvBytes += 4;
                        }
                    }

                    while (msnvBytes < pEnd)
                    {
                        *dBytes = unchecked(*msnvBytes);
                        dBytes++;
                        msnvBytes++;
                    }

                    index += length;
                }

                *dBytes = Messages.SOH;
                dBytes++;
                index++;

                if (header.PossDupFlag > 0)
                {
                    *dBytes = 52; // 4
                    dBytes++;
                    *dBytes = 51; // 3
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    *dBytes = header.PossDupFlag;
                    dBytes++;

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index += 5;
                }

                if (header.PossResend > 0)
                {
                    *dBytes = 57; // 9
                    dBytes++;
                    *dBytes = 55; // 7
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    *dBytes = header.PossResend;
                    dBytes++;

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index += 5;
                }

                *dBytes = 53; // 5
                dBytes++;
                *dBytes = 50; // 2
                dBytes++;
                *dBytes = 61; // =
                dBytes++;
                index += 3;

                fixed (byte* pSTVBytes = sendingTimeValue)
                {
                    //byte* stvBytes = pSTVBytes;
                    length = sendingTimeValue.Length;
                    Messages.SendingTimeCopy(dBytes, pSTVBytes, length);
                    dBytes += length;
                    index += length;
                }

                *dBytes = Messages.SOH;
                dBytes++;
                index++;

                if (origSendingTime == true)
                {
                    *dBytes = 49; // 1
                    dBytes++;
                    *dBytes = 50; // 2
                    dBytes++;
                    *dBytes = 50; // 2
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    index += 4;

                    fixed (byte* pOSTVBytes = sendingTimeValue)
                    {
                        length = sendingTimeValue.Length;
                        Messages.SendingTimeCopy(dBytes, pOSTVBytes, length);
                        dBytes += length;

                        index += length;
                    }

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index++;
                }

                // </Header>

                *dBytes = 52; // 4
                dBytes++;
                *dBytes = 53; // 5
                dBytes++;
                *dBytes = 61; // =
                dBytes++;
                index += 3;

                fixed (byte* pRSNVBytes = refSeqNumValue)
                {
                    byte* rsnvBytes = pRSNVBytes;
                    length = refSeqNumValue.Length;
                    pEnd = rsnvBytes + length;

                    if (length > 3)
                    {
                        *((Int32*)dBytes) = unchecked(*((Int32*)rsnvBytes));
                        dBytes += 4;
                        rsnvBytes += 4;
                    }

                    while (rsnvBytes < pEnd)
                    {
                        *dBytes = unchecked(*rsnvBytes);
                        dBytes++;
                        rsnvBytes++;
                    }

                    index += length;
                }

                *dBytes = Messages.SOH;
                dBytes++;
                index++;

                if (refTagIDValue != null)
                {
                    *dBytes = 51; // 3
                    dBytes++;
                    *dBytes = 55; // 7
                    dBytes++;
                    *dBytes = 49; // 1
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    index += 4;

                    fixed (byte* pRTIDVBytes = refTagIDValue)
                    {
                        byte* rtidvBytes = pRTIDVBytes;
                        length = refTagIDValue.Length;
                        pEnd = rtidvBytes + length;

                        if (length > 3)
                        {
                            *((Int32*)dBytes) = unchecked(*((Int32*)rtidvBytes));
                            dBytes += 4;
                            rtidvBytes += 4;
                        }

                        while (rtidvBytes < pEnd)
                        {
                            *dBytes = unchecked(*rtidvBytes);
                            dBytes++;
                            rtidvBytes++;
                        }

                        index += length;
                    }

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index++;
                }

                if ((refMsgType != null) && (refMsgType.Length > 0))
                {
                    *dBytes = 51; // 3
                    dBytes++;
                    *dBytes = 55; // 7
                    dBytes++;
                    *dBytes = 50; // 2
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    index += 4;

                    *dBytes = refMsgType[0];
                    dBytes++;
                    index++;

                    if (refMsgType.Length == 2)
                    {
                        *dBytes = refMsgType[1];
                        dBytes++;
                        index++;
                    }

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index++;
                }

                if (refTagIDValue != null)
                {
                    *dBytes = 51; // 3
                    dBytes++;
                    *dBytes = 55; // 7
                    dBytes++;
                    *dBytes = 51; // 3
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    index += 4;

                    fixed (byte* pSRRVBytes = srrValue)
                    {
                        byte* srrvBytes = pSRRVBytes;
                        length = srrValue.Length;
                        pEnd = srrvBytes + length;

                        while (srrvBytes < pEnd)
                        {
                            *dBytes = unchecked(*srrvBytes);
                            dBytes++;
                            srrvBytes++;
                        }

                        index += length;
                    }

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index++;
                }

                if (text != null)
                {
                    *dBytes = 53; // 5
                    dBytes++;
                    *dBytes = 56; // 8
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    index += 3;

                    fixed (byte* pTextBytes = text)
                    {
                        byte* textBytes = pTextBytes;
                        length = text.Length;
                        pEnd = textBytes + length;

                        if (length > 3)
                        {
                            count = length / 4;

                            for (x = 0; x < count; x++)
                            {
                                *((Int32*)dBytes) = unchecked(*((Int32*)textBytes));
                                dBytes += 4;
                                textBytes += 4;
                            }
                        }

                        while (textBytes < pEnd)
                        {
                            *dBytes = unchecked(*textBytes);
                            dBytes++;
                            textBytes++;
                        }

                        index += length;
                    }

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index++;
                }

                // <Trailer>

                int sumValue = 0;
                byte* srcBytes = pBytes;
                pEnd = srcBytes + index;

                while (srcBytes < pEnd)
                {
                    sumValue += *srcBytes;
                    srcBytes++;
                }

                int checkSumValue = sumValue % 256;
                byte[] checkSum = new byte[6];
                CheckSum.WriteBytes(checkSum, checkSumValue);

                fixed (byte* pCSBytes = checkSum)
                {
                    byte* csBytes = pCSBytes;

                    *((Int32*)dBytes) = unchecked(*((Int32*)csBytes));
                    dBytes += 4;
                    csBytes += 4;

                    *((Int16*)dBytes) = unchecked(*((Int16*)csBytes));
                    dBytes += 2;

                    index += 6;
                }

                *dBytes = Messages.SOH;

                // </Trailer>
            }

            return bytes;
        }

        public static byte[] GetBytes(ProSecuritiesTrading.MOEX.FIX.Base.Group.Header header, int msgSeqNum, bool origSendingTime)
        {
            return new byte[0];
        }
    }
}
