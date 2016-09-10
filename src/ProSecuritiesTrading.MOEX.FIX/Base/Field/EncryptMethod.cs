using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class EncryptMethod
    {
        public const int Tag = 98;
        public static readonly byte[] TagBytes;
        // 98=0
        public static readonly byte[] EncryptMethod0Bytes;

        static EncryptMethod()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 57;
            TagBytes[1] = 56;

            EncryptMethod0Bytes = new byte[4];
            EncryptMethod0Bytes[0] = 57;
            EncryptMethod0Bytes[1] = 56;
            EncryptMethod0Bytes[2] = 61;
            EncryptMethod0Bytes[3] = 48; // 0
        }
    }
}
