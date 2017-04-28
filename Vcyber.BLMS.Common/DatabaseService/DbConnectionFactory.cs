using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.Data;

namespace Vcyber.BLMS.Common
{
    /// <summary>
    /// 数据库连接工厂
    /// </summary>
    public sealed class DbConnectionFactory
    {
        #region ==== 构造函数 ====

        private DbConnectionFactory()
        { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection CreateConnection()
        {
            ConnectionStringSettings settings=ConfigurationManager.ConnectionStrings["DbConnection"];

            if (settings == null || string.IsNullOrEmpty(settings.ProviderName.Trim()))
            {
                throw new Exception("数据库连接配置不正确。");
            }

            IDbConnection con = null;
            string dbType = settings.ProviderName.Trim();
            string url=settings.ConnectionString;

            switch (dbType)
            {
                case "MySql":con=new MySqlConnection().CreateConnection(url);break;
                case "System.Data.SqlClient": con = new SqlServerConnection().CreateConnection(url); break;
                default:
                    break;
            }

            if (con==null)
            {
                throw new Exception("数据库连接配置不正确。");
            }

            return con;
        }

        #endregion
    }
}
