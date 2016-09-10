using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class NoTradingSessions
    {
        public const int Tag = 386;
        public static readonly byte[] TagBytes;
        public const byte Value1 = 49;

        static NoTradingSessions()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 51;
            TagBytes[1] = 56;
            TagBytes[2] = 54;
        }
    }
}
