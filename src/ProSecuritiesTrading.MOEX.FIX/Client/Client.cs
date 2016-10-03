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
using System.Net;
using System.Net.Sockets;

using ProSecuritiesTrading.MOEX.FIX.ASTS;
using ProSecuritiesTrading.MOEX.FIX.Base.Converter;
using ProSecuritiesTrading.MOEX.FIX.Base.Field;
using ProSecuritiesTrading.MOEX.FIX.Base.Message;

namespace ProSecuritiesTrading.MOEX.FIX.Client
{
    public class Client
    {
        private class StateObject
        {
            public Socket WorkSocket;
            //public const int InitialSizeBuffer = Messages.BeginStringBodyLength.Length + 11;
            public const int InitialSizeBuffer = 32;
            public byte[] Buffer;
            public byte[] Cache = null;
            public int ReceivedBytesCount = 0;
            public int Index = 0;
            public bool InitialBufferReceived = false;

            public StateObject(Socket workSocket)
            {
                this.WorkSocket = workSocket;
                this.Buffer = new byte[InitialSizeBuffer];
            }
        }

        //private ProSecuritiesTrading.MOEX.FIX.Base.Service.IService service;
        private ProSecuritiesTrading.MOEX.FIX.Base.Service.ServiceBase service;
        private Socket socket;
        private IPEndPoint ip;
        private bool clientConnected = false;
        private bool sendDataServer = false;
        private ClientErrorEventHandler clientError;

        internal event ClientErrorEventHandler ClientError
        {
            add
            {
                this.clientError += value;
            }
            remove
            {
                this.clientError -= value;
            }
        }

        private void OnClientError(ClientErrorEventArgs e)
        {
            ClientErrorEventHandler handler = this.clientError;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal Client(ProSecuritiesTrading.MOEX.FIX.Base.Service.ServiceBase service)
        {
            this.service = service;
        }

        private void LoadClient(string ip, int port)
        {
            try
            {
                this.ip = new IPEndPoint(IPAddress.Parse(ip), port);
                this.socket = new Socket(this.ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Connect();
            }
            catch (Exception e)
            {
                this.OnClientError(new ClientErrorEventArgs(e, "LoadClient"));
            }
        }

        private void Connect()
        {
            try
            {
                this.socket.BeginConnect(this.ip, new AsyncCallback(ConnectCallBack), this.socket);
            }
            catch (Exception e)
            {
                this.OnClientError(new ClientErrorEventArgs(e, "Connect"));
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                this.socket.EndConnect(ar);
                this.clientConnected = true;
                Start();
            }
            catch (Exception e)
            {
                this.OnClientError(new ClientErrorEventArgs(e, "ConnectCallBack"));
            }
        }

        private void Disconnect()
        {
            try
            {
                this.socket.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), this.socket);
            }
            catch (Exception e)
            {
                this.OnClientError(new ClientErrorEventArgs(e, "Disconnect"));
            }
        }

        private void DisconnectCallBack(IAsyncResult ar)
        {
            try
            {
                Socket handler = ar.AsyncState as Socket;
                handler.EndDisconnect(ar);
                this.sendDataServer = false;
                this.clientConnected = false;
                this.service.OnClientConnected(false);
            }
            catch (Exception e)
            {
                this.OnClientError(new ClientErrorEventArgs(e, "DisconnectCallBack"));
            }
        }

        private void Receive()
        {
            StateObject state = new StateObject(this.socket);
            state.WorkSocket.BeginReceive(state.Buffer, 0, StateObject.InitialSizeBuffer, SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            StateObject state = ar.AsyncState as StateObject;
            Socket handler = state.WorkSocket;

            try
            {
                if (handler.Connected == false)
                {
                    return;
                }

                int count = handler.EndReceive(ar);

                if ((count > 0) || ((count == 0) && (state.InitialBufferReceived == true)))
                {
                    state.ReceivedBytesCount += count;
                    int index;
                    int length;
                    int count2;
                    int x;

                Label_1:
                    if (state.InitialBufferReceived == true)
                    {
                        if (state.ReceivedBytesCount == state.Buffer.Length)
                        {
                            index = state.Buffer.Length - CheckSum.WithSOHLength;
                            int sumValue = 0;

                            for (x = 0; x < index; x++)
                            {
                                sumValue += state.Buffer[x];
                            }

                            int checkSumValue = sumValue % 256;
                            byte[] checkSum = new byte[3];
                            checkSum[2] = StringConverter.NumeralsStringASCIIBytes[checkSumValue % 10];
                            checkSumValue /= 10;
                            checkSum[1] = StringConverter.NumeralsStringASCIIBytes[checkSumValue % 10];
                            checkSumValue /= 10;
                            checkSum[0] = StringConverter.NumeralsStringASCIIBytes[checkSumValue % 10];

                            if ((state.Buffer[index++] == 49) && (state.Buffer[index++] == 48) && (state.Buffer[index++] == 61) && (state.Buffer[index++] == checkSum[0]) && (state.Buffer[index++] == checkSum[1]) && (state.Buffer[index++] == checkSum[2]) && (state.Buffer[index] == 1))
                            {
                                this.service.ProcessingReceivedMessage(state.Buffer, state.Index);
                            }
                            else
                            {
                                // Error
                            }

                            state.Buffer = new byte[StateObject.InitialSizeBuffer];

                            if (state.Cache == null)
                            {
                                state.ReceivedBytesCount = 0;
                            }
                            else
                            {
                                unsafe
                                {
                                    fixed (byte* pCache = state.Cache, pBuffer = state.Buffer)
                                    {
                                        byte* cacheBytes = pCache;
                                        byte* dBuffer = pBuffer;
                                        length = state.Cache.Length;
                                        byte* pEnd = cacheBytes + length;
                                        count2 = length / 4;

                                        for (x = 0; x < count2; x++)
                                        {
                                            *(Int32*)dBuffer = *(Int32*)cacheBytes;
                                            cacheBytes += 4;
                                            dBuffer += 4;
                                        }

                                        while (cacheBytes < pEnd)
                                        {
                                            *dBuffer = *cacheBytes;
                                            cacheBytes++;
                                            dBuffer++;
                                        }
                                    }
                                }

                                state.ReceivedBytesCount = length;
                                state.Cache = null;
                            }

                            state.InitialBufferReceived = false;
                            state.Index = 0;
                            handler.BeginReceive(state.Buffer, state.ReceivedBytesCount, state.Buffer.Length - state.ReceivedBytesCount, SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                            return;
                        }
                    }
                    else
                    {
                        length = Messages.BeginStringBodyLength.Length;
                        byte[] newBuffer = null;

                        if (state.ReceivedBytesCount >= length)
                        {
                            unsafe
                            {
                                fixed (byte* pBytes = state.Buffer, pBSBLBytes = Messages.BeginStringBodyLength)
                                {
                                    byte* pBuffer = pBytes;
                                    byte* bsblBytes = pBSBLBytes;
                                    byte* pEnd = pBuffer + state.ReceivedBytesCount;
                                    bool boolValue = false;
                                    index = 0;

                                    while (pBuffer < pEnd)
                                    {
                                        if ((pBuffer < (pEnd - 1)) && (*(pBuffer + 1) == 61) && (*pBuffer == 56))
                                        {
                                            if (state.ReceivedBytesCount < (index + length))
                                            {
                                                newBuffer = new byte[StateObject.InitialSizeBuffer];

                                                fixed (byte* pNewBuffer = newBuffer)
                                                {
                                                    byte* dNewBuffer = pNewBuffer;
                                                    count2 = (state.ReceivedBytesCount - index) / 4;

                                                    if (count2 > 0)
                                                    {
                                                        for (x = 0; x < count2; x++)
                                                        {
                                                            *(Int32*)dNewBuffer = *(Int32*)pBuffer;
                                                            pBuffer += 4;
                                                            dNewBuffer += 4;
                                                        }
                                                    }

                                                    while (pBuffer < pEnd)
                                                    {
                                                        *dNewBuffer = *pBuffer;
                                                        pBuffer++;
                                                        dNewBuffer++;
                                                    }

                                                    state.Buffer = newBuffer;
                                                }

                                                state.ReceivedBytesCount = state.ReceivedBytesCount - index;
                                                handler.BeginReceive(state.Buffer, state.ReceivedBytesCount, state.Buffer.Length - state.ReceivedBytesCount, SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                                                return;
                                            }

                                            if (Messages.BeginStringBodyLength.Length == 12)
                                            {
                                                if ((*(Int64*)pBuffer == *(Int64*)bsblBytes) && (*(Int32*)(pBuffer + 8) == *(Int32*)(bsblBytes + 8)))
                                                {
                                                    boolValue = true;
                                                    pBuffer += 12;
                                                    bsblBytes += 12;
                                                    index += 12;
                                                }
                                            }
                                            else
                                            {
                                                count2 = length / 4;
                                                bool boolValue2 = true;

                                                for (x = 0; x < count2; x++)
                                                {
                                                    if (*(Int32*)pBuffer != *(Int32*)bsblBytes)
                                                    {
                                                        boolValue2 = false;
                                                        break;
                                                    }

                                                    pBuffer += 4;
                                                    bsblBytes += 4;
                                                    index += 4;
                                                }

                                                if (boolValue2 == true)
                                                {
                                                    while (bsblBytes < pEnd)
                                                    {
                                                        if (*pBuffer != *bsblBytes)
                                                        {
                                                            boolValue2 = false;
                                                            break;
                                                        }

                                                        pBuffer++;
                                                        bsblBytes++;
                                                        index++;
                                                    }
                                                }

                                                boolValue = boolValue2;
                                            }

                                            if (boolValue == true)
                                            {
                                                break;
                                            }
                                        }

                                        pBuffer++;
                                        index++;
                                    }

                                    if (boolValue == true)
                                    {
                                        int length3;

                                        if ((state.ReceivedBytesCount - index) > 0)
                                        {
                                            int startIndex = index;
                                            int length2 = 0;
                                            bool boolValue3 = false;
                                            byte byteValue;

                                            while (pBuffer < pEnd)
                                            {
                                                byteValue = *pBuffer;

                                                if (byteValue == Messages.SOH)
                                                {
                                                    boolValue3 = true;
                                                    break;
                                                }

                                                // byteValue == 61
                                                if ((length2 > 10) || (byteValue < 48) || (byteValue > 57))
                                                {
                                                    // Error
                                                    state.Buffer = new byte[StateObject.InitialSizeBuffer];
                                                    state.ReceivedBytesCount = 0;
                                                    handler.BeginReceive(state.Buffer, state.ReceivedBytesCount, state.Buffer.Length - state.ReceivedBytesCount, SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                                                    return;
                                                }

                                                pBuffer++;
                                                length2++;
                                            }

                                            if (boolValue3 == true)
                                            {
                                                //byte* bodyLengthValue = stackalloc byte[length2];
                                                //byte[] bodyLengthValueBytes = new byte[length2];

                                                pBuffer -= length2;

                                                /*
                                                for (x = 0; x < bodyLengthValueBytes.Length; x++)
                                                {
                                                    bodyLengthValueBytes[x] = *pBuffer;
                                                    pBuffer++;
                                                }

                                                int bodyLengthValue = int.Parse(Encoding.ASCII.GetString(bodyLengthValueBytes));
                                                */

                                                int bodyLengthValue = 0;
                                                pEnd = pBuffer + length2;

                                                while (pBuffer < pEnd)
                                                {
                                                    bodyLengthValue = (bodyLengthValue * 10) + (*pBuffer - 48);
                                                    pBuffer++;
                                                }

                                                length3 = state.ReceivedBytesCount - (startIndex - length);

                                                newBuffer = new byte[length + length2 + 1 + bodyLengthValue + CheckSum.WithSOHLength];
                                                int length4 = newBuffer.Length;

                                                fixed (byte* pNewBuffer = newBuffer)
                                                {
                                                    pBuffer = pBytes;
                                                    byte* dNewBuffer = pNewBuffer;
                                                    pEnd = pBuffer + ((state.ReceivedBytesCount < length4) ? state.ReceivedBytesCount : length4);
                                                    pBuffer += (state.ReceivedBytesCount - length3);
                                                    count2 = ((length3 < length4) ? length3 : length4) / 4;

                                                    for (x = 0; x < count2; x++)
                                                    {
                                                        *(Int32*)dNewBuffer = *(Int32*)pBuffer;
                                                        pBuffer += 4;
                                                        dNewBuffer += 4;
                                                    }

                                                    while (pBuffer < pEnd)
                                                    {
                                                        *dNewBuffer = *pBuffer;
                                                        pBuffer++;
                                                        dNewBuffer++;
                                                    }

                                                    state.Buffer = newBuffer;
                                                }

                                                if (length3 > length4)
                                                {
                                                    int length5 = length3 - length4;

                                                    byte[] newCache = new byte[length5];

                                                    fixed (byte* pNewCache = newCache)
                                                    {
                                                        byte* dNewCahce = pNewCache;
                                                        pEnd = pBuffer + length5;

                                                        count2 = length5 / 4;

                                                        for (x = 0; x < count2; x++)
                                                        {
                                                            *(Int32*)dNewCahce = *(Int32*)pBuffer;
                                                            pBuffer += 4;
                                                            dNewCahce += 4;
                                                        }

                                                        while (pBuffer < pEnd)
                                                        {
                                                            *dNewCahce = *pBuffer;
                                                            pBuffer++;
                                                            dNewCahce++;
                                                        }

                                                        state.Cache = newCache;
                                                    }
                                                }

                                                state.InitialBufferReceived = true;
                                                state.Index = length + length2 + 1;

                                                if (length3 < length4)
                                                {
                                                    state.ReceivedBytesCount = length3;
                                                }
                                                else
                                                {
                                                    state.ReceivedBytesCount = length4;
                                                    goto Label_1;
                                                }

                                                handler.BeginReceive(state.Buffer, state.ReceivedBytesCount, state.Buffer.Length - state.ReceivedBytesCount, SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                                                return;
                                            }
                                        }

                                        length3 = state.ReceivedBytesCount - (index - length);
                                        newBuffer = new byte[StateObject.InitialSizeBuffer];

                                        if (newBuffer.Length > length3)
                                        {
                                            fixed (byte* pNewBuffer = newBuffer)
                                            {
                                                pBuffer = pBytes;
                                                byte* dNewBuffer = pNewBuffer;
                                                pEnd = pBuffer + state.ReceivedBytesCount;
                                                pBuffer += (state.ReceivedBytesCount - length3);
                                                count2 = length3 / 4;

                                                for (x = 0; x < count2; x++)
                                                {
                                                    *(Int32*)dNewBuffer = *(Int32*)pBuffer;
                                                    pBuffer += 4;
                                                    dNewBuffer += 4;
                                                }

                                                while (pBuffer < pEnd)
                                                {
                                                    *dNewBuffer = *pBuffer;
                                                    pBuffer++;
                                                    dNewBuffer++;
                                                }

                                                state.Buffer = newBuffer;
                                            }

                                            state.ReceivedBytesCount = length3;
                                        }
                                        else
                                        {
                                            // Error
                                            state.ReceivedBytesCount = 0;
                                        }

                                        handler.BeginReceive(state.Buffer, state.ReceivedBytesCount, state.Buffer.Length - state.ReceivedBytesCount, SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                                        return;
                                    }
                                    else
                                    {
                                        pBuffer--;

                                        if (*pBuffer == 56)
                                        {
                                            newBuffer = new byte[StateObject.InitialSizeBuffer];
                                            newBuffer[0] = *pBuffer;
                                            state.Buffer = newBuffer;
                                            state.ReceivedBytesCount = 1;
                                            handler.BeginReceive(state.Buffer, state.ReceivedBytesCount, state.Buffer.Length - state.ReceivedBytesCount, SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                                            return;
                                        }

                                        pBuffer = pBytes;
                                        pEnd = pBuffer + state.ReceivedBytesCount;

                                        if (state.ReceivedBytesCount == 32)
                                        {
                                            *(Decimal*)pBuffer = 0;
                                            pBuffer += 16;
                                            *(Decimal*)pBuffer = 0;
                                        }
                                        else
                                        {
                                            count2 = state.ReceivedBytesCount / 4;

                                            for (x = 0; x < count2; x++)
                                            {
                                                *(Int32*)pBuffer = 0;
                                                pBuffer += 4;
                                            }

                                            while (pBuffer < pEnd)
                                            {
                                                *pBuffer = 0;
                                                pBuffer++;
                                            }
                                        }

                                        state.ReceivedBytesCount = 0;
                                        handler.BeginReceive(state.Buffer, state.ReceivedBytesCount, state.Buffer.Length - state.ReceivedBytesCount, SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    handler.BeginReceive(state.Buffer, state.ReceivedBytesCount, state.Buffer.Length - state.ReceivedBytesCount, SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                }
                else
                {
                    this.sendDataServer = false;
                    this.clientConnected = false;
                    this.service.OnClientConnected(false);
                }
            }
            catch (Exception e)
            {
                this.OnClientError(new ClientErrorEventArgs(e, "ReceiveCallBack"));

                try
                {
                    if (handler.Connected == true)
                    {
                        if (state.InitialBufferReceived == true)
                        {
                            state.InitialBufferReceived = false;
                            state.Index = 0;
                        }

                        state.Buffer = new byte[StateObject.InitialSizeBuffer];
                        state.ReceivedBytesCount = 0;

                        if (state.Cache != null)
                        {
                            state.Cache = null;
                        }

                        handler.BeginReceive(state.Buffer, 0, StateObject.InitialSizeBuffer, SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                    }
                }
                catch (Exception ex)
                {
                    this.OnClientError(new ClientErrorEventArgs(ex, "ReceiveCallBack2"));
                }
            }
        }

        private void Start()
        {
            this.sendDataServer = true;
            Receive();
            this.service.OnClientConnected(true);
        }

        internal void Send(byte[] message)
        {
            try
            {
                this.socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(SendCallBack), this.socket);
            }
            catch (Exception e)
            {
                this.OnClientError(new ClientErrorEventArgs(e, "Send"));
            }
        }

        private void SendCallBack(IAsyncResult ar)
        {
            try
            {
                Socket handler = ar.AsyncState as Socket;
                handler.EndSend(ar);
            }
            catch (Exception e)
            {
                this.OnClientError(new ClientErrorEventArgs(e, "SendCallBack"));
            }
        }

        internal void ConnectClient(string ip, int port)
        {
            if (this.clientConnected == false)
            {
                try
                {
                    this.LoadClient(ip, port);
                }
                catch (Exception e)
                {
                    this.OnClientError(new ClientErrorEventArgs(e, "ConnectClient"));
                }
            }
        }

        internal void DisconnectClient()
        {
            if (this.clientConnected == true)
            {
                try
                {
                    Disconnect();
                    /*
                    socket.Close();
                    this.clientConnected = false;
                    socket.Dispose();
                    */
                }
                catch (Exception e)
                {
                    this.clientConnected = false;
                    this.OnClientError(new ClientErrorEventArgs(e, "DisconnectClient"));
                }
            }
        }

        internal bool ClientConnected
        {
            get
            {
                return this.clientConnected;
            }
        }
    }
}
