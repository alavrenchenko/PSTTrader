using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class TestReqID
    {
        public const int Tag = 112;
        public static readonly byte[] TagBytes;

        static TestReqID()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 49;
            TagBytes[1] = 50;
        }
    }
}
