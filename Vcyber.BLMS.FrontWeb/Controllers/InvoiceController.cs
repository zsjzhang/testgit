using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.FrontWeb.Models;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class InvoiceController : Controller
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

        public InvoiceController()
        {
        }

        public InvoiceController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion
        private string ErrorTips = "上传图片格式不对";
        private string ContentTypeTips = "图片最大不超过20M";
        private string ServiceTypeTips = "请选择服务类型";
        //
        // GET: /Invoice/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadInvoice()
        {
            return View();
        }

        public ActionResult DoUploadInvoice()
        {
            
            //图片的扩展名
            string _imageExtendName = string.Empty;
            //获取当前用户帐号
            string userId = User.Identity.GetUserId();
            //图片访问的域名
            string _imageDomain = System.Web.Configuration.WebConfigurationManager.AppSettings["ImgPath"];
            //图片的保存路径
            string _imagePath = HttpContext.Server.MapPath("/Upload");
            //图片的访问url
            string _imageUrl = string.Empty;
            string _serviceType = Request["ServiceType"];
            HttpFileCollectionBase _files = Request.Files;
            if (_files.Count > 0)
            {
               
                string _tmpFileName = _files[0].FileName;
                //校验后缀名
                if (!string.IsNullOrEmpty(_tmpFileName))
                {
                    _imageExtendName = _tmpFileName.Substring(_tmpFileName.LastIndexOf("."));
                }
                if (!".jpg".Equals(_imageExtendName) && !".png".Equals(_imageExtendName) && !".gif".Equals(_imageExtendName))
                {
                    return Json(new { code = 401, imgurl = _imageUrl, msg = ErrorTips }, "text/html", JsonRequestBehavior.AllowGet);
                }
                //校验图片大小
                int _fileLength = _files[0].ContentLength;
                int _maxFileLength = 20 * 1024 * 1024;
                if (_fileLength >= _maxFileLength)
                {
                    return Json(new { code = 401, imgurl = _imageUrl, msg = ContentTypeTips }, "text/html", JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(_serviceType))
                {
                    return Json(new { code = 401, imgurl = _imageUrl, msg = ServiceTypeTips }, "text/html", JsonRequestBehavior.AllowGet);
                }
                //图片的名称
                string _imageName = DateTime.Now.Ticks.ToString() + _imageExtendName;
                //图片的全名称
                string _imageFullName = System.IO.Path.Combine(_imagePath, _imageName);
                _files[0].SaveAs(_imageFullName);
                _imageUrl = string.Format("{0}/{1}/{2}", _imageDomain, "Upload", _imageName);


                InvoiceForReserve _entity = new InvoiceForReserve();
                _entity.ImageUrl = _imageUrl;
                _entity.MembershipId = userId;
                _entity.CreateTime = DateTime.Now;
                _entity.ServiceType = Request["ServiceType"];
                int _result = _AppContext.InvoiceForReserveApp.Insert(_entity);
                return Json(new { code = 200, imgurl = _imageUrl }, "text/html", JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 400, imgurl = _imageUrl }, "text/html", JsonRequestBehavior.AllowGet);
        }
    }
}