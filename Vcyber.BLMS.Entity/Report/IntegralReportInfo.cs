using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 会员积分分析
    /// </summary>
    public class IntegralReportInfo
    {
        #region ==== 构造函数 ====

        public IntegralReportInfo() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 经销商区域
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 
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
        /// 首次购车获取
        /// </summary>
        public int HGCTotal { get; set; }

        /// <summary>
        /// 曾换购获取
        /// </summary>
        public int HZHTotal { get; set; }

        /// <summary>
        /// 维保获取
        /// </summary>
        public int HWBTotal { get; set; }

        /// <summary>
        /// 维修消费
        /// </summary>
        public int XWXTotal { get; set; }

        /// <summary>
        /// 保养消费
        /// </summary>
        public int XBYTotal { get; set; }

        /// <summary>
        /// 机场服务消费
        /// </summary>
        public int XJCTotal { get; set; }

        /// <summary>
        /// 礼品消费
        /// </summary>
        public int XLPTotal { get; set; }

        /// <summary>
        /// 累计失效积分
        /// </summary>
        public int SXTotal { get; set; }

        #endregion
    }

    #region====服务明细====
    public class ServiceModel
    {

        public int rowid
        {
            get;
            set;
        }
        /// <summary>
        /// 申请时间
        /// </summary>
        public string CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        public string OrderType
        {
        get;
        set;
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName
        {
            get;
            set;
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdentityNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN
        {
            get;
            set;
        }
        /// <summary>
        /// 预约项目
        /// </summary>
        public string MaintainType
        {
            get;
            set;
        }
        /// <summary>
        /// 渠道
        /// </summary>
        public string DataSource
        {
            get;
            set;
        }
        /// <summary>
        /// 串码
        /// </summary>
        public string SNCode
        {
            get;
            set;
        }
        /// <summary>
        /// 是否免费
        /// </summary>
        public string SendType
        {
            get;
            set;
        }
        /// <summary>
        /// 是否使用
        /// </summary>
        public string IsUse
        {
            get;
            set;
        }
        /// <summary>
        /// 使用时间
        /// </summary>
        public string UseTime
        {
            get;
            set;
        }
        /// <summary>
        /// 预约机场
        /// </summary>
        public string AirportName
        {
            get;
            set;
        }
        /// <summary>
        /// 实际使用机场
        /// </summary>
        public string UseAdd
        {
            get;
            set;
        }
        /// <summary>
        /// 经销商店代码
        /// </summary>
        public string DealerId
        {
            get;
            set;
        }
        /// <summary>
        /// 经销商（店名）
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 经销商地址
        /// </summary>
        public string Address
        {
            get;
            set;
        }
        /// <summary>
        /// 服务使用状态
        /// </summary>
        public string Status
        {
            get;
            set;
        }
        /// <summary>
        /// 区域
        /// </summary>
        public string Area
        {
         get;
         set;
        }
        /// <summary>
        /// 办事处
        /// </summary>
        public string Region
        {
            get;
            set;
        }


    }
    #endregion

}
