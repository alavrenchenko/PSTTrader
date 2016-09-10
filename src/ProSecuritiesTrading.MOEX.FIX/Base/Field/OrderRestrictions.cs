using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class OrderRestrictions
    {
        public const int Tag = 529;
        public static readonly byte[] TagBytes;
        public const byte Value5 = 53;

        static OrderRestrictions()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 53;
            TagBytes[1] = 52;
            TagBytes[2] = 57;
        }
    }
}
