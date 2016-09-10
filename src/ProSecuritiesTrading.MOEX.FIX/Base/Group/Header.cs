using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProSecuritiesTrading.MOEX.FIX.Base.Message;

namespace ProSecuritiesTrading.MOEX.FIX.Base.Group
{
    public class Header
    {
        // Tag = 8, String, FIX.4.4
        public readonly byte[] BeginString;

        public readonly byte[] BodyLength = null;
        public readonly byte[] MsgType = null;
        public readonly byte[] SenderCompID;
        public readonly byte[] TargetCompID;
        public readonly byte[] MsgSeqNum = null;
        public byte PossDupFlag = 0;
        public byte PossResend = 0;
        public readonly byte[] SendingTime = null;
        public readonly byte[] OrigSendingTime = null;
        public readonly byte[] SenderAndTargetCompIDWithSOH;

        public Header(byte[] beginString, string senderCompID, string targetCompID, byte possDupFlag, byte possResend)
        {
            this.BeginString = new byte[beginString.Length];
            Buffer.BlockCopy(beginString, 0, this.BeginString, 0, beginString.Length);
            this.SenderCompID = Encoding.ASCII.GetBytes("49=" + senderCompID);
            this.TargetCompID = Encoding.ASCII.GetBytes("56=" + targetCompID);
            this.PossDupFlag = possDupFlag;
            this.PossResend = possResend;

            int index = 0;
            int x;
            this.SenderAndTargetCompIDWithSOH = new byte[this.SenderCompID.Length + this.TargetCompID.Length + 2];

            if (this.SenderCompID.Length < 5)
            {
                for (x = 0; x < this.SenderCompID.Length; x++)
                {
                    this.SenderAndTargetCompIDWithSOH[index] = this.SenderCompID[x];
                    index++;
                }
            }
            else
            {
                Buffer.BlockCopy(this.SenderCompID, 0, this.SenderAndTargetCompIDWithSOH, index, this.SenderCompID.Length);
                index += this.SenderCompID.Length;
            }

            this.SenderAndTargetCompIDWithSOH[index] = Messages.SOH;
            index++;

            if (this.TargetCompID.Length < 5)
            {
                for (x = 0; x < this.TargetCompID.Length; x++)
                {
                    this.SenderAndTargetCompIDWithSOH[index] = this.TargetCompID[x];
                    index++;
                }
            }
            else
            {
                Buffer.BlockCopy(this.TargetCompID, 0, this.SenderAndTargetCompIDWithSOH, index, this.TargetCompID.Length);
                index += this.TargetCompID.Length;
            }

            this.SenderAndTargetCompIDWithSOH[index] = Messages.SOH;
        }

        public static void GetBytes()
        {

        }
    }
}
