using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class OrdStatus
    {
        public const int Tag = 39;
        public static readonly byte[] TagBytes;
        /// <summary>
        /// '0' (Новая/Активная)
        /// </summary>
        public const byte Value0 = 48;
        /// <summary>
        /// '1' (Частично исполнена)
        /// </summary>
        public const byte Value1 = 49;
        /// <summary>
        /// '2' (Исполнена)
        /// </summary>
        public const byte Value2 = 50;
        /// <summary>
        /// '4' (Отменена/Снята)
        /// </summary>
        public const byte Value4 = 52;
        /// <summary>
        /// '6' (Рассматривается отмена/снятие)
        /// </summary>
        public const byte Value6 = 54;
        /// <summary>
        /// '8' (Отклонена)
        /// </summary>
        public const byte Value8 = 56;
        /// <summary>
        /// '9' (Ожидает наступления события)
        /// </summary>
        public const byte Value9 = 57;
        /// <summary>
        /// 'E' (Рассматривается изменение)
        /// </summary>
        public const byte E = 69;

        static OrdStatus()
        {
            TagBytes = new byte[2];
            TagBytes[0] = 51;
            TagBytes[1] = 57;
        }
    }
}
