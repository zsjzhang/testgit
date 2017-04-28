using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;

namespace Vcyber.BLMS.Common
{
    public class MySqlDbHelper
    {
        private static string connStr;

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="configName">config文件中指定的数据库连接名称</param>
        /// <returns></returns>
        public static MySql.Data.MySqlClient.MySqlConnection GetConnection(string configName)
        {
            if (string.IsNullOrEmpty(connStr))
            {
                if (ConfigurationManager.ConnectionStrings[configName] != null)
                {
                    connStr = ConfigurationManager.ConnectionStrings[configName].ConnectionString;
                }
            }
            MySql.Data.MySqlClient.MySqlConnection cnn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
            try
            {
                if (cnn.State != ConnectionState.Open)
                    cnn.Open();
            }
            catch (Exception)
            {
                cnn.Close();
                throw;
            }

            return cnn;
        }
    }
}
