using System;
using System.Web.Http;
using System.Web.Mvc;
using Vcyber.BLMS.WebApi.Areas.HelpPage.ModelDescriptions;
using Vcyber.BLMS.WebApi.Areas.HelpPage.Models;

namespace Vcyber.BLMS.WebApi.Areas.HelpPage.Controllers
{
    using System.Configuration;

    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            if (ConfigurationManager.AppSettings["ShowHelp"] == "1")
            {
                ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
                return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
            }
            return null;
        }

        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
    }
}