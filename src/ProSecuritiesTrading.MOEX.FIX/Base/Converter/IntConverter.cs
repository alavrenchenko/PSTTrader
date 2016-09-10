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
    public static class IntConverter
    {
        public static readonly byte[] NumeralsStringASCIIBytes;

        static IntConverter()
        {
            NumeralsStringASCIIBytes = ConverterBase.NumeralsStringASCIIBytes;
        }

        public static bool ParseInt32(byte[] bytes, out Int32 value)
        {
            value = 0;
            int length = bytes.Length;

            if (length == 0)
            {
                return false;
            }

            Int32 number = 0;
            bool positiveNumber = (bytes[0] != 45) ? true : false;

            if (positiveNumber == true)
            {
                if (length > 10)
                {
                    return false;
                }
            }
            else if (length > 11)
            {
                return false;
            }

            byte byteValue;

            try
            {
                for (int x = ((positiveNumber == true) ? 0 : 1); x < length; x++)
                {
                    byteValue = bytes[x];

                    if ((byteValue < 48) || (byteValue > 57))
                    {
                        return false;
                    }

                    number = (number * 10) + (byteValue - 48);
                }

                if ((positiveNumber == false) && (number > 0))
                {
                    number *= -1;
                }
            }
            catch
            {
                return false;
            }

            value = number;

            return true;
        }

        public static bool ParseUInt32(byte[] bytes, out UInt32 value)
        {
            value = 0;
            int length = bytes.Length;

            if (length == 0)
            {
                return false;
            }

            UInt32 number = 0;

            if (length > 10)
            {
                return false;
            }

            byte byteValue;

            try
            {
                for (int x = 0; x < length; x++)
                {
                    byteValue = bytes[x];

                    if ((byteValue < 48) || (byteValue > 57))
                    {
                        return false;
                    }

                    number = (number * 10) + (UInt32)(byteValue - 48);
                }
            }
            catch
            {
                return false;
            }

            value = number;

            return true;
        }

        public static bool ParsePositiveInt32(byte[] bytes, out Int32 value)
        {
            value = 0;
            int length = bytes.Length;

            if (length == 0)
            {
                return false;
            }

            Int32 number = 0;

            if (length > 10)
            {
                return false;
            }

            byte byteValue;

            try
            {
                for (int x = 0; x < length; x++)
                {
                    byteValue = bytes[x];

                    if ((byteValue < 48) || (byteValue > 57))
                    {
                        return false;
                    }

                    number = (number * 10) + (byteValue - 48);
                }
            }
            catch
            {
                return false;
            }

            value = number;

            return true;
        }
    }
}
