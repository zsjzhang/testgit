using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Application
{
    using System.Security.Cryptography.X509Certificates;

    using PetaPoco;

    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity.User;

    /// <summary>
    /// 微信用户和BM会员匹配关系
    /// </summary>
    public interface IUserWXBindApp
    {
        /// <summary>
        /// 根据微信的OpenId获取BM的UserId
        /// </summary>
        /// <param name="openId">BM UserId</param>
        /// <returns></returns>
        string GetUserIdByOpenId(string openId);

        /// <summary>
        /// 根据微信的OpenId获取BM的UserName
        /// </summary>
        /// <param name="openId">BM UserId</param>
        /// <returns></returns>
        string GetUserNameByOpenId(string openId);
        
        /// <summary>
        /// 验证Vin
        /// </summary>
        /// <param name="vin">车辆vin</param>
        /// <para name="tel">手机号码</para>
        /// <returns>0：未注册，1：三星会员，2：待验证三星会员，3：普通会员，4：非车主会员(粉丝)</returns>
        EVinVerificationStatus VerifyVin(string vin, string tel);

        void InsertBindData(WXBind bind);

        int UnbindWX(string openId, string userId);

        Page<WXBindData> GetWXBindData(DateTime dateFrom, DateTime dateTo, string vin, string userName, string mobile, EWXBindStatus status, int page, int itemsPerPage);

        /// <summary>
        /// 更新身份证号
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int UpdateIdentityNumber(string userId, string identityNo);

        string GetOpenIdByMobile(string mobile);

        string GetOneVinByUserId(string userId);

        List<WXBind> GetBindStatus(string openId, string vin,string mobile);

        string GenerateNickname(string nickname);
    }

}
