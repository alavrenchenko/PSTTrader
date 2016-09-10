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

namespace ProSecuritiesTrading.MOEX.FIX.Base.Converter
{
    public static class StringConverter
    {
        public static readonly byte[] NumeralsStringASCIIBytes;

        static StringConverter()
        {
            NumeralsStringASCIIBytes = ConverterBase.NumeralsStringASCIIBytes;
            /*
            NumeralsStringASCIIBytes = new byte[10];
            NumeralsStringASCIIBytes[0] = 48;
            NumeralsStringASCIIBytes[1] = 49;
            NumeralsStringASCIIBytes[2] = 50;
            NumeralsStringASCIIBytes[3] = 51;
            NumeralsStringASCIIBytes[4] = 52;
            NumeralsStringASCIIBytes[5] = 53;
            NumeralsStringASCIIBytes[6] = 54;
            NumeralsStringASCIIBytes[7] = 55;
            NumeralsStringASCIIBytes[8] = 56;
            NumeralsStringASCIIBytes[9] = 57;
            */
        }

        public static byte[] FormatUInt32(int value)
        {
            byte[] bytes = new byte[10];
            int index = 9;

            do
            {
                bytes[index] = NumeralsStringASCIIBytes[value % 10];
                index--;
            }
            while (((value /= 10) > 0) && (index > 0));

            byte[] newBytes = new byte[9 - index];
            index++;

            if (newBytes.Length < 5)
            {
                for (int x = 0; x < newBytes.Length; x++)
                {
                    newBytes[x] = bytes[index];
                    index++;
                }
            }
            else
            {
                Buffer.BlockCopy(bytes, index, newBytes, 0, newBytes.Length);
            }

            return newBytes;
        }

        /// <summary>
        /// Сборка: x64, Оптимизированный код.
        /// </summary>
        public unsafe static byte[] FormatDouble(double value)
        {
            bool positiveNumber = (value >= 0.0) ? true : false;
            double value2 = (positiveNumber == true) ? value : (value * (-1));

            Int64 number = (Int64)value2;
            Int64 number2 = 0;
            //double number3 = (double)((decimal)value2 - number);
            double tick = 10000000000.0;
            /*
            double number3 = (value2 - number) * tick;
            UInt64 number4 = (UInt64)number3;
            double number5 = ((number4 < (number3 - 0.5)) ? (number4 + 1) : number4) / tick;
            */
            double number5 = ((Int64)((((value2 - number) * tick) - 0.5) + 1)) / tick;

            int length = 0;

            if (number5 > 0.0)
            {
                tick /= 10;

                do
                {
                    //number3 *= 10;
                    number5 = ((Int64)((((number5 * 10) * tick) - 0.5) + 1)) / tick;
                    tick /= 10;
                    length++;
                }
                while (((number5 - (Int64)number5) > 0.0) && (length < 10));

                number2 = (Int64)number5;
            }

            int length2 = 10;
            byte* numberBytes = stackalloc byte[length2];
            byte* pEnd;
            int index = 0;
            int count;

            do
            {
                if (index == length2)
                {
                    length2 *= 2;
                    byte* newNumberBytes = stackalloc byte[length2];
                    pEnd = numberBytes;
                    numberBytes -= index;
                    count = index / 8;
                    index = 0;

                    for (int x = 0; x < count; x++)
                    {
                        *(Int64*)newNumberBytes = *(Int64*)numberBytes;
                        numberBytes += 8;
                        newNumberBytes += 8;
                        index += 8;
                    }

                    while (numberBytes < pEnd)
                    {
                        *newNumberBytes = *numberBytes;
                        numberBytes++;
                        newNumberBytes++;
                        index++;
                    }

                    numberBytes = newNumberBytes;
                    numberBytes++;
                    index++;
                }

                *numberBytes = NumeralsStringASCIIBytes[number % 10];
                numberBytes++;
                index++;
            }
            while ((number /= 10) > 0);

            byte[] bytes = new byte[((positiveNumber == true) ? 0 : 1) + index + ((length > 0) ? (length + 1) : 0)];

            fixed (byte* pBytes = bytes)
            {
                byte* dBytes = pBytes;
                pEnd = numberBytes - index;
                numberBytes--;

                if (positiveNumber == false)
                {
                    *dBytes = 45;
                    dBytes++;
                }

                while (numberBytes >= pEnd)
                {
                    *dBytes = *numberBytes;
                    dBytes++;
                    numberBytes--;
                }

                if (length > 0)
                {
                    *dBytes = 46;
                    dBytes += length;
                    
                    do
                    {
                        *dBytes = NumeralsStringASCIIBytes[number2 % 10];
                        number2 /= 10;
                        dBytes--;
                        length--;
                    }
                    while (length > 0);
                }
            }

            return bytes;
        }

        /// <summary>
        /// Encoding ASCII.
        /// </summary>
        public static string GetString(byte[] bytes)
        {
            int length = bytes.Length;
            char[] chars = new char[length];

            for (int x = 0; x < length; x++)
            {
                chars[x] = (char)bytes[x];
            }

            return new String(chars);
        }

        /// <summary>
        /// Encoding ASCII.
        /// </summary>
        public unsafe static byte[] GetBytes(string s)
        {
            char[] chars = s.ToCharArray();
            byte[] bytes = new byte[chars.Length];

            fixed (byte* pBytes = bytes)
            {
                fixed (char* pChars = chars)
                {
                    byte* dBytes = pBytes;
                    char* srcChars = pChars;
                    char* pEnd = srcChars + chars.Length;

                    while (srcChars < pEnd)
                    {
                        *dBytes = unchecked((byte)*srcChars);
                        dBytes++;
                        srcChars++;
                    }
                }
            }

            return bytes;
        }
    }
}
