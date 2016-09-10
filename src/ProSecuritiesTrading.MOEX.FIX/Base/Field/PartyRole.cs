using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class PartyRole
    {
        public const int Tag = 452;
        public static readonly byte[] TagBytes;
        public static readonly byte[] Value1Bytes;
        public static readonly byte[] Value3Bytes;
        public static readonly byte[] Value12Bytes;
        public static readonly byte[] Value17Bytes;

        static PartyRole()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 52;
            TagBytes[1] = 53;
            TagBytes[2] = 50;

            Value1Bytes = new byte[1];
            Value1Bytes[0] = 49;

            Value3Bytes = new byte[1];
            Value3Bytes[0] = 51;

            Value12Bytes = new byte[2];
            Value12Bytes[0] = 49;
            Value12Bytes[1] = 50;

            Value17Bytes = new byte[2];
            Value17Bytes[0] = 49;
            Value17Bytes[1] = 55;
        }
    }
}
