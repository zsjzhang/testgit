using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace Vcyber.BLMS.Common
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public interface IConnection
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IDbConnection CreateConnection(string url);
    }
}
