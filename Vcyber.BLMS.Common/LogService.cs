using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace Vcyber.BLMS.Common
{
    /// <summary>
    /// 日志服务
    /// </summary>
    public class LogService
    {
        #region ==== 私有字段 ====

        private static readonly LogService instance = new LogService();

        private ILog logInstance = null;

        #endregion

        #region ==== 构造函数 ====

        private LogService()
        {
            logInstance = LogManager.GetLogger("loginfo");
        }

        #endregion

        #region ==== 公共属性 ====

        public static LogService Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 加载log4net配置信息
        /// </summary>
        public void LoadConfig()
        {
            log4net.Config.DOMConfigurator.Configure();
        }

        public void LoadConfig(string configPath)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(configPath);
            //file.Open(System.IO.FileMode.Open);
            log4net.Config.DOMConfigurator.Configure(file);
        }

        public void Info(string message)
        {
            this.logInstance.Info(message);
        }

        public void Info(string message, Exception ex)
        {
            this.logInstance.Info(message, ex);
        }

        public void Error(string message)
        {
            this.logInstance.Error(message);
        }

        public void Error(string message, Exception ex)
        {
            this.logInstance.Error(message, ex);
        }

        public void Debug(string message, Exception ex)
        {
            this.logInstance.Debug(message, ex);
        }

        public void Debug(string message)
        {
            this.logInstance.Debug(message);
        }

        #endregion
    }
}
