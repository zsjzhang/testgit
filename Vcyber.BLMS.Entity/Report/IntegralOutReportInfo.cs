using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 会员消耗积分报表信息
    /// </summary>
    public class IntegralOutReportInfo
    {
        #region ==== 构造函数 ====

        public IntegralOutReportInfo() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 经销商区域
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 办事处
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 会员Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 订单形式
        /// </summary>
        public int OrderMode { get; set; }

        /// <summary>
        /// 积分值
        /// </summary>
        public int IntegralValue { get; set; }

        /// <summary>
        /// 积分获得时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        #endregion
    }
}
