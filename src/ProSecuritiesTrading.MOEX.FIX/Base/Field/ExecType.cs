using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class ExecType
    {
        public const int Tag = 150;
        public static readonly byte[] TagBytes;
        /// <summary>
        /// '0' (Размещение)
        /// </summary>
        public const byte Value0 = 48;
        
        /// <summary>
        /// '4' (Снятие)
        /// </summary>
        public const byte Value4 = 52;
        /// <summary>
        /// '5' (Изменено)
        /// </summary>
        public const byte Value5 = 53;
        /// <summary>
        /// '6' (Рассматривается снятие)
        /// </summary>
        public const byte Value6 = 54;
        /// <summary>
        /// '8' (Отклонение некорректной заявки)
        /// </summary>
        
        /// <summary>
        /// 'F' (Сделка)
        /// </summary>
        public const byte F = 70;
        /// <summary>
        /// 'I' (Статус заявки)
        /// </summary>
        public const byte I = 73;

        static ExecType()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 53;
            TagBytes[2] = 48;
        }
    }
}
