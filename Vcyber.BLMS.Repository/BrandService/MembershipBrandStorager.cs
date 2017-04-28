using System;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;
using System.Collections.Generic;

namespace Vcyber.BLMS.Repository
{
    public class MembershipBrandStorager : IMembershipBrandStorager
    {
        public int AddMembershipBrand(MembershipBrand item)
        {
            string sql = @"INSERT INTO MembershipBrand(UserId,BrandName,ServiceCode,IsMember,JoinTime) 
                            VALUES(@UserId,@BrandName,@ServiceCode,@IsMember,GETDATE())";
            return DbHelp.Execute(sql, new { @UserId = item.UserId, @BrandName = item.BrandName, @ServiceCode = item.ServiceCode, @IsMember = item.IsMember });
        }

        public IEnumerable<MembershipBrand> SelectMembershipBrandByUserId(string userId)
        {
            string sql = "SELECT * FROM MembershipBrand WHERE UserId = @UserId";
            return DbHelp.Query<MembershipBrand>(sql, new { @UserId = userId });
        }
    }
}
