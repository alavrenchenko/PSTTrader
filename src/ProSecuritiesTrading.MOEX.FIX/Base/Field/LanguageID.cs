using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class LanguageID
    {
        public const int Tag = 6936;
        public static readonly byte[] TagBytes;
        // 6936=R
        public static readonly byte[] LanguageIDRBytes;
        // 6936=E
        public static readonly byte[] LanguageIDEBytes;
        public const byte R = 82;
        public const byte E = 69;

        static LanguageID()
        {
            TagBytes = new byte[4];
            TagBytes[0] = 54;
            TagBytes[1] = 57;
            TagBytes[2] = 51;
            TagBytes[3] = 54;

            LanguageIDRBytes = new byte[6];
            LanguageIDRBytes[0] = 54;
            LanguageIDRBytes[1] = 57;
            LanguageIDRBytes[2] = 51;
            LanguageIDRBytes[3] = 54;
            LanguageIDRBytes[4] = 61;
            LanguageIDRBytes[5] = 82; // R

            LanguageIDEBytes = new byte[6];
            LanguageIDEBytes[0] = 54;
            LanguageIDEBytes[1] = 57;
            LanguageIDEBytes[2] = 51;
            LanguageIDEBytes[3] = 54;
            LanguageIDEBytes[4] = 61;
            LanguageIDEBytes[5] = 69; // E
        }
    }
}
