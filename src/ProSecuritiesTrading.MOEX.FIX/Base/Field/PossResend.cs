using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class PossResend
    {
        public const int Tag = 97;
        public static readonly byte[] TagBytes;
        // 43=Y
        public static readonly byte[] PossResendYBytes;
        // 43=N
        public static readonly byte[] PossResendNBytes;
        public const byte Y = 89;
        public const byte N = 78;

        static PossResend()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 57;
            TagBytes[1] = 55;

            PossResendYBytes = new byte[4];
            PossResendYBytes[0] = 57;
            PossResendYBytes[1] = 55;
            PossResendYBytes[2] = 61;
            PossResendYBytes[3] = 89; // Y

            PossResendNBytes = new byte[4];
            PossResendNBytes[0] = 57;
            PossResendNBytes[1] = 55;
            PossResendNBytes[2] = 61;
            PossResendNBytes[3] = 78; // N
        }
    }
}
