using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class OptionSettlDate
    {
        public const int Tag = 5020;
        public static readonly byte[] TagBytes;

        static OptionSettlDate()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 53;
            TagBytes[1] = 48;
            TagBytes[2] = 50;
            TagBytes[3] = 48;
        }
    }
}
