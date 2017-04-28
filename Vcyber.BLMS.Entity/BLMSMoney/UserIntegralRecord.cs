using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity.BLMSMoney
{


    public class UserIntegralRecordDetail
    {
        public int TotalIntegral { get; set; }//总积分 

        public int ResidueIntegral { get; set; } //剩余积分  

        public List<UserIntegralRecord> UserIntegrals { get; set; }//积分明细

    }


    public class UserIntegralRecord
    {


        #region ==== 构造函数 ====

        public UserIntegralRecord() { }

        #endregion

        #region ==== 公共属性 ====

        ///// <summary>
        ///// 
        ///// </summary>
        //public int Id { get; set; }

        ///// <summary>
        ///// 用户id
        ///// </summary>
        //public string userId { get; set; }

        ///// <summary>
        ///// 积分来源 经销商入会返积分30
        ///// </summary>
        //public string integralSource { get; set; }

        //public string integralSourceName
        //{
        //    get
        //    {
        //        int source = 0;
        //        if (int.TryParse(this.integralSource, out source))
        //        {
        //            return System.Enum.GetName(typeof(EIRuleType), int.Parse(this.integralSource));
        //        }
        //        else
        //            return this.integralSource;
        //    }
        //}

        /// <summary>
        /// 新增/消费
        /// </summary>
        public string IntegralType { get; set; }

        /// <summary>
        /// 经销商名称/礼品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 积分值
        /// </summary>
        public int value { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public int surplus { get; set; }

        /// <summary>
        /// 积分变动时间
        /// </summary>
        public string CreateTime { get; set; }

        ///// <summary>
        ///// (0:有效；2：失效)
        ///// </summary>
        //public int datastate { get; set; }

        ///// <summary>
        ///// 备注
        ///// </summary>
        //public string remark { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public string CreateTime { get; set; }


        ///// <summary>
        /////积分开始有效时间
        ///// </summary>
        //public string IntegralBeginDate
        //{
        //    get;
        //    set;
        //}
        ///// <summary>
        /////积分失效时间
        ///// </summary>
        //public string IntegralInvalidDate
        //{
        //    get;
        //    set;
        //}




        #endregion
    }


}