using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 会员编号后六位生成操作
    /// </summary>
    public class MemberNumberRepository : IMemberNumberRepository
    {
        #region ==== 构造函数 ====

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Insert()
        {
            string sql = "insert into MemberNumber(CreateTime) values(getdate());select @@identity";
            int id = DbHelp.ExecuteScalar<int>(sql);
            return id >= 999999 ? id - 999999 : id;
        }

        #endregion
    }
}
