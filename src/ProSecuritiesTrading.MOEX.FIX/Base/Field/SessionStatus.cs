using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class SessionStatus
    {
        public const int Tag = 1409;
        public static readonly byte[] TagBytes;
        // 1409=0
        public static readonly byte[] SessionStatus0Bytes;
        // 1409=3
        public static readonly byte[] SessionStatus3Bytes;

        static SessionStatus()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 49;
            TagBytes[1] = 52;
            TagBytes[2] = 48;
            TagBytes[3] = 57;

            SessionStatus0Bytes = new byte[6];
            SessionStatus0Bytes[0] = 49;
            SessionStatus0Bytes[1] = 52;
            SessionStatus0Bytes[2] = 48;
            SessionStatus0Bytes[3] = 57;
            SessionStatus0Bytes[4] = 61;
            SessionStatus0Bytes[5] = 48;

            SessionStatus3Bytes = new byte[6];
            SessionStatus3Bytes[0] = 49;
            SessionStatus3Bytes[1] = 52;
            SessionStatus3Bytes[2] = 48;
            SessionStatus3Bytes[3] = 57;
            SessionStatus3Bytes[4] = 61;
            SessionStatus3Bytes[5] = 51;
        }
    }
}
