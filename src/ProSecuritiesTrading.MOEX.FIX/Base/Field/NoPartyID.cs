using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class NoPartyID
    {
        public const int Tag = 453;
        public static readonly byte[] TagBytes;

        static NoPartyID()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 52;
            TagBytes[1] = 53;
            TagBytes[2] = 51;
        }
    }
}
