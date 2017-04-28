using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.FrontWeb.Models.BBS;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    [HandleError]
    public class BBSHomeController : Controller
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

        public BBSHomeController()
        {
        }

        public BBSHomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion


        private const int _PostPageSize = 10;
        private const int _CommentPageSize = 10;

        private BBSEntities db = new BBSEntities();

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return Redirect("~/bbs");
            List<BBSColumns> col = (from m in db.BBSColumns select m).ToList();
            var newUser = (from m in db.BBSMember orderby m.Id descending select m).FirstOrDefault();
            ViewBag.NewUser = newUser == null ? String.Empty : newUser.UserName;
            ViewBag.MemberCount = db.BBSMember.Count();
            //var a = db.BBSGuestBook.Where(e => e.G_Recycle == 1);

            ViewBag.CommentCount = db.BBSComment.Count(e => e.BBSGuestBook.G_Recycle == 1);// a.Sum(e => e.Comment.Count());
            ViewBag.GuestBookCount = db.BBSGuestBook.Count(e => e.G_Recycle == 1);

            DateTime _curDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime _PreDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString());
            ViewBag.CurDayBookCount = db.BBSGuestBook.Where(c => c.G_Time != null && c.G_Time > _curDate).ToList().Count;
            ViewBag.PrevDayBookCount = db.BBSGuestBook.Where(c => c.G_Time != null && c.G_Time > _PreDate && c.G_Time <= _curDate).ToList().Count();
            ViewBag.TotalGuest = db.BBSMember.ToList().Count();
            return View(col);
        }

        /// <summary>
        /// 置顶帖子
        /// </summary>
        /// <returns></returns>
        public ActionResult TopBBSGuestBook()
        {
            List<BBSGuestBook> _result = new List<BBSGuestBook>();
            List<BBSGuestBook> _gus = db.BBSGuestBook.ToList();
            //获取审核的并且设置为置顶的帖子
            IEnumerable<BBSGuestBook> _booklist = _gus.Where(c => c.G_isTop && c.G_approved);
            if (_booklist != null && _booklist.Any())
            {
                //最多取5条置顶帖子
                if (_booklist.Count() > 5)
                {
                    _result = _booklist.OrderByDescending(c => c.LastUpdateTime).Take(5).ToList();
                }
                else
                {
                    _result = _booklist.OrderByDescending(c => c.LastUpdateTime).ToList();
                }
            }
            return View(_result);

        }

        /// <summary>
        /// 列子列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public ActionResult Show(int? id, int? Page)
        {
            //List<BBSGuestBook> bookss = (from m in db.BBSGuestBook where m.G_Recycle == 1 && m.Column_Id == id orderby m.Id descending select m).ToList();
            var col = db.BBSColumns.FirstOrDefault(e => e.Id == id);
            if (col == null)
            {
                ViewBag.Message = "不存在该栏目!";
                ViewBag.Message1 = "主页面";
                ViewBag.Url = "/bbsHome/Index";
                return View();
            }
            int PageIndex = Page ?? 1;
            if (PageIndex < 1) PageIndex = 1;
            var list = db.BBSGuestBook.Where(e => e.G_Recycle == 1 && e.Column_Id == id && e.G_approved).OrderByDescending(e => e.Id);

            ViewBag.PageIndex = PageIndex;
            ViewBag.PageSize = _PostPageSize;
            ViewBag.ReCordCount = list.Count();

            //ViewBag.ColumnName"] = bookss.First().Columns.Column_Name;
            ViewBag.ColumnName = col.Column_Name;
            ViewBag.ColumnId = col.Id;
            ViewBag.Id = id;


            int _CurDayBookCount = 0;
            int _PrevDayBookCount = 0;
            DateTime _curDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime _PreDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString());
            if (list != null && list.Any())
            {
                _CurDayBookCount = list.Where(c => c.G_Time != null && c.G_Time > _curDate).ToList().Count;
                _PrevDayBookCount = list.Where(c => c.G_Time != null && c.G_Time > _PreDate && c.G_Time <= _curDate).ToList().Count();
            }
            ViewBag.CurDayBookCount = _CurDayBookCount;
            ViewBag.PrevDayBookCount = _PrevDayBookCount;
            ViewBag.TotalGuest = db.BBSMember.ToList().Count();
            return View(FPage.GetPageList(list, PageIndex, _PostPageSize));
        }

        /// <summary>
        /// 论坛帖子分页
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxPagerShow(int? id, int? Page, int pageSize = _PostPageSize)
        {//List<BBSGuestBook> bookss = (from m in db.BBSGuestBook where m.G_Recycle == 1 && m.Column_Id == id orderby m.Id descending select m).ToList();
            int _id = 1;
            if (id == null)
            {
                int.TryParse(Request.QueryString["id"], out _id);
            }
            id = _id;
            var col = db.BBSColumns.FirstOrDefault(e => e.Id == id);
            if (col == null)
            {
                ViewBag.Message = "不存在该栏目!";
                ViewBag.Message1 = "主页面";
                ViewBag.Url = "/bbsHome/Index";
                return View();
            }
            int PageIndex = Page ?? 1;
            if (PageIndex < 1) PageIndex = 1;
            var list = db.BBSGuestBook.Where(e => e.G_Recycle == 1 && e.Column_Id == id && e.G_approved).OrderByDescending(e => e.Id);
            ViewBag.PageIndex = PageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.ReCordCount = list.Count();

            //ViewBag.ColumnName"] = bookss.First().Columns.Column_Name;
            ViewBag.ColumnName = col.Column_Name;
            ViewBag.ColumnId = col.Id;
            return View(FPage.GetPageList(list, PageIndex, pageSize));
        }

        /// <summary>
        /// 创建帖子
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            IEnumerable<BBSColumns> col = from m in db.BBSColumns select m;
            ViewBag.PList = col.Select(e => new SelectListItem() { Text = e.Column_Name, Value = e.Id.ToString() });
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new { returnUrl = "/BBSHome/Create" });

            }
            return View();
        }

        /// <summary>
        /// 创建帖子保存
        /// </summary>
        /// <param name="gb"></param>
        /// <param name="G_Content"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(BBSGuestBook gb, string G_Content)
        {
            IEnumerable<BBSColumns> col = from m in db.BBSColumns select m;
            ViewBag.PList = col.Select(e => new SelectListItem() { Text = e.Column_Name, Value = e.Id.ToString() });
            if (!ModelState.IsValid)
                return View();
            if (string.IsNullOrEmpty(gb.G_Title))
            {
                ModelState.AddModelError("G_Title", "标题不得为空！");
                return View();
            }
            if (string.IsNullOrEmpty(G_Content))
            {
                ModelState.AddModelError("G_content", "留言内容不得为空！");
                return View();
            }
            if (gb.G_Content.Length > 375)
            {
                ModelState.AddModelError("G_content", "留言数量不得大于375字！");
                return View();
            }
            if (Request.Form["YanZheng"].ToUpper() != Session["GoogleCode"].ToString())
            {
                ModelState.AddModelError("YanZhengError", "验证码错误！");
                return View();
            }



            gb.MemberId = User.Identity.GetUserId<string>();
            gb.G_Time = DateTime.Now;
            gb.G_Recycle = 1;
            gb.LastUpdateTime = DateTime.Now;
            gb.MemberName = UserManager.FindById(User.Identity.GetUserId()).NickName;
            gb.LastUpdateMemberName = gb.MemberName;

            db.BBSGuestBook.Add(gb);

            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult LogOff()
        {
            Session["User_qx"] = null;
            Session["UID"] = null;
            Session["username"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {

            if (!Check.CheckPower(this.ControllerContext)) return View("../bbsshared/Message");

            BBSGuestBook gb = (from m in db.BBSGuestBook where m.Id == id select m).First();
            if (!ModelState.IsValid)
                //return View(gb); need todo
                return null;
            gb.G_Recycle = 2;
            db.SaveChanges();
            ViewBag.Message = "该留言已被删除到回收站!";
            ViewBag.Message1 = "留言管理页面";
            ViewBag.Url = "/Admin/Admin_GBook";
            return View("../bbsshared/Message");
        }

        public ActionResult HuiFu(int id)
        {
            if (!Check.CheckPower(this.ControllerContext)) return View("../bbsshared/Message");
            BBSGuestBook gb = (from m in db.BBSGuestBook where m.Id == id select m).First();
            return View(gb);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult HuiFu(int Id, string G_HF_Content)
        {
            BBSGuestBook gbook = (from m in db.BBSGuestBook where m.Id == Id select m).First();
            if (string.IsNullOrEmpty(G_HF_Content))
            {
                ModelState.AddModelError("G_HF_Content", "回复内容不得为空！");
                return View(gbook);
            }
            if (G_HF_Content.Length > 375)
            {
                ModelState.AddModelError("G_HF_Content", "回复内容不得大于375字！");
                return View(gbook);
            }
            if (!ModelState.IsValid)
                return View(gbook);
            gbook.G_HF_Content = G_HF_Content;

            db.SaveChanges();
            ViewBag.Message = "该留言回复成功!";
            ViewBag.Message1 = "主页面";
            ViewBag.Url = "/BBSHome/Index";
            return View("../bbsshared/Message"); ;
        }

        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public ActionResult ColumSection(string columnName)
        {
            ViewBag.columnName = columnName;
            var col = db.BBSColumns.SingleOrDefault(c => c.Column_Name == columnName);
            return View(col);
        }

        /// <summary>
        /// 热门帖子
        /// </summary>
        /// <returns></returns>
        public ActionResult HoTipcs()
        {
            var gbs = db.BBSGuestBook.OrderByDescending(s => s.G_ResponseCount).Take(5).ToList();
            return View(gbs);

        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Comment(int? id, int? page)
        {
            var list = db.BBSGuestBook.Where(e => e.Id == id);
            var gb = list.FirstOrDefault();

            if (gb == null)
            {
                ViewData["Message"] = "不存在该留言!";
                ViewData["Message1"] = "主页面";
                ViewData["Url"] = "/Home/Index";
                return View("../BBSShared/Message");
            }
            gb.G_ReadCount = gb.G_ReadCount + 1;
            db.SaveChanges();

            ViewData["Column_Name"] = gb.BBSColumns.Column_Name;
            int PageIndex = page ?? 1;
            if (PageIndex < 1) PageIndex = 1;

            var CommList = db.BBSComment.Where(e => e.GuestBookId == gb.Id).OrderBy(e => e.Id);
            ViewData["PageIndex"] = PageIndex;
            ViewData["PageSize"] = _CommentPageSize;
            ViewData["ReCordCount"] = CommList.Count();
            ViewData["col"] = FPage.GetPageList(CommList, PageIndex, _CommentPageSize).ToList();

            ViewData["floor"] = ((PageIndex - 1) * _CommentPageSize) + 1;
            return View(gb);

        }

        /// <summary>
        /// 异步加载评论
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult AjaxPagerComment(int? id, int? page, int pageSize = _PostPageSize)
        {

            var list = db.BBSGuestBook.Where(e => e.Id == id);
            var gb = list.FirstOrDefault();

            if (gb == null)
            {
                ViewData["Message"] = "不存在该留言!";
                ViewData["Message1"] = "主页面";
                ViewData["Url"] = "/Home/Index";
                return View("../BBSShared/Message");
            }
            gb.G_ReadCount = gb.G_ReadCount + 1;
            db.SaveChanges();

            ViewData["Column_Name"] = gb.BBSColumns.Column_Name;
            int PageIndex = page ?? 1;
            if (PageIndex < 1) PageIndex = 1;

            var CommList = db.BBSComment.Where(e => e.GuestBookId == gb.Id).OrderBy(e => e.Id);
            ViewData["PageIndex"] = PageIndex;
            ViewData["PageSize"] = pageSize;
            ViewData["ReCordCount"] = CommList.Count();
            ViewData["col"] = FPage.GetPageList(CommList, PageIndex, pageSize).ToList();

            ViewData["floor"] = ((PageIndex - 1) * pageSize) + 1;
            return View(gb);
        }

        /// <summary>
        /// 热门活动
        /// </summary>
        /// <returns></returns>
        public ActionResult HotActivities()
        {
            List<BBSColumns> col = (from m in db.BBSColumns
                                    select m).ToList();
            return View(col);
        }

        /// <summary>
        /// 帖子评论
        /// </summary>
        /// <param name="com"></param>
        /// <param name="id"></param>
        /// <param name="Comments"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Comment(BBSComment com, int id, string Comments, int? page)
        {
            var list = db.BBSGuestBook.Where(e => e.Id == id);
            var gb = list.FirstOrDefault();
            if (!Check.IsLogin(this.ControllerContext))
                return RedirectToAction("LogonPage", "Account", new { returnUrl = string.Format("/BBSHome/Comment/{0}", id) });


            ViewData["Column_Name"] = gb.BBSColumns.Column_Name;

            int PageSize = 10;
            int PageIndex = page ?? 1;
            if (PageIndex < 1) PageIndex = 1;
            var CommList = db.BBSComment.Where(e => e.GuestBookId == gb.Id).OrderBy(e => e.Id);
            ViewData["PageIndex"] = PageIndex;
            ViewData["PageSize"] = PageSize;
            ViewData["ReCordCount"] = CommList.Count();
            ViewData["col"] = FPage.GetPageList(CommList, PageIndex, PageSize).ToList();


            ViewData["floor"] = ((PageIndex - 1) * PageSize) + 1;

            if (Comments.Length > 100)
            {
                ModelState.AddModelError("Comment_Messages", "回复内容不得超过100字！");
                return View(gb);
            }
            if (Request.Form["YanZheng"].ToUpper() != Session["GoogleCode"].ToString())
            {
                ModelState.AddModelError("YanZhengError", "验证码错误！");
                return View(gb);
            }

            com.Comments = Comments;
            com.GuestBookId = id;
            com.C_Time = DateTime.Now;
            com.MemberId = User.Identity.GetUserId();
            var u = UserManager.FindById(User.Identity.GetUserId());

            gb.LastUpdateMemberName = u.NickName;
            gb.LastUpdateTime = DateTime.Now;
            gb.G_ResponseCount = gb.G_ResponseCount + 1;

            db.BBSComment.Add(com);
            db.SaveChanges();
            return RedirectToAction("Comment", "BBSHome", new { id = id });
        }
    }
}
