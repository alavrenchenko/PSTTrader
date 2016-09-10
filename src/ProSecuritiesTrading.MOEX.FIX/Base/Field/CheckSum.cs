using System;

using ProSecuritiesTrading.MOEX.FIX.Base.Converter;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class CheckSum
    {
        public const int Tag = 10;
        public static readonly byte[] TagBytes;
        public const int Length = 6;
        public const int WithSOHLength = 7;

        static CheckSum()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 49;
            TagBytes[1] = 48;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkSumBytes">Length = 6.</param>
        /// <param name="value"></param>
        public static void WriteBytes(byte[] checkSumBytes, int value)
        {
            checkSumBytes[0] = 49; // 1
            checkSumBytes[1] = 48; // 0
            checkSumBytes[2] = 61; // =
            checkSumBytes[3] = 48;
            checkSumBytes[4] = 48;

            int index = 5;

            do
            {
                checkSumBytes[index] = StringConverter.NumeralsStringASCIIBytes[value % 10];
                index--;
            }
            while (((value /= 10) > 0) && (index > 2));
        }

        public static byte[] GetBytes(int value)
        {
            byte[] bytes = new byte[6];
            bytes[0] = 49; // 1
            bytes[1] = 48; // 0
            bytes[2] = 61; // =
            bytes[3] = 48;
            bytes[4] = 48;

            int index = 5;

            do
            {
                bytes[index] = StringConverter.NumeralsStringASCIIBytes[value % 10];
                index--;
            }
            while (((value /= 10) > 0) && (index > 2));


            return bytes;
        }
    }
}
