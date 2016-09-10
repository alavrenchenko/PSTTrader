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
    public static class DoubleConverter
    {
        public static readonly byte[] NumeralsStringASCIIBytes;

        static DoubleConverter()
        {
            NumeralsStringASCIIBytes = ConverterBase.NumeralsStringASCIIBytes;
        }

        public static bool ParseDouble(byte[] bytes, out double value)
        {
            value = 0.0;
            int length = bytes.Length;

            if (length == 0)
            {
                return false;
            }

            double number = 0.0;
            bool positiveNumber = (bytes[0] != 45) ? true : false;
            
            try
            {
                byte byteValue;
                bool point = false;
                int x;

                for (x = ((positiveNumber == true) ? 0 : 1); x < length; x++)
                {
                    byteValue = bytes[x];

                    if ((byteValue < 48) || (byteValue > 57))
                    {
                        if (byteValue == 46)
                        {
                            point = true;
                            x++;
                            break;
                        }

                        return false;
                    }

                    number = (number * 10) + (byteValue - 48);
                }

                if ((point == true) && (x < length))
                {
                    Int64 number2 = 0;
                    //double number2 = 0.0;
                    int number3 = 1;

                    do
                    {
                        byteValue = bytes[x];

                        if ((byteValue < 48) || (byteValue > 57))
                        {
                            return false;
                        }

                        number2 = (number2 * 10) + (Int64)(byteValue - 48);
                        number3 *= 10;
                        x++;
                    }
                    while (x < length);

                    if (number2 > 0.0)
                    {
                        number = number + ((double)number2 / number3);
                    }
                }

                if ((positiveNumber == false) && (number > 0.0))
                {
                    number *= -1.0;
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
