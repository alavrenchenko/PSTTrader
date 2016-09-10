using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class PartyIDSource
    {
        public const int Tag = 447;
        public static readonly byte[] TagBytes;
        public const byte D = 68;

        static PartyIDSource()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 52;
            TagBytes[1] = 52;
            TagBytes[2] = 55;
        }
    }
}
