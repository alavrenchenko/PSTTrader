using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class NewPassword
    {
        public const int Tag = 925;
        public static readonly byte[] TagBytes;

        static NewPassword()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 57;
            TagBytes[1] = 50;
            TagBytes[2] = 53;
        }
    }
}
