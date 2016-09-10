using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class HeartBtInt
    {
        public const int Tag = 108;
        public static readonly byte[] TagBytes;

        static HeartBtInt()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 48;
            TagBytes[2] = 56;
        }

        // Seconds
        public static byte[] GetBytes(int interval)
        {
            byte[] intervalBytes = ProSecuritiesTrading.MOEX.FIX.Base.Converter.StringConverter.FormatUInt32(interval);
            byte[] bytes = new byte[intervalBytes.Length + 4];
            bytes[0] = 49;
            bytes[1] = 48;
            bytes[2] = 56;
            bytes[3] = 61;

            if (intervalBytes.Length < 5)
            {
                int index = 4;

                for (int x = 0; x < intervalBytes.Length; x++)
                {
                    bytes[index] = intervalBytes[x];
                    index++;
                }
            }
            else
            {
                Buffer.BlockCopy(intervalBytes, 0, bytes, 4, intervalBytes.Length);
            }

            return bytes;
        }
    }
}
