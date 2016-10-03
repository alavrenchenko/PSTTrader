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
    public class OrderMassCancelReport
    {
        public static OrderMassCancelReportData GetOrderMassCancelReportData(byte[] buffer, int index)
        {
            HeaderData headerData = new HeaderData();
            headerData.MsgType = "r";

            OrderMassCancelReportData messageData = new OrderMassCancelReportData(buffer, headerData);

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
                            byteValue = buffer[index];

                            if (buffer[index + 1] == 61) // 00=
                            {
                                index += 2;

                                if (byteValue == 48) // 0 (CheckSum (10))
                                {
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 checkSumValue = 0;

                                    if (IntConverter.ParsePositiveInt32(valueBytes, out checkSumValue) == true)
                                    {
                                        messageData.CheckSum = checkSumValue;
                                    }

                                    index += valueBytes.Length;
                                }
                                else if (byteValue == 49) // 1 (ClOrdID (11))
                                {
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.ClOrdID = StringConverter.GetString(valueBytes);
                                    messageData.ClOrdIDBytes = valueBytes;
                                    index += valueBytes.Length;
                                }
                                else
                                {
                                    boolValue = false;
                                    continue;
                                }
                            }
                            else if (buffer[index + 2] == 61) // 000=
                            {
                                index++;

                                if ((byteValue == 50) && (buffer[index] == 50)) // 22 (OrigSendingTime (122))
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
                                else if ((byteValue == 54) && (buffer[index] == 55)) // 67 (SecurityType (167))
                                {
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.SecurityType = StringConverter.GetString(valueBytes);
                                    messageData.SecurityTypeBytes = valueBytes;
                                    index += valueBytes.Length;
                                }
                                else
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }
                            }
                            else if (byteValue == 61) // 0=
                            {
                                // Account (1)
                                index++;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                messageData.Account = StringConverter.GetString(valueBytes);
                                messageData.AccountBytes = valueBytes;
                                index += valueBytes.Length;
                            }
                            else
                            {
                                // Error
                                boolValue = false;
                                break;
                            }

                            break;
                        }
                    case 51: // 3
                        {
                            index++;
                            byteValue = buffer[index];

                            if (buffer[index + 1] == 61)
                            {
                                index += 2;

                                if (byteValue == 52) // 4 (MsgSeqNum (34))
                                {
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 msgSeqNumValue = 0;

                                    if (IntConverter.ParsePositiveInt32(valueBytes, out msgSeqNumValue) == true)
                                    {
                                        headerData.MsgSeqNum = msgSeqNumValue;
                                    }

                                    index += valueBytes.Length;
                                }
                                else if (byteValue == 55) // 7 (OrderID (37))
                                {
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.OrderID = StringConverter.GetString(valueBytes);
                                    messageData.OrderIDBytes = valueBytes;
                                    index += valueBytes.Length;
                                }
                                else
                                {
                                    boolValue = false;
                                    continue;
                                }
                            }
                            else if ((byteValue == 51) && (buffer[index + 1] == 54) && (buffer[index + 2] == 61)) // 36= (TradingSessionID (336))
                            {
                                index += 3;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                messageData.TradingSessionID = StringConverter.GetString(valueBytes);
                                messageData.TradingSessionIDBytes = valueBytes;
                                index += valueBytes.Length;
                            }
                            else
                            {
                                // Error
                                boolValue = false;
                                break;
                            }

                            break;
                        }
                    case 52: // 4
                        {
                            index++;
                            byteValue = buffer[index];

                            if (buffer[index + 1] == 61)
                            {
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
                                    boolValue = false;
                                    break;
                                }
                            }
                            else if (buffer[index + 2] == 61)
                            {
                                index++;

                                if (byteValue == 54) // 6
                                {
                                    if (buffer[index] == 48) // 0 (Product (460))
                                    {
                                        index += 2;
                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                        Int32 product = 0;

                                        if (IntConverter.ParsePositiveInt32(valueBytes, out product) == true)
                                        {
                                            messageData.Product = product;
                                        }

                                        index += valueBytes.Length;
                                    }
                                    else if (buffer[index] == 49) // 1 (CFICode (461))
                                    {
                                        index += 2;
                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                        messageData.CFICode = StringConverter.GetString(valueBytes);
                                        messageData.CFICodeBytes = valueBytes;
                                        index += valueBytes.Length;
                                    }
                                    else
                                    {
                                        // Error
                                        boolValue = false;
                                    }
                                }
                                else if ((byteValue == 53) && (buffer[index] == 51)) // 53 (NoPartyID (453))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 noPartyID = 0;
                                    PartyData[] parties = null;

                                    if ((IntConverter.ParsePositiveInt32(valueBytes, out noPartyID) == true) && (noPartyID > 0))
                                    {
                                        parties = new PartyData[noPartyID];
                                    }

                                    index += valueBytes.Length;

                                    if (parties == null)
                                    {
                                        // Error
                                        boolValue = false;
                                        continue;
                                    }

                                    string partyID = null;
                                    byte partyIDSource = 0;
                                    int partyRole = -1;
                                    x = 0;
                                    int number = noPartyID - 1;

                                Label_1:
                                    byte byteValue2 = buffer[index + 2];
                                    byte byteValue3 = buffer[index + 3];

                                    if ((buffer[index + 1] == 52) && (buffer[index + 4] == 61))
                                    {
                                        if (byteValue2 == 52)
                                        {
                                            if ((partyID == null) && (byteValue3 == 56))
                                            {
                                                index += 5;
                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                partyID = StringConverter.GetString(valueBytes);
                                                index += valueBytes.Length;

                                                if (partyID != null)
                                                {
                                                    if ((partyIDSource == 0) || (partyRole == -1))
                                                    {
                                                        goto Label_1;
                                                    }
                                                }
                                                else
                                                {
                                                    // Error
                                                    boolValue = false;
                                                    continue;
                                                }
                                            }
                                            else if ((partyIDSource == 0) && (byteValue3 == 55))
                                            {
                                                index += 5;

                                                if (buffer[index + 1] != Messages.SOH)
                                                {
                                                    if (buffer[index] != Messages.SOH)
                                                    {
                                                        boolValue = false;
                                                    }

                                                    break;
                                                }

                                                partyIDSource = buffer[index];
                                                index++;

                                                if (partyIDSource > 0)
                                                {
                                                    if ((partyID == null) || (partyRole == -1))
                                                    {
                                                        goto Label_1;
                                                    }
                                                }
                                                else
                                                {
                                                    // Error
                                                    boolValue = false;
                                                    continue;
                                                }
                                            }
                                        }
                                        else if ((partyRole == -1) && (byteValue2 == 53) && (byteValue3 == 50))
                                        {
                                            index += 5;
                                            valueBytes = Messages.GetValueBytes(buffer, index);

                                            if (IntConverter.ParsePositiveInt32(valueBytes, out partyRole) == false)
                                            {
                                                partyRole = -1;
                                            }

                                            index += valueBytes.Length;

                                            if (partyRole > -1)
                                            {
                                                if ((partyID == null) || (partyIDSource == 0))
                                                {
                                                    goto Label_1;
                                                }
                                            }
                                            else
                                            {
                                                // Error
                                                boolValue = false;
                                                continue;
                                            }
                                        }

                                        if ((partyID != null) && (partyIDSource > 0) && (partyRole > -1))
                                        {
                                            parties[x] = new PartyData(partyID, partyIDSource, partyRole);

                                            if (x < number)
                                            {
                                                partyID = null;
                                                partyIDSource = 0;
                                                partyRole = -1;
                                                x++;
                                                goto Label_1;
                                            }
                                        }
                                        else
                                        {
                                            // Error
                                            boolValue = false;
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        // Error
                                        boolValue = false;
                                        continue;
                                    }

                                    messageData.Parties = parties;
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
                                break;
                            }

                            break;
                        }
                    case 53: // 5
                        {
                            index++;
                            byteValue = buffer[index];

                            if (buffer[index + 1] == 61)
                            {
                                index += 2;

                                switch (byteValue)
                                {
                                    case 54: // 6 (TargetCompID (56))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            headerData.TargetCompID = StringConverter.GetString(valueBytes);
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 50: // 2 (SendingTime (52))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            Int64 ticks = 0;

                                            if (DateTimeConverter.ParseDateTime(valueBytes, out ticks) == true)
                                            {
                                                headerData.SendingTime = new DateTime(ticks);
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 53: // 5 (Symbol (55))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.Symbol = StringConverter.GetString(valueBytes);
                                            messageData.SymbolBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 52: // 4 (Side (54))
                                        {
                                            if (buffer[index + 1] != Messages.SOH)
                                            {
                                                if (buffer[index] != Messages.SOH)
                                                {
                                                    boolValue = false;
                                                }

                                                break;
                                            }

                                            messageData.Side = buffer[index];
                                            index++;

                                            break;
                                        }
                                    case 56: // 8 (Text (58))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.Text = Encoding.UTF8.GetString(valueBytes);
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    default:
                                        boolValue = false;
                                        continue;
                                }
                            }
                            else if (buffer[index + 2] == 61)
                            {
                                index++;

                                if ((byteValue == 50) && (buffer[index] == 54)) // 26 (SecondaryClOrdID (526))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.SecondaryClOrdID = StringConverter.GetString(valueBytes);
                                    messageData.SecondaryClOrdIDBytes = valueBytes;
                                    index += valueBytes.Length;
                                }
                                else if (byteValue == 51) // 3
                                {
                                    byteValue = buffer[index];

                                    if (byteValue == 48) // 0 (MassCancelRequestType (530))
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

                                        messageData.MassCancelRequestType = buffer[index];
                                        index++;
                                    }
                                    else if (byteValue == 49) // 1 (MassCancelResponse (531))
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

                                        messageData.MassCancelResponse = buffer[index];
                                        index++;
                                    }
                                    else if (byteValue == 50) // 2 (MassCancelRejectReason (532))
                                    {
                                        index += 2;
                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                        Int32 mcrrValue = 0;

                                        if (IntConverter.ParsePositiveInt32(valueBytes, out mcrrValue) == true)
                                        {
                                            messageData.MassCancelRejectReason = mcrrValue;
                                        }

                                        index += valueBytes.Length;
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
                            }
                            else
                            {
                                boolValue = false;
                            }

                            break;
                        }
                    case 54: // 6
                        {
                            index++;

                            if ((buffer[index] != 48) || (buffer[index + 1] != 61)) // 0= (TransactTime (60))
                            {
                                // Error
                                boolValue = false;
                                continue;
                            }

                            index += 2;
                            valueBytes = Messages.GetValueBytes(buffer, index);
                            Int64 ttTicks = 0;

                            if (DateTimeConverter.ParseDateTime(valueBytes, out ttTicks) == true)
                            {
                                messageData.TransactTime = new DateTime(ttTicks);
                            }

                            index += valueBytes.Length;

                            break;
                        }
                    case 57: // 9
                        {
                            index++;
                            byteValue = buffer[index];

                            if (byteValue == 55) // 7 (PossResend (97))
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

                                headerData.PossResend = buffer[index];
                                index++;
                            }
                            else if ((byteValue == 52) && (buffer[index + 1] == 49) && (buffer[index + 2] == 50) && (buffer[index + 3] == 61)) // 412= (OrigTime (9412))
                            {
                                index += 4;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                Int32 origTime = 0;

                                if (IntConverter.ParseInt32(valueBytes, out origTime) == true)
                                {
                                    messageData.OrigTime = origTime;
                                }

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

/*
public static OrderMassCancelReportData GetOrderMassCancelReportData(byte[] buffer, int index)
        {
            HeaderData headerData = new HeaderData();
            headerData.MsgType = "8";

            OrderMassCancelReportData messageData = new OrderMassCancelReportData(buffer, headerData);

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
*/