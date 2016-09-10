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

using ProSecuritiesTrading.PSTTrader.Core.Output;
using ProSecuritiesTrading.MOEX.FIX.Base.Field;
using ProSecuritiesTrading.MOEX.FIX.Base.Group;
using ProSecuritiesTrading.MOEX.FIX.Base.Group.Data;
using ProSecuritiesTrading.MOEX.FIX.Base.Converter;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class ExecutionReport
    {
        public static ExecutionReportData GetExecutionReportData(byte[] buffer, int index)
        {
            HeaderData headerData = new HeaderData();
            headerData.MsgType = "8";

            ExecutionReportData messageData = new ExecutionReportData(buffer, headerData);

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

                                switch (byteValue)
                                {
                                    case 48: // 0 (CheckSum (10))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            Int32 checkSumValue = 0;

                                            if (IntConverter.ParsePositiveInt32(valueBytes, out checkSumValue) == true)
                                            {
                                                messageData.CheckSum = checkSumValue;
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 49: // 1 (ClOrdID (11))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.ClOrdID = StringConverter.GetString(valueBytes);
                                            messageData.ClOrdIDBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 50: // 2 (Commission (12))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            double commission;

                                            if (DoubleConverter.ParseDouble(valueBytes, out commission) == true)
                                            {
                                                if (messageData.CommissionData == null)
                                                {
                                                    messageData.CommissionData = new CommissionData();
                                                }

                                                messageData.CommissionData.Commission = commission;
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 51: // 3 (CommType (13))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            int commType;

                                            if (IntConverter.ParsePositiveInt32(valueBytes, out commType) == true)
                                            {
                                                if (messageData.CommissionData == null)
                                                {
                                                    messageData.CommissionData = new CommissionData();
                                                }

                                                messageData.CommissionData.CommType = commType;
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 52: // 4 (CumQty (14))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            int cumQty;

                                            if (IntConverter.ParsePositiveInt32(valueBytes, out cumQty) == true)
                                            {
                                                messageData.CumQty = cumQty;
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 55: // 7 (ExecID)
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.ExecID = StringConverter.GetString(valueBytes);
                                            messageData.ExecIDBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    default:
                                        boolValue = false;
                                        continue;
                                }
                            }
                            else if (buffer[index + 2] == 61) // 000=
                            {
                                index++;

                                switch (byteValue)
                                {
                                    case 48: // 0
                                        {
                                            if (buffer[index] == 51) // 3 (OrdRejReason (103))
                                            {
                                                index += 2;
                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                Int32 ordRejReason;

                                                if (IntConverter.ParsePositiveInt32(valueBytes, out ordRejReason) == true)
                                                {
                                                    messageData.OrdRejReason = ordRejReason;
                                                }

                                                index += valueBytes.Length;
                                            }

                                            break;
                                        }
                                    case 50: // 2
                                        {
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

                                            break;
                                        }
                                    case 51: // 3
                                        {
                                            if (buffer[index] == 54) // 6 (NoMiscFees(136))
                                            {
                                                index += 2;
                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                Int32 noMiscFees = 0;
                                                List<MiscFeeData> miscFees = null;

                                                if ((IntConverter.ParsePositiveInt32(valueBytes, out noMiscFees) == true) && (noMiscFees > 0))
                                                {
                                                    miscFees = new List<MiscFeeData>(noMiscFees);
                                                }

                                                index += valueBytes.Length;

                                                if (miscFees == null)
                                                {
                                                    // Error
                                                    boolValue = false;
                                                    continue;
                                                }

                                                double miscFeeAmt = 0.0;
                                                int miscFeeType = 0;
                                                bool mfa = false;
                                                bool mft = false;
                                                x = 1;

                                            Label_1:
                                                if ((buffer[index + 1] == 49) && (buffer[index + 2] == 51) && (buffer[index + 4] == 61))
                                                {
                                                    index += 2;

                                                    if ((mfa == false) && (buffer[index + 1] == 55))
                                                    {
                                                        index += 3;
                                                        valueBytes = Messages.GetValueBytes(buffer, index);

                                                        if (DoubleConverter.ParseDouble(valueBytes, out miscFeeAmt) == true)
                                                        {
                                                            mfa = true;
                                                        }

                                                        index += valueBytes.Length;

                                                        if (mfa == true)
                                                        {
                                                            if (mft == false)
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
                                                    else if ((mft == false) && (buffer[index + 1] == 57))
                                                    {
                                                        index += 3;
                                                        valueBytes = Messages.GetValueBytes(buffer, index);

                                                        if (IntConverter.ParsePositiveInt32(valueBytes, out miscFeeType) == true)
                                                        {
                                                            mft = true;
                                                        }

                                                        index += valueBytes.Length;

                                                        if (mft == true)
                                                        {
                                                            if (mfa == false)
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

                                                    if ((mfa == true) && (mft == true))
                                                    {
                                                        miscFees.Add(new MiscFeeData(miscFeeAmt, miscFeeType));

                                                        if (x < noMiscFees)
                                                        {
                                                            mfa = false;
                                                            mft = false;
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

                                                messageData.MiscFees = miscFees;
                                            }
                                            else
                                            {
                                                boolValue = false;
                                            }

                                            break;
                                        }
                                    case 53: // 5
                                        {
                                            byteValue = buffer[index];
                                            index += 2;

                                            switch (byteValue)
                                            {
                                                case 48: // 0 (ExecType (150))
                                                    {
                                                        if (buffer[index + 1] != Messages.SOH)
                                                        {
                                                            if (buffer[index] != Messages.SOH)
                                                            {
                                                                boolValue = false;
                                                            }

                                                            break;
                                                        }

                                                        messageData.ExecType = buffer[index];
                                                        index++;

                                                        break;
                                                    }
                                                case 49: // 1 (LeavesQty (151))
                                                    {
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        Int32 leavesQty;

                                                        if (IntConverter.ParsePositiveInt32(valueBytes, out leavesQty) == true)
                                                        {
                                                            messageData.LeavesQty = leavesQty;
                                                        }

                                                        index += valueBytes.Length;

                                                        break;
                                                    }
                                                case 50: // 2 (CashOrderQty (152))
                                                    {
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        double cashOrderQty;

                                                        if (DoubleConverter.ParseDouble(valueBytes, out cashOrderQty) == true)
                                                        {
                                                            messageData.CashOrderQty = cashOrderQty;
                                                        }

                                                        index += valueBytes.Length;

                                                        break;
                                                    }
                                                case 57: // 9 (AccruedInterestAmt(159))
                                                    {
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        double aia;

                                                        if (DoubleConverter.ParseDouble(valueBytes, out aia) == true)
                                                        {
                                                            messageData.AccruedInterestAmt = aia;
                                                        }

                                                        index += valueBytes.Length;

                                                        break;
                                                    }
                                                default:
                                                    boolValue = false;
                                                    continue;
                                            }

                                            break;
                                        }
                                    case 54: // 6
                                        {
                                            byteValue = buffer[index];
                                            index += 2;

                                            if (byteValue == 55) // 7 (SecurityType (167))
                                            {
                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                messageData.SecurityType = StringConverter.GetString(valueBytes);
                                                messageData.SecurityTypeBytes = valueBytes;
                                                index += valueBytes.Length;
                                            }
                                            else if (byteValue == 56) // 8 (EffectiveTime (168))
                                            {
                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                Int64 ticks = 0;

                                                if (DateTimeConverter.ParseDateTime(valueBytes, out ticks) == true)
                                                {
                                                    messageData.EffectiveTime = new DateTime(ticks);
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
                                        continue;
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
                            else if (buffer[index + 3] == 61) // 0000=
                            {
                                // MaxPriceLevels (1090)
                                index++;

                                if ((byteValue != 48) || (buffer[index++] != 57) || (buffer[index] != 48))
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }

                                index += 2;

                                if ((buffer[index] == 49) && (buffer[index + 1] == Messages.SOH))
                                {
                                    messageData.MaxPriceLevels = 1;
                                    index++;
                                }
                                else
                                {
                                    Int32 mpl;

                                    if (IntConverter.ParsePositiveInt32(valueBytes, out mpl) == true)
                                    {
                                        messageData.MaxPriceLevels = mpl;
                                    }

                                    index += valueBytes.Length;
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
                    case 52: // 4
                        {
                            index++;
                            byteValue = buffer[index];

                            if (buffer[index + 1] == 61)
                            {
                                index += 2;

                                switch (byteValue)
                                {
                                    case 57: // 9 (SenderCompID (49))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            headerData.SenderCompID = StringConverter.GetString(valueBytes);
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 51: // 3 (PossDupFlag (43))
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

                                            break;
                                        }
                                    case 49: // 1 (OrigClOrdID (41))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.OrigClOrdID = StringConverter.GetString(valueBytes);
                                            messageData.OrigClOrdIDBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 48: // 0 (OrdType (40))
                                        {
                                            if (buffer[index + 1] != Messages.SOH)
                                            {
                                                if (buffer[index] != Messages.SOH)
                                                {
                                                    boolValue = false;
                                                }

                                                break;
                                            }

                                            messageData.OrdType = buffer[index];
                                            index++;

                                            break;
                                        }
                                    case 52: // 4 (Price (44))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            double price;

                                            if (DoubleConverter.ParseDouble(valueBytes, out price) == true)
                                            {
                                                messageData.Price = price;
                                            }

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
                                else if ((byteValue == 50) && (buffer[index] == 51)) // 23 // (PriceType (423))
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

                                    messageData.PriceType = buffer[index];
                                    index++;
                                }
                                else if ((byteValue == 53) && (buffer[index] == 51)) // 53 (NoPartyID (453))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 noPartyID = 0;
                                    List<PartyData> parties = null;

                                    if ((IntConverter.ParsePositiveInt32(valueBytes, out noPartyID) == true) && (noPartyID > 0))
                                    {
                                        parties = new List<PartyData>(noPartyID);
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
                                    x = 1;

                                Label_2:
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
                                                        goto Label_2;
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
                                                        goto Label_2;
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
                                                    goto Label_2;
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
                                            parties.Add(new PartyData(partyID, partyIDSource, partyRole));

                                            if (x < noPartyID)
                                            {
                                                partyID = null;
                                                partyIDSource = 0;
                                                partyRole = -1;
                                                x++;
                                                goto Label_2;
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
                                //byteValue = buffer[index];
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
                                    case 57: // 9 (TimeInForce (59))
                                        {
                                            if (buffer[index + 1] != Messages.SOH)
                                            {
                                                if (buffer[index] != Messages.SOH)
                                                {
                                                    boolValue = false;
                                                }

                                                break;
                                            }

                                            messageData.TimeInForce = buffer[index];
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
                                if (byteValue != 50) // 2
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }

                                index++;
                                byteValue = buffer[index];

                                if (byteValue == 54) // 6 (SecondaryClOrdID (526))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.SecondaryClOrdID = StringConverter.GetString(valueBytes);
                                    messageData.SecondaryClOrdIDBytes = valueBytes;
                                    index += valueBytes.Length;
                                }
                                if (byteValue == 56) // 8 (OrderCapacity (528))
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

                                    messageData.OrderCapacity = buffer[index];
                                    index++;
                                }
                                if (byteValue == 57) // 9 (OrderRestrictions (529))
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

                                    messageData.OrderRestrictions = buffer[index];
                                    index++;
                                }
                                else
                                {
                                    // Error
                                    boolValue = false;
                                }
                            }
                            else if (buffer[index + 3] == 61)
                            {
                                index++;

                                switch (byteValue)
                                {
                                    case 48: // 0
                                        {
                                            // OptionSettlDate (5020)
                                            if ((buffer[index] != 50) || (buffer[index + 1] != 48)) // 20
                                            {
                                                // Error
                                                boolValue = false;
                                                break;
                                            }

                                            index += 3;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            Int64 osdTicks = 0;

                                            if (DateTimeConverter.ParseDate(valueBytes, out osdTicks) == true)
                                            {
                                                messageData.OptionSettlDate = new DateTime(osdTicks);
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 52: // 4
                                        {
                                            // OptionSettlType (5459)
                                            if ((buffer[index] != 53) || (buffer[index + 1] != 57)) // 59
                                            {
                                                // Error
                                                boolValue = false;
                                                break;
                                            }

                                            index += 3;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.OptionSettlType = StringConverter.GetString(valueBytes);
                                            messageData.OptionSettlTypeBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 50: // 2
                                        {
                                            // TradeThruTime (5202)
                                            if ((buffer[index] != 48) || (buffer[index + 1] != 50)) // 02
                                            {
                                                // Error
                                                boolValue = false;
                                                break;
                                            }

                                            index += 3;

                                            if (buffer[index + 1] != Messages.SOH)
                                            {
                                                if (buffer[index] != Messages.SOH)
                                                {
                                                    boolValue = false;
                                                }

                                                break;
                                            }

                                            messageData.TradeThruTime = buffer[index];
                                            index++;

                                            break;
                                        }
                                    case 49: // 1
                                        {
                                            // InstitutionID (5155)
                                            if ((buffer[index] != 53) || (buffer[index + 1] != 53)) // 55
                                            {
                                                // Error
                                                boolValue = false;
                                                break;
                                            }

                                            index += 3;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.InstitutionID = StringConverter.GetString(valueBytes);
                                            messageData.InstitutionIDBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    default:
                                        // Error
                                        boolValue = false;
                                        continue;
                                }
                            }
                            else
                            {
                                boolValue = false;
                            }
                                /*
                            else
                            {
                                Int32 number = 5;
                                int endIndex = index + 3;
                                bool boolValue2 = false;

                                do
                                {
                                    byteValue = buffer[index];

                                    if ((byteValue < 48) || (byteValue > 57))
                                    {
                                        break;
                                    }

                                    number = (number * 10) + (byteValue - 48);
                                    index++;
                                }
                                while (index < endIndex);

                                if (buffer[index] != 61)
                                {
                                    // Error
                                    boolValue = false;
                                    continue;
                                }

                                switch (number)
                                {
                                    case 526:
                                        {
                                            break;
                                        }
                                    case 5020:
                                        {
                                            break;
                                        }
                                    case 5459:
                                        {
                                            break;
                                        }
                                    case 5202:
                                        {
                                            break;
                                        }
                                    case 528:
                                        {
                                            break;
                                        }
                                    case 529:
                                        {
                                            break;
                                        }
                                    case 5155:
                                        {
                                            break;
                                        }
                                }
                            }
                            */

                            break;
                        }
                    case 51: // 3
                        {
                            index++;
                            byteValue = buffer[index];

                            if (buffer[index + 1] == 61)
                            {
                                index += 2;

                                switch (byteValue)
                                {
                                    case 52: // 4 (MsgSeqNum (34))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            Int32 msgSeqNumValue = 0;

                                            if (IntConverter.ParsePositiveInt32(valueBytes, out msgSeqNumValue) == true)
                                            {
                                                headerData.MsgSeqNum = msgSeqNumValue;
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 55: // 7 (OrderID (37))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.OrderID = StringConverter.GetString(valueBytes);
                                            messageData.OrderIDBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 57: // 9 (OrdStatus (39))
                                        {
                                            messageData.OrdStatus = buffer[index];
                                            index++;

                                            break;
                                        }
                                    case 56: // 8 (OrderQty (38))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            Int32 orderQty = 0;

                                            if (IntConverter.ParsePositiveInt32(valueBytes, out orderQty) == true)
                                            {
                                                messageData.OrderQty = orderQty;
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 50: // 2 (LastQty (32))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            Int32 lastQty = 0;

                                            if (IntConverter.ParsePositiveInt32(valueBytes, out lastQty) == true)
                                            {
                                                messageData.LastQty = lastQty;
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 49: // 1 (LastPx (31))
                                        {
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            double lastPx;

                                            if (DoubleConverter.ParseDouble(valueBytes, out lastPx) == true)
                                            {
                                                messageData.LastPx = lastPx;
                                            }

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

                                if ((byteValue == 55) && (buffer[index] == 56)) // 78 (ExecRestatementReason (378))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 err = 0;

                                    if (IntConverter.ParsePositiveInt32(valueBytes, out err) == true)
                                    {
                                        messageData.ExecRestatementReason = err;
                                    }

                                    index += valueBytes.Length;
                                }
                                else if ((byteValue == 51) && (buffer[index] == 54)) // 36 (TradingSessionID (336))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.TradingSessionID = StringConverter.GetString(valueBytes);
                                    messageData.TradingSessionIDBytes = valueBytes;
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
                                break;
                            }
                            
                            break;
                        }
                    case 54: // 6
                        {
                            index++;
                            byteValue = buffer[index];

                            if (byteValue == 61) // AvgPx (6)
                            {
                                index++;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                double avgPx;

                                if (DoubleConverter.ParseDouble(valueBytes, out avgPx) == true)
                                {
                                    messageData.AvgPx = avgPx;
                                }

                                index += valueBytes.Length;
                            }
                            else if (buffer[index + 1] == 61)
                            {
                                if (byteValue == 52) // 4 (SettlDate (64))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int64 sdTicks = 0;

                                    if (DateTimeConverter.ParseDate(valueBytes, out sdTicks) == true)
                                    {
                                        messageData.SettlDate = new DateTime(sdTicks);
                                    }

                                    index += valueBytes.Length;
                                }
                                else if (byteValue == 48) // 0 (TransactTime (60))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int64 ttTicks = 0;

                                    if (DateTimeConverter.ParseDateTime(valueBytes, out ttTicks) == true)
                                    {
                                        messageData.TransactTime = new DateTime(ttTicks);
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
                                index++;

                                switch (byteValue)
                                {
                                    case 51: // 3
                                        {
                                            // WorkingIndicator (636)
                                            if ((buffer[index] != 54) || (buffer[index + 1] != 61)) // 6=
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

                                            messageData.WorkingIndicator = buffer[index];
                                            index++;

                                            break;
                                        }
                                    case 50: // 2
                                        {
                                            // TradingSessionSubID (625)
                                            if ((buffer[index] != 53) || (buffer[index + 1] != 61)) // 5=
                                            {
                                                // Error
                                                boolValue = false;
                                                break;
                                            }

                                            index += 2;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.TradingSessionSubID = StringConverter.GetString(valueBytes);
                                            messageData.TradingSessionSubIDBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 48: // 0
                                        {
                                            // CurrencyCode (6029)
                                            if ((buffer[index] != 50) || (buffer[index + 1] != 57) || (buffer[index + 2] != 61)) // 29=
                                            {
                                                // Error
                                                boolValue = false;
                                                break;
                                            }

                                            index += 3;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.CurrencyCode = StringConverter.GetString(valueBytes);
                                            messageData.CurrencyCodeBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 54: // 6
                                        {
                                            // StipulationValue (6636)
                                            if ((buffer[index] != 51) || (buffer[index + 1] != 54) || (buffer[index + 2] != 61)) // 36=
                                            {
                                                // Error
                                                boolValue = false;
                                                break;
                                            }

                                            index += 3;

                                            if (buffer[index + 1] != Messages.SOH)
                                            {
                                                if (buffer[index] != Messages.SOH)
                                                {
                                                    boolValue = false;
                                                }

                                                break;
                                            }

                                            messageData.StipulationValue = buffer[index];
                                            index++;

                                            break;
                                        }
                                    default:
                                        // Error
                                        boolValue = false;
                                        continue;
                                }
                            }

                            break;
                        }
                    case 55: // 7
                        {
                            index++;

                            if (buffer[index + 2] == 61)
                            {
                                // NoTrdRegTimestamps (768)
                                if ((buffer[index] != 54) || (buffer[index + 1] != 56)) // 68
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }

                                index += 3;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                Int32 noTrdRegTimestamps = 0;
                                List<TrdRegTimestampData> trdRegTimestamps = null;

                                if ((IntConverter.ParsePositiveInt32(valueBytes, out noTrdRegTimestamps) == true) && (noTrdRegTimestamps > 0))
                                {
                                    trdRegTimestamps = new List<TrdRegTimestampData>(noTrdRegTimestamps);
                                }

                                index += valueBytes.Length;

                                if (trdRegTimestamps == null)
                                {
                                    // Error
                                    boolValue = false;
                                    continue;
                                }

                                Int64 trdRegTimestampTicks = -1;
                                int trdRegTimestampType = -1;
                                x = 1;

                            Label_3:
                                byteValue = buffer[index + 2];

                                if ((buffer[index + 1] != 55) || (buffer[index + 4] != 61))
                                {
                                    // Error
                                    boolValue = false;
                                    continue;
                                }

                                if ((trdRegTimestampTicks == -1) && (byteValue == 54) && (buffer[index + 3] == 57))
                                {
                                    index += 5;
                                    valueBytes = Messages.GetValueBytes(buffer, index);

                                    if (DateTimeConverter.ParseDateTime(valueBytes, out trdRegTimestampTicks) == false)
                                    {
                                        trdRegTimestampTicks = -1;
                                    }

                                    index += valueBytes.Length;

                                    if (trdRegTimestampTicks == -1)
                                    {
                                        // Error
                                        boolValue = false;
                                        continue;
                                    }

                                    if (trdRegTimestampType == -1)
                                    {
                                        goto Label_3;
                                    }
                                }
                                else if ((trdRegTimestampType == -1) && (byteValue == 55) && (buffer[index + 3] == 48))
                                {
                                    index += 5;
                                    valueBytes = Messages.GetValueBytes(buffer, index);

                                    if (IntConverter.ParsePositiveInt32(valueBytes, out trdRegTimestampType) == false)
                                    {
                                        trdRegTimestampType = -1;
                                    }

                                    index += valueBytes.Length;

                                    if (trdRegTimestampType == -1)
                                    {
                                        // Error
                                        boolValue = false;
                                        continue;
                                    }

                                    if (trdRegTimestampTicks == -1)
                                    {
                                        goto Label_3;
                                    }
                                }

                                if ((trdRegTimestampTicks == -1) || (trdRegTimestampType == -1))
                                {
                                    // Error
                                    boolValue = false;
                                    continue;
                                }

                                trdRegTimestamps.Add(new TrdRegTimestampData(new DateTime(trdRegTimestampTicks), trdRegTimestampType));

                                if (x < noTrdRegTimestamps)
                                {
                                    trdRegTimestampTicks = -1;
                                    trdRegTimestampType = -1;
                                    x++;
                                    goto Label_3;
                                }

                                messageData.TrdRegTimestamps = trdRegTimestamps;
                            }
                            else if (buffer[index + 3] == 61)
                            {
                                if ((buffer[index] != 54) || (buffer[index + 1] != 57)) // 69
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }

                                index += 2;

                                if (buffer[index] == 53) // 5 (CoverID (7695))
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

                                    messageData.CoverID = buffer[index];
                                    index++;
                                }
                                else if (buffer[index] == 51) // 3 (ClientAccID (7693))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.ClientAccID = StringConverter.GetString(valueBytes);
                                    messageData.ClientAccIDBytes = valueBytes;
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

                            break;
                        }
                    case 57: // 9
                        {
                            index++;

                            if (buffer[index] == 55) // 7 (PossResend (97))
                            {
                                if (buffer[index + 1] != 61)
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (buffer[index + 3] != 61)
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }

                                switch (buffer[index])
                                {
                                    case 52: // 4
                                        {
                                            index++;

                                            // OrigTime (9412)
                                            if ((buffer[index] != 49) || (buffer[index + 1] != 50)) // 12
                                            {
                                                // Error
                                                boolValue = false;
                                                break;
                                            }

                                            index += 3;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            Int32 origTime = 0;

                                            if (IntConverter.ParseInt32(valueBytes, out origTime) == true)
                                            {
                                                messageData.OrigTime = origTime;
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 49: // 1
                                        {
                                            index++;

                                            // TradeIssueDate (9173)
                                            if ((buffer[index] != 55) || (buffer[index + 1] != 51)) // 73
                                            {
                                                // Error
                                                boolValue = false;
                                                break;
                                            }

                                            index += 3;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            Int64 tidTicks = 0;

                                            if (DateTimeConverter.ParseDate(valueBytes, out tidTicks) == true)
                                            {
                                                messageData.TradeIssueDate = new DateTime(tidTicks);
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 57: // 9
                                        {
                                            index++;

                                            // OrigOrderID (9945)
                                            if ((buffer[index] != 52) || (buffer[index + 1] != 53)) // 45
                                            {
                                                // Error
                                                boolValue = false;
                                                break;
                                            }

                                            index += 3;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.OrigOrderID = StringConverter.GetString(valueBytes);
                                            messageData.OrigOrderIDBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 53: // 5
                                        {
                                            index++;

                                            // ParentID (9580)
                                            if ((buffer[index] != 56) || (buffer[index + 1] != 48)) // 80
                                            {
                                                // Error
                                                boolValue = false;
                                                break;
                                            }

                                            index += 3;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.ParentID = StringConverter.GetString(valueBytes);
                                            messageData.ParentIDBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    default:
                                        // Error
                                        boolValue = false;
                                        continue;
                                }
                            }

                            if (buffer[index + 1] != 61)
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
                                    index++;
                                }
                            }
                            else if (buffer[index + 3] == 61)
                            {
                                switch (buffer[index])
                                {

                                }
                            }
                            else
                            {
                                // Error
                                boolValue = false;
                            }

                            break;
                        }
                    case 50: // 2
                        {
                            if (buffer[index + 3] != 61)
                            {
                                // Error
                                boolValue = false;
                                break;
                            }

                            index++;
                            byteValue = buffer[index];

                            if ((byteValue == 51) && (buffer[index + 1] == 54)) // 36 (Yield (236))
                            {
                                index += 3;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                double yield;

                                if (DoubleConverter.ParseDouble(valueBytes, out yield) == true)
                                {
                                    messageData.Yield = yield;
                                }

                                index += valueBytes.Length;
                            }
                            else if ((byteValue == 55) && (buffer[index + 1] == 56)) // 78 (MDEntryID (278))
                            {
                                index += 3;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                messageData.MDEntryID = StringConverter.GetString(valueBytes);
                                messageData.MDEntryIDBytes = valueBytes;
                                index += valueBytes.Length;
                            }
                            else
                            {
                                // Error
                                boolValue = false;
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
