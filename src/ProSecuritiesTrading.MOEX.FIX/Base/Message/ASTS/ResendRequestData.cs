using System;

using ProSecuritiesTrading.MOEX.FIX.Base.Group;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class ResendRequestData
    {
        public byte[] MessageBytes;
        public HeaderData Header;

        public int BeginSeqNo = 0;
        public int EndSeqNo = 0;

        // <Trailer>
        public int CheckSum = -1;
        // </Trailer>

        public ResendRequestData(byte[] buffer, HeaderData header)
        {
            this.MessageBytes = buffer;
            this.Header = header;
        }
    }
}
