using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 题型枚举
    /// </summary>
    public enum EQuestionType
    {
        /// <summary>
        /// 单选题
        /// </summary>
        [EnumDescribe("单选题")]
        Radio = 0,

        /// <summary>
        /// 多选题
        /// </summary>
        [EnumDescribe("多选题")]
        Check = 1,

        /// <summary>
        /// 判断题
        /// </summary>
        [EnumDescribe("判断题")]
        Judge = 2,

        /// <summary>
        /// 留言题
        /// </summary>
        [EnumDescribe("留言题")]
        Message = 3,

        /// <summary>
        /// 矩阵单选题
        /// </summary>
        [EnumDescribe("矩阵单选题")]
        JzRadio = 4,

        /// <summary>
        /// 矩阵多选题
        /// </summary>
        [EnumDescribe("矩阵多选题")]
        JzCheck = 5,

        /// <summary>
        /// 矩阵子题
        /// </summary>
        [EnumDescribe("矩阵子题")]
        JzChildren = 6,

        /// <summary>
        /// 满意度题
        /// </summary>
        [EnumDescribe("满意度题")]
        Satisfied = 7,

        /// <summary>
        /// 多项填空题
        /// </summary>
        [EnumDescribe("多项填空题")]
        JzMessage = 8,

        /// <summary>
        /// 排序题
        /// </summary>
        [EnumDescribe("排序题")]
        Sort = 9,

        /// <summary>
        /// 填空题
        /// </summary>
        [EnumDescribe("填空题")]
        FillText = 10
    }

    /// <summary>
    /// 状态
    /// </summary>
    public enum EState
    {
        /// <summary>
        /// 已删除
        /// </summary>
        Delete,

        /// <summary>
        /// 正常
        /// </summary>
        Normal
    }

    /// <summary>
    /// 子项类型
    /// </summary>
    public enum EChildrenType
    {
        /// <summary>
        /// 选项
        /// </summary>
        [EnumDescribe("选项")]
        Option = 0,

        /// <summary>
        /// 填空选项
        /// </summary>
        [EnumDescribe("选项")]
        tkOption = 1,

        /// <summary>
        /// 下拉选项
        /// </summary>
        [EnumDescribe("下拉选项")]
        dpOption=2,

        /// <summary>
        /// 子选项
        /// </summary>
        [EnumDescribe("子选项")]
        childrenOption =3,

        /// <summary>
        /// 关联选项
        /// </summary>
        [EnumDescribe("关联选项")]
        RelationOption =4
    }


    /// <summary>
    /// 问卷状态
    /// </summary>
    public enum EQuestionnaireState
    {
        /// <summary>
        /// 已删除
        /// </summary>
        Delete = 0,

        /// <summary>
        /// 未开始
        /// </summary>
        Ready = 1,

        /// <summary>
        /// 已开始
        /// </summary>
        Start = 2,

        /// <summary>
        /// 已结束
        /// </summary>
        End = 3
    }

    /// <summary>
    /// BM用于导出数据的用户类型
    /// </summary>
    public enum EQuestionnaireMemberLevel
    {
        /// <summary>
        /// 游客
        /// </summary>
        Visitor = 0,

        /// <summary>
        /// 一星会员
        /// </summary>
        OneMember = 1,

        /// <summary>
        /// 二星会员
        /// </summary>
        TwoMember = 2,

        /// <summary>
        /// 三星会员
        /// </summary>
        ThreeMember = 3,

        /// <summary>
        /// 索九会员
        /// </summary>
        SjMember = 4
    }

    /// <summary>
    /// 问卷类型
    /// </summary>
    public enum EQuestionnaireType
    {
        /// <summary>
        /// BM系统
        /// </summary>
        BM = 0,

        /// <summary>
        /// CS系统
        /// </summary>
        CS = 1
    }
}
