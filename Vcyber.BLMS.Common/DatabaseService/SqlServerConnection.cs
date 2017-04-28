using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    /// <summary>
    /// 创建sql server数据连接
    /// </summary>
    public class SqlServerConnection : IConnection
    {
        /// <summary>
        /// 创建sql server数据库连接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public System.Data.IDbConnection CreateConnection(string url)
        {
            return new System.Data.SqlClient.SqlConnection(url);
        }
    }
}
