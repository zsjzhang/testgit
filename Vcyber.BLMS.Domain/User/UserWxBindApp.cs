using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Domain
{
    using AspNet.Identity.SQL;

    using Microsoft.AspNet.Identity;

    using PetaPoco;

    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity.User;
    using Vcyber.BLMS.IRepository;

    public class UserWXBindApp: IUserWXBindApp
    {
        public string GetUserIdByOpenId(string openId)
        {
            return _DbSession.UserWxStorager.GetUserIdByOpenId(openId);
        }

        public string GetUserNameByOpenId(string openId)
        {
            return _DbSession.UserWxStorager.GetUserNameByOpenId(openId);
        }

        public EVinVerificationStatus VerifyVin(string vin, string tel)
        {
            throw new NotImplementedException();
        }

        public void InsertBindData(WXBind bind)
        {
            _DbSession.UserWxStorager.InsertBindData(bind);
        }

        public int UnbindWX(string openId, string userId)
        {
            return _DbSession.UserWxStorager.UnbindWX(openId, userId);
        }

        public Page<WXBindData> GetWXBindData(
            DateTime dateFrom,
            DateTime dateTo,
            string vin,
            string userName,
            string mobile,
            EWXBindStatus status,
            int page,
            int itemsPerPage)
        {
            return _DbSession.UserWxStorager.GetWXBindData(dateFrom, dateTo, vin, userName, mobile, status, page, itemsPerPage);
        }

        public int UpdateIdentityNumber(string userId, string identityNo)
        {
            return _DbSession.UserWxStorager.UpdateIdentityNumber(userId, identityNo);
        }

        public string GetOpenIdByMobile(string mobile)
        {
            return _DbSession.UserWxStorager.GetOpenIdByMobile(mobile);
        }

        public string GetOneVinByUserId(string userId)
        {
            return _DbSession.UserWxStorager.GetOneVinByUserId(userId);
        }

        public List<WXBind> GetBindStatus(string openId, string vin, string mobile)
        {
            return _DbSession.UserWxStorager.GetBindStatus(openId, vin, mobile);
        }

        public string GenerateNickname(string nickname)
        {
            if (string.IsNullOrEmpty(nickname)) nickname = "蓝缤会员";
            var names= _DbSession.UserWxStorager.GetNickname(nickname);
            bool success = false;
            if (names.Count > 0)
            {
                for (int i = 0; i < 20000; i++)
                {
                    var _nickname = nickname + i.ToString("000");
                    if (!names.Contains(_nickname)) return _nickname;
                }
                return Guid.NewGuid().ToString().Replace("-", "");
            }
            return nickname;
        }
    }
}
