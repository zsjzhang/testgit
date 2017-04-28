using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 短信数据
    /// </summary>
    public class SmsData
    {
        #region ==== 构造函数 ====

        public SmsData() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 积分支付值
        /// </summary>
        public int IntegralPayValue { get; set; }

        /// <summary>
        /// 蓝豆支付值
        /// </summary>
        public int BlueBenPayValue { get; set; }

        /// <summary>
        /// 总积分
        /// </summary>
        public int TotalIntegral { get; set; }

        /// <summary>
        /// 总蓝豆
        /// </summary>
        public int TotalBlueBen { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidateCode { get; set; }

        /// <summary>
        /// 服务码
        /// </summary>
        public string ServiceCode { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }


        #endregion
    }
}
