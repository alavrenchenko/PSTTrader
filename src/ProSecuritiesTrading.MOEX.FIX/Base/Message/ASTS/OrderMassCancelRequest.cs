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
using ProSecuritiesTrading.MOEX.FIX.Base.Converter;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class OrderMassCancelRequest
    {
        // 35=q
        public static readonly byte[] MsgTypeBytes;

        static OrderMassCancelRequest()
        {
            int length = MsgType.TagBytes.Length;
            MsgTypeBytes = new byte[length + 2];

            for (int x = 0; x < length; x++)
            {
                MsgTypeBytes[x] = MsgType.TagBytes[x];
            }

            MsgTypeBytes[length] = 61; // =
            MsgTypeBytes[length + 1] = 113; // q
        }

        /// <summary>
        /// Сборка: x64, Оптимизированный код.
        /// </summary>
        /// <param name="sClOID">SecondaryClOrdID.</param>
        /// <param name="mcrt">MassCancelRequestType. Null: 0.</param>
        /// <param name="tSID">TradingSessionID. Null: 255.</param>
        /// <param name="product">Null: 0.</param>
        /// <param name="cfiCode">Null: false</param>
        /// <param name="st">SecurityType.</param>
        /// <param name="side">Null: 0.</param>
        public unsafe static byte[] UnsafeGetBytes(ProSecuritiesTrading.MOEX.FIX.Base.Group.Header header, int msgSeqNum, bool origSendingTime, byte[] clOrdID, byte[] sClOID, byte mcrt, byte[] tSID, byte[] symbol, byte product, bool cfiCode, byte[] st, byte side, byte[] account, Parties parties)
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
             * 
             * ClOrdID = 10
             * SecondaryClOrdID = 11
             * MassCancelRequestType = 12
             * TradingSessionID = 13
             * 
             * <Instrument>
             * Symbol = 14
             * Product = 15
             * CFICode = 16
             * SecurityType = 17
             * </Instrument>
             * 
             * Side = 18
             * TransactTime = 19
             * Account = 20
             * 
             * <Parties>
             * NoPartyID = 21
             * PartyID = 22
             * PartyIDSource = 23
             * PartyRole = 24
             * </Parties>
             * 
             * <Trailer>
             *  CheckSum = 25
             * </Trailer>
             * */

            int index = 0;
            int x;
            byte[] msgSeqNumValue = StringConverter.FormatUInt32(msgSeqNum);
            int msgSeqNumLength = msgSeqNumValue.Length + 3;

            byte[] sendingTimeValue = DateTimeConverter.GetBytes(DateTime.UtcNow.Ticks);

            // TransactTime = SendingTime

            byte[] partiesBytes = null;

            if ((parties != null) && (parties.PartiesBytes != null))
            {
                partiesBytes = parties.PartiesBytes;
            }

            // TagValue.Length without SOH.
            // ((TagValue) ? Length : 0) with SOH.
            int headerLength = MsgTypeBytes.Length + header.SenderAndTargetCompIDWithSOH.Length + msgSeqNumLength + ((header.PossDupFlag > 0) ? 5 : 0) + ((header.PossResend > 0) ? 5 : 0) + (sendingTimeValue.Length + 3) + ((origSendingTime == true) ? (sendingTimeValue.Length + 5) : 0) + 3;

            // TagValue.Length without SOH.
            // ((TagValue) ? Length : 0) with SOH.
            // SecurityType with SOH.
            int instrumentLength = (symbol != null) ? ((symbol.Length + 3) + ((product > 0) ? 6 : 0) + ((cfiCode == true) ? CFICode.CFICodeMRCXXXWithSOH.Length : 0) + ((st != null) ? st.Length : 0) + 1) : 0;

            int partiesLength = (partiesBytes != null) ? partiesBytes.Length : 0;

            // TagValue.Length without SOH.
            // ((TagValue) ? Length : 0) with SOH.
            // MassCancelRequestType.Length = 6. With SOH.
            // Side.Length = 5. With SOH.
            // TransactTime = SendingTime.
            int baseLength = (clOrdID.Length + 3) + ((sClOID != null) ? (sClOID.Length + 5) : 0) + 6 + ((tSID != null) ? (tSID.Length + 5) : 0) + ((side > 0) ? 5: 0) + (sendingTimeValue.Length + 3) + ((account != null) ? (account.Length + 3) : 0) + 2;

            int bodyLengthValue = headerLength + baseLength + instrumentLength + partiesLength;
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
                *dBytes = 113; // q
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

                *dBytes = 49; // 1
                dBytes++;
                *dBytes = 49; // 1
                dBytes++;
                *dBytes = 61; // =
                dBytes++;
                index += 3;

                fixed (byte* pClOrdID = clOrdID)
                {
                    length = clOrdID.Length;

                    if (length == 32)
                    {
                        *(Decimal*)dBytes = *(Decimal*)pClOrdID;
                        dBytes += 16;
                        *(Decimal*)dBytes = *(Decimal*)(pClOrdID + 16);
                        dBytes += 16;
                    }
                    else
                    {
                        Messages.BytesCopy(dBytes, pClOrdID, length);
                        dBytes += length;
                    }

                    index += length;
                }

                *dBytes = Messages.SOH;
                dBytes++;
                index++;

                if (sClOID != null)
                {
                    *dBytes = 53; // 5
                    dBytes++;
                    *dBytes = 50; // 2
                    dBytes++;
                    *dBytes = 54; // 6
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    index += 4;

                    fixed (byte* pSClOIDBytes = sClOID)
                    {
                        byte* sClOIDBytes = pSClOIDBytes;
                        length = sClOID.Length;
                        pEnd = sClOIDBytes + length;

                        if (length > 3)
                        {
                            count = length / 4;

                            for (x = 0; x < count; x++)
                            {
                                *(Int32*)dBytes = unchecked(*(Int32*)sClOIDBytes);
                                dBytes += 4;
                                sClOIDBytes += 4;
                            }
                        }

                        while (sClOIDBytes < pEnd)
                        {
                            *dBytes = unchecked(*sClOIDBytes);
                            dBytes++;
                            sClOIDBytes++;
                        }

                        index += length;
                    }

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index++;
                }

                *dBytes = 53; // 5
                dBytes++;
                *dBytes = 51; // 3
                dBytes++;
                *dBytes = 48; // 0
                dBytes++;
                *dBytes = 61; // =
                dBytes++;
                *dBytes = mcrt;
                dBytes++;
                *dBytes = Messages.SOH;
                dBytes++;
                index += 6;

                if (tSID != null)
                {
                    *dBytes = 51; // 3
                    dBytes++;
                    *dBytes = 51; // 3
                    dBytes++;
                    *dBytes = 54; // 6
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    index += 4;

                    fixed (byte* pTSID = tSID)
                    {
                        length = tSID.Length;

                        if (length == 4)
                        {
                            *(Int32*)dBytes = unchecked(*(Int32*)pTSID);
                            dBytes += 4;
                        }
                        else
                        {
                            byte* tSIDBytes = pTSID;
                            pEnd = tSIDBytes + length;

                            if (length > 1)
                            {
                                *(Int16*)dBytes = unchecked(*(Int16*)tSIDBytes);
                                dBytes += 2;
                                tSIDBytes += 2;
                            }

                            while (tSIDBytes < pEnd)
                            {
                                *dBytes = unchecked(*tSIDBytes);
                                dBytes++;
                                tSIDBytes++;
                            }
                        }

                        index += length;
                    }

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index++;
                }

                // <Instrument>

                if (symbol != null)
                {
                    *dBytes = 53; // 5
                    dBytes++;
                    *dBytes = 53; // 5
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    index += 3;

                    fixed (byte* pSymbolBytes = symbol)
                    {
                        length = symbol.Length;

                        if (length == 4)
                        {
                            *(Int32*)dBytes = unchecked(*(Int32*)pSymbolBytes);
                            dBytes += 4;
                        }
                        else if (length == 12)
                        {
                            *(Int64*)dBytes = unchecked(*(Int64*)pSymbolBytes);
                            dBytes += 8;
                            *(Int32*)dBytes = unchecked(*(Int32*)(pSymbolBytes + 8));
                            dBytes += 4;
                        }
                        else
                        {
                            byte* symbolBytes = pSymbolBytes;
                            pEnd = symbolBytes + length;

                            if (length > 3)
                            {
                                count = length / 4;

                                for (x = 0; x < count; x++)
                                {
                                    *(Int32*)dBytes = unchecked(*(Int32*)symbolBytes);
                                    dBytes += 4;
                                    symbolBytes += 4;
                                }
                            }

                            while (symbolBytes < pEnd)
                            {
                                *dBytes = unchecked(*symbolBytes);
                                dBytes++;
                                symbolBytes++;
                            }
                        }

                        index += length;
                    }

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index++;

                    if (product > 0)
                    {
                        *dBytes = 52; // 4
                        dBytes++;
                        *dBytes = 54; // 6
                        dBytes++;
                        *dBytes = 48; // 0
                        dBytes++;
                        *dBytes = 61; // =
                        dBytes++;
                        *dBytes = product;
                        dBytes++;
                        *dBytes = Messages.SOH;
                        dBytes++;
                        index += 6;
                    }

                    // With SOH.
                    if (cfiCode == true)
                    {
                        fixed (byte* pCFICodeBytes = CFICode.CFICodeMRCXXXWithSOH)
                        {
                            length = CFICode.CFICodeMRCXXXWithSOH.Length;

                            if (length == 11)
                            {
                                *(Int64*)dBytes = unchecked(*(Int64*)pCFICodeBytes);
                                dBytes += 8;
                                *(Int16*)dBytes = unchecked(*(Int16*)(pCFICodeBytes + 8));
                                dBytes += 2;
                                *dBytes = unchecked(*(pCFICodeBytes + 10));
                                dBytes++;
                            }
                            else
                            {
                                byte* cfiCodeBytes = pCFICodeBytes;
                                pEnd = cfiCodeBytes + length;

                                if (length > 3)
                                {
                                    count = length / 4;

                                    for (x = 0; x < count; x++)
                                    {
                                        *(Int32*)dBytes = unchecked(*(Int32*)cfiCodeBytes);
                                        dBytes += 4;
                                        cfiCodeBytes += 4;
                                    }
                                }

                                while (cfiCodeBytes < pEnd)
                                {
                                    *dBytes = unchecked(*cfiCodeBytes);
                                    dBytes++;
                                    cfiCodeBytes++;
                                }
                            }

                            index += length;
                        }
                    }

                    // SecurityType with SOH.
                    if (st != null)
                    {
                        fixed (byte* pSTBytes = st)
                        {
                            byte* stBytes = pSTBytes;
                            length = st.Length;
                            pEnd = stBytes + length;

                            if (length > 8)
                            {
                                *(Int64*)dBytes = unchecked(*(Int64*)stBytes);
                                dBytes += 8;
                                stBytes += 8;

                                if (length > 9)
                                {
                                    *(Int16*)dBytes = unchecked(*(Int16*)stBytes);
                                    dBytes += 2;
                                    stBytes += 2;

                                    if (length == 11)
                                    {
                                        *dBytes = unchecked(*stBytes);
                                        dBytes++;
                                        stBytes++;
                                    }
                                }
                                else
                                {
                                    *dBytes = unchecked(*stBytes);
                                    dBytes++;
                                    stBytes++;
                                }
                            }

                            if (stBytes < pEnd)
                            {
                                if (length > 13)
                                {
                                    count = length / 4;

                                    for (x = 0; x < count; x++)
                                    {
                                        *(Int32*)dBytes = unchecked(*(Int32*)stBytes);
                                        dBytes += 4;
                                        stBytes += 4;
                                    }
                                }

                                while (stBytes < pEnd)
                                {
                                    *dBytes = unchecked(*stBytes);
                                    dBytes++;
                                    stBytes++;
                                }
                            }

                            index += length;
                        }
                    }
                }

                // </Instrument>

                if (side > 0)
                {
                    *dBytes = 53; // 5
                    dBytes++;
                    *dBytes = 52; // 4
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    *dBytes = side;
                    dBytes++;
                    *dBytes = Messages.SOH;
                    dBytes++;
                    index += 5;
                }

                *dBytes = 54; // 6
                dBytes++;
                *dBytes = 48; // 0
                dBytes++;
                *dBytes = 61; // =
                dBytes++;
                index += 3;

                fixed (byte* pTTVBytes = sendingTimeValue)
                {
                    length = sendingTimeValue.Length;
                    Messages.SendingTimeCopy(dBytes, pTTVBytes, length);
                    dBytes += length;
                    index += length;
                }

                *dBytes = Messages.SOH;
                dBytes++;
                index++;

                if (account != null)
                {
                    *dBytes = 49; // 1
                    dBytes++;
                    *dBytes = 61; // =
                    dBytes++;
                    index += 2;

                    fixed (byte* pAccountBytes = account)
                    {
                        byte* accountBytes = pAccountBytes;
                        length = account.Length;
                        pEnd = accountBytes + length;

                        if (length > 3)
                        {
                            count = length / 4;

                            for (x = 0; x < count; x++)
                            {
                                *(Int32*)dBytes = unchecked(*(Int32*)accountBytes);
                                dBytes += 4;
                                accountBytes += 4;
                            }
                        }
                        else if (length > 1)
                        {
                            *(Int16*)dBytes = unchecked(*(Int16*)accountBytes);
                            dBytes += 2;
                            accountBytes += 2;
                        }

                        while (accountBytes < pEnd)
                        {
                            *dBytes = unchecked(*accountBytes);
                            dBytes++;
                            accountBytes++;
                        }

                        index += length;
                    }

                    *dBytes = Messages.SOH;
                    dBytes++;
                    index++;
                }

                // <Parties>

                // With SOH.
                if (partiesBytes != null)
                {
                    fixed (byte* pPartiesBytes = partiesBytes)
                    {
                        length = partiesBytes.Length;
                        Messages.BytesCopy(dBytes, pPartiesBytes, length);
                        dBytes += length;
                        index += length;
                    }
                }

                // </Parties>

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
