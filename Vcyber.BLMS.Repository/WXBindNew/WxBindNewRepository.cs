using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.User;

namespace Vcyber.BLMS.Repository
{
    public class WxBindNewRepository
    {
        public bool IsWxBind_New(string openId)
        {
            string sql = @"
select * from WXBind wx
inner join Membership m on wx.UserId=m.Id
where wx.openId=@openId
";
            return DbHelp.Execute(sql, new { @openId = openId }) > 0;
        }

        public WXBindData GetUser(string openId)
        {
            string sql = @"
select 
wx.UserId,
wx.UserName,
wx.OpenId,
wx.Vin,
wx.Tel as PhoneNumber
from WXBind wx
where wx.openId=@openId
";
            return DbHelp.QueryOne<WXBindData>(sql, new { @openId = openId });
        }
    }
}
