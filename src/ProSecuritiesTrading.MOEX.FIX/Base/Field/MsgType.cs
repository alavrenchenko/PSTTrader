using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class MsgType
    {
        public const int Tag = 35;
        public static readonly byte[] TagBytes;

        static MsgType()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 51;
            TagBytes[1] = 53;
        }
    }
}
