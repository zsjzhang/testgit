using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.IRepository
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity.User;

    public interface IUserWXStorager
    {
        /// <summary>
        /// 根据OpenId获取UserId
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        string GetUserIdByOpenId(string openId);

        void InsertBindData(WXBind bind);

        int UnbindWX(string openId, string userId);

        string GetUserNameByOpenId(string openId);
        
        Page<WXBindData> GetWXBindData(DateTime dateFrom, DateTime dateTo, string vin, string userName, string mobile, EWXBindStatus status, int page, int itemsPerPage);

        int UpdateIdentityNumber(string userId, string identityNo);

        string GetOpenIdByMobile(string mobile);

        string GetOneVinByUserId(string userId);

        List<WXBind> GetBindStatus(string openId, string vin, string mobile);

        List<string> GetNickname(string nickname);
    }
}
