using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class PartyID
    {
        public const int Tag = 448;
        public static readonly byte[] TagBytes;

        static PartyID()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 52;
            TagBytes[1] = 52;
            TagBytes[2] = 56;
        }
    }
}
