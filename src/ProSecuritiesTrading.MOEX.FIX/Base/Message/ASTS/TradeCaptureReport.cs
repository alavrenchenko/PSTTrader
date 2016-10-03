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

using ProSecuritiesTrading.MOEX.FIX.Base.Field;
using ProSecuritiesTrading.MOEX.FIX.Base.Group;
using ProSecuritiesTrading.MOEX.FIX.Base.Group.Data;
using ProSecuritiesTrading.MOEX.FIX.Base.Converter;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class TradeCaptureReport
    {
        public static TradeCaptureReportData GetTradeCaptureReportData(byte[] buffer, int index)
        {
            HeaderData headerData = new HeaderData();
            headerData.MsgType = "AE";

            TradeCaptureReportData messageData = new TradeCaptureReportData(buffer, headerData);

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
                                else if (byteValue == 55) // 7 (ExecID)
                                {
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.ExecID = StringConverter.GetString(valueBytes);
                                    messageData.ExecIDBytes = valueBytes;
                                    index += valueBytes.Length;
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
                                switch (byteValue)
                                {
                                    case 50: // 2
                                        {
                                            index++;

                                            if ((buffer[index] != 50) || (buffer[index + 1] != 61)) // 2= (OrigSendingTime (122))
                                            {
                                                // Error
                                                boolValue = false;
                                                continue;
                                            }

                                            index += 2;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            Int64 ticks = 0;

                                            if (DateTimeConverter.ParseDateTime(valueBytes, out ticks) == true)
                                            {
                                                headerData.SendingTime = new DateTime(ticks);
                                            }

                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 53: // 5
                                        {
                                            index++;

                                            if ((buffer[index] != 48) || (buffer[index + 1] != 61)) // 0= (ExecType (150))
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

                                            messageData.ExecType = buffer[index];
                                            index++;

                                            break;
                                        }
                                    case 54: // 6
                                        {
                                            index++;

                                            if ((buffer[index] != 55) || (buffer[index + 1] != 61)) // 7= (SecurityType (167))
                                            {
                                                // Error
                                                boolValue = false;
                                                continue;
                                            }

                                            index += 2;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            messageData.SecurityType = StringConverter.GetString(valueBytes);
                                            messageData.SecurityTypeBytes = valueBytes;
                                            index += valueBytes.Length;

                                            break;
                                        }
                                    case 48: // 0
                                        {
                                            index++;
                                            byteValue = buffer[index];

                                            if (byteValue == 49) // 1
                                            {
                                                index++;
                                                byteValue = buffer[index];

                                                if (byteValue == 48) // 0
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

                                                    if (byteValue == 52) // 4 (Price1 (10104))
                                                    {
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        double price1;

                                                        if (DoubleConverter.ParseDouble(valueBytes, out price1) == true)
                                                        {
                                                            messageData.Price1 = price1;
                                                        }

                                                        index += valueBytes.Length;
                                                    }
                                                    else if (byteValue == 53) // 5 (CurrentRepoValue (10105))
                                                    {
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        double crvValue;

                                                        if (DoubleConverter.ParseDouble(valueBytes, out crvValue) == true)
                                                        {
                                                            messageData.CurrentRepoValue = crvValue;
                                                        }

                                                        index += valueBytes.Length;
                                                    }
                                                    else if (byteValue == 54) // 6 (CurrentRepoBackValue (10106))
                                                    {
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        double crbvValue;

                                                        if (DoubleConverter.ParseDouble(valueBytes, out crbvValue) == true)
                                                        {
                                                            messageData.CurrentRepoBackValue = crbvValue;
                                                        }

                                                        index += valueBytes.Length;
                                                    }
                                                    else
                                                    {
                                                        // Error
                                                        boolValue = false;
                                                        continue;
                                                    }
                                                }
                                                else if (byteValue == 49) // 1
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

                                                    switch (byteValue)
                                                    {
                                                        case 48: // 0 (TradeBalance (10110))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                Int32 tbValue;

                                                                if (IntConverter.ParsePositiveInt32(valueBytes, out tbValue) == true)
                                                                {
                                                                    messageData.TradeBalance = tbValue;
                                                                }

                                                                index += valueBytes.Length;

                                                                break;
                                                            }
                                                        case 49: // 1 (TotalAmount (10111))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                double taValue;

                                                                if (DoubleConverter.ParseDouble(valueBytes, out taValue) == true)
                                                                {
                                                                    messageData.TotalAmount = taValue;
                                                                }

                                                                index += valueBytes.Length;

                                                                break;
                                                            }
                                                        case 55: // 7 (LastCouponPaymentValue (10117))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                double lcpvValue;

                                                                if (DoubleConverter.ParseDouble(valueBytes, out lcpvValue) == true)
                                                                {
                                                                    messageData.LastCouponPaymentValue = lcpvValue;
                                                                }

                                                                index += valueBytes.Length;

                                                                break;
                                                            }
                                                        case 56: // 8 (LastCouponPaymentDate (10118))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                Int64 lcpdTicks;

                                                                if (DateTimeConverter.ParseDate(valueBytes, out lcpdTicks) == true)
                                                                {
                                                                    messageData.LastCouponPaymentDate = new DateTime(lcpdTicks);
                                                                }

                                                                index += valueBytes.Length;

                                                                break;
                                                            }
                                                        case 57: // 9 (LastPrincipalPaymentValue (10119))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                double lppvValue;

                                                                if (DoubleConverter.ParseDouble(valueBytes, out lppvValue) == true)
                                                                {
                                                                    messageData.LastPrincipalPaymentValue = lppvValue;
                                                                }

                                                                index += valueBytes.Length;

                                                                break;
                                                            }
                                                        default:
                                                            // Error
                                                            boolValue = false;
                                                            continue;
                                                    }
                                                }
                                                else if (byteValue == 50) // 2
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

                                                    switch (byteValue)
                                                    {
                                                        case 48: // 0 (LastPrincipalPaymentDate (10120))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                Int64 lppdTicks;

                                                                if (DateTimeConverter.ParseDate(valueBytes, out lppdTicks) == true)
                                                                {
                                                                    messageData.LastPrincipalPaymentDate = new DateTime(lppdTicks);
                                                                }

                                                                index += valueBytes.Length;

                                                                break;
                                                            }
                                                        case 49: // 1 (ExpectedDiscount (10121))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                double edValue;

                                                                if (DoubleConverter.ParseDouble(valueBytes, out edValue) == true)
                                                                {
                                                                    messageData.ExpectedDiscount = edValue;
                                                                }

                                                                index += valueBytes.Length;

                                                                break;
                                                            }
                                                        case 50: // 2 (ExpectedQty (10122))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                Int32 eqValue;

                                                                if (IntConverter.ParsePositiveInt32(valueBytes, out eqValue) == true)
                                                                {
                                                                    messageData.ExpectedQty = eqValue;
                                                                }

                                                                index += valueBytes.Length;

                                                                break;
                                                            }
                                                        case 51: // 3 (ExpectedRepoValue (10123))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                double ervValue;

                                                                if (DoubleConverter.ParseDouble(valueBytes, out ervValue) == true)
                                                                {
                                                                    messageData.ExpectedRepoValue = ervValue;
                                                                }

                                                                index += valueBytes.Length;

                                                                break;
                                                            }
                                                        case 52: // 4 (ExpectedRepoBackValue (10124))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                double erbvValue;

                                                                if (DoubleConverter.ParseDouble(valueBytes, out erbvValue) == true)
                                                                {
                                                                    messageData.ExpectedRepoBackValue = erbvValue;
                                                                }

                                                                index += valueBytes.Length;

                                                                break;
                                                            }
                                                        case 53: // 5 (ExpectedReturnValue (10125))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                double expectedReturnValue;

                                                                if (DoubleConverter.ParseDouble(valueBytes, out expectedReturnValue) == true)
                                                                {
                                                                    messageData.ExpectedReturnValue = expectedReturnValue;
                                                                }

                                                                index += valueBytes.Length;

                                                                break;
                                                            }
                                                        case 54: // 6 (PreciseBalance (10126))
                                                            {
                                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                                Int32 pbValue;

                                                                if (IntConverter.ParsePositiveInt32(valueBytes, out pbValue) == true)
                                                                {
                                                                    messageData.PreciseBalance = pbValue;
                                                                }

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
                                                    // Error
                                                    boolValue = false;
                                                    continue;
                                                }
                                            }
                                            else if (byteValue == 53) // 5
                                            {
                                                index++;

                                                if ((buffer[index] != 54) || (buffer[index + 1] != 61)) // 6= (CalculatedCcyLastQty (1056))
                                                {
                                                    // Error
                                                    boolValue = false;
                                                    continue;
                                                }

                                                index += 2;
                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                double cclqValue;

                                                if (DoubleConverter.ParseDouble(valueBytes, out cclqValue) == true)
                                                {
                                                    messageData.CalculatedCcyLastQty = cclqValue;
                                                }

                                                index += valueBytes.Length;
                                            }
                                            else if (byteValue == 48) // 0
                                            {
                                                if ((buffer[index] != 48) || (buffer[++index] != 51) || (buffer[++index] != 61)) // 03= (OperationType (10003))
                                                {
                                                    // Error
                                                    boolValue = false;
                                                    continue;
                                                }

                                                index++;
                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                Int32 otValue;

                                                if (IntConverter.ParsePositiveInt32(valueBytes, out otValue) == true)
                                                {
                                                    messageData.OperationType = otValue;
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
                                        // Error
                                        boolValue = false;
                                        continue;
                                }
                            }
                            
                            break;
                        }
                    case 50: // 2
                        {
                            index++;

                            if ((buffer[index] != 51) || (buffer[++index] != 54) || (buffer[++index] != 61)) // 36= (Yield (236))
                            {
                                // Error
                                boolValue = false;
                                continue;
                            }

                            index++;
                            valueBytes = Messages.GetValueBytes(buffer, index);
                            double yield;

                            if (DoubleConverter.ParseDouble(valueBytes, out yield) == true)
                            {
                                messageData.Yield = yield;
                            }

                            index += valueBytes.Length;

                            break;
                        }
                    case 51: // 3
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
                            else if (byteValue == 50) // 2 (LastQty (32))
                            {
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                Int32 lastQty = 0;

                                if (IntConverter.ParsePositiveInt32(valueBytes, out lastQty) == true)
                                {
                                    messageData.LastQty = lastQty;
                                }

                                index += valueBytes.Length;
                            }
                            else if (byteValue == 49) // 1 (LastPx (31))
                            {
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                double lastPx;

                                if (DoubleConverter.ParseDouble(valueBytes, out lastPx) == true)
                                {
                                    messageData.LastPx = lastPx;
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
                                else if (byteValue == 52) // 4 (Price (44))
                                {
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    double price;

                                    if (DoubleConverter.ParseDouble(valueBytes, out price) == true)
                                    {
                                        messageData.Price = price;
                                    }

                                    index += valueBytes.Length;
                                }
                                else
                                {
                                    // Error
                                    boolValue = false;
                                    continue;
                                }
                            }
                            else if (buffer[index + 2] == 61)
                            {
                                if (byteValue == 54) // 6
                                {
                                    index++;

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
                                        break;
                                    }
                                }
                                else if ((byteValue == 50) && (buffer[index + 1] == 51)) // 23 (PriceType (423))
                                {
                                    index += 3;

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

                            break;
                        }
                    case 53: // 5
                        {
                            index++;
                            byteValue = buffer[index];

                            if (buffer[index + 1] == 61)
                            {
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
                                else if (byteValue == 53) // 5 (Symbol (55))
                                {
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.Symbol = StringConverter.GetString(valueBytes);
                                    messageData.SymbolBytes = valueBytes;
                                    index += valueBytes.Length;
                                }
                                else
                                {
                                    // Error
                                    boolValue = false;
                                    continue;
                                }
                            }
                            else if (buffer[index + 2] == 61)
                            {
                                if (byteValue == 55) // 7
                                {
                                    index++;

                                    if (buffer[index] == 49) // 1 (TradeReportID (571))
                                    {
                                        index += 2;
                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                        messageData.TradeReportID = StringConverter.GetString(valueBytes);
                                        messageData.TradeReportIDBytes = valueBytes;
                                        index += valueBytes.Length;
                                    }
                                    else if (buffer[index] == 48) // 0 (PreviouslyReported (570))
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

                                        messageData.PreviouslyReported = buffer[index];
                                        index++;
                                    }
                                    else
                                    {
                                        // Error
                                        boolValue = false;
                                        break;
                                    }
                                }
                                else if ((byteValue == 53) && (buffer[index + 1] == 50)) // 52 (NoSides (552))
                                {
                                    index += 3;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 noSides;
                                    SideData[] sides = null;

                                    if ((IntConverter.ParsePositiveInt32(valueBytes, out noSides) == true) && (noSides > 0))
                                    {
                                        sides = new SideData[noSides];
                                    }

                                    index += valueBytes.Length;

                                    if (sides == null)
                                    {
                                        // Error
                                        boolValue = false;
                                        continue;
                                    }

                                    x = 0;
                                    int number = noSides - 1;

                                Label_1:
                                    index++;

                                    if ((buffer[index] != 53) || (buffer[++index] != 52) && (buffer[++index] != 61)) // 54= (Side (54))
                                    {
                                        // Error
                                        if (messageData.Error == 0)
                                        {
                                            messageData.Error = 1;
                                        }

                                        boolValue = false;
                                        continue;
                                    }

                                    SideData sideData = new SideData();
                                    valueBytes = Messages.GetValueBytes(buffer, index);

                                    if (buffer[index + 1] != Messages.SOH)
                                    {
                                        if (buffer[index] != Messages.SOH)
                                        {
                                            boolValue = false;
                                        }

                                        break;
                                    }

                                    sideData.Side = buffer[index];
                                    index++;

                                Label_2:
                                    index++;
                                    byteValue = buffer[index];

                                    switch (byteValue)
                                    {
                                        case 49: // 1
                                            {
                                                index++;
                                                byteValue = buffer[index];

                                                if (byteValue == 61) // =
                                                {
                                                    // Account (1)
                                                    index++;
                                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                                    sideData.Account = StringConverter.GetString(valueBytes);
                                                    sideData.AccountBytes = valueBytes;
                                                    index += valueBytes.Length;
                                                }
                                                else if (buffer[index + 1] == 61)
                                                {
                                                    if (byteValue == 49) // 1 (ClOrdID (11))
                                                    {
                                                        index += 2;
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        sideData.ClOrdID = StringConverter.GetString(valueBytes);
                                                        sideData.ClOrdIDBytes = valueBytes;
                                                        index += valueBytes.Length;
                                                    }
                                                    else if (byteValue == 50) // 2 (Commission (12))
                                                    {
                                                        index += 2;
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        double commission;

                                                        if (DoubleConverter.ParseDouble(valueBytes, out commission) == true)
                                                        {
                                                            if (sideData.CommissionData == null)
                                                            {
                                                                sideData.CommissionData = new CommissionData();
                                                            }

                                                            sideData.CommissionData.Commission = commission;
                                                        }

                                                        index += valueBytes.Length;
                                                    }
                                                    else if (byteValue == 51) // 3 (CommType (13))
                                                    {
                                                        index += 2;
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        int commType;

                                                        if (IntConverter.ParsePositiveInt32(valueBytes, out commType) == true)
                                                        {
                                                            if (sideData.CommissionData == null)
                                                            {
                                                                sideData.CommissionData = new CommissionData();
                                                            }

                                                            sideData.CommissionData.CommType = commType;
                                                        }

                                                        index += valueBytes.Length;
                                                    }
                                                    else
                                                    {
                                                        index -= 2;
                                                        goto Label_3;
                                                    }
                                                }
                                                else
                                                {
                                                    if (byteValue == 53) // 5
                                                    {
                                                        if ((buffer[index + 1] != 57) || (buffer[index + 2] != 61)) // 9= (AccruedInterestAmt (159))
                                                        {
                                                            index -= 2;
                                                            goto Label_3;
                                                        }

                                                        index += 3;
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        double aiaValue;

                                                        if (DoubleConverter.ParseDouble(valueBytes, out aiaValue) == true)
                                                        {
                                                            sideData.AccruedInterestAmt = aiaValue;
                                                        }

                                                        index += valueBytes.Length;
                                                    }
                                                    else if (byteValue == 51) // 3
                                                    {
                                                        if ((buffer[index + 1] != 54) || (buffer[index + 2] != 61)) // 6= (NoMiscFees (136))
                                                        {
                                                            index -= 2;
                                                            goto Label_3;
                                                        }

                                                        index += 3;
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        Int32 noMiscFees = 0;
                                                        MiscFeeData[] miscFees = null;

                                                        if ((IntConverter.ParsePositiveInt32(valueBytes, out noMiscFees) == true) && (noMiscFees > 0))
                                                        {
                                                            miscFees = new MiscFeeData[noMiscFees];
                                                        }

                                                        index += valueBytes.Length;

                                                        if (miscFees == null)
                                                        {
                                                            goto Label_2;
                                                        }

                                                        double miscFeeAmt = 0.0;
                                                        int miscFeeType = 0;
                                                        bool mfa = false;
                                                        bool mft = false;
                                                        int i = 0;
                                                        int number2 = noMiscFees - 1;

                                                    Label_4:
                                                        if ((buffer[index + 1] == 49) && (buffer[index + 2] == 51) && (buffer[index + 4] == 61))
                                                        {
                                                            if ((mfa == false) && (buffer[index + 3] == 55))
                                                            {
                                                                index += 5;
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
                                                                        goto Label_4;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    // Error
                                                                    if (messageData.Error == 0)
                                                                    {
                                                                        messageData.Error = 1;
                                                                    }

                                                                    goto Label_3;
                                                                }
                                                            }
                                                            else if ((mft == false) && (buffer[index + 3] == 57))
                                                            {
                                                                index += 5;
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
                                                                        goto Label_4;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    // Error
                                                                    if (messageData.Error == 0)
                                                                    {
                                                                        messageData.Error = 1;
                                                                    }

                                                                    goto Label_3;
                                                                }
                                                            }

                                                            if ((mfa == true) && (mft == true))
                                                            {
                                                                miscFees[x] = new MiscFeeData(miscFeeAmt, miscFeeType);

                                                                if (i < number2)
                                                                {
                                                                    mfa = false;
                                                                    mft = false;
                                                                    i++;
                                                                    goto Label_4;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                // Error
                                                                if (messageData.Error == 0)
                                                                {
                                                                    messageData.Error = 1;
                                                                }

                                                                goto Label_3;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            // Error
                                                            if (messageData.Error == 0)
                                                            {
                                                                messageData.Error = 1;
                                                            }

                                                            goto Label_3;
                                                        }

                                                        sideData.MiscFees = miscFees;
                                                    }
                                                    else if (byteValue == 54) // 6
                                                    {
                                                        if ((buffer[index + 1] != 50) || (buffer[index + 2] != 61)) // 2= (SettlInstID (162))
                                                        {
                                                            index -= 2;
                                                            goto Label_3;
                                                        }

                                                        index += 3;
                                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                                        sideData.SettlInstID = StringConverter.GetString(valueBytes);
                                                        sideData.SettlInstIDBytes = valueBytes;
                                                        index += valueBytes.Length;
                                                    }
                                                    else
                                                    {
                                                        index -= 2;
                                                        goto Label_3;
                                                    }
                                                }

                                                break;
                                            }
                                        case 51: // 3
                                            {
                                                index++;
                                                byteValue = buffer[index];

                                                if (byteValue == 55) // 7 (OrderID (37))
                                                {
                                                    if (buffer[index + 1] != 61) // =
                                                    {
                                                        index -= 2;
                                                        goto Label_3;
                                                    }

                                                    index += 2;
                                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                                    sideData.OrderID = StringConverter.GetString(valueBytes);
                                                    sideData.OrderIDBytes = valueBytes;
                                                    index += valueBytes.Length;
                                                }
                                                else if (byteValue == 51) // 3
                                                {
                                                    if ((buffer[index + 1] != 54) || (buffer[index + 2] != 61)) // 6= (TradingSessionID (336))
                                                    {
                                                        index -= 2;
                                                        goto Label_3;
                                                    }

                                                    index += 3;
                                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                                    sideData.TradingSessionID = StringConverter.GetString(valueBytes);
                                                    sideData.TradingSessionIDBytes = valueBytes;
                                                    index += valueBytes.Length;
                                                }
                                                else if (byteValue == 56) // 8
                                                {
                                                    if ((buffer[index + 1] != 49) || (buffer[index + 2] != 61)) // 1= (GrossTradeAmt (381))
                                                    {
                                                        index -= 2;
                                                        goto Label_3;
                                                    }

                                                    index += 3;
                                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                                    double gtaValue;

                                                    if (DoubleConverter.ParseDouble(valueBytes, out gtaValue) == true)
                                                    {
                                                        sideData.GrossTradeAmt = gtaValue;
                                                    }

                                                    index += valueBytes.Length;
                                                }
                                                else
                                                {
                                                    index -= 2;
                                                    goto Label_3;
                                                }

                                                break;
                                            }
                                        case 52: // 4
                                            {
                                                if ((buffer[index + 1] != 53) || (buffer[index + 2] != 51) || (buffer[index + 3] != 61)) // 53= (NoPartyID (453))
                                                {
                                                    index--;
                                                    goto Label_3;
                                                }

                                                index += 4;
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
                                                    boolValue = false;
                                                    continue;
                                                }

                                                string partyID = null;
                                                byte partyIDSource = 0;
                                                int partyRole = -1;
                                                int j = 0;
                                                int number3 = noPartyID - 1;

                                            Label_5:
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
                                                                    goto Label_5;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                // Error
                                                                if (messageData.Error == 0)
                                                                {
                                                                    messageData.Error = 1;
                                                                }

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

                                                                // Error
                                                                if (messageData.Error == 0)
                                                                {
                                                                    messageData.Error = 1;
                                                                }

                                                                continue;
                                                            }

                                                            partyIDSource = buffer[index];
                                                            index++;

                                                            if (partyIDSource > 0)
                                                            {
                                                                if ((partyID == null) || (partyRole == -1))
                                                                {
                                                                    goto Label_5;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                // Error
                                                                if (messageData.Error == 0)
                                                                {
                                                                    messageData.Error = 1;
                                                                }

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
                                                                goto Label_5;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            // Error
                                                            if (messageData.Error == 0)
                                                            {
                                                                messageData.Error = 1;
                                                            }

                                                            boolValue = false;
                                                            continue;
                                                        }
                                                    }

                                                    if ((partyID != null) && (partyIDSource > 0) && (partyRole > -1))
                                                    {
                                                        parties[j] = new PartyData(partyID, partyIDSource, partyRole);

                                                        if (j < number3)
                                                        {
                                                            partyID = null;
                                                            partyIDSource = 0;
                                                            partyRole = -1;
                                                            j++;
                                                            goto Label_5;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // Error
                                                        if (messageData.Error == 0)
                                                        {
                                                            messageData.Error = 1;
                                                        }

                                                        boolValue = false;
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    // Error
                                                    if (messageData.Error == 0)
                                                    {
                                                        messageData.Error = 1;
                                                    }

                                                    boolValue = false;
                                                    continue;
                                                }

                                                sideData.Parties = parties;

                                                break;
                                            }
                                        case 53: // 5
                                            {
                                                if ((buffer[index + 1] != 50) || (buffer[index + 2] != 54) || (buffer[index + 3] != 61)) // 26= (SecondaryClOrdID (526))
                                                {
                                                    index--;
                                                    goto Label_3;
                                                }

                                                index += 4;
                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                sideData.SecondaryClOrdID = StringConverter.GetString(valueBytes);
                                                sideData.SecondaryClOrdIDBytes = valueBytes;
                                                index += valueBytes.Length;

                                                break;
                                            }
                                        case 54: // 6
                                            {
                                                if ((buffer[index + 1] != 50) || (buffer[index + 2] != 53) || (buffer[index + 3] != 61)) // 25= (TradingSessionSubID (625))
                                                {
                                                    index--;
                                                    goto Label_3;
                                                }

                                                index += 4;
                                                valueBytes = Messages.GetValueBytes(buffer, index);
                                                sideData.TradingSessionSubID = StringConverter.GetString(valueBytes);
                                                sideData.TradingSessionSubIDBytes = valueBytes;
                                                index += valueBytes.Length;

                                                break;
                                            }
                                        case 57: // 9
                                            {
                                                if ((buffer[index + 1] != 50) || (buffer[index + 3] != 61))
                                                {
                                                    index--;
                                                    goto Label_3;
                                                }

                                                byteValue = buffer[index + 2];

                                                if (byteValue == 48) // 0 (EndAccruedInterestAmt (920))
                                                {
                                                    index += 4;
                                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                                    double eaiaValue;

                                                    if (DoubleConverter.ParseDouble(valueBytes, out eaiaValue) == true)
                                                    {
                                                        sideData.EndAccruedInterestAmt = eaiaValue;
                                                    }

                                                    index += valueBytes.Length;
                                                }
                                                else if (byteValue == 49) // 1 (StartCash (921))
                                                {
                                                    index += 4;
                                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                                    double scValue;

                                                    if (DoubleConverter.ParseDouble(valueBytes, out scValue) == true)
                                                    {
                                                        sideData.StartCash = scValue;
                                                    }

                                                    index += valueBytes.Length;
                                                }
                                                else if (byteValue == 50) // 2 (EndCash (922))
                                                {
                                                    index += 4;
                                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                                    double ecValue;

                                                    if (DoubleConverter.ParseDouble(valueBytes, out ecValue) == true)
                                                    {
                                                        sideData.EndCash = ecValue;
                                                    }

                                                    index += valueBytes.Length;
                                                }
                                                else
                                                {
                                                    index--;
                                                    goto Label_3;
                                                }

                                                break;
                                            }
                                        default:
                                            index--;
                                            goto Label_3;
                                    }

                                    if (x < number)
                                    {
                                        if ((buffer[index + 1] == 53) && (buffer[index + 2] == 52) && (buffer[index + 3] == 61)) // 54= (Side (54))
                                        {
                                            goto Label_3;
                                        }
                                    }

                                    goto Label_2;

                                Label_3:
                                    if ((sideData.Parties != null) && (sideData.TradingSessionID != null) && (sideData.TradingSessionSubID != null))
                                    {
                                        sides[x] = sideData;

                                        if (x < number)
                                        {
                                            x++;
                                            goto Label_1;
                                        }
                                    }
                                    else
                                    {
                                        // Error
                                        if (messageData.Error == 0)
                                        {
                                            messageData.Error = 1;
                                        }

                                        boolValue = false;
                                        continue;
                                    }

                                    messageData.Sides = sides;
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

                            break;
                        }
                    case 54: // 6
                        {
                            index++;
                            byteValue = buffer[index];

                            if (buffer[index + 1] == 61)
                            {
                                index += 2;

                                if (byteValue == 48) // 0 (TransactTime (60))
                                {
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int64 ttTicks = 0;

                                    if (DateTimeConverter.ParseDateTime(valueBytes, out ttTicks) == true)
                                    {
                                        messageData.TransactTime = new DateTime(ttTicks);
                                    }

                                    index += valueBytes.Length;
                                }
                                else if (byteValue == 51) // 3 (SettlType (63))
                                {
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.SettlType = StringConverter.GetString(valueBytes);
                                    messageData.SettlTypeBytes = valueBytes;
                                    index += valueBytes.Length;
                                }
                                else if (byteValue == 52) // 4 (SettlDate (64))
                                {
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int64 sdTicks = 0;

                                    if (DateTimeConverter.ParseDate(valueBytes, out sdTicks) == true)
                                    {
                                        messageData.SettlDate = new DateTime(sdTicks);
                                    }

                                    index += valueBytes.Length;
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
                                if (byteValue == 52) // 4
                                {
                                    if ((buffer[index + 1] != 48) || (buffer[index + 2] != 61)) // 0= (Price2 (640))
                                    {
                                        // Error
                                        boolValue = false;
                                        break;
                                    }

                                    index += 3;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    double price2;

                                    if (DoubleConverter.ParseDouble(valueBytes, out price2) == true)
                                    {
                                        messageData.Price2 = price2;
                                    }

                                    index += valueBytes.Length;
                                }
                                else if (byteValue == 48) // 0
                                {
                                    if ((buffer[index + 1] != 50) || (buffer[index + 2] != 57) || (buffer[index + 3] != 61)) // 29= (CurrencyCode (6029))
                                    {
                                        // Error
                                        boolValue = false;
                                        break;
                                    }

                                    index += 4;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.CurrencyCode = StringConverter.GetString(valueBytes);
                                    messageData.CurrencyCodeBytes = valueBytes;
                                    index += valueBytes.Length;
                                }
                                else
                                {
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
                            byteValue = buffer[index];

                            if (buffer[index + 1] == 61)
                            {
                                if (byteValue != 53) // 5 (TradeDate (75))
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }

                                index += 2;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                Int64 tdTicks = 0;

                                if (DateTimeConverter.ParseDate(valueBytes, out tdTicks) == true)
                                {
                                    messageData.TradeDate = new DateTime(tdTicks);
                                }

                                index += valueBytes.Length;
                            }
                            else
                            {
                                if (byteValue == 49) // 1
                                {
                                    index++;

                                    if ((buffer[index] != 49) || (buffer[++index] != 61)) // 1= (NoUnderlyings (711))
                                    {
                                        // Error
                                        boolValue = false;
                                        continue;
                                    }

                                    index++;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 noUnderlyings;
                                    UnderlyingInstrumentData[] underlyingInstruments = null;

                                    if ((IntConverter.ParsePositiveInt32(valueBytes, out noUnderlyings) == true) && (noUnderlyings > 0))
                                    {
                                        underlyingInstruments = new UnderlyingInstrumentData[noUnderlyings];
                                    }

                                    index += valueBytes.Length;

                                    if (underlyingInstruments == null)
                                    {
                                        boolValue = false;
                                        continue;
                                    }

                                    x = 0;
                                    int number4 = noUnderlyings - 1;

                                Label_6:
                                    UnderlyingInstrumentData uid = new UnderlyingInstrumentData();
                                    byteValue = buffer[index + 1];

                                    if ((byteValue == 51) && (uid.UnderlyingSymbol == null) && (buffer[index + 2] == 49) && (buffer[index + 3] == 49) && (buffer[index + 4] == 61)) // 311= (UnderlyingSymbol (311))
                                    {
                                        index += 5;
                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                        uid.UnderlyingSymbol = StringConverter.GetString(valueBytes);
                                        uid.UnderlyingSymbolBytes = valueBytes;
                                        index += valueBytes.Length;
                                    }

                                    if ((byteValue == 56) && (uid.UnderlyingStipulations == null) && (buffer[index + 2] == 56) && (buffer[index + 3] == 55) && (buffer[index + 4] == 61)) // 887= (NoUnderlyingStips (887))
                                    {
                                        index += 5;
                                        valueBytes = Messages.GetValueBytes(buffer, index);
                                        Int32 nusValue;
                                        UnderlyingStipData[] underlyingStipulations = null;

                                        if ((IntConverter.ParsePositiveInt32(valueBytes, out nusValue) == true) && (nusValue > 0))
                                        {
                                            underlyingStipulations = new UnderlyingStipData[nusValue];
                                        }

                                        index += valueBytes.Length;

                                        if (underlyingStipulations == null)
                                        {
                                            if ((buffer[index + 1] == 56) && (buffer[index + 2] == 56) && (buffer[index + 3] == 56) && (buffer[index + 4] == 61)) // 888= (UnderlyingStipType (888))
                                            {
                                                index += 5;

                                                while ((index < bufferLength) && (buffer[index] != Messages.SOH))
                                                {
                                                    index++;
                                                }
                                            }

                                            if ((buffer[index + 1] == 56) && (buffer[index + 2] == 56) && (buffer[index + 3] == 57) && (buffer[index + 4] == 61)) // 889= (UnderlyingStipValue (889))
                                            {
                                                index += 5;

                                                while ((index < bufferLength) && (buffer[index] != Messages.SOH))
                                                {
                                                    index++;
                                                }
                                            }

                                            goto Label_7;
                                        }

                                        int a = 0;
                                        int number5 = nusValue - 1;

                                    Label_8:
                                        byte[] ustBytes = null;
                                        byte[] usvBytes = null;

                                        if ((buffer[index + 1] == 56) && (buffer[index + 2] == 56) && (buffer[index + 3] == 56) && (buffer[index + 4] == 61)) // 888= (UnderlyingStipType (888))
                                        {
                                            index += 5;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            ustBytes = valueBytes;
                                            index += valueBytes.Length;
                                        }

                                        if ((buffer[index + 1] == 56) && (buffer[index + 2] == 56) && (buffer[index + 3] == 57) && (buffer[index + 4] == 61)) // 889= (UnderlyingStipValue (889))
                                        {
                                            index += 5;
                                            valueBytes = Messages.GetValueBytes(buffer, index);
                                            usvBytes = valueBytes;
                                            index += valueBytes.Length;
                                        }
                                        
                                        if (ustBytes != null)
                                        {
                                            underlyingStipulations[a] = new UnderlyingStipData(ustBytes, usvBytes);

                                            if (a < number5)
                                            {
                                                a++;
                                                goto Label_8;
                                            }
                                        }
                                        else
                                        {
                                            // Error
                                            boolValue = false;
                                            continue;
                                        }

                                        uid.UnderlyingStipulations = underlyingStipulations;
                                    }

                                Label_7:
                                    /*
                                    if ((uid.UnderlyingSymbol != null) || (uid.UnderlyingStipulations != null))
                                    {

                                    }
                                    */

                                    underlyingInstruments[x] = uid;

                                    if (x < number4)
                                    {
                                        x++;
                                        goto Label_6;
                                    }

                                    messageData.UnderlyingInstruments = underlyingInstruments;
                                }
                                else if (byteValue == 54) // 6
                                {
                                    index++;

                                    if ((buffer[index] != 57) || (buffer[++index] != 51) || (buffer[++index] != 61)) // 93= (ClientAccID (7693))
                                    {
                                        // Error
                                        boolValue = false;
                                        continue;
                                    }

                                    index++;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.ClientAccID = StringConverter.GetString(valueBytes);
                                    messageData.ClientAccIDBytes = valueBytes;
                                    index += valueBytes.Length;
                                }
                                else
                                {
                                    // Error
                                    boolValue = false;
                                    continue;
                                }
                            }

                            break;
                        }
                    case 56: // 8
                        {
                            if (buffer[index + 3] != 61)
                            {
                                // Error
                                boolValue = false;
                                break;
                            }

                            index++;

                            if (buffer[index] == 50) // 2
                            {
                                index++;

                                if (buffer[index] == 56) // 8 (TrdType (828))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 ttValue = 0;

                                    if (IntConverter.ParsePositiveInt32(valueBytes, out ttValue) == true)
                                    {
                                        messageData.TrdType = ttValue;
                                    }

                                    index += valueBytes.Length;
                                }
                                else if (buffer[index] == 57) // 9 (TrdSubType (829))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 tstValue = 0;

                                    if (IntConverter.ParsePositiveInt32(valueBytes, out tstValue) == true)
                                    {
                                        messageData.TrdSubType = tstValue;
                                    }

                                    index += valueBytes.Length;
                                }
                                else
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }
                            }
                            else if (buffer[index] == 49) // 1
                            {
                                index++;

                                if (buffer[index] != 56) // 8 (SecondaryTradeReportID (818))
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }

                                index += 2;
                                valueBytes = Messages.GetValueBytes(buffer, index);
                                messageData.SecondaryTradeReportID = StringConverter.GetString(valueBytes);
                                messageData.SecondaryTradeReportIDBytes = valueBytes;
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
                            if (buffer[index + 3] == 61)
                            {
                                index++;

                                if (buffer[index] != 49) // 1
                                {
                                    // Error
                                    boolValue = false;
                                    continue;
                                }

                                index++;
                                byteValue = buffer[index];

                                if (byteValue == 54) // 6 (StartDate (916))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int64 startDateTicks = 0;

                                    if (DateTimeConverter.ParseDate(valueBytes, out startDateTicks) == true)
                                    {
                                        if (messageData.FinancingDetails == null)
                                        {
                                            messageData.FinancingDetails = new FinancingDetails();
                                        }

                                        messageData.FinancingDetails.StartDate = new DateTime(startDateTicks);
                                    }

                                    index += valueBytes.Length;
                                }
                                else if (byteValue == 55) // 7 (EndDate (917))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int64 endDateTicks = 0;

                                    if (DateTimeConverter.ParseDate(valueBytes, out endDateTicks) == true)
                                    {
                                        if (messageData.FinancingDetails == null)
                                        {
                                            messageData.FinancingDetails = new FinancingDetails();
                                        }

                                        messageData.FinancingDetails.EndDate = new DateTime(endDateTicks);
                                    }

                                    index += valueBytes.Length;
                                }
                                else if (byteValue == 57) // 9 (DeliveryType (919))
                                {
                                    index += 2;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    Int32 dtValue = 0;

                                    if (IntConverter.ParsePositiveInt32(valueBytes, out dtValue) == true)
                                    {
                                        if (messageData.FinancingDetails == null)
                                        {
                                            messageData.FinancingDetails = new FinancingDetails();
                                        }

                                        messageData.FinancingDetails.DeliveryType = dtValue;
                                    }

                                    index += valueBytes.Length;
                                }
                                else
                                {
                                    // Error
                                    boolValue = false;
                                    break;
                                }
                            }
                            else if (buffer[index + 4] == 61)
                            {
                                index++;

                                if (buffer[index] == 53) // 5
                                {
                                    index++;

                                    if ((buffer[index] != 56) || (buffer[index + 1] != 48)) // 80 (ParentID (9580))
                                    {
                                        // Error
                                        boolValue = false;
                                        continue;
                                    }

                                    index += 3;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.ParentID = StringConverter.GetString(valueBytes);
                                    messageData.ParentIDBytes = valueBytes;
                                    index += valueBytes.Length;
                                }
                                else if (buffer[index] == 57) // 9
                                {
                                    index++;

                                    if ((buffer[index] != 51) || (buffer[index + 1] != 56)) // 38 (ClearingHandlingType (9938))
                                    {
                                        // Error
                                        boolValue = false;
                                        continue;
                                    }

                                    index += 3;
                                    valueBytes = Messages.GetValueBytes(buffer, index);
                                    messageData.ClearingHandlingType = StringConverter.GetString(valueBytes);
                                    messageData.ClearingHandlingTypeBytes = valueBytes;
                                    index += valueBytes.Length;
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
