using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class EffectiveTime
    {
        public const int Tag = 168;
        public static readonly byte[] TagBytes;

        static EffectiveTime()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 54;
            TagBytes[2] = 56;
        }
    }
}
