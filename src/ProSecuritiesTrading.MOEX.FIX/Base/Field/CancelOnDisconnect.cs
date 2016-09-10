using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class CancelOnDisconnect
    {
        public const int Tag = 6867;
        public static readonly byte[] TagBytes;
        // 6867=A
        public static readonly byte[] CancelOnDisconnectABytes;

        static CancelOnDisconnect()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 54;
            TagBytes[1] = 56;
            TagBytes[2] = 54;
            TagBytes[3] = 55;

            CancelOnDisconnectABytes = new byte[6];
            CancelOnDisconnectABytes[0] = 54;
            CancelOnDisconnectABytes[1] = 56;
            CancelOnDisconnectABytes[2] = 54;
            CancelOnDisconnectABytes[3] = 55;
            CancelOnDisconnectABytes[4] = 61;
            CancelOnDisconnectABytes[5] = 65;
        }
    }
}
