using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class CaptchaController : BaseController
    {
        //
        // GET: /Captcha/
        public ActionResult Index()
        {
            Dictionary<int, string> _result = new Dictionary<int, string>();
            _result.Add(0, "zhangsan0");
            _result.Add(1, "zhangsan1");
            _result.Add(2, "zhangsan2");

            ViewBag.title = "发送验证码";
            ViewData["content"] = "验证码为123456";
            return View(_result);
        }

        [HttpPost]
        public ContentResult GetContent()
        {
            Dictionary<int, string> _result = new Dictionary<int, string>();
            _result.Add(0, "zhangsan0");
            _result.Add(1, "zhangsan1");
            _result.Add(2, "zhangsan2");

            ViewBag.title = "发送验证码";
            ViewData["content"] = "验证码为123456";
            string result = RenderPartialViewToString("Index", _result);
            return Content(result);
        }

        /// <summary>
        /// 验证码发送
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendCaptcha(string mobile)
        {
            ReturnResult _result = _AppContext.UserSecurityApp.SendMobileVerifyCode(mobile, 4,"blms");
            if (_result.IsSuccess)
            {
                return Json(new { code = 200, msg = _result.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 400, msg = _result.Message }, JsonRequestBehavior.AllowGet);

           // return Json(new { code = 400, msg = "发送失败" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult sendCaptchaCenter(string mobile)
        {
            ReturnResult _result = _AppContext.UserSecurityApp.SendMobileVerifyCode(mobile, 4, "blms");
            if (_result.IsSuccess)
            {
                return Json(new { code = 200, msg = _result.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 400, msg = _result.Message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证码发送
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult sendCaptchaAndCheckImageCode(string mobile,string imageCode)
        {
            //确认验证码
            var validateCode = string.Empty;

            if (Session["ValidateCode"] != null)
                validateCode = Session["ValidateCode"].ToString();

            if (!validateCode.Equals(imageCode))
            {
                return Json(new { code = 401, msg = "图形验证码输入错误" });
            }
            ReturnResult _result = _AppContext.UserSecurityApp.SendMobileVerifyCode(mobile, 4, "blms");
            if (_result.IsSuccess)
            {
                return Json(new { code = 200, msg = _result.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 400, msg = _result.Message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult sendCaptchaByFindPassWord(string mobile, string imageCode)
        {
            //确认验证码
            var validateCode = string.Empty;

            if (Session["ValidateCode"] != null)
                validateCode = Session["ValidateCode"].ToString();

            if (!validateCode.Equals(imageCode))
            {
                return Json(new { code = 401, msg = "图形验证码输入错误" });
            }
            ReturnResult _result = _AppContext.UserSecurityApp.FindPassword(mobile,"blms");
            if (_result.IsSuccess)
            {
                return Json(new { code = 200, msg = _result.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 400, msg = _result.Message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult sendCaptchaByChangePassword(string mobile, string imageCode)
        {
            //确认验证码
            var validateCode = string.Empty;

            if (Session["ValidateCode"] != null)
                validateCode = Session["ValidateCode"].ToString();

            if (!validateCode.Equals(imageCode))
            {
                return Json(new { code = 401, msg = "图形验证码输入错误" });
            }
            ReturnResult _result = _AppContext.UserSecurityApp.ChangePassword(mobile, "blms");
            if (_result.IsSuccess)
            {
                return Json(new { code = 200, msg = _result.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 400, msg = _result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult sendCaptchaByRegister(string mobile, string imageCode)
        {
            //确认验证码
            var validateCode = string.Empty;

            if (Session["ValidateCode"] != null)
                validateCode = Session["ValidateCode"].ToString();

            if (!validateCode.Equals(imageCode))
            {
                return Json(new { code = 401, msg = "图形验证码输入错误" });
            }
            ReturnResult _result = _AppContext.UserSecurityApp.RegisterAcount(mobile, "blms");
            if (_result.IsSuccess)
            {
                return Json(new { code = 200, msg = _result.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 400, msg = _result.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}