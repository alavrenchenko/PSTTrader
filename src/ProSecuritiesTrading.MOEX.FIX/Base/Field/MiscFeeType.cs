using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class MiscFeeType
    {
        public const int Tag = 139;
        public static readonly byte[] TagBytes;
        /// <summary>
        /// '4' (Комиссия биржи).
        /// </summary>
        public static readonly byte[] Value4;
        /// <summary>
        /// '98' (Комиссия за клиринг).
        /// </summary>
        public static readonly byte[] Value98;
        /// <summary>
        /// '99' (Комиссия за тех. доступ).
        /// </summary>
        public static readonly byte[] Value99;

        static MiscFeeType()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 51;
            TagBytes[2] = 57;

            Value4 = new byte[1];
            Value4[0] = 52;

            Value98 = new byte[2];
            Value98[0] = 57;
            Value98[1] = 56;

            Value99 = new byte[2];
            Value99[0] = 57;
            Value99[1] = 57;
        }
    }
}
