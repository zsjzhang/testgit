using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.ManageWeb.Models;
using AspNet.Identity.SQL;
using System.IO;
using System.Text;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
   // [MvcAuthorize]
    public class CardController : Controller
    {
        //
        // GET: /Card/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Airport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddAirport(Airport entity)
        {
            var result = new ReturnResult { IsSuccess = false };

            result.IsSuccess = _AppContext.AirportServiceApp.AddAirport(entity);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAirport(Airport entity)
        {
            var result = new ReturnResult { IsSuccess = false };

            result.IsSuccess = _AppContext.AirportServiceApp.UpdateAirport(entity);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAirport(int id)
        {
            var result = new ReturnResult { IsSuccess = false };

            result.IsSuccess = _AppContext.AirportServiceApp.DeleteAirport(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SelectAirportRoomList(string province, string city, string airport, int pageindex, int pagesize)
        {
            int total = 0;

            var cardList = _AppContext.AirportServiceApp.SelectAirportRoomList(province, city, airport).ToList<Airport>();

            total = cardList.Count();

            var list = cardList.Skip(pagesize * (pageindex - 1)).Take(pagesize);

            var result = new { data = list, pos = pageindex, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SelectAirportList(string province, string city)
        {
            var list = _AppContext.AirportServiceApp.SelectAirportList(province, city);

            var result = list.ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CardList(string phoneNumber, int? state, string start, string end, int pageindex, int pagesize, string iscallcenter)
        {
            int total = 0;
            int noSend = 0;

            var cardList = _AppContext.AirportServiceApp.SelectSNCard(phoneNumber, state ?? 0, iscallcenter, start, end, pageindex, 10, out total).ToList().Skip(pagesize * (pageindex - 1)).Take(pagesize);

            _AppContext.AirportServiceApp.SelectSNCardCount(phoneNumber, state ?? 0, out noSend);

            var result = new { data = cardList, pos = pageindex, total_count = total, count = new { Total = total, NoSend = noSend } };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CardStateList()
        {

            _AppContext.AirportServiceApp.ReadCheckinData();

            var result = new ReturnResult() { IsSuccess = true };

            List<object> data = new List<object>();

            data.Add(new { Id = (int)ESNCardStatus.Created, Statu = EnumExtension.GetDiscribe<ESNCardStatus>(ESNCardStatus.Created) });
            data.Add(new { Id = (int)ESNCardStatus.Send, Statu = EnumExtension.GetDiscribe<ESNCardStatus>(ESNCardStatus.Send) });
            data.Add(new { Id = (int)ESNCardStatus.Used, Statu = EnumExtension.GetDiscribe<ESNCardStatus>(ESNCardStatus.Used) });


            //查询所有机场
            IEnumerable<Airport> air = _AppContext.AirportServiceApp.SelectAirportList();

            result.Data = new { status = data.ToArray(), airports = air.ToArray() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AirportProvinceList()
        {
            var list = _AppContext.AirportServiceApp.SelectAirportProvince();

            var result = list.ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AirportCityList(string province)
        {
            var list = _AppContext.AirportServiceApp.SelectCityByProvince(province);

            var result = list.ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AirportRoomList(string airportName)
        {
            var result = new ReturnResult() { IsSuccess = true };

            //查询所有机场
            IEnumerable<Airport> air = _AppContext.AirportServiceApp.SelectAirportRoomList(airportName);

            result.Data = air.ToArray();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Send(int id, string code, string phonenumber, string truephonenumber, int airportId)
        {
            var result = new ReturnResult() { IsSuccess = true };

            var frontUserStore = new FrontUserStore<FrontIdentityUser>();
            var membership = frontUserStore.FindByNameAsync(phonenumber);

            if (membership.Result == null)
            {
                result.IsSuccess = false;
                result.Message = "只能发放给系统注册用户，请确认手机号";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            //如果用户填写了其他手机号，则发送到该手机号上
            if (!string.IsNullOrEmpty(truephonenumber))
                phonenumber = truephonenumber;

            result = _AppContext.AirportServiceApp.SendCard(membership.Result.Id, id, code, phonenumber, (int)ESendType.System, airportId, "blms");

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendCardSMS(string code)
        {
            var result = new ReturnResult() { IsSuccess = true };

            result = _AppContext.AirportServiceApp.SendCardSMS(code);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ImportPhone(string path)
        {
            var result = new ReturnResult() { IsSuccess = true };
            StreamReader sr = new StreamReader(Server.MapPath("../" + path), Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                try
                {
                    int result1 = SetHash(line + ConfigurationManager.AppSettings["SendType"].ToString());

                    if (result1 >= 3)
                    {
                        continue;
                    }
                    string cardtype = ConfigurationManager.AppSettings["CardType" + result1];
                    var userId = "";
                    RedisExtend redis = null;
                    using (redis = new RedisExtend())
                    {
                        redis.Connect();
                        userId = redis.GetHashSet("Member", line);
                        if (string.IsNullOrWhiteSpace(userId))
                        {
                            var member = _AppContext.DealerMembershipApp.SelectMemberListByphoneNumber(line).ToList();

                            if (member != null && member.Count > 0)
                            {
                                userId = member.FirstOrDefault().Id;
                                redis.SetHashField("Member", line, userId);
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(userId))
                    {
                        var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(cardtype);//single table select
                        SendCustom(customCardInfo, RandomNumberHelper.GetUserCustomCardCode(), userId, line, "0000-1111-2222");
                    }
                }
                catch (Exception ex)
                {
                    LogService.Instance.Error(ex.Message + "\r\n" + ex.StackTrace);
                    result.IsSuccess = false;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void SendCustom(CustomCardInfo customCardInfo, string cardCode, string userId, string phone, string Vin)
        {
            ReturnResult res = new ReturnResult() { IsSuccess = true };

            var customcard = new CustomCard()
            {
                CardType = customCardInfo.CardType,
                CardCode = cardCode,
                CardId = customCardInfo.Id,
                CreateTime = DateTime.Now,
                IsSave = true,
                IsCancel = false,
                UserId = userId,
                IsReissue = false,
                Tel = phone,
                IsSend = true,
                OpenId = Vin,
                Source = "blms"
            };

            // 用户卡券信息入库
            res = _AppContext.CustomCardApp.AddCustomCard(customcard);
            if (res.IsSuccess)
            {
                using (RedisExtend redis = new RedisExtend())
                {
                    int i = 0;

                    redis.Connect();

                    string str = redis.GetHashSet("tj", phone + ConfigurationManager.AppSettings["SendType"].ToString());

                    int.TryParse(str,out i);

                    redis.SetHashField("tj", phone + ConfigurationManager.AppSettings["SendType"].ToString(), (i + 1)+"");
                }
                //发送卡券短信信息；
                _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo, new CustomCardSms() { CardCode = cardCode }, phone);

                //更新卡券信息库存；
                _AppContext.CustomCardInfoApp.UpdateCustomCardQuantityByType(customcard.CardType);
            }
        }

        public int SetHash(string phonenumber)
        {

            int i = 0;
            using (RedisExtend redis = new RedisExtend())
            {
                redis.Connect();

                string str = redis.GetHashSet("tj", phonenumber);

                int.TryParse(str, out i);

                //if (i < 1)//第一次+1
                //{
                //    redis.SetHashField("tj", phonenumber, "1");
                //}
                //else //N+1
                //{
                //    redis.SetHashField("tj", phonenumber, i + 1 + "");
                //}
            }
            return i;

        }

        [HttpPost]
        public JsonResult Import(string path)
        {
            var result = new ReturnResult() { IsSuccess = true };

            result = _AppContext.AirportServiceApp.ImportSNCard(path);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult Export(string phoneNumber, int? state, string isCallCenter)
        {
            int cou = 0;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            List<SNCard> cards = new List<SNCard>();

            //获取list数据
            if (string.IsNullOrEmpty(isCallCenter))
                cards = _AppContext.AirportServiceApp.SelectSNCardCount(phoneNumber, state ?? 0, out cou).ToList();
            else
                cards = _AppContext.AirportServiceApp.SelectSNCardCount(phoneNumber, state ?? 0, out cou).Where(x => x.IsCallCenter == isCallCenter).ToList();

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("编号");
            row1.CreateCell(1).SetCellValue("卡券号");

            row1.CreateCell(2).SetCellValue("用户姓名");
            row1.CreateCell(3).SetCellValue("身份证");
            row1.CreateCell(4).SetCellValue("预约机场");
            row1.CreateCell(5).SetCellValue("实际使用机场");

            row1.CreateCell(6).SetCellValue("状态");
            row1.CreateCell(7).SetCellValue("发放用户");
            row1.CreateCell(8).SetCellValue("发放时间");
            row1.CreateCell(9).SetCellValue("发放方式");
            row1.CreateCell(10).SetCellValue("使用时间");
            row1.CreateCell(11).SetCellValue("创建时间");
            row1.CreateCell(12).SetCellValue("来源");

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < cards.Count(); i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(cards[i].Id.ToString());
                rowtemp.CreateCell(1).SetCellValue(cards[i].SNCode.ToString());

                rowtemp.CreateCell(2).SetCellValue(cards[i].RealName == null ? "" : cards[i].RealName);
                rowtemp.CreateCell(3).SetCellValue(cards[i].IdentityNumber == null ? "" : cards[i].IdentityNumber);
                rowtemp.CreateCell(4).SetCellValue(cards[i].AirportName == null ? "" : cards[i].AirportName);
                rowtemp.CreateCell(5).SetCellValue(cards[i].UseAdd == null ? "" : cards[i].UseAdd);

                rowtemp.CreateCell(6).SetCellValue(cards[i].Status.ToString());
                rowtemp.CreateCell(7).SetCellValue(cards[i].PhoneNumber == null ? "" : cards[i].PhoneNumber);
                rowtemp.CreateCell(8).SetCellValue(cards[i].SendTime == null ? "" : cards[i].SendTime.ToString());
                rowtemp.CreateCell(9).SetCellValue(cards[i].SendTypeName);
                rowtemp.CreateCell(10).SetCellValue(cards[i].UseTime == null ? "" : cards[i].UseTime.ToString());
                rowtemp.CreateCell(11).SetCellValue(cards[i].CreateTime.ToString());
                rowtemp.CreateCell(12).SetCellValue(cards[i].DataSource);
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "Card.xls");
        }

        public JsonResult GetSN(string userId, string phoneNumber)
        {
            var result = new ReturnResult() { IsSuccess = true };

            //userId = "d574238f-c1d1-436a-86f1-669893ceb4c1";
            //phoneNumber = "13717869314";

            //result = _AppContext.AirportServiceApp.GetSNByJF(userId, phoneNumber);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void Template()
        {
            //StreamReader sr = new StreamReader(HttpContext.Server.MapPath("../Content/Template.txt"), System.Text.Encoding.GetEncoding(936));

            string path = HttpContext.Server.MapPath("../Content/File/Template.txt");

            Response.ContentType = "text/plain";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + path);//这个响应头实现下载保存

            //路径还有编码
            Response.Write(System.IO.File.ReadAllText(path, System.Text.Encoding.GetEncoding(936)));
        }
    }
}