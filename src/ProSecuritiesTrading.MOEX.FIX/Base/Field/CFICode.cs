using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class CFICode
    {
        public const int Tag = 461;
        public static readonly byte[] TagBytes;
        public static readonly byte[] CFICodeMRCXXXWithSOH;

        static CFICode()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 52;
            TagBytes[1] = 54;
            TagBytes[2] = 49;

            CFICodeMRCXXXWithSOH = new byte[11];
            CFICodeMRCXXXWithSOH[0] = 52; // 4
            CFICodeMRCXXXWithSOH[1] = 54; // 6
            CFICodeMRCXXXWithSOH[2] = 49; // 1
            CFICodeMRCXXXWithSOH[3] = 61; // =
            CFICodeMRCXXXWithSOH[4] = 77; // M
            CFICodeMRCXXXWithSOH[5] = 82; // R
            CFICodeMRCXXXWithSOH[6] = 67; // C
            CFICodeMRCXXXWithSOH[7] = 88; // X
            CFICodeMRCXXXWithSOH[8] = 88; // X
            CFICodeMRCXXXWithSOH[9] = 88; // X
            CFICodeMRCXXXWithSOH[10] = ProSecuritiesTrading.MOEX.FIX.Base.Message.Messages.SOH;
        }
    }
}
