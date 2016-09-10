using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProSecuritiesTrading.PSTTrader.Core.Output;
using ProSecuritiesTrading.MOEX.FIX.Base.Field;
using ProSecuritiesTrading.MOEX.FIX.Base.Group;
using ProSecuritiesTrading.MOEX.FIX.Base.Converter;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class SequenceReset
    {
        // 35=4
        public static readonly byte[] MsgTypeBytes;

        static SequenceReset()
        {
            int length = MsgType.TagBytes.Length;
            MsgTypeBytes = new byte[length + 2];

            for (int x = 0; x < length; x++)
            {
                MsgTypeBytes[x] = MsgType.TagBytes[x];
            }

            MsgTypeBytes[length] = 61; // =
            MsgTypeBytes[length + 1] = 52; // 4
        }

        public static SequenceResetData GetSequenceResetData(byte[] buffer, int index)
        {
            HeaderData headerData = new HeaderData();
            headerData.MsgType = "4";

            SequenceResetData messageData = new SequenceResetData(buffer, headerData);

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

                            break;
                        }
                    case 51: // 3
                        {
                            index++;

                            if (buffer[index + 1] != 61)
                            {
                                // Error
                                boolValue = false;
                                break;
                            }

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
                            else if (buffer[index] == 54) // 6 (NewSeqNo (36))
                            {
                                index += 2;

                                valueBytes = Messages.GetValueBytes(buffer, index);
                                Int32 newSeqNoValue = 0;

                                if (IntConverter.ParsePositiveInt32(valueBytes, out newSeqNoValue) == true)
                                {
                                    messageData.NewSeqNo = newSeqNoValue;
                                }

                                index += valueBytes.Length;
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
                                else if (buffer[index] == 51) // 3 (GapFillFlag (123))
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

                                    messageData.GapFillFlag = buffer[index];
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
        public unsafe static byte[] UnsafeGetBytes(ProSecuritiesTrading.MOEX.FIX.Base.Group.Header header, int msgSeqNum, bool origSendingTime, byte gapFillFlag, int newSeqNo)
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

            byte[] newSeqNoValue = StringConverter.FormatUInt32(newSeqNo);

            // TagValue.Length without SOH.
            // ((TagValue) ? Length : 0) with SOH.
            int bodyLengthValue = MsgTypeBytes.Length + header.SenderAndTargetCompIDWithSOH.Length + msgSeqNumLength + ((header.PossDupFlag > 0) ? 5 : 0) + ((header.PossResend > 0) ? 5 : 0) + (sendingTimeValue.Length + 3) + ((origSendingTime == true) ? (sendingTimeValue.Length + 5) : 0) + ((gapFillFlag > 0) ? 6 : 0) + (newSeqNoValue.Length + 3) + 4;
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
                *dBytes = 52; // 4
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

                if (gapFillFlag > 0)
                {
                    *dBytes = 49; // 1
                    dBytes++;
                    *dBytes = 50; // 2
                    dBytes++;
                    *dBytes = 51; // 3
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    *dBytes = gapFillFlag;
                    dBytes++;

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index += 6;
                }

                *dBytes = 51; // 3
                dBytes++;
                *dBytes = 54; // 6
                dBytes++;
                *dBytes = 61; // =
                dBytes++;
                index += 3;

                fixed (byte* pNSNVBytes = newSeqNoValue)
                {
                    byte* nsnvBytes = pNSNVBytes;
                    length = newSeqNoValue.Length;
                    pEnd = nsnvBytes + length;

                    if (length > 3)
                    {
                        *((Int32*)dBytes) = unchecked(*((Int32*)nsnvBytes));
                        dBytes += 4;
                        nsnvBytes += 4;
                    }

                    while (nsnvBytes < pEnd)
                    {
                        *dBytes = unchecked(*nsnvBytes);
                        dBytes++;
                        nsnvBytes++;
                    }

                    index += length;
                }

                *dBytes = Messages.SOH;
                dBytes++;
                index++;

                // <Trailer>

                int sumValue = 0;
                byte* srcBytes = pBytes;
                pEnd = srcBytes + index;

                while (srcBytes <= pEnd)
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
