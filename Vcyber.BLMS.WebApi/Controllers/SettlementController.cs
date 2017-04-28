using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNet.Identity.SQL;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.WebApi.Filter;
using Vcyber.BLMS.WebApi.Models;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;

namespace Vcyber.BLMS.WebApi.Controllers
{
    //日志文件
    [WebApiExceptionFilter]
    /// <summary>
    /// 结算
    /// </summary>
    public class SettlementController : ApiController
    {
        /// <summary>
        /// 积分结算列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(SettlementList))]
        [Route("api/Settlement/SettlementList")]
        [IOVAuthorize]
        public IHttpActionResult SettlementList(string createtime = "")
        {
            var result = new ReturnObject("200",null);
            try
            {
                IEnumerable<SettlementList> query = _AppContext.SettlementApp.GetXDSettlementList(createtime);
                result.Content = query;
                //result.Code = "200";
                //result.Message = "操作成功";
            }
            catch (Exception ex)
            {
                result.Code = "500";
                result.Message = ex.Message;
            }
            return this.Ok(result);
        }

        /// <summary>
        /// 积分结算状态
        /// </summary>
        /// <param name="settlementstate"></param>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Settlement/SettlementStatus")]
        [IOVAuthorize]
        public IHttpActionResult SettlementStatus(string settlementstate, string id)
        {
            var result = new ReturnObject("200", null);
            try
            {
                //IEnumerable<SettlementList> query = _AppContext.SettlementApp.GetXDSettlementStatus(settlementstate,dealerid);
                //var query =
                    _AppContext.SettlementApp.UpdateXDSettlementStatus(settlementstate,id);
                //IEnumerable<SettlementList> query = _AppContext.UserMessageRecordApp.GetUserMessageRecords(dealerid);
                //result.Content = query;
            }
            catch (Exception ex)
            {
                result.Code = "500";
                result.Message = ex.Message;
            }
            return this.Ok(result);
        }

        #region 积分结算过程中，如果DMS和BM的结算总值不一样，则DMS把自己的List清单发送过来
        /// <summary>
        /// 积分结算过程中，DMS的积分清单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Settlement/GetDMSIntegralList")]
        public IHttpActionResult GetDMSIntegralList(DMSIntergralListV data)
        {
            
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            var jsonData = Serializer.Serialize(data);
            BLMS.Common.LogService.Instance.Info("调用GetDMSIntegralList接口" + string.Format("积分结算传入参数：{0}||时间：{1}", jsonData, DateTime.Now.ToString()));
            //BLMS.Common.LogService.Instance.Info("调用GetDMSIntegralList接口" + string.Format("积分结算传入参数：{0}{1}{2}{3}{4}{5}", data.DealerId,data.EndTime,data.FromTime,data.ISAgree,data.TotalMoney,data.TotalPoint));

            if (data.IntegraList == null)
            {
                return Json(new { IsSuccess = false, Message = "IntegraList参数不能为空" });
            }
            bool BMbl = true;
            bool DMSbl = true;
            try
            {
                //先修改BM结算表的状态值
                BMbl = _AppContext.SettlementApp.UpdateBMSettlementStatus(data);
                DMSbl = _AppContext.SettlementApp.InsertDMSIntegralList(data);
                if (BMbl == true && DMSbl == true)
                {
                    BLMS.Common.LogService.Instance.Info("调用GetDMSIntegralList接口" + string.Format("积分结算（成功），方法：GetDMSIntegralList 传入参数：{0}||时间：{1}", jsonData, DateTime.Now.ToString()));
                    return Json(new { IsSuccess = true, Message = "SUCCESS" });
                }
                else
                {
                    BLMS.Common.LogService.Instance.Info("调用GetDMSIntegralList接口" + string.Format("积分结算（失败），方法：GetDMSIntegralList 传入参数：{0}||时间：{1}", jsonData, DateTime.Now.ToString()));

                    //Stream st = System.Web.HttpContext.Current.Request.InputStream;
                    //byte[] by = new byte[st.Length];
                    //s.Read(by, 0, (int)s.Length);
                    //var postStr1 = Encoding.UTF8.GetString(by);
                    //BLMS.Common.LogService.Instance.Info(postStr1);
                    return Json(new { IsSuccess = false, Message = "FAIL" });
                }
            }
            catch (Exception ex)
            {
                BLMS.Common.LogService.Instance.Info(ex.Message);
                return Json(new { IsSuccess = false, Message = "异常" });
            }
        }
        #endregion
    }
}