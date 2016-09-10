using System;

using ProSecuritiesTrading.MOEX.FIX.Base.Group;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Message.ASTS
{
    public class TestRequestData
    {
        public byte[] MessageBytes;
        public HeaderData Header;

        public string TestReqID = null;
        public byte[] TestReqIDBytes = null;

        // <Trailer>
        public int CheckSum = -1;
        // </Trailer>

        public TestRequestData(byte[] buffer, HeaderData header)
        {
            this.MessageBytes = buffer;
            this.Header = header;
        }
    }
}
