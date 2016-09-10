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
using System.Diagnostics;

using ProSecuritiesTrading.PSTTrader.Core.Output;
using ProSecuritiesTrading.MOEX.FIX.Base.Field;
using ProSecuritiesTrading.MOEX.FIX.Base.Group;
using ProSecuritiesTrading.MOEX.FIX.Base.Converter;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class Logon
    {
        // 35=A
        public static readonly byte[] MsgTypeBytes;

        static Logon()
        {
            int length = MsgType.TagBytes.Length;
            MsgTypeBytes = new byte[length + 2];

            for (int x = 0; x < length; x++)
            {
                MsgTypeBytes[x] = MsgType.TagBytes[x];
            }

            MsgTypeBytes[length] = 61; // =
            MsgTypeBytes[length + 1] = 65; // A
        }

        public unsafe static LogonData GetLogonData(byte* pBytes)
        {


            HeaderData headerData = new HeaderData();
            headerData.MsgType = "A";

            LogonData logonData = new LogonData(null, headerData);

            return logonData;
        }

        public static LogonData GetLogonData(byte[] buffer, int index)
        {
            int bufferLength = buffer.Length;
            bool boolValue = true;
            byte[] valueBytes = null;

            HeaderData headerData = new HeaderData();
            headerData.MsgType = "A";

            LogonData logonData = new LogonData(buffer, headerData);

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

                            if (buffer[index + 1] == 61)
                            {
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
                            }
                            else if ((buffer[index++] == 53) && (buffer[index] == 52) && (buffer[index + 1] == 61)) // 54 (Password (554))
                            {
                                index += 2;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                index += valueBytes.Length;
                            }
                            else
                            {
                                boolValue = false;
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

                                if (IntConverter.ParseInt32(valueBytes, out msgSeqNumValue) == true)
                                {
                                    headerData.MsgSeqNum = msgSeqNumValue;
                                }

                                index += valueBytes.Length;
                            }

                            break;
                        }
                    case 57: // 9
                        {
                            index++;

                            if (buffer[index + 1] == 61)
                            {
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
                                else if (buffer[index] == 56) // 8 (EncryptMethod (98))
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

                                    byteValue = buffer[index];

                                    if ((byteValue < 48) || (byteValue > 57))
                                    {
                                        break;
                                    }

                                    logonData.EncryptMethod = byteValue - 48;
                                }
                            }
                            else if ((buffer[index++] == 50) && (buffer[index] == 53) && (buffer[index + 1] == 61)) // 25 (NewPassword (925))
                            {
                                index += 2;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                index += valueBytes.Length;
                            }
                            else
                            {
                                boolValue = false;
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
                            else if (buffer[index] == 48) // 0
                            {
                                index++;

                                if (buffer[index] == 56) // 8 (HeartBtInt (108))
                                {
                                    if (buffer[index + 1] != 61)
                                    {
                                        // Error
                                        boolValue = false;
                                        break;
                                    }

                                    index += 2;

                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 heartBtIntValue = 0;

                                    if (IntConverter.ParseInt32(valueBytes, out heartBtIntValue) == true)
                                    {
                                        logonData.HeartBtInt = heartBtIntValue;
                                    }

                                    index += valueBytes.Length;
                                }
                                else // CheckSum (10)
                                {
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
                                        logonData.CheckSum = checkSumValue;
                                    }

                                    index += valueBytes.Length;
                                }
                            }
                            else if (buffer[index] == 52) // 4
                            {
                                index++;

                                if (buffer[index] == 49) // 1 (ResetSeqNumFlag (141))
                                {
                                    if (buffer[index + 1] != 61)
                                    {
                                        // Error
                                        boolValue = false;
                                        break;
                                    }

                                    index += 2;

                                    if (buffer[index + 1] != Messages.SOH)
                                    {
                                        if (buffer[index] != Messages.SOH)
                                        {
                                            boolValue = false;
                                        }

                                        break;
                                    }

                                    logonData.ResetSeqNumFlag = buffer[index];
                                }
                                else if ((buffer[index++] == 48) && (buffer[index] == 57) && (buffer[index + 1] == 61)) // 09 (SessionStatus (1409))
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

                                    byteValue = buffer[index];

                                    if ((byteValue < 48) || (byteValue > 57))
                                    {
                                        break;
                                    }

                                    logonData.SessionStatus = byteValue - 48;
                                }
                                else
                                {
                                    boolValue = false;
                                }
                            }

                            break;
                        }
                    case 54: // 6
                        {
                            index++;

                            if (buffer[index] == 56) // 8
                            {
                                index++;

                                if ((buffer[index++] == 54) && (buffer[index] == 55) && (buffer[index + 1] == 61)) // 67 (CancelOnDisconnect (6867))
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

                                    logonData.CancelOnDisconnect = buffer[index];
                                }
                                else
                                {
                                    boolValue = false;
                                }
                            }
                            else if (buffer[index] == 57) // 9
                            {
                                index++;

                                if ((buffer[index++] == 51) && (buffer[index] == 54) && (buffer[index + 1] == 61)) // 36 (LanguageID (6936))
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

                                    logonData.LanguageID = buffer[index];
                                }
                                else
                                {
                                    boolValue = false;
                                }
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

            return logonData;
        }

        /// <summary>
        /// Сборка: x64, Оптимизированный код.
        /// </summary>
        /// <param name="password">Encoding ASCII.</param>
        /// <param name="newPassword">Encoding ASCII.</param>
        [System.Security.SecuritySafeCritical]
        public unsafe static byte[] UnsafeGetBytes(ProSecuritiesTrading.MOEX.FIX.Base.Group.Header header, int msgSeqNum, bool origSendingTime, byte encryptMethod, byte[] heartBtInt, byte resetSeqNumFlag, string password, string newPassword, byte cancelOnDisconnect, byte languageID)
        {
            //---------------------------------------------
            /*
            Stopwatch stopwatch = new Stopwatch();
            Stopwatch stopwatch2 = new Stopwatch();
            Stopwatch stopwatch3 = new Stopwatch();
            Stopwatch stopwatch4 = new Stopwatch();
            Stopwatch stopwatch5 = new Stopwatch();
            Stopwatch stopwatch6 = new Stopwatch();
            Stopwatch stopwatch7 = new Stopwatch();
            Stopwatch stopwatch8 = new Stopwatch();
            Stopwatch stopwatch9 = new Stopwatch();
            Stopwatch stopwatch10 = new Stopwatch();
            stopwatch.Start();
            */
            //---------------------------------------------

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
             *  EncryptMethod = 10
             *  HeartBtInt = 11
             *  ResetSeqNumFlag = 12
             *  Password = 13
             *  NewPassword = 14
             *  SessionStatus = 15
             *  CancelOnDisconnect (COD) = 16
             *  LanguageID = 17
             * <Trailer>
             *  CheckSum = 18
             * </Trailer>
             * */

            //---------------------------------------------
            //stopwatch2.Start();
            //---------------------------------------------

            int index = 0;
            int x;
            byte[] msgSeqNumValue = StringConverter.FormatUInt32(msgSeqNum);
            int msgSeqNumLength = msgSeqNumValue.Length + 3;

            //---------------------------------------------
            //stopwatch2.Stop();
            //stopwatch3.Start();
            //---------------------------------------------

            /*
            DateTime time = DateTime.UtcNow;
            byte[] sendingTimeValue = Encoding.ASCII.GetBytes(time.ToString(Messages.TimeFormat));
            */
            byte[] sendingTimeValue = DateTimeConverter.GetBytes(DateTime.UtcNow.Ticks);

            //---------------------------------------------
            //stopwatch3.Stop();
            //stopwatch4.Start();
            //---------------------------------------------

            //byte[] passwordValue = Encoding.ASCII.GetBytes(password);
            //byte[] newPasswordValue = (newPassword != null) ? Encoding.ASCII.GetBytes(newPassword) : null;

            // Encoding ASCII
            char[] passwordValueChars = password.ToCharArray();
            char[] newPasswordValueChars = (newPassword != null) ? newPassword.ToCharArray() : null;

            //---------------------------------------------
            //stopwatch4.Stop();
            //stopwatch5.Start();
            //---------------------------------------------

            // TagValue.Length without SOH.
            // ((TagValue) ? Length : 0) with SOH.
            // EncryptMethod Length = 4. Without SOH.
            int bodyLengthValue = MsgTypeBytes.Length + header.SenderAndTargetCompIDWithSOH.Length + msgSeqNumLength + ((header.PossDupFlag > 0) ? 5 : 0) + ((header.PossResend > 0) ? 5 : 0) + (sendingTimeValue.Length + 3) + ((origSendingTime == true) ? (sendingTimeValue.Length + 5) : 0) + 4 + heartBtInt.Length + ((resetSeqNumFlag > 0) ? 6 : 0) + (passwordValueChars.Length + 4) + ((newPasswordValueChars != null) ? (newPasswordValueChars.Length + 5) : 0) + ((cancelOnDisconnect > 0) ? 7 : 0) + ((languageID > 0) ? 7 : 0) + 6;
            byte[] bodyLengthValueBytes = StringConverter.FormatUInt32(bodyLengthValue);

            //---------------------------------------------
            //stopwatch5.Stop();
            //stopwatch6.Start();
            //---------------------------------------------

            byte[] bytes = new byte[BeginString.BeginStringBytes.Length + bodyLengthValueBytes.Length + 2 + bodyLengthValue + CheckSum.WithSOHLength + 2];

            //---------------------------------------------
            //stopwatch6.Stop();
            //stopwatch7.Start();
            //---------------------------------------------

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
                *dBytes = 65; // A
                dBytes++;

                *dBytes = Messages.SOH;
                dBytes++;
                index += 5;

                //---------------------------------------------
                //stopwatch7.Stop();
                //stopwatch8.Start();
                //---------------------------------------------

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
                    /*
                    dBytesEnd = dBytes + length;

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
                        count = length / 4;

                        for (x = 0; x < count; x++)
                        {
                            *((Int32*)dBytes) = unchecked(*((Int32*)stvBytes));
                            dBytes += 4;
                            stvBytes += 4;
                        }

                        while (dBytes < dBytesEnd)
                        {
                            *dBytes = unchecked(*stvBytes);
                            dBytes++;
                            stvBytes++;
                        }
                    }
                    */
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

                //---------------------------------------------
                //stopwatch8.Stop();
                //stopwatch9.Start();
                //---------------------------------------------

                *dBytes = 57; // 9
                dBytes++;
                *dBytes = 56; // 8
                dBytes++;
                *dBytes = 61; // =
                dBytes++;
                *dBytes = encryptMethod;
                dBytes++;

                *dBytes = Messages.SOH;
                dBytes++;
                index += 5;

                fixed (byte* pHBtIBytes = heartBtInt)
                {
                    byte* hbtiBytes = pHBtIBytes;
                    length = heartBtInt.Length;
                    pEnd = hbtiBytes + length;

                    count = length / 4;

                    for (x = 0; x < count; x++)
                    {
                        *((Int32*)dBytes) = unchecked(*((Int32*)hbtiBytes));
                        dBytes += 4;
                        hbtiBytes += 4;
                    }

                    while (hbtiBytes < pEnd)
                    {
                        *dBytes = unchecked(*hbtiBytes);
                        dBytes++;
                        hbtiBytes++;
                    }

                    index += length;
                }

                *dBytes = Messages.SOH;
                dBytes++;
                index++;

                if (resetSeqNumFlag > 0)
                {
                    *dBytes = 49; // 1
                    dBytes++;
                    *dBytes = 52; // 4
                    dBytes++;
                    *dBytes = 49; // 1
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    *dBytes = resetSeqNumFlag;
                    dBytes++;

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index += 6;
                }

                *dBytes = 53; // 5
                dBytes++;
                *dBytes = 53; // 5
                dBytes++;
                *dBytes = 52; // 4
                dBytes++;
                *dBytes = 61; // =
                dBytes++;
                index += 4;

                // Encoding ASCII
                fixed (char* pPVChars = passwordValueChars)
                {
                    char* pvChars = pPVChars;
                    length = passwordValueChars.Length;
                    char* pvCharsEnd = pvChars + length;

                    while (pvChars < pvCharsEnd)
                    {
                        *dBytes = unchecked((byte)*pvChars);
                        dBytes++;
                        pvChars++;
                    }

                    index += length;
                }

                *dBytes = Messages.SOH;
                dBytes++;
                index++;


                if (newPasswordValueChars != null)
                {
                    *dBytes = 57; // 9
                    dBytes++;
                    *dBytes = 50; // 2
                    dBytes++;
                    *dBytes = 53; // 5
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    index += 4;

                    // Encoding ASCII
                    fixed (char* pNPVChars = newPasswordValueChars)
                    {
                        char* npvChars = pNPVChars;
                        length = newPasswordValueChars.Length;
                        char* npvCharsEnd = npvChars + length;

                        while (npvChars < npvCharsEnd)
                        {
                            *dBytes = unchecked((byte)*npvChars);
                            dBytes++;
                            npvChars++;
                        }

                        index += length;
                    }

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index++;
                }

                if (cancelOnDisconnect > 0)
                {
                    *dBytes = 54; // 6
                    dBytes++;
                    *dBytes = 56; // 8
                    dBytes++;
                    *dBytes = 54; // 6
                    dBytes++;
                    *dBytes = 55; // 7
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    *dBytes = cancelOnDisconnect;
                    dBytes++;

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index += 7;
                }

                if (languageID > 0)
                {
                    *dBytes = 54; // 6
                    dBytes++;
                    *dBytes = 57; // 9
                    dBytes++;
                    *dBytes = 51; // 3
                    dBytes++;
                    *dBytes = 54; // 6
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    *dBytes = languageID;
                    dBytes++;

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index += 7;
                }

                //---------------------------------------------
                //stopwatch9.Stop();
                //stopwatch10.Start();
                //---------------------------------------------

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

                //---------------------------------------------
                //stopwatch10.Stop();
                //---------------------------------------------

            }
            //---------------------------------------------
            //stopwatch.Stop();
            //ProSecuritiesTrading.PSTTrader.Core.Output.OutputEventArgs.ProcessEventArgs(new ProSecuritiesTrading.PSTTrader.Core.Output.OutputEventArgs("Logon, GetBytes:\n   Bytes:\n      Length: " + bytes.Length.ToString() + "\n      Elapsed time, ticks:\n         Total: " + stopwatch.ElapsedTicks.ToString() + "\n         Calculated total: " + (stopwatch2.ElapsedTicks + stopwatch3.ElapsedTicks + stopwatch4.ElapsedTicks + stopwatch5.ElapsedTicks + stopwatch6.ElapsedTicks + stopwatch7.ElapsedTicks + stopwatch8.ElapsedTicks + stopwatch9.ElapsedTicks + stopwatch10.ElapsedTicks).ToString() + "\n         2: " + stopwatch2.ElapsedTicks.ToString() + "\n         3: " + stopwatch3.ElapsedTicks.ToString() + "\n         4: " + stopwatch4.ElapsedTicks.ToString() + "\n         5: " + stopwatch5.ElapsedTicks.ToString() + "\n         6: " + stopwatch6.ElapsedTicks.ToString() + "\n         7: " + stopwatch7.ElapsedTicks.ToString() + "\n         8: " + stopwatch8.ElapsedTicks.ToString() + "\n         9: " + stopwatch9.ElapsedTicks.ToString() + "\n         10: " + stopwatch10.ElapsedTicks.ToString() + "\n"));
            //---------------------------------------------

            return bytes;
        }

        public static byte[] GetBytes(ProSecuritiesTrading.MOEX.FIX.Base.Group.Header header, int msgSeqNum, bool origSendingTime, byte encryptMethod, byte[] heartBtInt, byte resetSeqNumFlag, string password, string newPassword, byte cancelOnDisconnect, byte languageID)
        {
            //---------------------------------------------
            /*
            Stopwatch stopwatch = new Stopwatch();
            Stopwatch stopwatch2 = new Stopwatch();
            Stopwatch stopwatch3 = new Stopwatch();
            Stopwatch stopwatch4 = new Stopwatch();
            Stopwatch stopwatch5 = new Stopwatch();
            Stopwatch stopwatch6 = new Stopwatch();
            Stopwatch stopwatch7 = new Stopwatch();
            Stopwatch stopwatch8 = new Stopwatch();
            Stopwatch stopwatch9 = new Stopwatch();
            Stopwatch stopwatch10 = new Stopwatch();
            stopwatch.Start();
            */
            //---------------------------------------------

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
             *  EncryptMethod = 10
             *  HeartBtInt = 11
             *  ResetSeqNumFlag = 12
             *  Password = 13
             *  NewPassword = 14
             *  SessionStatus = 15
             *  CancelOnDisconnect (COD) = 16
             *  LanguageID = 17
             * <Trailer>
             *  CheckSum = 18
             * </Trailer>
             * */

            //---------------------------------------------
            //stopwatch2.Start();
            //---------------------------------------------

            int index;
            int x;
            byte[] msgSeqNumValue = StringConverter.FormatUInt32(msgSeqNum);
            int msgSeqNumLength = msgSeqNumValue.Length + 3;

            //---------------------------------------------
            //stopwatch2.Stop();
            //---------------------------------------------

            /*
            byte[] msgSeqNum = new byte[msgSeqNumValue.Length + 3];
            
            msgSeqNum[0] = 51; // 3
            msgSeqNum[1] = 52; // 4
            msgSeqNum[2] = 61; // =

            if (msgSeqNumValue.Length < 5)
            {
                index = 3;

                for (x = 0; x < msgSeqNumValue.Length; x++)
                {
                    msgSeqNum[index] = msgSeqNumValue[x];
                    index++;
                }
            }
            else
            {
                Buffer.BlockCopy(msgSeqNumValue, 0, msgSeqNum, 3, msgSeqNumValue.Length);
            }
            */

            //---------------------------------------------
            //stopwatch3.Start();
            //---------------------------------------------

            /*
            DateTime time = DateTime.UtcNow;
            byte[] sendingTimeValue = Encoding.ASCII.GetBytes(time.ToString(Messages.TimeFormat));
            */
            byte[] sendingTimeValue = DateTimeConverter.GetBytes(DateTime.UtcNow.Ticks);

            //---------------------------------------------
            //stopwatch3.Stop();
            //stopwatch4.Start();
            //---------------------------------------------

            byte[] passwordValue = Encoding.ASCII.GetBytes(password);
            byte[] newPasswordValue = (newPassword != null) ? Encoding.ASCII.GetBytes(newPassword) : null;

            //---------------------------------------------
            //stopwatch4.Stop();
            //stopwatch5.Start();
            //---------------------------------------------

            // TagValue.Length without SOH.
            // ((TagValue) ? Length : 0) with SOH.
            // EncryptMethod Length = 4. Without SOH.
            int bodyLengthValue = MsgTypeBytes.Length + header.SenderAndTargetCompIDWithSOH.Length + msgSeqNumLength + ((header.PossDupFlag > 0) ? 5 : 0) + ((header.PossResend > 0) ? 5 : 0) + (sendingTimeValue.Length + 3) + ((origSendingTime == true) ? (sendingTimeValue.Length + 5) : 0) + 4 + heartBtInt.Length + ((resetSeqNumFlag > 0) ? 6 : 0) + (passwordValue.Length + 4) + ((newPasswordValue != null) ? (newPasswordValue.Length + 5) : 0) + ((cancelOnDisconnect > 0) ? 7 : 0) + ((languageID > 0) ? 7 : 0) + 6;
            byte[] bodyLengthValueBytes = StringConverter.FormatUInt32(bodyLengthValue);

            //---------------------------------------------
            //stopwatch5.Stop();
            //stopwatch6.Start();
            //---------------------------------------------

            byte[] bytes = new byte[BeginString.BeginStringBytes.Length + bodyLengthValueBytes.Length + 2 + bodyLengthValue + CheckSum.WithSOHLength + 2];

            //---------------------------------------------
            //stopwatch6.Stop();
            //stopwatch7.Start();
            //---------------------------------------------

            Buffer.BlockCopy(header.BeginString, 0, bytes, 0, header.BeginString.Length);
            index = header.BeginString.Length;
            bytes[index] = Messages.SOH;
            index++;

            bytes[index++] = 57; // 9
            bytes[index++] = 61; // =

            if (bodyLengthValueBytes.Length < 5)
            {
                for (x = 0; x < bodyLengthValueBytes.Length; x++)
                {
                    bytes[index] = bodyLengthValueBytes[x];
                    index++;
                }
            }
            else
            {
                Buffer.BlockCopy(bodyLengthValueBytes, 0, bytes, index, bodyLengthValueBytes.Length);
                index += bodyLengthValueBytes.Length;
            }

            bytes[index] = Messages.SOH;
            index++;

            bytes[index++] = 51; // 3
            bytes[index++] = 53; // 5
            bytes[index++] = 61; // =
            bytes[index++] = 65; // A
            bytes[index++] = Messages.SOH;

            //---------------------------------------------
            //stopwatch7.Stop();
            //stopwatch8.Start();
            //---------------------------------------------

            Buffer.BlockCopy(header.SenderAndTargetCompIDWithSOH, 0, bytes, index, header.SenderAndTargetCompIDWithSOH.Length);
            index += header.SenderAndTargetCompIDWithSOH.Length;

            bytes[index++] = 51; // 3
            bytes[index++] = 52; // 4
            bytes[index++] = 61; // =

            if (msgSeqNumValue.Length < 5)
            {
                for (x = 0; x < msgSeqNumValue.Length; x++)
                {
                    bytes[index] = msgSeqNumValue[x];
                    index++;
                }
            }
            else
            {
                Buffer.BlockCopy(msgSeqNumValue, 0, bytes, index, msgSeqNumValue.Length);
                index += msgSeqNumValue.Length;
            }

            bytes[index] = Messages.SOH;
            index++;

            if (header.PossDupFlag > 0)
            {
                bytes[index++] = 52; // 4
                bytes[index++] = 51; // 3
                bytes[index++] = 61; // =
                bytes[index++] = header.PossDupFlag;

                bytes[index] = Messages.SOH;
                index++;
            }

            if (header.PossResend > 0)
            {
                bytes[index++] = 57; // 9
                bytes[index++] = 55; // 7
                bytes[index++] = 61; // =
                bytes[index++] = header.PossResend;

                bytes[index] = Messages.SOH;
                index++;
            }


            bytes[index++] = 53; // 5
            bytes[index++] = 50; // 2
            bytes[index++] = 61; // =

            Buffer.BlockCopy(sendingTimeValue, 0, bytes, index, sendingTimeValue.Length);
            index += sendingTimeValue.Length;
            bytes[index] = Messages.SOH;
            index++;

            if (origSendingTime == true)
            {
                bytes[index++] = 49; // 1
                bytes[index++] = 50; // 2
                bytes[index++] = 50; // 2
                bytes[index++] = 61; // =

                Buffer.BlockCopy(sendingTimeValue, 0, bytes, index, sendingTimeValue.Length);
                index += sendingTimeValue.Length;
                bytes[index] = Messages.SOH;
                index++;
            }

            //---------------------------------------------
            //stopwatch8.Stop();
            //stopwatch9.Start();
            //---------------------------------------------

            bytes[index++] = 57; // 9
            bytes[index++] = 56; // 8
            bytes[index++] = 61; // =
            bytes[index++] = encryptMethod;
            bytes[index++] = Messages.SOH;

            Buffer.BlockCopy(heartBtInt, 0, bytes, index, heartBtInt.Length);
            index += heartBtInt.Length;
            bytes[index] = Messages.SOH;
            index++;

            if (resetSeqNumFlag > 0)
            {
                bytes[index++] = 49; // 1
                bytes[index++] = 52; // 4
                bytes[index++] = 49; // 1
                bytes[index++] = 61; // =
                bytes[index++] = resetSeqNumFlag;

                bytes[index] = Messages.SOH;
                index++;
            }

            bytes[index++] = 53; // 5
            bytes[index++] = 53; // 5
            bytes[index++] = 52; // 4
            bytes[index++] = 61; // =

            if (passwordValue.Length < 5)
            {
                for (x = 0; x < passwordValue.Length; x++)
                {
                    bytes[index] = passwordValue[x];
                    index++;
                }
            }
            else
            {
                Buffer.BlockCopy(passwordValue, 0, bytes, index, passwordValue.Length);
                index += passwordValue.Length;
            }

            bytes[index] = Messages.SOH;
            index++;

            if (newPasswordValue != null)
            {
                bytes[index++] = 57; // 9
                bytes[index++] = 50; // 2
                bytes[index++] = 53; // 5
                bytes[index++] = 61; // =

                if (newPasswordValue.Length < 5)
                {
                    for (x = 0; x < newPasswordValue.Length; x++)
                    {
                        bytes[index] = newPasswordValue[x];
                        index++;
                    }
                }
                else
                {
                    Buffer.BlockCopy(newPasswordValue, 0, bytes, index, newPasswordValue.Length);
                    index += newPasswordValue.Length;
                }

                bytes[index] = Messages.SOH;
                index++;
            }

            if (cancelOnDisconnect > 0)
            {
                bytes[index++] = 54; // 6
                bytes[index++] = 56; // 8
                bytes[index++] = 54; // 6
                bytes[index++] = 55; // 7
                bytes[index++] = 61; // =
                bytes[index++] = cancelOnDisconnect;

                bytes[index] = Messages.SOH;
                index++;
            }

            if (languageID > 0)
            {
                bytes[index++] = 54; // 6
                bytes[index++] = 57; // 9
                bytes[index++] = 51; // 3
                bytes[index++] = 54; // 6
                bytes[index++] = 61; // =
                bytes[index++] = languageID;

                bytes[index] = Messages.SOH;
                index++;
            }

            //---------------------------------------------
            //stopwatch9.Stop();
            //stopwatch10.Start();
            //---------------------------------------------

            int sumValue = 0;

            for (x = 0; x <= index; x++)
            {
                sumValue += bytes[x];
            }

            int checkSumValue = sumValue % 256;
            byte[] checkSum = new byte[6];
            CheckSum.WriteBytes(checkSum, checkSumValue);

            Buffer.BlockCopy(checkSum, 0, bytes, index, checkSum.Length);
            index += checkSum.Length;
            bytes[index] = Messages.SOH;

            //---------------------------------------------
            //stopwatch10.Stop();
            //stopwatch.Stop();
            //ProSecuritiesTrading.PSTTrader.Core.Output.OutputEventArgs.ProcessEventArgs(new ProSecuritiesTrading.PSTTrader.Core.Output.OutputEventArgs("Logon, GetBytes:\n   Bytes:\n      Length: " + bytes.Length.ToString() + "\n      Elapsed time, ticks:\n         Total: " + stopwatch.ElapsedTicks.ToString() + "\n         Calculated total: " + (stopwatch2.ElapsedTicks + stopwatch3.ElapsedTicks + stopwatch4.ElapsedTicks + stopwatch5.ElapsedTicks + stopwatch6.ElapsedTicks + stopwatch7.ElapsedTicks + stopwatch8.ElapsedTicks + stopwatch9.ElapsedTicks + stopwatch10.ElapsedTicks).ToString() + "\n         2: " + stopwatch2.ElapsedTicks.ToString() + "\n         3: " + stopwatch3.ElapsedTicks.ToString() + "\n         4: " + stopwatch4.ElapsedTicks.ToString() + "\n         5: " + stopwatch5.ElapsedTicks.ToString() + "\n         6: " + stopwatch6.ElapsedTicks.ToString() + "\n         7: " + stopwatch7.ElapsedTicks.ToString() + "\n         8: " + stopwatch8.ElapsedTicks.ToString() + "\n         9: " + stopwatch9.ElapsedTicks.ToString() + "\n         10: " + stopwatch10.ElapsedTicks.ToString() + "\n"));
            //---------------------------------------------

            return bytes;
        }
    }
}
