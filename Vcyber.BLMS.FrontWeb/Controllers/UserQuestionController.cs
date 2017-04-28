using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class UserQuestionController : Controller
    {
        //
        // GET: /UserQuestion/
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        //获取问题列表
        [HttpPost]
        public ActionResult GetQuestions()
        {
            var result = new ReturnResult() { IsSuccess = true };

            var questions = _AppContext.UserSecurityApp.SelectQuestion();

            result.Data = questions.ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //创建用户密保
        [HttpPost]
        public ActionResult CreateUserQuestion(UserPwQuestion[] userQuestions)
        {
            string UserGuid = "a0630772-e5af-4eef-91f2-10395ace6546";

            var result = new ReturnResult { IsSuccess = true };

            var errors = new Dictionary<string, string>();

            //模型验证结果
            bool IsValid = true;

            //验证安全问题
            for (int x = 0; x < userQuestions.Length; x++)
            {
                if (string.IsNullOrEmpty(userQuestions[x].Answer) || userQuestions[x].PwId == 0)
                {
                    IsValid = false;
                    errors["Answer" + x] = "请选择问题并输入问题答案";
                }
            }

            if (!IsValid)
            {
                result.IsSuccess = false;
                result.Message = "输入数据验证未通过";
            }
            else
            {
                var userInfo = _AppContext.UserInfoApp.GetOne(UserGuid);

                //创建密保
                result = _AppContext.UserSecurityApp.CreateQuestionAndAnswer(userQuestions.ToList(), userInfo.Id);
            }

            //附加错误信息
            result.Errors = errors;

            return Json(result, JsonRequestBehavior.AllowGet);
        }


	}
}