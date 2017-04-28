using System;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity;
using System.Configuration;
using System.Collections.Generic;

namespace Vcyber.BLMS.Domain
{
    public class BrandServiceApp : IBrandServiceApp
    {
        public ReturnResult GetBrandServiceCode(string userId, string phoneNumber, string brandName)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            //获取用户已经领取的权益码
            BrandServiceCode code = _DbSession.BrandServiceCodeStorager.SelectBrandServiceCodeByUserId(userId, brandName);

            //已经领取的情况
            if (code != null)
            {
                result.IsSuccess = false;
                result.Message = "每位用户只能领取一次权益码,请使用之前领取的权益码";
                result.Data = code;
                return result;
            }

            //取出一张
            code = _DbSession.BrandServiceCodeStorager.GetBrandServiceCode(brandName);

            if (code != null)
            {
                //绑定权益码和用户关联
                bool isSuccess = _DbSession.BrandServiceCodeStorager.SendServiceCode(code.ServiceCode, userId) > 0;

                if (!isSuccess)
                {
                    result.IsSuccess = false;
                    result.Message = "权益码领取失败";
                    return result;
                }

                //发送短信
                string smsString = string.Format(ConfigurationManager.AppSettings["BRANDSERVICESMS"], code.ServiceCode);

                SmsApp sms = new SmsApp();
                sms.SendSMS(phoneNumber, smsString);
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "权益码获取失败";
            }

            result.Data = code;

            return result;
        }

        public ReturnResult AddMembershipBrand(MembershipBrand item)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            bool isSuccess = _DbSession.MembershipBrandStorager.AddMembershipBrand(item) > 0;

            if (!isSuccess)
            {
                result.IsSuccess = false;
                result.Message = "保存记录信息失败";
            }

            return result;
        }

        public IEnumerable<MembershipBrand> SelectMembershipBrandByUserId(string userId)
        {
            return _DbSession.MembershipBrandStorager.SelectMembershipBrandByUserId(userId);
        }

        public BrandServiceCode SelectBrandServiceCodeByUserId(string userId, string brandName)
        {
            return _DbSession.BrandServiceCodeStorager.SelectBrandServiceCodeByUserId(userId, brandName);
        }
    }
}
