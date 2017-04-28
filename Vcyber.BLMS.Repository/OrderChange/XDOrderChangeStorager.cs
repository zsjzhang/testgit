using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class XDOrderChangeStorager : IXDOrderChangeStorager
    {
        public XDOrderChangeStorager() { }

        public bool IsCanOrderChange(int activityId, string mobile, int orderChangeType)
        {
            string sql = "SELECT COUNT(1) FROM XD_OrderChange WHERE ActivityId=@ActivityId and Mobile=@Mobile and OrderChangeType=@OrderChangeType AND IsValid=1";
            return DbHelp.ExecuteScalar<int>(sql, new { ActivityId = activityId, Mobile = mobile, OrderChangeType = orderChangeType}) > 0;
        }
        public bool AddOrderChange(XDOrderChange xDOrderChange)
        {
            string sql = "INSERT INTO [XD_OrderChange]([ActivityId],[CarSeriers],[Name],[Mobile],[ShopProvince],[ShopCity],[ShopDistrict],[ShopCode],[DealName],[SendProvince],[SendCity],[OldCarBrand],[OldCarSeriers],[OldCarLicenseYear],[OldCarLicenseMonth],[InviteCode],[OldCarDriver],[OrderChangeType],[OrderChangeSource],[OrderChangeTime],[IsValid],[CreaterId],[CreaterName],[CreaterTime],[UpdaterId],[UpdaterName],[UpdaterTime],[SendDistrinct],[SendAddress])VALUES(@ActivityId,@CarSeriers,@Name,@Mobile,@ShopProvince,@ShopCity,@ShopDistrict,@ShopCode,@DealName,@SendProvince,@SendCity,@OldCarBrand,@OldCarSeriers,@OldCarLicenseYear,@OldCarLicenseMonth,@InviteCode,@OldCarDriver,@OrderChangeType,@OrderChangeSource,@OrderChangeTime,@IsValid,@CreaterId,@CreaterName,@CreaterTime,@UpdaterId,@UpdaterName,@UpdaterTime,@SendDistrinct,@SendAddress);";
            return DbHelp.Execute(sql, xDOrderChange) > 0;
        }

        public XDOrderChange GetOrderChangeByMobile(int activityId, string mobile)
        {
            string sql = "SELECT * FROM XD_OrderChange WHERE ActivityId=@ActivityId and Mobile=@Mobile";
            return DbHelp.QueryOne<XDOrderChange>(sql, new { ActivityId = activityId, Mobile = mobile });

        }




        public bool IsCanOrderChange(int activityId, string mobile)
        {
            string sql = "SELECT COUNT(1) FROM XD_OrderChange WHERE ActivityId=@ActivityId and Mobile=@Mobile AND IsValid=1";
            return DbHelp.ExecuteScalar<int>(sql, new {ActivityId=activityId, Mobile = mobile }) > 0;
        }
        public bool IsCanOrderChange(string carSeriers, string mobile, string shopCode)
        {
            string sql = "SELECT COUNT(*) FROM dbo.XD_OrderChange AS xoc WHERE xoc.CarSeriers = @CarSeriers AND xoc.Mobile = @Mobile AND xoc.ShopCode = @ShopCode AND xoc.IsValid = 1";
            return DbHelp.ExecuteScalar<int>(sql, new { CarSeriers = carSeriers, Mobile = mobile, ShopCode = shopCode }) > 0;
        }
    }
}
