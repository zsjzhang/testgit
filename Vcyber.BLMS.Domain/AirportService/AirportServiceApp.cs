using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using AspNet.Identity.SQL;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Common.Web;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class AirportServiceApp : IAirportServiceApp
    {
        /// <summary>
        /// 从机场获取核销数据
        /// </summary>
        /// <returns>返回处理结果</returns>
        public ReturnResult ReadCheckinData()
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            if (ConfigurationManager.AppSettings["AIR_ReadCheckinDataUrl"] == null || ConfigurationManager.AppSettings["AIR_Partner"] == null
                || ConfigurationManager.AppSettings["AIR_Sku"] == null || ConfigurationManager.AppSettings["AIR_Key"] == null)
            {
                result.IsSuccess = false;
                result.Message = "配置信息未设置，请确认配置文件";
                return result;
            }

            //取得服务URL和相关参数
            string AIR_ReadCheckinDataUrl = ConfigurationManager.AppSettings["AIR_ReadCheckinDataUrl"];
            string AIR_Partner = ConfigurationManager.AppSettings["AIR_Partner"];
            string AIR_Sku = ConfigurationManager.AppSettings["AIR_Sku"];
            string AIR_Key = ConfigurationManager.AppSettings["AIR_Key"];

            //生成随机参数
            string random = DateTime.Now.ToString("yyyyMMddHHmmss");

            //参数
            string sign_string = "random=" + random + "&partner=" + AIR_Partner + "&sku=" + AIR_Sku;

            MD5 md5 = MD5.Create();
            byte[] md5a = md5.ComputeHash(Encoding.UTF8.GetBytes(sign_string + AIR_Key));
            string md5b = BitConverter.ToString(md5a);
            string sign = md5b.Replace("-", "");

            AIR_ReadCheckinDataUrl += sign_string + "&sign=" + sign;

            //调用服务
            var data = Vcyber.BLMS.Common.Web.WebUtils.JsonToObj<ReadCheckinData>(Vcyber.BLMS.Common.Web.WebUtils.GET_WebRequestHTML("utf-8", AIR_ReadCheckinDataUrl, null), null);

            //处理数据
            if (data.result == -1)
            {
                result.IsSuccess = false;
                result.Message = data.description;
                return result;
            }

            foreach (var item in data.data.data)
            {
                string batchNo = data.data.BatchNo;

                //保存使用记录
                SaveSNUsedRecord(item, batchNo);
            }

            return result;
        }

        /// <summary>
        /// 根据批次确认核销数据
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回处理结果</returns>
        public ReturnResult ConfirmData(string batchNo)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            if (ConfigurationManager.AppSettings["AIR_ConfirmDataUrl"] == null || ConfigurationManager.AppSettings["AIR_Partner"] == null
                || ConfigurationManager.AppSettings["AIR_Key"] == null)
            {
                result.IsSuccess = false;
                result.Message = "配置信息未设置，请确认配置文件";
                return result;
            }

            //取得服务URL和相关参数
            string AIR_ConfirmDataUrl = ConfigurationManager.AppSettings["AIR_ConfirmDataUrl"];
            string AIR_Partner = ConfigurationManager.AppSettings["AIR_Partner"];
            string AIR_Key = ConfigurationManager.AppSettings["AIR_Key"];

            string sign_string = "batchno=" + batchNo + "&partner=" + AIR_Partner;

            MD5 md5 = MD5.Create();
            byte[] md5a = md5.ComputeHash(Encoding.UTF8.GetBytes(sign_string + AIR_Key));
            string md5b = BitConverter.ToString(md5a);
            string sign = md5b.Replace("-", "");

            AIR_ConfirmDataUrl += sign_string + "&sign=" + sign;

            //调用服务
            var data = Vcyber.BLMS.Common.Web.WebUtils.JsonToObj<ConfirmData>(Vcyber.BLMS.Common.Web.WebUtils.GET_WebRequestHTML("utf-8", AIR_ConfirmDataUrl, null), null);

            //数据集
            result.Data = data;

            return result;
        }

        /// <summary>
        /// 用户领取免费机场服务码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="phoneNumber">领取的手机号</param>
        /// <param name="number">领取的张数</param>
        /// <param name="sendType">下发方式</param>
        /// <param name="airportId">机场编号</param>
        /// <returns>返回处理结果</returns>
        public ReturnResult GetFreeServiceCard(string userId, string phoneNumber, int number, int sendType, int airportId)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            //1.查询用户已经领取的免费张数(不包括系统下发和积分兑换的)
            var cardList = _DbSession.SNCardStorager.SelectSNCardByUser(userId).Where(x => x.SendType != (int)ESendType.System && x.SendType != (int)ESendType.Trade);

            //是否超过2张限额
            bool isPass = cardList == null ? number > 2 : cardList.Count() + number > 2;

            //积分兑换和系统不算次数
            if (sendType == 3 || sendType == 4)
            {
                isPass = false;
            }

            //本次要获取的串码数据
            IEnumerable<SNCard> cards = null;

            if (isPass)
            {
                result.IsSuccess = false;
                result.Message = "预约失败，免费次数已经用完或请求的总次数超过两次";
                return result;
            }
            else
            {
                cards = _DbSession.SNCardStorager.GetSNCard(number);
            }

            //无法取到服务码或不够数量
            if (cards == null || cards.Count() < number)
            {
                result.IsSuccess = false;
                result.Message = "预约失败，服务码已经无法兑换，库存不足";
                return result;
            }

            //返回服务码数据
            result.Data = cards;

            return ServiceCardSendAndSms(userId, phoneNumber, sendType, airportId, result, cards);
        }


        //服务码下发及发送信息
        private static ReturnResult ServiceCardSendAndSms(string userId, string phoneNumber, int sendType, int airportId, ReturnResult result, IEnumerable<SNCard> cards)
        {
            foreach (var card in cards)
            {
                //下发服务码
                bool isOk = _DbSession.SNCardStorager.SendSNCard(userId, card.Id, sendType, airportId, phoneNumber, "blms");

                if (!isOk)
                {
                    result.IsSuccess = false;
                    result.Message = "下发服务码失败";
                    return result;
                }

                //查询机场信息
                Airport airport = _DbSession.SNCardStorager.SelectAirport(airportId);

                //构建短信内容
                string smsString = string.Empty;
                if (airport == null)
                {
                    smsString = string.Format(ConfigurationManager.AppSettings["AIR_CARDSENDSMS_NULL"], card.SNCode, card.SNCode, string.Format("{0:D}", DateTime.Now.AddDays(90)));
                }
                else
                {
                    smsString = string.Format(ConfigurationManager.AppSettings["AIR_CARDSENDSMS"], card.SNCode, airport.AirportAddress, card.SNCode, string.Format("{0:D}", DateTime.Now.AddDays(90)));
                }

               


                //4.发送串码短信
                SmsApp sms = new SmsApp();
                var sendResult = sms.SendSMS(phoneNumber, smsString);

                //短信发送失败
                if (!sendResult.IsSuccess)
                {
                    result.IsSuccess = false;
                    result.Message = "短信发送失败";
                    return result;
                }
            }

            return result;
        }


        /// <summary>
        /// 积分兑换机场服务码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="phoneNumber">领取的手机号</param>
        /// <param name="number">领取的张数</param>
        /// <param name="sendType">下发方式</param>
        /// <param name="airportId">机场编号</param>
        /// <returns></returns>
        public ReturnResult TradeServiceCardByIntegral(string userId, string phoneNumber, int number, int airportId)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            //兑换服务
            TradePort port = new TradePort();

            //默认值
            int serviceCardIntegral = 1800;

            //获取兑换所需积分值
            if (ConfigurationManager.AppSettings["ServiceCardIntegral"] != null && ConfigurationManager.AppSettings["ServiceCardIntegral"] != "")
            {
                int.TryParse(ConfigurationManager.AppSettings["ServiceCardIntegral"], out serviceCardIntegral);
            }

            //积分兑换
            bool isSuccess = port.TradeService(userId, serviceCardIntegral * number);

            //兑换失败
            if (!isSuccess)
            {
                result.IsSuccess = false;
                result.Message = "兑换服务失败";
                return result;
            }

            //本次要获取的串码数据
            var cards = _DbSession.SNCardStorager.GetSNCard(number);

            //无法取到服务码或不够数量
            if (cards == null || cards.Count() < number)
            {
                result.IsSuccess = false;
                result.Message = "预约失败，服务码已经无法兑换，库存不足";
                return result;
            }

            //返回服务码数据
            result.Data = cards;

            return ServiceCardSendAndSms(userId, phoneNumber, (int)ESendType.Trade, airportId, result, cards);
        }

        /// <summary>
        /// 领取机场服务码(优先领取免费次数，不够用积分兑换)
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="phoneNumber">领取的手机号</param>
        /// <param name="number">领取的张数</param>
        /// <param name="sendType">下发方式</param>
        /// <param name="airportId">机场编号</param>
        /// <returns>返回处理结果</returns>
        public ReturnResult GetServiceCardAuto(string userId, string phoneNumber, int number, int sendType, int airportId)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            //查询用户已经领取的免费张数(不包括系统下发和积分兑换的)
            var cardList = _DbSession.SNCardStorager.SelectSNCardByUser(userId).Where(x => x.SendType != (int)ESendType.System && x.SendType != (int)ESendType.Trade);

            //本次需要积分兑换的张数
            int editNumber = 0;

            //是否超过2张限额
            bool isPass = false;

            if (cardList == null)
            {
                isPass = number > 2;
                editNumber = number - 2;
            }
            else
            {
                isPass = cardList.Count() + number > 2;
                editNumber = cardList.Count() + number - 2;
            }

            //本次要获取的串码数据
            IEnumerable<SNCard> cards = null;

            //如果超过2张的情况，用积分兑换服务（180元等值积分兑换一次）
            if (isPass)
            {
                //兑换服务
                TradePort port = new TradePort();

                //兑换默认值
                int serviceCardIntegral = 1800;

                //获取兑换所需积分值
                if (ConfigurationManager.AppSettings["ServiceCardIntegral"] != null && ConfigurationManager.AppSettings["ServiceCardIntegral"] != "")
                {
                    int.TryParse(ConfigurationManager.AppSettings["ServiceCardIntegral"], out serviceCardIntegral);
                }

                //积分兑换
                bool isSuccess = port.TradeService(userId, serviceCardIntegral * editNumber);

                //兑换失败
                if (!isSuccess)
                {
                    result.IsSuccess = false;
                    result.Message = "兑换服务失败";
                    return result;
                }
            }

            cards = _DbSession.SNCardStorager.GetSNCard(number);

            //无法取到服务码或不够数量
            if (cards == null || cards.Count() < number)
            {
                result.IsSuccess = false;
                result.Message = "预约失败，服务码已经无法兑换，库存不足";
                return result;
            }

            //返回服务码数据
            result.Data = cards;

            foreach (var card in cards)
            {
                //下发服务码
                bool isOk;

                if (editNumber == 0)
                    isOk = _DbSession.SNCardStorager.SendSNCard(userId, card.Id, sendType, airportId, phoneNumber, "blms");
                else
                    isOk = _DbSession.SNCardStorager.SendSNCard(userId, card.Id, (int)ESendType.Trade, airportId, phoneNumber, "blms");

                if (!isOk)
                {
                    result.IsSuccess = false;
                    result.Message = "下发服务码失败";
                    return result;
                }

                //查询机场信息
                Airport airport = _DbSession.SNCardStorager.SelectAirport(airportId);

                //构建短信内容
                string smsString = string.Empty;
                if (airport == null)
                {
                    smsString = string.Format(ConfigurationManager.AppSettings["AIR_CARDSENDSMS_NULL"], card.SNCode, card.SNCode, string.Format("{0:D}", DateTime.Now.AddDays(90)));
                }
                else
                {
                    smsString = string.Format(ConfigurationManager.AppSettings["AIR_CARDSENDSMS"], card.SNCode, airport.AirportAddress, card.SNCode, string.Format("{0:D}", DateTime.Now.AddDays(90)));
                }


                //4.发送串码短信
                SmsApp sms = new SmsApp();
                var sendResult = sms.SendSMS(phoneNumber, smsString);

                //短信发送失败
                if (!sendResult.IsSuccess)
                {
                    result.IsSuccess = false;
                    result.Message = "短信发送失败";
                    return result;
                }

                editNumber--;
            }

            return result;
        }


        /// <summary>
        /// 保存服务码使用记录
        /// </summary>
        /// <param name="record">机场核销数据</param>
        /// <param name="batchNo">核销批次号</param>
        /// <returns>返回处理结果</returns>
        private ReturnResult SaveSNUsedRecord(ReadCheckinDataItemDetail record, string batchNo)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            //查询服务码信息
            var card = _DbSession.SNCardStorager.SelectSNCardByCode(record.code);

            //已使用的服务码直接跳过
            if (card == null || card.Status == (int)ESNCardStatus.Used)
                return result;

            //1.保存使用记录数据
            bool isOk = _DbSession.SNCardStorager.AddUsedRecord(record, batchNo);

            //保存失败
            if (!isOk)
            {
                result.IsSuccess = false;
                result.Message = "保存使用记录失败";
                return result;
            }

            //2.更新服务码状态等
            bool isSuccess = _DbSession.SNCardStorager.UseSNCard(record);

            //更新状态失败
            if (!isSuccess)
            {
                result.IsSuccess = false;
                result.Message = "更新服务码状态数据失败";
                return result;
            }

            //查询领取手机号
            var phoneNumber = card.PhoneNumber;

            string smsString = string.Empty;
            if (ConfigurationManager.AppSettings["AIR_CARDUSEDSMS"] == null || ConfigurationManager.AppSettings["AIR_CARDUSEDSMS"] == "")
            {
                smsString = "您于{0}，在{1}机场成功使用1次机场候机尊享服务，服务码：{2}（串码）。祝您旅途愉快！";
            }
            else
                smsString = ConfigurationManager.AppSettings["AIR_CARDUSEDSMS"];

            //3.发送消费短信
            SmsApp sms = new SmsApp();
            var sendResult = sms.SendSMS(phoneNumber, string.Format(smsString, record.checkintime, record.info, record.code));

            if (!sendResult.IsSuccess)
            {
                result.IsSuccess = false;
                result.Message = "短信发送失败";
                return result;
            }

            return result;
        }


        /// <summary>
        /// 预约机场服务
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="phoneNumber">验证码接收手机号</param>
        /// <param name="num1">免费次数</param>
        /// <param name="num2">兑换次数</param>
        /// <param name="airportId">机场编号</param>
        /// <returns></returns>
        public ReturnResult GetServiceCard(string userId, string phoneNumber, int num1, int num2, int airportId, string isCallCenter, string dataSource)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            //1.查询用户已经领取的张数(不包括系统发放和积分兑换的)
            var cardList = _DbSession.SNCardStorager.SelectSNCardByUser(userId).Where(x => x.SendType != (int)ESendType.System && x.SendType != (int)ESendType.Trade);

            if (num1 > 0)
            {
                //是否超过2张限额
                bool isPass = false;
                var user = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId).Result;
                var mlevel = user.MLevel;
                var freeNum = 0;
                if (mlevel == 11)
                {
                    freeNum = 2;
                }
                else if (mlevel == 12)
                {
                    freeNum = 3;
                }
                //安全性验证
                if (cardList == null)
                    isPass = num1 > freeNum;
                else
                    isPass = cardList.Count() + num1 > freeNum;

                //2.如果超过2张的情况
                if (isPass)
                {
                    result.IsSuccess = false;
                    result.Message = string.Format("预约的免费次数超出{0}次", freeNum);
                    SmsApp sms = new SmsApp();
                    sms.SendSMS(phoneNumber, "您的免费候机服务已用完。如需继续使用，可通过bluemembers网站、微信或下载官方App使用积分兑换（1800积分/1人次）。回复数字“1”了解下载地址。");
                    return result;
                }
            }

            //本次要获取的串码数据
            IEnumerable<SNCard> cards = null;
            //兑换默认值
            int serviceCardIntegral = 1800;

            //获取兑换所需积分值
            if (ConfigurationManager.AppSettings["ServiceCardIntegral"] != null && ConfigurationManager.AppSettings["ServiceCardIntegral"] != "")
            {
                int.TryParse(ConfigurationManager.AppSettings["ServiceCardIntegral"], out serviceCardIntegral);
            }
            //积分兑换服务
            if (num2 > 0)
            {
                TradePort port = new TradePort();
                bool isSuccess = port.TradeService(userId, serviceCardIntegral * num2);
                //向积分记录表中添加积分记录
                if (isSuccess && dataSource == "blms_wechat")
                {
                    _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                    {
                        userId = userId,
                        integralSource = "60",
                        value = serviceCardIntegral * num2 * -1,
                        datastate = 0,
                        remark = "微信预约机场服务",
                        CreateTime = DateTime.Now
                    });
                }
                else if (isSuccess && dataSource == "blms")
                {
                    _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral()
                    {
                        userId = userId,
                        integralSource = "41",
                        value = serviceCardIntegral * num2 * (-1),
                        datastate = 0,
                        remark = "前台预约候机尊享服务扣积分",
                        CreateTime = DateTime.Now,

                    });
                }
                else if (isSuccess && dataSource == "blms_wap")
                {
                    _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral()
                    {
                        userId = userId,
                        integralSource = "42",
                        value = serviceCardIntegral * num2 * (-1),
                        datastate = 0,
                        remark = "wap预约候机尊享服务扣积分",
                        CreateTime = DateTime.Now,

                    });
                }
                if (!isSuccess)
                {
                    result.IsSuccess = false;
                    result.Message = "兑换服务失败";
                    return result;
                }
            }

            cards = _DbSession.SNCardStorager.GetSNCard(num1 + num2);

            //无法取到服务码或不够数量
            if (cards == null || cards.Count() < (num1 + num2))
            {
                result.IsSuccess = false;
                result.Message = "预约失败，服务码已经无法兑换，库存不足";
                return result;
            }

            foreach (var card in cards)
            {
                //下发服务码
                bool isOk;

                if (num1 > 0)
                    isOk = _DbSession.SNCardStorager.SendSNCard(userId, card.Id, (int)ESendType.WebSite, airportId, phoneNumber, dataSource);
                else
                    isOk = _DbSession.SNCardStorager.SendSNCard(userId, card.Id, (int)ESendType.Trade, airportId, phoneNumber, dataSource);

                if (!isOk)
                {
                    result.IsSuccess = false;
                    result.Message = "下发服务码失败";
                    return result;
                }

                //查询机场信息
                Airport airport = _DbSession.SNCardStorager.SelectAirport(airportId);

                //构建短信内容
                string smsString = string.Empty;
                if (airport == null)
                {
                    smsString = string.Format(ConfigurationManager.AppSettings["AIR_CARDSENDSMS_NULL"], card.SNCode, card.SNCode, string.Format("{0:D}", DateTime.Now.AddDays(90)));
                }
                else
                {
                    smsString = string.Format(ConfigurationManager.AppSettings["AIR_CARDSENDSMS"], card.SNCode, airport.AirportAddress, card.SNCode, string.Format("{0:D}", DateTime.Now.AddDays(90)));
                }

                //发送串码短信
                SmsApp sms = new SmsApp();
                var sendResult = sms.SendSMS(phoneNumber, smsString);
               
                //短信发送失败
                if (!sendResult.IsSuccess)
                {
                    result.IsSuccess = false;
                    result.Message = "短信发送失败";
                    return result;
                }

                num1--;
            }
            if (result.IsSuccess)
            {
                _AppContext.UserMessageRecordApp.Insert(new UserMessageRecord()
                {
                    UserId = userId,
                    MsgType = MessageType.IntegralConsum,
                    MsgContent = string.Format(@" 您好，您于{0}使用{1}积分兑换机场服务，请您至机场出示服务码享受服务，祝您旅途愉快。", DateTime.Now, num2 * 1800)
                });
                
            }
            result.Data = cards;

            return result;
        }


        /// <summary>
        /// 分页查询服务码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<SNCard> SelectSNCard(string phoneNumber, int status, string iscallcenter, string start, string end, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.SNCardStorager.SelectSNCard(phoneNumber, status, iscallcenter, start, end, pageIndex, pageSize, out totalCount);
        }

        /// <summary>
        /// 卡券统计
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="status"></param>
        /// <param name="noSend"></param>
        public IEnumerable<SNCard> SelectSNCardCount(string phoneNumber, int status, out int noSend)
        {
            return _DbSession.SNCardStorager.SelectSNCardCount(phoneNumber, status, out noSend);
        }

        /// <summary>
        /// 获取用户持有的服务编码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>结果集</returns>
        public IEnumerable<SNCard> SelectSNCardByUser(string userId)
        {
            return _DbSession.SNCardStorager.SelectSNCardByUser(userId);
        }

        /// <summary>
        /// 获取单个候机服务券详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SNCard GetCardByUserDetails(string userid, string id)
        {
            return _DbSession.SNCardStorager.GetCardByUserDetails(userid,id);
        }

        /// <summary>
        /// 获取用户可预约的机场服务次数
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>返回可预约的次数</returns>
        public int GetSNCardNumber(string userId)
        {
            //查询用户已经领取的免费张数(不包括系统下发和积分兑换的)
            var cards = _DbSession.SNCardStorager.SelectSNCardByUser(userId).Where(x => x.SendType != (int)ESendType.System && x.SendType != (int)ESendType.Trade);
          
            //扣减
           // int number = 2 - (cards == null ? 0 : cards.Count());

            return cards.Count();
        }

        /// <summary>
        /// 导入卡券
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public ReturnResult ImportSNCard(string path)
        {
            var result = new ReturnResult { IsSuccess = true };

            if (string.IsNullOrEmpty(path))
            {
                result.IsSuccess = false;
                result.Message = "导入出错,请选择正确的数据文件";
                return result;
            }

            StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("../" + path), Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                SNCard card = new SNCard
                {
                    SNCode = line,
                    Status = (int)ESNCardStatus.Created,
                    CreateTime = DateTime.Now
                };

                bool isSuccess = _DbSession.SNCardStorager.AddSNCard(card);

                if (!isSuccess)
                {
                    result.IsSuccess = false;
                    result.Message = "导入卡券出错，请检查文件";
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// 发放卡券
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="phonenumber"></param>
        /// <param name="code">
        /// <param name="sendType"></param>
        /// <returns></returns>
        public ReturnResult SendCard(string userid, int id, string code, string phonenumber, int sendType, int airportId, string dataSource)
        {
            var result = new ReturnResult { IsSuccess = true };

            //下发服务码
            bool isOk = _DbSession.SNCardStorager.SendSNCard(userid, id, sendType, airportId, phonenumber, dataSource);

            if (!isOk)
            {
                result.IsSuccess = false;
                result.Message = "下发服务码失败";
                return result;
            }

            //查询机场信息
            Airport airport = _DbSession.SNCardStorager.SelectAirport(airportId);

            string smsString = string.Empty;
            if (airport == null)
            {
                smsString = string.Format(ConfigurationManager.AppSettings["AIR_CARDSENDSMS_NULL"], code, code, string.Format("{0:D}", DateTime.Now.AddDays(90)));
            }
            else
            {
                smsString = string.Format(ConfigurationManager.AppSettings["AIR_CARDSENDSMS"], code, airport.AirportAddress, code, string.Format("{0:D}", DateTime.Now.AddDays(90)));
            }



            //发送串码短信
            SmsApp sms = new SmsApp();
            var sendResult = sms.SendSMS(phonenumber, smsString);

            //短信发送失败
            if (!sendResult.IsSuccess)
            {
                result.IsSuccess = false;
                result.Message = "短信发送失败";
                return result;
            }

            return result;
        }

        /// <summary>
        /// 重新发送服务码短信
        /// </summary>
        /// <param name="code">服务码编号</param>
        /// <returns>发送结果</returns>
        public ReturnResult SendCardSMS(string code)
        {
            var result = new ReturnResult { IsSuccess = true };

            //下发服务码
            var card = _DbSession.SNCardStorager.SelectSNCardByCode(code);

            //查询机场信息
            Airport airport = _DbSession.SNCardStorager.SelectAirport(card.AirportId);
            string smsString = string.Empty;
            if (airport == null)
            {
                smsString = string.Format(ConfigurationManager.AppSettings["AIR_CARDSENDSMS_NULL"], code, code, string.Format("{0:D}", DateTime.Now.AddDays(90)));
            }
            else
            {
                //smsString = string.Format(ConfigurationManager.AppSettings["AIR_CARDSENDSMS"], airport.AirportName, code, airport.AirportAddress, code, DateTime.Now.AddDays(8).ToString("yyyy年MM月dd日"));
                smsString = string.Format(ConfigurationManager.AppSettings["AIR_CARDSENDSMS"], code, airport.AirportAddress, code, string.Format("{0:D}", DateTime.Now.AddDays(90)));
              //var sendResult = sms.SendSMS(Entity.Enum.ESmsType.机场服务_预约成功下发短信, card.RecivePhoneNumber, new string[] { code, airport.AirportAddress, code, string.Format("{0:D}", DateTime.Now.AddDays(90)) });
            }


            //发送串码短信
            SmsApp sms = new SmsApp();
            var sendResult = sms.SendSMS(card.RecivePhoneNumber, smsString);

            //短信发送失败
            if (!sendResult.IsSuccess)
            {
                result.IsSuccess = false;
                result.Message = "短信发送失败";
                return result;
            }

            return result;
        }

        /// <summary>
        /// 查询服务券详细信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public SNCard SelectCard(string code)
        {
            return _DbSession.SNCardStorager.SelectSNCardByCode(code);
        }

        /// <summary>
        /// 查询服务券的使用信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public SNUsedRecord SelectCardUsedRecord(string code)
        {
            return _DbSession.SNCardStorager.SelectCardUsedRecord(code);
        }

        /// <summary>
        /// 返回所有机场所在省
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> SelectAirportProvince()
        {
            return _DbSession.SNCardStorager.SelectProvince();
        }

        public IEnumerable<string> SelectCityByProvince(string province)
        {
            return _DbSession.SNCardStorager.SelectCityByProvince(province);
        }

        /// <summary>
        /// 查询所有机场列表
        /// </summary>
        /// <returns>机场列表</returns>
        public IEnumerable<Airport> SelectAirportList()
        {
            return _DbSession.SNCardStorager.SelectAirportList();
        }

        /// <summary>
        /// 根据省、市查询机场
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="city">市</param>
        /// <returns></returns>
        public IEnumerable<Airport> SelectAirportList(string province, string city)
        {
            return _DbSession.SNCardStorager.SelectAirportList(province, city);
        }

        /// <summary>
        /// 根据机场名称查询机场候机室列表
        /// </summary>
        /// <param name="airportName">机场名称</param>
        /// <returns>机场候机室列表</returns>
        public IEnumerable<Airport> SelectAirportRoomList(string airportName)
        {
            return _DbSession.SNCardStorager.SelectAirportRoomList(airportName);
        }

        /// <summary>
        /// 获取所有机场候车室
        /// </summary>
        /// <param name="airportName"></param>
        /// <returns></returns>
        public IEnumerable<Airport> AllAirportRoomList()
        {
            return _DbSession.SNCardStorager.AllAirportRoomList();
        }

        public IEnumerable<Airport> SelectAirportRoomList(string province, string city, string airport)
        {
            return _DbSession.SNCardStorager.SelectAirportRoomList(province, city, airport);
        }


        /// <summary>
        /// 用户轮询
        /// </summary>
        /// <param name="identityNumber">身份证号</param>
        /// <returns></returns>
        public bool MembershipMonitor(string identityNumber)
        {
            return _DbSession.SNCardStorager.MembershipMonitor(identityNumber);
        }

        /// <summary>
        /// 用户轮询积分
        /// </summary>
        /// <param name="identityNumber">身份证号</param>
        /// <returns></returns>
        public bool MembershipMonitorIntegral(string identityNumber)
        {
            return _DbSession.SNCardStorager.MembershipMonitorIntegral(identityNumber);
        }

        /// <summary>
        /// 预约候机
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public IEnumerable<SNCard> AirportServiceList(string phone)
        {
            return _DbSession.SNCardStorager.AirportServiceList(phone);
        }

        /// <summary>
        /// 活动抽奖，提供服务码
        /// </summary>
        /// <param name="phoneNumber">用户手机号</param>
        /// <returns>服务码</returns>
        public string SendCardByActivity(string phoneNumber, string userId)
        {
            SNCard card = _DbSession.SNCardStorager.GetSNCard(1).FirstOrDefault();
            string cardCode = string.Empty;
            if(card!=null)
            { 
                //下发服务码
                bool isOk = _DbSession.SNCardStorager.SendSNCard(userId, card.Id, 4, 0, phoneNumber, "blms_wechat");
                cardCode = card.SNCode;
            }

            return cardCode;
        }

        public bool AddAirport(Airport airport)
        {
            return _DbSession.SNCardStorager.AddAirport(airport);
        }

        public bool UpdateAirport(Airport airport)
        {
            return _DbSession.SNCardStorager.UpdateAirport(airport);
        }

        public bool DeleteAirport(int Id)
        {
            return _DbSession.SNCardStorager.DeleteAirport(Id);
        }
    }
}
