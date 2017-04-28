using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class Question
    {
        #region ==== 构造函数 ====
        public Question() { }
        #endregion

        #region ==== 公用属性====

        public int Id { get; set; }

        /// <summary>
        /// 问题的内容
        /// </summary>
        public string QContent { get; set; }

        /// <summary>
        /// 题目类型：0 单选，1 多选，2 判断，3 留言
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 题目当前状态：0 删除，1 正常
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 所属问卷id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 问题是否为必填
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 矩阵填空题 循环次数
        /// </summary>
        public int Cycle { get; set; }

        /// <summary>
        /// 当问题为矩阵填空题时，设置是否选项文字前置
        /// </summary>
        public bool TextIsBefore { get; set; }

        /// <summary>
        /// 是否选中后才能填写填空
        /// </summary>
        public bool IsChecked { get; set; }
        #endregion
    }
}
