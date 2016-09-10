using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class TradeIssueDate
    {
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public const int Tag = 9173;
        /// <summary>
        /// MOEX, ASTS.
        /// </summary>
        public static readonly byte[] TagBytes;

        static TradeIssueDate()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 57;
            TagBytes[1] = 49;
            TagBytes[2] = 55;
            TagBytes[3] = 51;
        }
    }
}
