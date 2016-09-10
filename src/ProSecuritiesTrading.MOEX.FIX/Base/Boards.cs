/*
   Copyright (C) 2016 Alexey Lavrenchenko (http://prosecuritiestrading.com/)

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;

namespace ProSecuritiesTrading.MOEX.FIX.Base
{
    public static class Boards
    {
        public static readonly byte[][] BoardsBytes;

        static Boards()
        {
            BoardsBytes = new byte[112][];
            byte[] board = null;

            // Equity & Bond Market
            // Market ID: FOND
            // Order-driven market with T0 settlement

            // AUCT
            board = new byte[4];
            board[0] = 65;
            board[1] = 85;
            board[2] = 67;
            board[3] = 84;
            BoardsBytes[0] = board;

            // AUBB
            board = new byte[4];
            board[0] = 65;
            board[1] = 85;
            board[2] = 66;
            board[3] = 66;
            BoardsBytes[1] = board;

            // EQDB
            board = new byte[4];
            board[0] = 69;
            board[1] = 81;
            board[2] = 68;
            board[3] = 66;
            BoardsBytes[2] = board;

            // EQDP
            board = new byte[4];
            board[0] = 69;
            board[1] = 81;
            board[2] = 68;
            board[3] = 80;
            BoardsBytes[3] = board;

            // EQEO
            board = new byte[4];
            board[0] = 69;
            board[1] = 81;
            board[2] = 69;
            board[3] = 79;
            BoardsBytes[4] = board;

            // EQEU
            board = new byte[4];
            board[0] = 69;
            board[1] = 81;
            board[2] = 65;
            board[3] = 85;
            BoardsBytes[5] = board;

            // EQGO
            board = new byte[4];
            board[0] = 69;
            board[1] = 81;
            board[2] = 71;
            board[3] = 79;
            BoardsBytes[6] = board;

            // EQOB
            board = new byte[4];
            board[0] = 69;
            board[1] = 81;
            board[2] = 79;
            board[3] = 66;
            BoardsBytes[7] = board;

            // EQQI
            board = new byte[4];
            board[0] = 69;
            board[1] = 81;
            board[2] = 81;
            board[3] = 73;
            BoardsBytes[8] = board;

            // EQTC
            board = new byte[4];
            board[0] = 69;
            board[1] = 81;
            board[2] = 84;
            board[3] = 67;
            BoardsBytes[9] = board;

            // EQYO
            board = new byte[4];
            board[0] = 69;
            board[1] = 81;
            board[2] = 89;
            board[3] = 79;
            BoardsBytes[10] = board;

            // Market ID: FNDT
            // Order-driven market with T+ settlement

            // SMAL
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[11] = board;

            // TQBD
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 66;
            board[3] = 68;
            BoardsBytes[12] = board;

            // TQBR
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 66;
            board[3] = 82;
            BoardsBytes[13] = board;

            // TQDB
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 68;
            board[3] = 66;
            BoardsBytes[14] = board;

            // TQDE
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 68;
            board[3] = 69;
            BoardsBytes[15] = board;

            // TQIF
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 73;
            board[3] = 70;
            BoardsBytes[16] = board;

            // TQOB
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 79;
            board[3] = 66;
            BoardsBytes[17] = board;

            // TQOD
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 79;
            board[3] = 68;
            BoardsBytes[18] = board;

            // TQQD
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 81;
            board[3] = 68;
            BoardsBytes[19] = board;

            // TQQI
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 81;
            board[3] = 73;
            BoardsBytes[20] = board;

            // TQTC
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 84;
            board[3] = 67;
            BoardsBytes[21] = board;

            // TQTD
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 84;
            board[3] = 68;
            BoardsBytes[22] = board;

            // TQTF
            board = new byte[4];
            board[0] = 84;
            board[1] = 81;
            board[2] = 84;
            board[3] = 70;
            BoardsBytes[23] = board;

            // Market ID: REPT
            // REPO with Central counterparty

            // -

            // EQRD
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[24] = board;

            // EQRE
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[25] = board;

            // EQRP
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[26] = board;

            // EQWD
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[27] = board;

            // EQWE
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[28] = board;

            // EQWP
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[29] = board;

            // PSRD
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[30] = board;

            // PSRE
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[31] = board;

            // PSRP
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[32] = board;

            // PFNE
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[33] = board;

            // PFND
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[34] = board;

            // PFNU
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[35] = board;

            // Market ID: TECH
            // Technical boards

            // -

            // NADM
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[36] = board;

            // SADM
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[37] = board;

            // TADM
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[38] = board;

            // TRAN
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[39] = board;

            // Market ID: RPS
            // Negotiated deals and REPO (Quote-driven market)

            // -

            // FBCE
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[40] = board;

            // FBCU
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[41] = board;

            // FBCB
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[42] = board;

            // FBFX
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[43] = board;

            // IRK2
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[44] = board;

            // PACY
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[45] = board;

            // +
            // PAEU
            board = new byte[4];
            board[0] = 80;
            board[1] = 65;
            board[2] = 69;
            board[3] = 85;
            BoardsBytes[46] = board;

            // PAGB
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[47] = board;

            // +
            // PAUS
            board = new byte[4];
            board[0] = 80;
            board[1] = 65;
            board[2] = 85;
            board[3] = 83;
            BoardsBytes[48] = board;

            // PSAU
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[49] = board;

            // PSBB
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[50] = board;

            // PSDB
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[51] = board;

            // PSDE
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[52] = board;

            // PSEO
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[53] = board;

            // PSEQ
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[54] = board;

            // PSEU
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[55] = board;

            // PSGO
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[56] = board;

            // PSIF
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[57] = board;

            // PSOB
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[58] = board;

            // PSQI
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[59] = board;

            // PSSD
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[60] = board;

            // PSTC
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[61] = board;

            // PSTD
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[62] = board;

            // PSTF
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[63] = board;

            // PSYO
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[64] = board;

            // PTOD
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[65] = board;

            // PTSD
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[66] = board;

            // PTTD
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[67] = board;

            // RPEO
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[68] = board;

            // RPEU
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[69] = board;

            // RPEY
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[70] = board;

            // RPGO
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[71] = board;

            // RPMA
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[72] = board;

            // RPMO
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[73] = board;

            // RPUA
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[74] = board;

            // RPUO
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[75] = board;

            // Market ID: RPST
            // Negotiated deals with CCP (quote-driven market)

            // -

            // PTDE
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[76] = board;

            // PTEQ
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[77] = board;

            // PTIF
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[78] = board;

            // PTTF
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[79] = board;

            // PTOB
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[80] = board;

            // PTQI
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[81] = board;

            // PTTC
            board = new byte[4];
            board[0] = 80;
            board[1] = 65;
            board[2] = 69;
            board[3] = 85;
            BoardsBytes[82] = board;

            // SPEQ
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[83] = board;

            // SPOB
            board = new byte[4];
            board[0] = 80;
            board[1] = 65;
            board[2] = 85;
            board[3] = 83;
            BoardsBytes[84] = board;

            // Market ID: GCDP
            // РЕПО с ЦК: КСУ (клиринговые сертификаты участия) / REPO with CCP: GCP (general collateral pool certificates)

            // -

            // GCRP
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[85] = board;

            // GCOW
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[86] = board;

            // GCSW
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[87] = board;

            // GCOM
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[88] = board;

            // GCSM
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[89] = board;

            // GCTM
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[90] = board;

            // PSGC
            board = new byte[4];
            board[0] = 80;
            board[1] = 65;
            board[2] = 69;
            board[3] = 85;
            BoardsBytes[91] = board;

            // GCTR
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[92] = board;

            // Market ID: RPNG
            // OTC REPO with Central Bank

            // -

            // RPNG
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[93] = board;

            // RPFG
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[94] = board;

            // Money Market
            // Loans and Deposits

            // -

            // CBCR
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[95] = board;

            // CRED
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[96] = board;

            // DEPZ
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[97] = board;

            // DPAO
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[98] = board;

            // DPAC
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[99] = board;

            // DPFK
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[100] = board;

            // DPFO
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[101] = board;

            // DPPF
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[102] = board;

            // DPVB
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[103] = board;

            // FX Market
            // Market ID: CURR

            // -

            // AETS
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[104] = board;

            // AUCB
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[105] = board;

            // +
            // CETS
            board = new byte[4];
            board[0] = 67;
            board[1] = 69;
            board[2] = 84;
            board[3] = 83;
            BoardsBytes[106] = board;

            // CNGD
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[107] = board;

            // FUTN
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[108] = board;

            // FUTS
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[109] = board;

            // RSKC
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[110] = board;

            // SHFT
            board = new byte[4];
            board[0] = 83;
            board[1] = 77;
            board[2] = 65;
            board[3] = 76;
            BoardsBytes[111] = board;
        }
    }
}
