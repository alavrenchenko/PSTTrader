using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class Yield
    {
        public const int Tag = 236;
        public static readonly byte[] TagBytes;

        static Yield()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 50;
            TagBytes[1] = 51;
            TagBytes[2] = 54;
        }
    }
}
