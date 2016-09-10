using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class WorkingIndicator
    {
        public const int Tag = 636;
        public static readonly byte[] TagBytes;
        public const byte Y = 89;
        public const byte N = 78;

        static WorkingIndicator()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 54;
            TagBytes[1] = 51;
            TagBytes[2] = 54;
        }
    }
}
