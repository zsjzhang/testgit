using System;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Repository
{
    public class BrandServiceCodeStorager : IBrandServiceCodeStorager
    {
        public BrandServiceCode SelectBrandServiceCodeByUserId(string userId, string brandName)
        {
            string sql = "SELECT TOP 1 * FROM BrandServiceCode WHERE UserId = @UserId AND BrandName = @BrandName";
            return DbHelp.QueryOne<BrandServiceCode>(sql, new { @UserId = userId, @BrandName = brandName });
        }

        public BrandServiceCode SelectBrandServiceCodeByCode(string serviceCode)
        {
            string sql = "SELECT TOP 1 * FROM BrandServiceCode WHERE ServiceCode = @ServiceCode";
            return DbHelp.QueryOne<BrandServiceCode>(sql, new { @ServiceCode = serviceCode });
        }

        public BrandServiceCode GetBrandServiceCode(string brandName)
        {
            string sql = "SELECT TOP 1 * FROM BrandServiceCode WHERE BrandName = @BrandName AND IsSend = 'N'";
            return DbHelp.QueryOne<BrandServiceCode>(sql, new { @BrandName = brandName });
        }

        public int SendServiceCode(string serviceCode, string userId)
        {
            string sql = "UPDATE BrandServiceCode SET UserId = @UserId,IsSend = 'Y',SendTime = GETDATE() WHERE ServiceCode = @ServiceCode";
            return DbHelp.Execute(sql, new { @UserId = userId, @ServiceCode = serviceCode });
        }
    }
}
