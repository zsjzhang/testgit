using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 5种车辆养护产品实体
    /// </summary>
    public class MaintainService
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 卡券类型唯一GUID
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// 状态 1下架 2上架  
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 卡券名字
        /// </summary>
        public string CardName { get; set; }


        #region 附加属性
        /// <summary>
        /// 卡券logo
        /// </summary>
        public string CardLogoUrl
        {
            set;
            get;
        }
        public int  CradId
        {
            set;
            get;
        }

        /// <summary>
        /// 投放活动名称
        /// </summary>
        public string ActivityType { get; set; }



        /// <summary>
        /// 卡券开始有效期；
        /// </summary>
        public DateTime CardBeginDate
        {
            set;
            get;
        }

        /// <summary>
        /// 卡券结束有效期；
        /// </summary>
        public DateTime CardEndDate
        {
            set;
            get;
        }

        #endregion
       
    }
}
