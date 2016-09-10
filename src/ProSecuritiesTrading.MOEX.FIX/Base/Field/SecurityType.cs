using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Field
{
    public class SecurityType
    {
        public const int Tag = 167;
        public static readonly byte[] TagBytes;
        public static readonly byte[] SecurityTypeFXSPOTWithSOH;
        public static readonly byte[] SecurityTypeFXSWAPWithSOH;
        public static readonly byte[] SecurityTypeFXFWDWithSOH;
        public static readonly byte[] SecurityTypeFXBKTWithSOH;
        public static readonly byte[] SecurityTypeREPOWithSOH;

        static SecurityType()
        {
            TagBytes = new byte[3];
            TagBytes[0] = 49;
            TagBytes[1] = 54;
            TagBytes[2] = 55;

            SecurityTypeFXSPOTWithSOH = new byte[11];
            SecurityTypeFXSPOTWithSOH[0] = 49; // 1
            SecurityTypeFXSPOTWithSOH[1] = 54; // 6
            SecurityTypeFXSPOTWithSOH[2] = 55; // 7
            SecurityTypeFXSPOTWithSOH[3] = 61; // =
            SecurityTypeFXSPOTWithSOH[4] = 70; // F
            SecurityTypeFXSPOTWithSOH[5] = 88; // X
            SecurityTypeFXSPOTWithSOH[6] = 83; // S
            SecurityTypeFXSPOTWithSOH[7] = 80; // P
            SecurityTypeFXSPOTWithSOH[8] = 79; // O
            SecurityTypeFXSPOTWithSOH[9] = 84; // T
            SecurityTypeFXSPOTWithSOH[10] = ProSecuritiesTrading.MOEX.FIX.Base.Message.Messages.SOH;

            SecurityTypeFXSWAPWithSOH = new byte[11];
            SecurityTypeFXSWAPWithSOH[0] = 49; // 1
            SecurityTypeFXSWAPWithSOH[1] = 54; // 6
            SecurityTypeFXSWAPWithSOH[2] = 55; // 7
            SecurityTypeFXSWAPWithSOH[3] = 61; // =
            SecurityTypeFXSWAPWithSOH[4] = 70; // F
            SecurityTypeFXSWAPWithSOH[5] = 88; // X
            SecurityTypeFXSWAPWithSOH[6] = 83; // S
            SecurityTypeFXSWAPWithSOH[7] = 87; // W
            SecurityTypeFXSWAPWithSOH[8] = 65; // A
            SecurityTypeFXSWAPWithSOH[9] = 80; // P
            SecurityTypeFXSWAPWithSOH[10] = ProSecuritiesTrading.MOEX.FIX.Base.Message.Messages.SOH;

            SecurityTypeFXFWDWithSOH = new byte[10];
            SecurityTypeFXFWDWithSOH[0] = 49; // 1
            SecurityTypeFXFWDWithSOH[1] = 54; // 6
            SecurityTypeFXFWDWithSOH[2] = 55; // 7
            SecurityTypeFXFWDWithSOH[3] = 61; // =
            SecurityTypeFXFWDWithSOH[4] = 70; // F
            SecurityTypeFXFWDWithSOH[5] = 88; // X
            SecurityTypeFXFWDWithSOH[6] = 70; // F
            SecurityTypeFXFWDWithSOH[7] = 87; // W
            SecurityTypeFXFWDWithSOH[8] = 68; // D
            SecurityTypeFXFWDWithSOH[9] = ProSecuritiesTrading.MOEX.FIX.Base.Message.Messages.SOH;

            SecurityTypeFXBKTWithSOH = new byte[10];
            SecurityTypeFXBKTWithSOH[0] = 49; // 1
            SecurityTypeFXBKTWithSOH[1] = 54; // 6
            SecurityTypeFXBKTWithSOH[2] = 55; // 7
            SecurityTypeFXBKTWithSOH[3] = 61; // =
            SecurityTypeFXBKTWithSOH[4] = 70; // F
            SecurityTypeFXBKTWithSOH[5] = 88; // X
            SecurityTypeFXBKTWithSOH[6] = 66; // B
            SecurityTypeFXBKTWithSOH[7] = 75; // K
            SecurityTypeFXBKTWithSOH[8] = 84; // T
            SecurityTypeFXBKTWithSOH[9] = ProSecuritiesTrading.MOEX.FIX.Base.Message.Messages.SOH;

            SecurityTypeREPOWithSOH = new byte[10];
            SecurityTypeREPOWithSOH[0] = 49; // 1
            SecurityTypeREPOWithSOH[1] = 54; // 6
            SecurityTypeREPOWithSOH[2] = 55; // 7
            SecurityTypeREPOWithSOH[3] = 61; // =
            SecurityTypeREPOWithSOH[4] = 82; // R
            SecurityTypeREPOWithSOH[5] = 69; // E
            SecurityTypeREPOWithSOH[6] = 80; // P
            SecurityTypeREPOWithSOH[7] = 79; // O
            SecurityTypeREPOWithSOH[8] = ProSecuritiesTrading.MOEX.FIX.Base.Message.Messages.SOH;
        }
    }
}
