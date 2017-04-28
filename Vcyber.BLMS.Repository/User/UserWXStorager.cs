using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Repository
{
    using System.Data;

    using PetaPoco;

    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity.User;
    using Vcyber.BLMS.IRepository;
    using Vcyber.BLMS.Repository.Entity.Generated;

    public class UserWXStorager:IUserWXStorager
    {
        //public string GetUserIdByOpenId(string openId)
        //{
        //    return DbHelp.QueryOne<string>("select UserId from WXBind where OpenId=@0 and status=0", openId);
        //}
        public string GetUserIdByOpenId(string openId)
        {
            return DbHelp.QueryOne<string>("select UserId from WXBind where OpenId=@OpenId and status=0", new { @OpenId = openId });
        }
        public void InsertBindData(WXBind bind)
        {
            
            WXBind list = PocoHelper.CurrentDb().SingleOrDefault<WXBind>("where openid=@0", bind.OpenId);
          
            if (list != null && !string.IsNullOrEmpty(list.OpenId))//重新绑定
            {
                PocoHelper.CurrentDb().Update<WXBind>("set status=0, userid=@0, vin=@2, updatetime=getdate()  where openid=@1", bind.UserId, bind.OpenId, bind.Vin);
                return;
            }
            bind.CreateTime = DateTime.Now;
            bind.UpdateTime = bind.CreateTime;
            PocoHelper.CurrentDb().Insert("WXBind", "OpenId", false, bind);
        }

        public int UnbindWX(string openId, string userId)
        {
            return PocoHelper.CurrentDb().Update<WXBind>("set status=1 where openid=@0 and userid=@1", openId, userId);
        }

        public string GetUserNameByOpenId(string openId)
        {
            return PocoHelper.CurrentDb().FirstOrDefault<string>(
@"select a.[UserName] from Membership a
join WXBind b
on a.Id = b.UserId
where b.OpenId=@0
and b.Status=0", openId);
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
            Sql sql = new Sql(
                @"select UpdateTime as CreateTime,OpenId, RealName, PhoneNumber,Vin, CarCategory, UserName,
 point, bluebeans,gender,[Address] from (
select w.UpdateTime,w.OpenId, m.RealName, m.PhoneNumber,w.Vin, ic.CarCategory, m.UserName,
 sum(ut.value-ut.usevalue) as point, sum(ub.value-ub.usevalue) as bluebeans,
  m.gender, m.[Address] from Membership m
join WXBind w
on m.Id = w.UserId
left join userintegral ut
on ut.userId=m.Id
left join userbluebean ub
on ub.UserId = m.Id
left join IF_car ic
on w.Vin= ic.VIN");
            sql.Where("w.Status=@0", (int)status);
            sql.Where("w.UpdateTime between @0 and @1", dateFrom, dateTo);
            if (!string.IsNullOrEmpty(vin)) sql.Where("w.Vin=@0", vin);
            if (!string.IsNullOrEmpty(userName)) sql.Where("m.UserName=@0", userName);
            if (!string.IsNullOrEmpty(mobile)) sql.Where("m.PhoneNumber=@0", mobile);
            sql.Append("group by w.UpdateTime,w.OpenId, m.RealName, m.PhoneNumber,w.Vin, m.UserName, m.gender, m.[Address], ic.CarCategory) a order by UpdateTime, openid desc");
            return PocoHelper.CurrentDb().Page<WXBindData>(page, itemsPerPage,sql);
        }

        public int UpdateIdentityNumber(string userId, string identityNo)
        {
            return PocoHelper.CurrentDb()
                .Update<BLMS.Entity.Generated.Membership>(new Sql("set identitynumber=@0 where id=@1", identityNo, userId));
        }

        public string GetOpenIdByMobile(string mobile)
        {
            return PocoHelper.CurrentDb().FirstOrDefault<string>(
@"select b.OpenId from Membership a
join WXBind b
on a.Id = b.UserId
where a.UserName=@0
and b.Status=0", mobile);
            
        }

        public string GetOneVinByUserId(string userId)
        {
            return PocoHelper.CurrentDb().FirstOrDefault<string>(@"
select c.VIN from Membership a
join IF_Customer b
on a.IdentityNumber = b.IdentityNumber
join IF_Car c
on b.CustId=c.CustId
where a.Id=@0", userId);
        }

        public List<WXBind> GetBindStatus(string openId, string vin, string mobile)
        {
            var users = new List<string>();
            if (!string.IsNullOrEmpty(vin))
            {
                users = PocoHelper.CurrentDb().Fetch<string>(@"
select a.id from Membership a
join IF_Customer b
on a.IdentityNumber = b.IdentityNumber
join IF_Car c
on b.CustId=c.CustId
where c.vin=@0", vin);

            }
            Sql sql = new Sql("select distinct *  from wxbind where status=0 ");
            
            if (!string.IsNullOrEmpty(openId)) 
            {
                sql.Append(" and openid=@0", openId);
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                sql.Append(" and UserName=@0", mobile);
            }
            if (users.Count > 0)
            {
                sql.Append(" and UserId in (@0)", users);
            }
            return PocoHelper.CurrentDb().Fetch<WXBind>(sql);
           
        }

        public List<string> GetNickname(string nickname)
        {
            return PocoHelper.CurrentDb()
                .Fetch<string>("select nickname from membership where nickname like @0", nickname + "%");
        }

        public LevelAndNo GetUserLevelByVin(string vin)
        {
            string sql = @"SELECT M.MLevel,M.No,M.Id AS UserId FROM Membership M, IF_Customer C, IF_Car R 
                            WHERE M.IdentityNumber = C.IdentityNumber AND C.CustId = R.CustId AND R.VIN = @VIN";

            return DbHelp.QueryOne<LevelAndNo>(sql, new { @VIN = vin });
        }
    }
}
