using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Webdiyer.WebControls.Mvc;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class MsgCenterController : Controller
    {
        // GET: MsgCenter
        public ActionResult Index(int msgType = 0, int pageindex = 1)
        {
            int pageSize =8;
            int totalCount;
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account");
            }
            MsgCenterViewModel msgCenterViewModel = new MsgCenterViewModel();
            if (!string.IsNullOrEmpty(Request.QueryString["msgType"]))
            {
                msgCenterViewModel.CurrentMsgType = (MessageType)Convert.ToInt32(Request.QueryString["msgType"]);
            }
            var userId = User.Identity.GetUserId();
            if (msgType > 0)
            {
                _AppContext.UserMessageRecordApp.UpdateState(userId, msgType);
            }
            msgCenterViewModel.UnReadMsgList = _AppContext.UserMessageRecordApp.StatisticsUnReadMsgType(userId).ToList();
            var list = _AppContext.UserMessageRecordApp.GetUserMessageRecords(msgType, userId, pageindex, pageSize, out totalCount);
            msgCenterViewModel.PageList = new PagedList<UserMessageRecord>(list, pageindex, pageSize, totalCount);
            return View(msgCenterViewModel);
        }

    }
}