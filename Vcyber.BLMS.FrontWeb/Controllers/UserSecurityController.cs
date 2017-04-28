using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

using Vcyber.BLMS.FrontWeb.Models;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class UserSecurityController : Controller
    {
        //
        // GET: /UserSecurity/
        public ActionResult Index()
        {
            return View();
        }

        //获取用户信息
        [HttpPost]
        public ActionResult GetLoginUserInfo()
        {
            var returnResult = new ReturnResult { IsSuccess = true };

            //获取登录信息
            var userLoginInfo = this.GetLogin();

            //用户未登录
            if (string.IsNullOrEmpty(userLoginInfo.UserGuid))
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "用户未登录";
            }
            else
            {
                //查询详细用户信息
                var userInfo = _AppContext.UserInfoApp.GetOne(userLoginInfo.UserGuid);

                //返回用户信息
                var result = new
                {
                    UserName = userInfo.UserName,
                    PhoneNumber = userInfo.PhoneNumber.Substring(0,3) + "****" + userInfo.PhoneNumber.Substring(7,4),
                    Email = userInfo.Email.Substring(0,1) + "****" + userInfo.Email.Substring(userInfo.Email.IndexOf('@')-1)
                };

                returnResult.Data = result;
            }

            return Json(returnResult, JsonRequestBehavior.AllowGet);
        }

        //获取密保问题
        [HttpPost]
        public ActionResult GetSecurityQuestions(string PhoneNumber)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            var user = _AppContext.UserInfoApp.SelectOneByPhone(PhoneNumber);

            //获取用户问题
            var questions = _AppContext.UserSecurityApp.SelectQuestionAndAnswer(user.Id);

            //清空密保问题答案
            foreach (var question in questions)
            {
                question.Answer = "";
            }

            result.Data = questions.ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //获取密保问题
        [HttpPost]
        public ActionResult GetSecurityQuestionsByUser()
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            //this.GetLogin().UserGuid;
            string UserGuid = "a0630772-e5af-4eef-91f2-10395ace6546";

            var user = _AppContext.UserInfoApp.GetOne(UserGuid);

            //获取用户问题
            var questions = _AppContext.UserSecurityApp.SelectQuestionAndAnswer(user.Id);

            //清空密保问题答案
            foreach (var question in questions)
            {
                question.Answer = "";
            }

            result.Data = questions.ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //验证手机号
        [HttpPost]
        public ActionResult ValidatePhoneNumber(string PhoneNumber, string ValidateCode)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            var errors = new Dictionary<string, string>();

            //模型验证结果
            bool IsValid = true;

            if (string.IsNullOrEmpty(PhoneNumber))
            {
                errors["PhoneNumber"] = "请输入已绑定手机号";
                IsValid = false;
            }

            if (string.IsNullOrEmpty(ValidateCode))
            {
                errors["ValidateCode"] = "请输入验证码";
                IsValid = false;
            }

            if (!IsValid)
            {
                result.IsSuccess = false;
                result.Message = "输入数据验证未通过";
            }
            else
            {
                //验证手机号
                var userInfo = _AppContext.UserInfoApp.SelectOneByPhone(PhoneNumber);

                if (userInfo == null)
                {
                    errors["PhoneNumber"] = "手机号未绑定或输入不正确";

                    result.IsSuccess = false;
                    result.Message = "手机验证未通过";
                }
                else
                {
                    //确认验证码
                    var validateCode = string.Empty;

                    if (Session["ValidateCode"] != null)
                        validateCode = Session["ValidateCode"].ToString();

                    if (!validateCode.Equals(ValidateCode))
                    {
                        errors["ValidateCode"] = "验证码输入不正确";

                        result.IsSuccess = false;
                        result.Message = "验证码确认未通过";
                    }
                }
            }

            //附加错误信息
            result.Errors = errors;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //验证手机号+身份证
        [HttpPost]
        public ActionResult ValidateByPhoneAndIdentity(string PhoneNumber, string PhoneValidateCode, string IdentityNumber)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            var errors = new Dictionary<string, string>();

            //模型验证结果
            bool IsValid = true;

            if (string.IsNullOrEmpty(PhoneValidateCode))
            {
                errors["PhoneValidateCode"] = "请输入验证码";
                IsValid = false;
            }

            if (string.IsNullOrEmpty(IdentityNumber))
            {
                errors["IdentityNumber"] = "请输入身份证号码";
                IsValid = false;
            }

            if (!IsValid)
            {
                result.IsSuccess = false;
                result.Message = "输入数据验证未通过";
            }
            else
            {
                result = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(PhoneNumber, PhoneValidateCode);

                if (result.IsSuccess)
                {
                    var userInfo = _AppContext.UserInfoApp.SelectOneByPhoneAndIdentity(PhoneNumber, IdentityNumber);
                    if (userInfo == null)
                    {
                        result.IsSuccess = false;
                        result.Message = "身份验证未通过，请确认身份证信息输入正确";

                        errors["IdentityNumber"] = "身份证号码错误，请重新输入";
                    }

                    //返回用户信息
                    result.Data = userInfo;
                }
                else
                    errors["PhoneValidateCode"] = "请输入正确验证码";
            }

            //附加错误信息
            result.Errors = errors;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //验证手机号+密保问题
        [HttpPost]
        public ActionResult ValidateByPhoneAndSecurityQuestion(string PhoneNumber, string PhoneValidateCode, UserPwQuestion[] questions)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            var errors = new Dictionary<string, string>();

            //模型验证结果
            bool IsValid = true;

            if (string.IsNullOrEmpty(PhoneValidateCode))
            {
                errors["PhoneValidateCode"] = "请输入验证码";
                IsValid = false;
            }

            //验证安全问题
            for (int x = 0; x < questions.Length; x++)
            {
                if (string.IsNullOrEmpty(questions[x].Answer))
                {
                    IsValid = false;
                    errors["Answer" + x] = "请输入密保答案";
                }
            }

            if (!IsValid)
            {
                result.IsSuccess = false;
                result.Message = "输入数据验证未通过";
            }
            else
            {
                //验证手机验证码
                result = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(PhoneNumber, PhoneValidateCode);

                if (result.IsSuccess)
                {
                    //获取用户信息
                    var userInfo = _AppContext.UserInfoApp.SelectOneByPhone(PhoneNumber);

                    //获取用户问题
                    var ques = _AppContext.UserSecurityApp.SelectQuestionAndAnswer(userInfo.Id);

                    //验证密保问题
                    for (int i = 0; i < questions.Length; i++)
                    {
                        var q = questions[i];

                        if (ques.Where(x => x.PwId == q.PwId && x.Answer == q.Answer).Count() < 1)
                        {
                            result.IsSuccess = false;
                            result.Message = "密保答案错误，请重新输入";
                            errors["Answer" + i] = "密保答案错误，请重新输入";
                        }
                    }

                    //返回用户信息
                    result.Data = userInfo;
                }
                else
                    errors["PhoneValidateCode"] = "请输入正确验证码";
            }

            //附加错误信息
            result.Errors = errors;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //验证邮箱+身份证
        [HttpPost]
        public ActionResult ValidateByEmailAndIdentity(string Email, string IdentityNumber)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            var errors = new Dictionary<string, string>();

            //模型验证结果
            bool IsValid = true;

            if (string.IsNullOrEmpty(Email))
            {
                errors["Email"] = "请输入邮箱";
                IsValid = false;
            }

            if (string.IsNullOrEmpty(IdentityNumber))
            {
                errors["IdentityNumber"] = "请输入身份证号码";
                IsValid = false;
            }

            if (!IsValid)
            {
                result.IsSuccess = false;
                result.Message = "输入数据验证未通过";
            }
            else
            {
                var userInfo = _AppContext.UserInfoApp.SelectOneByIdentityNumber(IdentityNumber);
                if (userInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "身份验证未通过，请确认身份证信息输入正确";

                    errors["IdentityNumber"] = "身份证号码错误，请重新输入";
                }
                else
                {
                    if (userInfo.Email != Email)
                    {
                        result.IsSuccess = false;
                        result.Message = "邮箱验证未通过，请确认邮箱输入正确";

                        errors["Email"] = "邮箱错误，请重新输入";
                    }
                }

                //返回用户信息
                result.Data = userInfo;
            }

            //附加错误信息
            result.Errors = errors;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //验证身份证+密保问题
        [HttpPost]
        public ActionResult ValidateByIdentityAndSecurityQuestion(string IdentityNumber, UserPwQuestion[] questions)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            var errors = new Dictionary<string, string>();

            //模型验证结果
            bool IsValid = true;

            if (string.IsNullOrEmpty(IdentityNumber))
            {
                errors["IdentityNumber"] = "请输入身份证号码";
                IsValid = false;
            }

            //验证安全问题
            for (int x = 0; x < questions.Length; x++)
            {
                if (string.IsNullOrEmpty(questions[x].Answer))
                {
                    IsValid = false;
                    errors["Answer" + x] = "请输入密保答案";
                }
            }

            if (!IsValid)
            {
                result.IsSuccess = false;
                result.Message = "输入数据验证未通过";
            }
            else
            {
                //获取用户信息
                var userInfo = _AppContext.UserInfoApp.SelectOneByIdentityNumber(IdentityNumber);

                if (userInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "身份验证未通过，请确认身份证信息输入正确";

                    errors["IdentityNumber"] = "身份证号码错误，请重新输入";
                }
                else
                {
                    //获取用户问题
                    var ques = _AppContext.UserSecurityApp.SelectQuestionAndAnswer(userInfo.Id);

                    //验证密保问题
                    for (int i = 0; i < questions.Length; i++)
                    {
                        var q = questions[i];

                        if (ques.Where(x => x.PwId == q.PwId && x.Answer == q.Answer).Count() < 1)
                        {
                            result.IsSuccess = false;
                            result.Message = "密保答案错误，请重新输入";
                            errors["Answer" + i] = "密保答案错误，请重新输入";
                        }
                    }
                }

                //返回用户信息
                result.Data = userInfo;
            }

            //附加错误信息
            result.Errors = errors;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //验证支付密码
        [HttpPost]
        public ActionResult ValidatePayPassword(string UserGuid, string PayPassword)
        {
            var result = new ReturnResult { IsSuccess = true };

            var errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(PayPassword))
            {
                result.IsSuccess = false;
                errors["PayPassword"] = "请输入支付密码";

                result.Message = "数据验证未通过";
            }

            result.Errors = errors;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //验证支付密码+密保问题
        [HttpPost]
        public ActionResult ValidatePayPasswordAndSecurityQuestion(string PayPassword, UserPwQuestion[] questions)
        {
            string UserGuid = "a0630772-e5af-4eef-91f2-10395ace6546";

            var result = new ReturnResult { IsSuccess = true };

            var errors = new Dictionary<string, string>();

            //模型验证结果
            bool IsValid = true;

            if (string.IsNullOrEmpty(PayPassword))
            {
                IsValid = false;
                errors["PayPassword"] = "请输入支付密码";
            }

            //验证安全问题
            for (int x = 0; x < questions.Length; x++)
            {
                if (string.IsNullOrEmpty(questions[x].Answer))
                {
                    IsValid = false;
                    errors["Answer" + x] = "请输入密保答案";
                }
            }

            if (!IsValid)
            {
                result.IsSuccess = false;
                result.Message = "输入数据验证未通过";
            }
            else
            {
                //获取用户信息
                var userInfo = _AppContext.UserInfoApp.GetOne(UserGuid);

                //获取用户问题
                var ques = _AppContext.UserSecurityApp.SelectQuestionAndAnswer(userInfo.Id);

                //验证密保问题
                for (int i = 0; i < questions.Length; i++)
                {
                    var q = questions[i];

                    if (ques.Where(x => x.PwId == q.PwId && x.Answer == q.Answer).Count() < 1)
                    {
                        result.IsSuccess = false;
                        result.Message = "密保答案错误，请重新输入";
                        errors["Answer" + i] = "密保答案错误，请重新输入";
                    }
                }
            }

            //附加错误信息
            result.Errors = errors;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}