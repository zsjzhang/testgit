using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户积分
    /// </summary>
    public class UserIntegral
    {
        #region ==== 构造函数 ====

        public UserIntegral() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 积分来源 经销商入会返积分30
        /// </summary>
        public string integralSource { get; set; }

        public string integralSourceName
        {
            get
            {
                int source = 0;
                if (int.TryParse(this.integralSource, out source))
                {
                    return System.Enum.GetName(typeof(EIRuleType), int.Parse(this.integralSource));
                }
                else
                    return this.integralSource;
            }
        }

        /// <summary>
        /// 积分值
        /// </summary>
        public int value { get; set; }

        /// <summary>
        /// 已使用积分值
        /// </summary>
        public int usevalue { get; set; }

        public int remainValue
        {
            get
            {
                return value - usevalue;
            }
        }

        /// <summary>
        /// (0:有效；2：失效)
        /// </summary>
        public int datastate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string CreateTimeStr
        {
            get
            {
                return CreateTime.ToLongDateString();
            }
        }
        /// <summary>
        ///积分开始有效时间
        /// </summary>
        public DateTime IntegralBeginDate
        {
            get;
            set;
        }
        /// <summary>
        ///积分失效时间
        /// </summary>
        public DateTime IntegralInvalidDate
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 1=获得积分；2=消费积分
        /// </summary>
        public int IntegralType { get; set; }


        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }//add by wangchunrong 在积分明细记录表里添加礼品名称20161205
        #endregion
    }

}
