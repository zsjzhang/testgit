using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;

namespace Vcyber.BLMS.Common
{
    /// <summary>
    /// 创建mysql数据库连接
    /// </summary>
    public class MySqlConnection : IConnection
    {

        public IDbConnection CreateConnection(string url)
        {
            return new MySql.Data.MySqlClient.MySqlConnection(url);
        }
    }
}
