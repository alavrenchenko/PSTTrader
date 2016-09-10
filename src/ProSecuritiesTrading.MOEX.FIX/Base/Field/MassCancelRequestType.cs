using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class MassCancelRequestType
    {
        public const int Tag = 530;
        public static readonly byte[] TagBytes;
        public const byte Value1 = 49;
        public const byte Value7 = 55;

        static MassCancelRequestType()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 53;
            TagBytes[1] = 51;
            TagBytes[2] = 48;
        }
    }
}
