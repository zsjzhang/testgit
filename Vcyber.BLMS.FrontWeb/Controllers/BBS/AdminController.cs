using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.FrontWeb.Models.BBS;
using System.Data.Linq.SqlClient;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class BBSAdminController : Controller
    {
        //
        // GET: /Recycle/
        private BBSEntities db = new BBSEntities();
        public ActionResult Recycle(int? id, int? page)
        {
            if (!Check.CheckPower(this.ControllerContext)) return View("Message");

            int PageSize = 15;
            int PageIndex = page ?? 1;
            if (PageIndex < 1) PageIndex = 1;//5+1+a+s+p+x
            //List<BBSGuestBook> gb =(from m in db.BBSGuestBook where m.G_Recycle == 2 orderby m.Id descending select m).ToList();

            var list = db.BBSGuestBook.Where(e => e.G_Recycle == 2).OrderByDescending(e => e.Id);
            ViewData["PageIndex"] = PageIndex;
            ViewData["PageSize"] = PageSize;
            ViewData["ReCordCount"] = list.Count();
            //if (list.Count() == 0) return View(list);
            return View(FPage.GetPageList(list, PageIndex, PageSize));
        }

        public ActionResult Restore(int id)
        {
            BBSGuestBook gb = (from m in db.BBSGuestBook where m.Id == id select m).First();
            if (!ModelState.IsValid)
                return View(gb);
            gb.G_Recycle = 1;
            db.SaveChanges();
            ViewData["Message"] = "该留言已成功还原!";
            ViewData["Message1"] = "主页面";
            ViewData["Url"] = "/Home/Index";
            return View("Message");
        }

        public ActionResult Delete(int id)
        {

            BBSGuestBook gb = (from m in db.BBSGuestBook where m.Id == id select m).First();
            List<BBSComment> cm = (from m in db.BBSComment where m.GuestBookId == id select m).ToList();
            db.BBSGuestBook.Remove(gb);
            foreach (var delcm in cm)
            {
                db.BBSComment.Remove(delcm);
            }
            db.SaveChanges();
            ViewData["Message"] = "操作成功!该留言已彻底删除!";
            ViewData["Message1"] = "主页面";
            ViewData["Url"] = "/Home/Index";
            return View("Message");
        }

        public ActionResult Columns()
        {
            if (!Check.CheckPower(this.ControllerContext)) return View("Message");
            List<BBSColumns> col = (from m in db.BBSColumns select m).ToList();
            return View(col);
        }

        public ActionResult Add_Columns()
        {
            if (!Check.CheckPower(this.ControllerContext)) return View("Message");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add_Columns(BBSColumns colu)
        {
            if (!Check.CheckPower(this.ControllerContext)) return View("Message");

            Check.IsNull(colu.Column_Name, "Column_Name", "栏目名不能为空！", this);
            Check.IsNull(colu.Info, "Info", "栏目名不能为空！", this);
            if (!ModelState.IsValid) return View();

            Check.IsLength(colu.Column_Name, 15, "Column_Name", "栏目名字数超过规定字数！", this);
            Check.IsLength(colu.Info, 30, "Info", "描述字数超过规定字数！", this);
            if (!ModelState.IsValid) return View();

            BBSColumns col = (from m in db.BBSColumns where m.Column_Name == colu.Column_Name select m).FirstOrDefault();
            if (col == null)
            {
                db.BBSColumns.Add(colu);
                db.SaveChanges();
                return RedirectToAction("Columns");
            }
            else
            {
                ModelState.AddModelError("Column_Name", "数据库已存在改栏目名！");
                return View();
            }
        }

        public ActionResult Del_Columns(int id)
        {
            var iscol = from m in db.BBSGuestBook where m.Column_Id == id select m;
            if (iscol.Count() != 0)
            {
                ViewData["Message"] = "该栏目已经存在留言,请先处理留言,再进行此操作！";
                ViewData["Message1"] = "栏目管理面";
                ViewData["Url"] = "/Admin/Columns";
                return View("Message");
            }

            BBSColumns col = (from m in db.BBSColumns where m.Id == id select m).First();
            db.BBSColumns.Remove(col);
            db.SaveChanges();
            return RedirectToAction("Columns");
        }

        public ActionResult Edit_Columns(int id)
        {
            //5!1+a+s+p+x
            if (!Check.CheckPower(this.ControllerContext)) return View("Message");
            BBSColumns col = (from m in db.BBSColumns where m.Id == id select m).First();
            return View(col);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_Columns(int id, BBSColumns colu)
        {
            BBSColumns c = (from m in db.BBSColumns where m.Id == id select m).FirstOrDefault();

            Check.IsNull(colu.Column_Name, "Column_Name", "栏目名不能为空！", this);
            Check.IsNull(colu.Info, "Info", "栏目名不能为空！", this);
            if (!ModelState.IsValid) return View();

            Check.IsLength(colu.Column_Name, 15, "Column_Name", "栏目名字数超过规定字数！", this);
            Check.IsLength(colu.Info, 30, "Info", "描述字数超过规定字数！", this);
            if (!ModelState.IsValid) return View();

            BBSColumns col = (from m in db.BBSColumns where m.Column_Name == colu.Column_Name && m.Id != id select m).FirstOrDefault();
            if (col == null)
            {
                c.Info = colu.Info;
                c.Column_Name = colu.Column_Name;
                db.SaveChanges();
                return RedirectToAction("Columns");
            }
            else
            {
                ModelState.AddModelError("Column_Name", "数据库已存在改栏目名！");
                return View();
            }
        }

        public ActionResult Admin_GBook(int? id, int? page)
        {
            if (!Check.CheckPower(this.ControllerContext)) return View("Message");
            int PageSize = 15;
            int PageIndex = page ?? 1;

            var list = db.BBSGuestBook.Where(e => e.G_Recycle == 1).OrderByDescending(e => e.Id);
            ViewData["PageIndex"] = PageIndex;
            ViewData["PageSize"] = PageSize;
            ViewData["ReCordCount"] = list.Count();

            return View(FPage.GetPageList(list, PageIndex, PageSize));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Admin_GBook(string KeyWord, int? page)
        {
            if (!Check.CheckPower(this.ControllerContext)) return View("Message");
            int PageSize = 15;
            int PageIndex = page ?? 1;

            var list = db.BBSGuestBook.Where(e => e.G_Recycle == 1 && e.G_Title.IndexOf(KeyWord) > -1).OrderByDescending(e => e.Id);
            var NewUser = (from m in db.BBSMember orderby m.Id descending select m).First();
            ViewData["PageIndex"] = PageIndex;
            ViewData["PageSize"] = PageSize;
            ViewData["ReCordCount"] = list.Count();

            ViewData["NewUser"] = NewUser.UserName;
            ViewData["MemberCount"] = db.BBSMember.Count();
            ViewData["CommentCount"] = db.BBSComment.Count();

            return View(FPage.GetPageList(list, PageIndex, PageSize));
        }

        public ActionResult Move(int id)
        {
            if (!Check.CheckPower(this.ControllerContext)) return View("Message");
            IEnumerable<BBSColumns> col = from m in db.BBSColumns select m;
            ViewData["PList"] = col.Select(e => new SelectListItem() { Text = e.Column_Name, Value = e.Id.ToString() });
            BBSGuestBook gb = (from m in db.BBSGuestBook where m.Id == id select m).First();
            return View(gb);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Move(int id, int Column_Id)
        {
            BBSGuestBook gb = (from m in db.BBSGuestBook where m.Id == id select m).First();
            gb.Column_Id = Column_Id;
            db.SaveChanges();
            ViewData["Message"] = "该留言移动成功！";
            ViewData["Message1"] = "留言管理页面";
            ViewData["Url"] = "/Admin/Admin_GBook";
            return View("Message");
        }

       
    }
}
