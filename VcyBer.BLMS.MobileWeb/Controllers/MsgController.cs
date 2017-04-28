using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;

namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class MsgController : BaseController
    {
        /// <summary>
        /// 系统消息 
        /// </summary>
        /// <param name="msgType">消息类型1=系统 2=保养 3=积分变动 4=卡券 5=服务和活动</param>
        /// <returns></returns>
        public ActionResult MsgList(string msgType)
        {
            //获取当前登录人
            if (!this.User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Login", "Account");
                return Redirect("/Account/Login?url=" + Request.RawUrl);
            }
            var type = int.Parse(msgType);
            var userId = User.Identity.GetUserId();

            //更新消息状态
            _AppContext.UserMessageRecordApp.UpdateState(userId, type);

            //获取消息
            var list = _AppContext.UserMessageRecordApp.GetMsgList(userId, type).ToList();

            return View(list);
        }
    }
}