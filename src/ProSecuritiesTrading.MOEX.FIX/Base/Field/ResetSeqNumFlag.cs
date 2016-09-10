using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class ResetSeqNumFlag
    {
        public const int Tag = 141;
        public static readonly byte[] TagBytes;
        // 141=Y
        public static readonly byte[] ResetSeqNumFlagYBytes;
        // 141=N
        public static readonly byte[] ResetSeqNumFlagNBytes;
        public const byte Y = 89;
        public const byte N = 78;

        static ResetSeqNumFlag()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 52;
            TagBytes[2] = 49;

            ResetSeqNumFlagYBytes = new byte[4];
            ResetSeqNumFlagYBytes[0] = 49;
            ResetSeqNumFlagYBytes[1] = 52;
            ResetSeqNumFlagYBytes[2] = 49;
            ResetSeqNumFlagYBytes[3] = 61;
            ResetSeqNumFlagYBytes[4] = 89; // Y

            ResetSeqNumFlagNBytes = new byte[4];
            ResetSeqNumFlagNBytes[0] = 49;
            ResetSeqNumFlagNBytes[1] = 52;
            ResetSeqNumFlagNBytes[2] = 49;
            ResetSeqNumFlagNBytes[3] = 61;
            ResetSeqNumFlagNBytes[4] = 78; // N
        }
    }
}
