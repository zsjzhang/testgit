using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class SSIQuestionnaireController : Controller
    {
        // GET: SSIQuestionnaire
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostSSIQuestionnaire(string section_name, string section_tel, string section_car, string section_color, string arrays)
        {

            if (arrays == null || (arrays!=null && arrays.Split(',').Count()!=10) || string.IsNullOrWhiteSpace(section_name) || string.IsNullOrWhiteSpace(section_tel) || string.IsNullOrWhiteSpace(section_car) || string.IsNullOrWhiteSpace(section_car))
            {
                return Json(new{Code=201,Msg="参数不能为空"});
            }
            var array = arrays.Split(',');
            SSIQuestion question = new SSIQuestion
            {
                section_name=section_name,
                section_tel=section_tel,
                section_car=section_car,
                section_color=section_color,
                N1=array[0],
                N2 = array[1],
                N3 = array[2],
                N4 = array[3],
                N5 = array[4],
                N6 = array[5],
                N7 = array[6],
                N8 = array[7],
                N9 = array[8],
                N10 = array[9]
            };
            int flag=_AppContext.QuestionnaireApp.SSICreate(question);
            if (flag>0)
            {
                return Json(new { Code = 200, Msg = "提交成功" }); 
            }
            return Json(new { Code = 202, Msg = "未知错误" }); ;

        }

    }
}