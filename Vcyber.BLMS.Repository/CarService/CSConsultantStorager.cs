using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    using System.Configuration;

    using Vcyber.BLMS.Entity.Generated;

    /// <summary>
    /// 服务顾问操作
    /// </summary>
    public class CSConsultantStorager : ICSConsultantStorager
    {
        #region ==== 构造函数 ====

        public CSConsultantStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 获取经销中的服务顾问
        /// </summary>
        /// <param name="dealerId">经销Id</param>
        /// <returns></returns>
        public IEnumerable<CSConsultant> SelectList(string dealerId)
        {
            string sql = @"SELECT [Id]
      ,[Name]
      ,[DealerId]
      ,[DealerName]
      ,'"+ConfigurationManager.AppSettings["ImgPath"] +@"'+[Photo] Photo
      ,[Tel]
      ,[Sex]
      ,[Comment]
      ,[Age]
      ,[Title]
  FROM [CS_Consultant] where CS_Consultant.DealerId=@DealerId";
            return DbHelp.Query<CSConsultant>(sql, new { DealerId=dealerId });
        }

        #endregion
    }
}
