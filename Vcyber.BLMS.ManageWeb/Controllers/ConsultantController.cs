using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.ManageWeb.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class ConsultantController : Controller
    {
        private BLMS_Entities_Connection entities = new BLMS_Entities_Connection();
        // GET: Consultant
        [MvcAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [MvcAuthorize]
        public ActionResult Add()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetDealerIdForClient()
        {
            string delaerId = string.Empty;
            var userId = this.User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            if (!string.IsNullOrEmpty(user.DealerId))
                delaerId = user.DealerId;
            return Json(new
            {
                DealerId = delaerId,
                DealerName = user.DealerName
            }, JsonRequestBehavior.AllowGet);
        }

        private ApplicationUserManager _userManager;
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

        [MvcAuthorize]
        public JsonResult AddConsultant(CS_Consultant con)
        {
            var userId = this.User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            if (string.IsNullOrEmpty(user.DealerId) && string.IsNullOrEmpty(con.DealerId))
                return Json(new
                {
                    success = false,
                    msg = "请选择特约店"
                });
            else
            {
                con.DealerId = user.DealerId;
                con.DealerName = user.DealerName;
            }
            entities.CS_Consultant.Add(con);
            return Json(new
            {
                success = entities.SaveChanges() > 0
            });

        }

        [MvcAuthorize]
        public JsonResult DeleteConsultant(int id)
        {
            return Json(new
            {
                success = Delete(id)
            });
        }

        private bool Update(CS_Consultant con)
        {
            var condb = (from c in entities.CS_Consultant
                         where c.Id == con.Id
                         select c).FirstOrDefault();
            if (condb != null)
            {
                condb.Name = con.Name;
                condb.DealerId = con.DealerId;
                condb.Photo = con.Photo;
                condb.Tel = con.Tel;
                condb.Sex = con.Sex;
                condb.Comment = con.Comment;
            }
            return entities.SaveChanges() > 0;
        }

        [MvcAuthorize]
        public JsonResult SelectById(int id)
        {
            var condb = (from c in entities.CS_Consultant
                         where c.Id == id
                         select c).FirstOrDefault();
            return Json(condb, JsonRequestBehavior.AllowGet);
        }

        [MvcAuthorize]
        public JsonResult Select(string name, string dealerId, int Skip, int Count)
        {
            List<CS_Consultant> condb = new List<CS_Consultant>();
            var userId = this.User.Identity.GetUserId();
            var user = UserManager.FindById(userId);

            if (!string.IsNullOrEmpty(user.DealerId))
            {

                condb = (from c in entities.CS_Consultant
                         select c).Where(c => c.DealerId == user.DealerId).OrderBy(e => e.DealerName).Skip(Skip).Take(Count).ToList();
            }
            else
            {
                if (string.IsNullOrEmpty(dealerId))
                {
                    condb = (from c in entities.CS_Consultant
                             select c).OrderBy(e => e.DealerName).Skip(Skip).Take(Count).ToList();
                }
                else
                {
                    condb = (from c in entities.CS_Consultant
                             select c).Where(c => c.DealerId == dealerId).OrderBy(e => e.DealerName).Skip(Skip).Take(Count).ToList();
                }
            }

            return Json(new
            {
                data = condb,
                DealerId = user.DealerId,
                success = true
            }, JsonRequestBehavior.AllowGet);
        }

        [MvcAuthorize]
        public JsonResult SelectName(string name)
        {
            var condb = (from c in entities.CS_Consultant
                         where c.Name == name
                         select c).ToList();
            return Json(condb, JsonRequestBehavior.AllowGet);
        }

        private bool Delete(int id)
        {
            var condb = (from c in entities.CS_Consultant
                         where c.Id == id
                         select c).FirstOrDefault();
            entities.CS_Consultant.Remove(condb);
            return entities.SaveChanges() > 0;
        }
    }
}