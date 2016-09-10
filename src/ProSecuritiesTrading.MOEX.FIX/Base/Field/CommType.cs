using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class CommType
    {
        public const int Tag = 13;
        public static readonly byte[] TagBytes;
        /// <summary>
        /// Value = '3' (абсолютная величина комиссии (суммарная величина, выраженная в валюте расчетов)).
        /// </summary>
        public static readonly byte[] CommTypeValue3WithSOH;
        /// <summary>
        /// Value = '100' (величина комиссии, взимаемой за исполнение сделки срочным отчетом).
        /// </summary>
        public static readonly byte[] CommTypeValue100WithSOH;

        static CommType()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 49;
            TagBytes[1] = 51;

            CommTypeValue3WithSOH = new byte[5];
            CommTypeValue3WithSOH[0] = 49;
            CommTypeValue3WithSOH[1] = 51;
            CommTypeValue3WithSOH[2] = 61;
            CommTypeValue3WithSOH[3] = 51;
            CommTypeValue3WithSOH[4] = Message.Messages.SOH;

            CommTypeValue100WithSOH = new byte[7];
            CommTypeValue100WithSOH[0] = 49;
            CommTypeValue100WithSOH[1] = 51;
            CommTypeValue100WithSOH[2] = 61;
            CommTypeValue100WithSOH[3] = 49;
            CommTypeValue100WithSOH[4] = 48;
            CommTypeValue100WithSOH[5] = 48;
            CommTypeValue100WithSOH[6] = Message.Messages.SOH;
        }
    }
}
