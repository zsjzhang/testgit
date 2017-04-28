using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.Application;
using System.IO;
using System.Text;
using System.Transactions;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class DemoTestController : Controller
    {
        // GET: DemoTest
        public ActionResult Index()
        {
            //int tempData1 = _AppContext.UserIntegralApp.GetTotalIntegral("dd35cc3f-bd02-4f90-995c-6c938ae031dd");
            //_AppContext.TradePort.TradeService("dd35cc3f-bd02-4f90-995c-6c938ae031dd", 1800);
            //int tempData2 = _AppContext.UserIntegralApp.GetTotalIntegral("dd35cc3f-bd02-4f90-995c-6c938ae031dd");

            //int total;
            //_AppContext.ProductApp.GetProduct(new ProductSearchCondition() { CategoryID=2 }, new PageData() { Index = 1, Size = 10 }, out total);

            //int blueBeanValue;
            //bool result = _AppContext.BreadApp.BlueBeanBread(EBRuleType.登陆, "dd35cc3f-bd02-4f90-995c-6c938ae031dd", MemshipLevel.TwoStar, out blueBeanValue);


            // int total;
            //var tempData= _AppContext.ProductApp.GetProduct(EProductRecommend.XP,new PageData(){Index=0,Size=10},out total);

            int total;
            //bool result = _AppContext.BreadApp.EmpiricBread(EEmpiricRule.登陆, "dddddd", out total);


            return View();
        }
    }
}