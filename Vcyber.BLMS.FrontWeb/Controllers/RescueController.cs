using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class RescueController : Controller
    {
        private string apidomain = ConfigurationManager.AppSettings.Get("apidomain");
        //
        // GET: /Rescue/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FreeRoadRescue(Customer customerEntity)
        {

            if (customerEntity == null || string.IsNullOrEmpty(customerEntity.UserMobile) || string.IsNullOrEmpty(customerEntity.Address))
            {
                return Json(new { code = 400, msg = string.Empty, content = string.Empty, IsSuccess = false });
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apidomain);

                var requestJson = JsonConvert.SerializeObject(
                    new
                    {
                        PhoneNumber = customerEntity.UserMobile,
                        Address = customerEntity.Address,
                        Position = customerEntity.Position
                    });

                HttpContent httpContent = new StringContent(requestJson);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = client.PostAsync("api/SonataService/FreeRoadRescue", httpContent).Result.Content.ReadAsStringAsync().Result;
                return Json(result);
            }

        }
	}

    public class Customer
    {
        public int Id { get; set; }
        public string UserMobile { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public DateTime CreateTime { get; set; }
    }
}