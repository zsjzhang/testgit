using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.WebApi.Filter;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/BrandService")]
    public class BrandServiceController : ApiController
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="brandName"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ReturnResult))]
        [Route("GetBrandServiceCode")]
        [IOVAuthorize]
        public IHttpActionResult GetBrandServiceCode(string userId, string phoneNumber, string brandName)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            if (string.IsNullOrEmpty(userId))
            {
                result.Message = "用户编号为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (string.IsNullOrEmpty(brandName))
            {
                result.Message = "特约商户标签为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId);
            if (account == null || account.Result == null)
            {
                result.Message = "账号不存在";
                result.IsSuccess = false;
                return Ok(result);
            }
            var userInfo = account.Result;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                phoneNumber = userInfo.PhoneNumber;
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    result.IsSuccess = false;
                    result.Message = "用户电话号码不存在";
                    return Ok(result);
                }
            }
            result = _AppContext.BrandServiceApp.GetBrandServiceCode(userId, phoneNumber, brandName);
            return Ok(result);
        }

        //IEnumerable<MembershipBrand>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<MembershipBrand>))]
        [Route("SelectMembershipBrandByUserId")]
        [IOVAuthorize]
        public IHttpActionResult SelectMembershipBrandByUserId(string userId)
        {
            var rsult = _AppContext.BrandServiceApp.SelectMembershipBrandByUserId(userId);
            return Ok(rsult);
        }

        [HttpGet]
        [ResponseType(typeof(ReturnResult))]
        [Route("TobeMasonryMember")]
        [IOVAuthorize]
        public IHttpActionResult TobeMasonryMember(string userId)
        {
            ReturnResult result = new ReturnResult { IsSuccess = false };  //获取权益码
            try
            {

                var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId);
                if (account == null || account.Result == null)
                {
                    result.Message = "账号不存在";
                    return Ok(result);
                }

                var userBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(account.Result.Id);
                var userIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(account.Result.Id);
                var user = account.Result;
                if (user.MLevel < 10)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "非车主会员无法完成注册";
                    return Ok(result);
                }

                //if (user.IdentityNumber.Length < 15)
                //{
                //    result.IsSuccess = false;
                //    result.Data = 400;
                //    result.Message = "集团用户暂时无法完成注册";
                //    return Ok(result);
                //}
                //判断到已经是会员
                if (_AppContext.BrandServiceApp.SelectMembershipBrandByUserId(user.Id).Where(x => x.BrandName == "HaiHang" && x.IsMember == "Y").Count() > 0)
                {
                    result.IsSuccess = true;
                    result.Data = 201;
                    result.Message = "您现在已经是会员了";
                    return Ok(result);
                }
                //if (!string.IsNullOrEmpty(user.IdentityNumber))
                //{
                //    result.IsSuccess = false;
                //    result.Data = 400;
                //    result.Message = "车主信息有误~~";
                //    return Ok(result);
                //}
                var customer = _AppContext.CarServiceUserApp.GetCustomer(user.IdentityNumber);

                if (customer == null)
                {
                    result.IsSuccess = false;
                    result.Data = 400;
                    result.Message = "车主信息有误~~";
                    return Ok(result);
                }

                //处理姓名
                var family_name = "";
                var first_name = "";

                if (customer.CustName.Length < 1)
                {
                    family_name = "蓝";
                    first_name = "缤";
                }
                else if (customer.CustName.Length == 1)
                {
                    family_name = customer.CustName.Substring(0, 1);
                    first_name = "某";
                }
                else if (customer.CustName.Length == 2)
                {
                    family_name = customer.CustName.Substring(0, 1);
                    first_name = customer.CustName.Substring(1, 1);
                }
                else
                {
                    family_name = customer.CustName.Substring(0, 1);
                    first_name = customer.CustName.Substring(1, 2);
                }


                var client = new HaiHangServiceReference.MemberServiceClient();

                var request = new HaiHangServiceReference.RecruitmentMemberRequest
                {
                    securityKey = "BJ123",
                    civilization_title_code = customer.Gender == "男" ? "Mr" : "Mrs",
                    family_name = family_name,
                    first_name = first_name,
                    mobile_phone = user.UserName,
                    password = "999999",
                    email_option = 0,
                    email_optionSpecified = true,
                    segmentSpecified = true,
                    language = "CN",
                    segment = 4
                };

                //发送请求，获取结果
                var response = new HaiHangServiceReference.RecruitmentMemberResponse();

                //模拟接口
                //response.exitCode = 0;
                //response.account_number = "960289881942";

                response = client.submitRecruitmentMember(request);

                if (response.exitCode == 0)
                {
                    result.IsSuccess = true;
                    result.Data = response.account_number;
                    _AppContext.BrandServiceApp.AddMembershipBrand(new MembershipBrand
                    {
                        UserId = user.Id,
                        BrandName = "HaiHang",
                        IsMember = "Y",
                        JoinTime = DateTime.Now
                    });
                }
                else if (response.exitCode == -1 || response.exitCode == -9)
                {
                    result.IsSuccess = false;
                    result.Data = response.exitCode;
                    result.Message = "接口发生内部错误";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Data = response.exitCode;
                    result.Message = response.errorInfo;
                }

                //TO-DO 保存记录
                IF_RequestLog logEntity = new IF_RequestLog()
                {
                    UserId = user.Id,
                    RequestData = _AppContext.RequestLogApp.ConverterToJson(request),
                    ResponseData = _AppContext.RequestLogApp.ConverterToJson(response)
                };
                _AppContext.RequestLogApp.Add(logEntity);

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Data = "";
                result.Message = "接口错误：" + ex.Message;
                LogService.Instance.Error(ex.InnerException.Message);
                return Ok(result);
            }

        }
    }
}