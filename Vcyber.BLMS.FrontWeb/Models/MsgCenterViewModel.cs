using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Webdiyer.WebControls.Mvc;

namespace Vcyber.BLMS.FrontWeb
{
    public class MsgCenterViewModel
    {
        public IList<UnReadMsgStatistics> UnReadMsgList { get; set; }

        public PagedList<UserMessageRecord> PageList { get; set; }
        public MessageType CurrentMsgType { get; set; }

        public string GetMsgTypeTip(MessageType messageType)
        {
            var unReadMsgStatistics = UnReadMsgList.SingleOrDefault(p => p.MessageType == messageType);
            if (unReadMsgStatistics != null)
            {
                return unReadMsgStatistics.MsgCount + "条";
            }
            return "无";
        }
        public int GetMsgTypeCount(MessageType messageType)
        {
            var unReadMsgStatistics = UnReadMsgList.SingleOrDefault(p => p.MessageType == messageType);
            if (unReadMsgStatistics != null)
            {
                return unReadMsgStatistics.MsgCount;
            }
            return 0;
        }
        public string GetMsgTypeCss(MessageType messageType)
        {
            if (messageType == CurrentMsgType)
            {
                return "messages_active";
            }
            return string.Empty;
        }

        public string MsgTitle
        {
            get { return CurrentMsgType.GetDiscribe(); }
        }
    }
}