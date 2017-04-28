using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface ISendSMSSchedulePlanApp
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>执行结果</returns>
        bool AddSendSMSSchedulePlan(SendSMSSchedulePlan entity);

        /// <summary>
        /// 关闭或打开
        /// </summary>
        /// <returns>执行结果</returns>
        bool UpdateSendSMSSchedulePlan(int id, string isOpen);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>执行结果</returns>
        bool UpdateSendSMSSchedulePlan(SendSMSSchedulePlan entity);

        /// <summary>
        /// 根据条件查询请求列表
        /// </summary>
        /// <param name="start">计划开始时间</param>
        /// <param name="end">计划结束时间</param>
        /// <param name="serviceTitle">任务名称</param>
        /// <param name="timeType">定时类型</param>
        /// <param name="status">状态</param>
        /// <returns>结果列表</returns>
        IEnumerable<SendSMSSchedulePlan> SelectSendSMSSchedulePlanList(string start, string end, string serviceTitle, string timeType, string status, string carType);

        /// <summary>
        /// 查询短信发送记录
        /// </summary>
        /// <param name="start">发送时间</param>
        /// <param name="end">发送时间</param>
        /// <param name="userName">用户名</param>
        /// <param name="state">发送状态</param>
        /// <returns>结果列表</returns>
        IEnumerable<SendSMSSchedulePlanResult> SelectSendSMSSchedulePlanResultList(string start, string end, string userName, string state, string title, string carType);
    }
}
