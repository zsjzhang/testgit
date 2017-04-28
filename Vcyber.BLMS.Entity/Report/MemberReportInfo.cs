using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 会员报告信息
    /// </summary>
    public class MemberReportInfo
    {
        #region ==== 构造函数 ====

        public MemberReportInfo() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public int MLevel { get; set; }

        public string MLevelName
        {
            get
            {
                switch ((MemshipLevel)(this.MLevel))
                {
                    case MemshipLevel.OneStar: return "注册";
                        break;
                    case MemshipLevel.CommonCard: return "普卡";
                        break;
                    case MemshipLevel.SilverCard: return "银卡";
                        break;
                    case MemshipLevel.GoldCard: return "金卡";
                        break;
                    //case MemshipLevel.ThreeStar: return string.IsNullOrEmpty(this.No) ? "三星" : "银卡会员";
                    //    break;
                    //case MemshipLevel.QZ: return "潜客";
                    //case MemshipLevel.SJ: return "索九";
                    default: return "";
                        break;
                }
            }
        }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 车种
        /// </summary>
        public string CarCategory { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }

        /// <summary>
        /// 购车时间
        /// </summary>
        public DateTime BuyTime { get; set; }

        /// <summary>
        /// 店代码
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 购车区域
        /// </summary>
        public string BuyingArea { get; set; }

        /// <summary>
        /// 购车办事处
        /// </summary>
        public string BuyingRegion { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 会员时间
        /// </summary>
        public DateTime MemberTime { get; set; }

        /// <summary>
        /// 银卡申请状态
        /// </summary>
        public int YKStatus { get; set; }

        public string YKStatusName
        {
            get
            {
                switch (this.YKStatus)
                {
                    case 0: return "未申请"; break;
                    case 1: return "待审核"; break;
                    case 2: return "待审核"; break;
                    case 3: return "已通过"; break;
                    case 4: return "审核未通过"; break;
                    default: return "未申请";
                        break;
                }
            }
        }

        /// <summary>
        /// 申请渠道
        /// </summary>
        public string SDataSource { get; set; }

        public string SDataSourceName
        {
            get
            {
                switch (this.SDataSource)
                {
                    case "blms":return "网站";
                        break;
                    case "blms_web": return "APP";
                        break;
                    case "blms_wechat": return "微信";
                        break;
                    default: {
                        if (string.IsNullOrEmpty(this.SDataSource))
                        {
                            return "网站";
                        }
                        else
                        {
                            if (this.SDataSource.StartsWith("D"))
                            {
                                return "下线";
                            }

                            return "";
                        }
                    }
                        break;
                }

            }
        }

        /// <summary>
        /// 支付编号
        /// </summary>
        public string PayNumber { get; set; }

        /// <summary>
        /// 支付经销商名称
        /// </summary>
        public string PayDealerName { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 爱好
        /// </summary>
        public string Interest { get; set; }

        /// <summary>
        /// NO
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 是否已支付
        /// </summary>
        public string IsPay { get; set; }

        #endregion
    }
}
