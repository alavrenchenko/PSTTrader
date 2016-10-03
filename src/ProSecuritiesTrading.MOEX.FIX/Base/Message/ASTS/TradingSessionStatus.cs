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

using ProSecuritiesTrading.MOEX.FIX.Base.Field;
using ProSecuritiesTrading.MOEX.FIX.Base.Group;
using ProSecuritiesTrading.MOEX.FIX.Base.Group.Data;
using ProSecuritiesTrading.MOEX.FIX.Base.Converter;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class TradingSessionStatus
    {
        public static TradingSessionStatusData GetTradingSessionStatusData(byte[] buffer, int index)
        {
            HeaderData headerData = new HeaderData();
            headerData.MsgType = "h";

            TradingSessionStatusData messageData = new TradingSessionStatusData(buffer, headerData);

            int bufferLength = buffer.Length;
            bool boolValue = true;
            byte[] valueBytes = null;
            byte byteValue;
            int x = 0;

            while (index < bufferLength)
            {
                byteValue = buffer[index];

                if ((byteValue == 61) && (boolValue == true)) // =
                {
                    boolValue = false;
                    index++;
                    continue;
                }
                else if (byteValue == Messages.SOH)
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

                switch (byteValue)
                {
                    case 49: // 1
                        {
                            index++;

                            if ((buffer[index] != 48) || (buffer[index + 1] != 61)) // 0= (CheckSum (10))
                            {
                                // Error
                                boolValue = false;
                                continue;
                            }

                            index += 2;
                            valueBytes = Messages.GetValueBytes(buffer, index);
                            Int32 checkSumValue = 0;

                            if (IntConverter.ParsePositiveInt32(valueBytes, out checkSumValue) == true)
                            {
                                messageData.CheckSum = checkSumValue;
                            }

                            index += valueBytes.Length;

                            break;
                        }
                    case 51: // 3
                        {
                            index++;
                            byteValue = buffer[index];

                            if (buffer[index + 1] == 61) // 00=
                            {
                                if (byteValue != 52) // 4 (MsgSeqNum (34))
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }

                                index += 2;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                Int32 msgSeqNumValue = 0;

                                if (IntConverter.ParsePositiveInt32(valueBytes, out msgSeqNumValue) == true)
                                {
                                    headerData.MsgSeqNum = msgSeqNumValue;
                                }

                                index += valueBytes.Length;
                            }
                            else if (buffer[index + 2] == 61) // 000=
                            {
                                index++;

                                if (byteValue == 51) // 3
                                {
                                    if (buffer[index] != 54) // 6 (TradingSessionID (336))
                                    {
                                        // Error
                                        boolValue = false;
                                        break;
                                    }

                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.TradingSessionID = StringConverter.GetString(valueBytes);
                                    messageData.TradingSessionIDBytes = valueBytes;
                                    index += valueBytes.Length;
                                }
                                else if (byteValue == 50) // 2
                                {
                                    if (buffer[index] != 53) // 5 (UnsolicitedIndicator (325))
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

                                    messageData.UnsolicitedIndicator = buffer[index];
                                    index++;
                                }
                                else if (byteValue == 52) // 4
                                {
                                    if (buffer[index] != 48) // 0 (TradSesStatus (340))
                                    {
                                        // Error
                                        boolValue = false;
                                        break;
                                    }

                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 tssValue = 0;

                                    if (IntConverter.ParsePositiveInt32(valueBytes, out tssValue) == true)
                                    {
                                        messageData.TradSesStatus = tssValue;
                                    }

                                    index += valueBytes.Length;
                                }
                            }
                            else
                            {
                                // Error
                                boolValue = false;
                                continue;
                            }

                            break;
                        }
                    case 52: // 4
                        {
                            if (buffer[index + 2] != 61)
                            {
                                // Error
                                boolValue = false;
                                break;
                            }

                            index++;
                            byteValue = buffer[index];
                            index += 2;

                            if (byteValue == 57) // 9 (SenderCompID (49))
                            {
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                headerData.SenderCompID = StringConverter.GetString(valueBytes);
                                index += valueBytes.Length;
                            }
                            else if (byteValue == 51) // 3 (PossDupFlag (43))
                            {
                                if (buffer[index + 1] != Messages.SOH)
                                {
                                    if (buffer[index] != Messages.SOH)
                                    {
                                        boolValue = false;
                                    }

                                    break;
                                }

                                headerData.PossDupFlag = buffer[index];
                                index++;
                            }
                            else
                            {
                                // Error
                                boolValue = false;
                                continue;
                            }

                            break;
                        }
                    case 53: // 5
                        {
                            if (buffer[index + 2] != 61)
                            {
                                // Error
                                boolValue = false;
                                break;
                            }

                            index++;
                            byteValue = buffer[index];
                            index += 2;

                            if (byteValue == 54) // 6 (TargetCompID (56))
                            {
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                headerData.TargetCompID = StringConverter.GetString(valueBytes);
                                index += valueBytes.Length;
                            }
                            else if (byteValue == 50) // 2 (SendingTime (52))
                            {
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                Int64 ticks = 0;

                                if (DateTimeConverter.ParseDateTime(valueBytes, out ticks) == true)
                                {
                                    headerData.SendingTime = new DateTime(ticks);
                                }

                                index += valueBytes.Length;
                            }
                            else if (byteValue == 56) // 8 (Text (58))
                            {
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                messageData.Text = Encoding.UTF8.GetString(valueBytes);
                                index += valueBytes.Length;
                            }
                            else
                            {
                                // Error
                                boolValue = false;
                                continue;
                            }

                            break;
                        }
                    case 57: // 9
                        {
                            index++;

                            if ((buffer[index] != 55) || (buffer[index + 1] != 61)) // 7= (PossResend (97))
                            {
                                // Error
                                boolValue = false;
                                continue;
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

                            headerData.PossResend = buffer[index];
                            index++;

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
    }
}
