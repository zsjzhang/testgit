using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Logging;
using Vcyber.BLMS.ManageWeb.Models.BBS;
using Vcyber.BLMS.Application;
using AspNet.Identity.SQL;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class BBSAdminController : Controller
    {
        //
        // GET: /approve/
        private BBSEntities db = new BBSEntities();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult PartialPage(int columnId = 0, int approved = -1, int isTopped = -1, int isHoted = -1 , string title = null, int index = 1, int size = 10)
        {
            var q = from b in db.BBSGuestBook
                    select b;

            if (columnId > 0)
            {
                q = q.Where(b => b.BBSColumns.Id == columnId);
            }

            if (!string.IsNullOrEmpty(title))
            {
                q = q.Where(b => b.G_Title.Contains(title));
            }

            if (approved != -1)
            {
                q = q.Where(p => p.G_approved == (approved == 1));
            }

            if (isTopped != -1)
            {
                q = q.Where(p => p.G_isTop == (isTopped == 1));
            }

            if (isHoted != -1)
            {
                q = q.Where(p => p.G_isHot == (isHoted == 1));
            }

            int total = q.Count();
            var list = q.OrderByDescending(b => b.G_Time).Skip(size * (index - 1)).Take(size).ToList();

            int count = (int)Math.Ceiling((double)total / (double)size);
            ViewBag.PageIndex = index;
            ViewBag.PrePage = index > 1 ? (index - 1) : 1;
            ViewBag.NextPage = index < count ? (index + 1) : count;
            ViewBag.EndPage = count;

            return PartialView(list);
        }


        [HttpPost]
        public ActionResult Approve(int id)
        {
            var entity = db.BBSGuestBook.FirstOrDefault(b => b.Id == id);

            if (entity != null)
            {
                entity.G_approved = true;
                db.SaveChanges();
                return Content("ok");
            }

            return Content("no");
        }


        [HttpPost]
        public ActionResult Top(int id)
        {
            var entity = db.BBSGuestBook.FirstOrDefault(b => b.Id == id);

            if (entity != null)
            {
                entity.G_isTop = !entity.G_isTop;
                db.SaveChanges();

                if (entity.G_isTop)
                {
                   // var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(entity.MemberId);

                    //if (account != null && account.Result != null)
                    //{
                    //    int value;
                    //    _AppContext.BreadApp.BlueBeanBread(EBRuleType.论坛帖子置顶加精, entity.MemberId, (MemshipLevel)account.Result.MLevel, out value);
                    //    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.论坛帖子置顶与加精, entity.MemberId, out value);
                    //}
                }

                return Content("ok");
            }

            return Content("no");
        }

        [HttpPost]
        public ActionResult Hot(int id)
        {
            var entity = db.BBSGuestBook.FirstOrDefault(b => b.Id == id);

            if (entity != null)
            {
                entity.G_isHot = !entity.G_isHot;
                db.SaveChanges();

                if (entity.G_isHot)
                {
                    //var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(entity.MemberId);

                    //if (account != null && account.Result != null)
                    //{
                    //    int value;
                    //    _AppContext.BreadApp.BlueBeanBread(EBRuleType.论坛帖子置顶加精, entity.MemberId, (MemshipLevel)account.Result.MLevel, out value);
                    //    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.论坛帖子置顶与加精, entity.MemberId, out value);
                    //}
                }

                return Content("ok");
            }

            return Content("no");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var entity = db.BBSGuestBook.FirstOrDefault(b => b.Id == id);
                if (entity != null)
                {
                    db.BBSGuestBook.Remove(entity);
                    db.SaveChanges();
                    return Redirect("/bbsadmin/Index");
                }
            }
            catch (Exception ex)
            {
                return Content("<script type='text/javascript'>alert('删除失败'); location.href='/bbsadmin/Index';</script>");

            }

            return Content("<script type='text/javascript'>alert('删除失败'); location.href='/bbsadmin/Index';</script>");
        }


        public ActionResult Columns()
        {
            List<BBSColumns> col = (from m in db.BBSColumns select m).ToList();
            return View(col);
        }

        public ActionResult Add_Columns()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add_Columns(BBSColumns colu)
        {

            BBSColumns col = (from m in db.BBSColumns where m.Column_Name == colu.Column_Name select m).FirstOrDefault();
            if (col == null)
            {
                db.BBSColumns.Add(colu);
                db.SaveChanges();
                return RedirectToAction("Columns");
            }

            ModelState.AddModelError("Column_Name", "数据库已存在改栏目名！");
            return View();
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

            BBSColumns col = (from m in db.BBSColumns where m.Id == id select m).First();
            return View(col);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_Columns(int id, BBSColumns colu)
        {
            BBSColumns c = (from m in db.BBSColumns where m.Id == id select m).FirstOrDefault();


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

            int PageSize = 15;
            int PageIndex = page ?? 1;

            var list = db.BBSGuestBook.Where(e => e.G_Recycle == 1).OrderByDescending(e => e.Id);
            ViewData["PageIndex"] = PageIndex;
            ViewData["PageSize"] = PageSize;
            ViewData["ReCordCount"] = list.Count();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Admin_GBook(string KeyWord, int? page)
        {

            var list = db.BBSGuestBook.Where(e => e.G_Recycle == 1 && e.G_Title.IndexOf(KeyWord) > -1).OrderByDescending(e => e.Id);


            return View(list);
        }

        public ActionResult Move(int id)
        {
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
