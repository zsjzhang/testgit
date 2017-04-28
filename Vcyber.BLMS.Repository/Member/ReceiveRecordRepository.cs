using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Repository.Entity.Generated;
using PetaPoco;
using Vcyber.BLMS.Entity.Member;
namespace Vcyber.BLMS.Repository
{
    public class ReceiveRecordRepository : IReceiveRecordRepository
    {
        /// <summary>
        /// 添加一条领取记录
        /// </summary>
        /// <param name="obj">领取记录实体</param>
        /// <returns>成功是失败否</returns>
        public bool Add(ReceiveRecord obj)
        {
            var flag = false;
            var id = (int)PocoHelper.CurrentDb().Insert(obj);
            if (id > 0)
            {
                flag = true;
            }
            return flag;            
        }
        /// <summary>
        /// 领取记录是否存在
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="brandName">业务名称</param>
        /// <returns>存在是，不存在否</returns>
        public bool IsExist(string userId,string brandName)
        {
            var sql = Sql.Builder
                .Append("SELECT count(*) FROM dbo.ReceiveRecord AS rr")
                .Append("WHERE rr.UserId = @0 AND rr.BrandName = @1", userId, brandName);
            var flag = PocoHelper.CurrentDb().ExecuteScalar<int>(sql) > 0;
            return flag;
        }
    }
}
