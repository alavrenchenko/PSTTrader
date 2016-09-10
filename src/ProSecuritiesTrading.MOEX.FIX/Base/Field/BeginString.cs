using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class BeginString
    {
        public const int Tag = 8;
        public const byte TagByte = 56;
        // 8=FIX.4.4
        public static readonly byte[] BeginStringBytes;

        static BeginString()
        {
            BeginStringBytes = new byte[9];
            BeginStringBytes[0] = 56; // 8
            BeginStringBytes[1] = 61; // =
            BeginStringBytes[2] = 70; // F
            BeginStringBytes[3] = 73; // I
            BeginStringBytes[4] = 88; // X
            BeginStringBytes[5] = 46; // .
            BeginStringBytes[6] = 52; // 4
            BeginStringBytes[7] = 46; // .
            BeginStringBytes[8] = 52; // 4
        }
    }
}
