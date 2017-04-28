using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    using Vcyber.BLMS.Entity.Generated;

    public class CSConsultantApp : ICSConsultantApp
    {
        #region ==== 构造函数 ====

        public CSConsultantApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 获取经销中的服务顾问
        /// </summary>
        /// <param name="dealerId">经销Id</param>
        /// <returns></returns>
        public IEnumerable<CSConsultant> GetList(string dealerId)
        {
            return _DbSession.CSConsultantStorager.SelectList(dealerId);
        }

        #endregion
    }
}
