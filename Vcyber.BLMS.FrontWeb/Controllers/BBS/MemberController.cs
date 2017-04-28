using System; 
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.FrontWeb.Models;
using Vcyber.BLMS.FrontWeb.Models.BBS;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class BBSMemberController : Controller
    {

           #region ==== 私有字段 ====

        private ApplicationUserManager _userManager;

        private ApplicationSignInManager _signInManager;

        #endregion

        #region ==== 公共属性 ====

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }


        #endregion

        #region ==== 构造函数 ====

        public BBSMemberController()
        {
        }

        public BBSMemberController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion

        //
        // GET: /Member/
        private BBSEntities db = new BBSEntities();

        public ActionResult SetAtt()
        {
            if (!Check.IsLogin(ControllerContext)) 
                return View("../BBSShared/Message");
            var uid =  User.Identity.GetUserId();
            BBSMember user = (from m in db.BBSMember where m.Id == uid select m).First();
            return View(user);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetAtt(string SetAtt)
        {
            if (!Check.IsLogin(this.ControllerContext))
                return View("../BBSShared/Message");
            var uid = User.Identity.GetUserId();
            BBSMember user = (from m in db.BBSMember where m.Id == uid select m).First();
            if (SetAtt.Length > 50)
            {
                ModelState.AddModelError("SetAtt", "个性签名不得大于50字！");
                return View();
            }
            if (!ModelState.IsValid)
                return View(user);
            user.SetAtt = SetAtt;
            db.SaveChanges();

            ViewData["Message"] = "个性签名保存成功!";
            ViewData["Message1"] = "主页面";
            ViewData["Url"] = "/Home/Index";
            return View("../BBSShared/Message");

        } 

        public void CheckAndCreateDefaultBBSMember(ApplicationUser user)
        {
            bool isExist = db.BBSMember.Any(m => m.Id.Equals(user.Id));
            if (isExist)
            {
                return;
            }
            var use = new BBSMember();
            use.Email = user.Email;
            use.Id = user.Id;
            use.UserName = user.NickName;
            use.Head = user.FaceImage;
            use.Sex = user.GenderName;
            use.Power = "2";
            use.Head = new Random().Next(7).ToString();
            db.BBSMember.Add(use);
            db.SaveChanges();
        }
         
        

        public ActionResult ImageList()
        {

            var ms = db.BBSMember.Take(4).ToList();
            return View(ms);
        }
    }
}
