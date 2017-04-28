using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    using Vcyber.BLMS.Entity.Generated;

    /// <summary>
    /// 服务顾问业务
    /// </summary>
    public interface ICSConsultantApp
    {
        /// <summary>
        /// 获取经销中的服务顾问
        /// </summary>
        /// <param name="dealerId">经销Id</param>
        /// <returns></returns>
        IEnumerable<CSConsultant> GetList(string dealerId);
    }
}
