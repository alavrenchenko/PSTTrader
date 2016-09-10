using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Group
{
    public class HeaderData
    {
        public string BeginString;
        public int BodyLength = 0;
        public string MsgType = null;
        public string SenderCompID = null;
        public string TargetCompID = null;
        public int MsgSeqNum = 0;
        public byte PossDupFlag = 0;
        public byte PossResend = 0;
        public DateTime SendingTime = DateTime.MinValue;
        public DateTime OrigSendingTime = DateTime.MinValue;
    }
}
