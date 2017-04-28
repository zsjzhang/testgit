using System.Linq;
using System.Security.Principal;

namespace Vcyber.BLMS.FrontWeb.Models.BBS
{
    public class Check
    {
        private static BBSEntities db = new BBSEntities();

        public static BBSMember yanzhengyonghu(BBSMember use)
        {
            var u = from us in db.BBSMember where us.UserName == use.UserName && us.PassWord == use.PassWord select us;

            if (u.Count() != 0)
            {
                return u.First();
            }
            else { return null; }

        }
        public static bool YanzhengRegister(BBSMember use)
        {
            var u = from us in db.BBSMember where us.UserName == use.UserName select us;
            return (u.Count() != 0);
        }

        public static bool IsNull(string str, string Key, string ErrorMessage, System.Web.Mvc.Controller controller)
        {
            if (string.IsNullOrEmpty(str))
            {
                controller.ModelState.AddModelError(Key, ErrorMessage);
                return true;
            }
            return false;
        }
        public static bool IsLength(string str,int value, string Key, string ErrorMessage, System.Web.Mvc.Controller controller)
        {
            if (str.Length > value)
            {
                controller.ModelState.AddModelError(Key, ErrorMessage);
                return true;
            }
            return false;
        }
        public static bool CheckPower(System.Web.Mvc.ControllerContext filterContext)
        {
            if (!IsLogin(filterContext))
                return false;

            if (int.Parse(filterContext.HttpContext.Session["User_qx"].ToString()) != 1)
            {
                if (int.Parse(filterContext.HttpContext.Session["User_qx"].ToString()) != 1)
                {
                    filterContext.Controller.ViewData["Message"] = "很抱歉,您不是管理员,无法进行此操作！";
                    filterContext.Controller.ViewData["Message1"] = "主页";
                    filterContext.Controller.ViewData["Url"] = "/Home/Index";
                    return false;
                }
            }
            return true;
        }
        public static bool IsLogin(System.Web.Mvc.ControllerContext filterContext)
        {
            if ( !filterContext.HttpContext.User.Identity.IsAuthenticated )
            {
                filterContext.Controller.ViewData["Message"] = "很抱歉,您没登录无法进行此操作!";
                filterContext.Controller.ViewData["Message1"] = "登录页面";
                filterContext.Controller.ViewData["Url"] = "/Member/Login";

                return false;
            }
            return true;
        }

        public static string StieName
        {
            get { return "车主论坛"; }
        }
         
    }
}