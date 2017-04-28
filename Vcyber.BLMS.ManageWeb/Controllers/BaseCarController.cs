using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PetaPoco;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Domain.Common;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class BaseCarController : Controller
    {
        //
        // GET: /BaseCar/
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(int id)
        {
            CSBaseCar model = _AppContext.BaseCarApp.GetById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CSBaseCar model)
        {
            model.UpdateId = HttpContext.User.Identity.GetUserId();
            model.UpdateTime = DateTime.Now;
            model.IsDeleted = false;
            int result = _AppContext.BaseCarApp.Update(model);
            if (result != -1)
            {
                return Content("Y");
            }
            return View(model);
        }
        public ActionResult List(string sType)
        {
            if (string.IsNullOrEmpty(sType)) sType = "0";
            IEnumerable<CSBaseCar> result = _AppContext.BaseCarApp.QueryCars((ECarSeriesType)(int.Parse(sType)));
            ViewBag.Type = sType;
            return View(result);
        }

        [HttpPost]
        public ActionResult Create(string sName, string sType)
        {
            CSBaseCar entity = new CSBaseCar();
            entity.SeriesId = int.Parse(IdGenerator.GetId(SequenceCategory.CX));
            entity.SeriesName = sName;
            entity.Type = int.Parse(sType);
            entity.UpdateId = HttpContext.User.Identity.GetUserId();
            entity.UpdateTime = DateTime.Now;
            entity.IsDeleted = false;
            int result = _AppContext.BaseCarApp.Add(entity);
            if (result > 0)
            {
                return Content("Y");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Del(int id)
        {
            var result = _AppContext.BaseCarApp.Delete(id);
            if (result != -1)
            {
                return Content("Y");
            }
            return Content("N");
        }
    }
}