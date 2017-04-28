using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Repository.Entity.Generated;
using Vcyber.BLMS.IRepository;
namespace Vcyber.BLMS.Repository
{
    public class WeixinMerchantStorager : IWeixinMerchantStorager
    {
        /// <summary>
        /// 添加微信商户
        /// </summary>
        public int Add(WeixinMerchant obj) 
        {
            var sql = Sql.Builder
                .Append("INSERT INTO dbo.WeixinMerchant(Name,Phone,OpenId,DealerId,Remark,CreateTime)")
                .Append("VALUES(@0,@1,@2,@3,@4,GETDATE())", obj.Name, obj.Phone, obj.OpenId, obj.DealerId, obj.Remark);
            var id = (int)PocoHelper.DBContext().Execute(sql);
            return id;            
        }
        /// <summary>
        /// 查询用户在经销商下是否已添加
        /// </summary>
        public bool IsExist(string dealerId,string openId)
        {
            var sql = Sql.Builder.Append("SELECT COUNT(0) FROM dbo.WeixinMerchant AS wm WHERE DealerId = @0 AND OpenId = @1",dealerId,openId);
            var count = PocoHelper.DBContext().ExecuteScalar<int>(sql);
            return count > 0;
        }
    }
}
