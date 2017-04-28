using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户蓝豆
    /// </summary>
    public class UserblueBean
    {
        #region ==== 构造函数 ====

        public UserblueBean() { }

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
        /// 蓝豆来源
        /// </summary>
        public string integralSource { get; set; }

        public string integralSourceName
        {
            get
            {
                return System.Enum.GetName(typeof(EBRuleType), int.Parse(this.integralSource));
            }
        }

        /// <summary>
        /// 蓝豆值
        /// </summary>
        public int value { get; set; }

        /// <summary>
        /// 已使用蓝豆值
        /// </summary>
        public int usevalue { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime { get; set; }

        #endregion


        /// <summary>
        /// 添加蓝豆初始化
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="remark"></param>
        public void Grow(string userId, EBRuleType type, int value)
        {
            this.userId = userId;
            this.integralSource = type.ToString();
            this.value = value;//当前问卷规定的蓝豆
            this.usevalue = 0;//此处不用管
            this.datastate = 0;
            this.remark = type.ToString();
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
        }
    }
}
