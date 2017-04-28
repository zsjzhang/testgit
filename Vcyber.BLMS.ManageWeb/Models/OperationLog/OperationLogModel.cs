using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class OperationLogModel
    { 
        #region ==== 构造函数 ====

        public OperationLogModel()
        { }

        #endregion

        #region ==== 公共属性 ====

        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int Type { get; set; }

        public string TypeValue
        {
            get
            {
                return ((ELogType)this.Type).GetDiscribe();
            }


        }
        /// <summary>
        /// 日志关联数据源Id
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// 操作项目名称
        /// </summary>
        public string OperateItem { get; set; }

        /// <summary>
        /// 原始状态
        /// </summary>
        public string OriginalValue { get; set; }

        /// <summary>
        /// 当前操作状态
        /// </summary>
        public string CurrentValue { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public string OperateTime { get; set; }


        /// <summary>
        /// 操作人员
        /// </summary>
        public string OperaterId { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string OperaterName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion
    }
}