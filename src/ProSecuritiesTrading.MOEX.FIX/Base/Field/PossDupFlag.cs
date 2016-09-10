using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class PossDupFlag
    {
        public const int Tag = 43;
        public static readonly byte[] TagBytes;
        // 43=Y
        public static readonly byte[] PossDupFlagYBytes;
        // 43=N
        public static readonly byte[] PossDupFlagNBytes;
        public const byte Y = 89;
        public const byte N = 78;

        static PossDupFlag()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 52;
            TagBytes[1] = 51;

            PossDupFlagYBytes = new byte[4];
            PossDupFlagYBytes[0] = 52;
            PossDupFlagYBytes[1] = 51;
            PossDupFlagYBytes[2] = 61;
            PossDupFlagYBytes[3] = 89; // Y

            PossDupFlagNBytes = new byte[4];
            PossDupFlagNBytes[0] = 52;
            PossDupFlagNBytes[1] = 51;
            PossDupFlagNBytes[2] = 61;
            PossDupFlagNBytes[3] = 78; // N
        }
    }
}
