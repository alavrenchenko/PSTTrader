using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class Password
    {
        public const int Tag = 554;
        public static readonly byte[] TagBytes;

        static Password()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 53;
            TagBytes[1] = 53;
            TagBytes[2] = 52;
        }
    }
}
