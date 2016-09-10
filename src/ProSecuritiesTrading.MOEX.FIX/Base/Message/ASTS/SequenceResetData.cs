using System;

using ProSecuritiesTrading.MOEX.FIX.Base.Group;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class SequenceResetData
    {
        public byte[] MessageBytes;
        public HeaderData Header;

        public byte GapFillFlag = 0;
        public int NewSeqNo = 0;

        // <Trailer>
        public int CheckSum = -1;
        // </Trailer>

        public SequenceResetData(byte[] buffer, HeaderData header)
        {
            this.MessageBytes = buffer;
            this.Header = header;
        }
    }
}
