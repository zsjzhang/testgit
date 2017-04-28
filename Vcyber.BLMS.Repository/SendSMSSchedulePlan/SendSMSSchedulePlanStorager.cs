using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class SendSMSSchedulePlanStorager : ISendSMSSchedulePlanStorager
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>执行结果</returns>
        public bool AddSendSMSSchedulePlan(SendSMSSchedulePlan entity)
        {
            string sql = @"Insert Into SendSMSSchedulePlan(ServiceTitle,TimeType,ScheduleTime,ValueTime,SMSContent,IsOpen,CarCategory) 
                        Values(@ServiceTitle,@TimeType,@ScheduleTime,@ValueTime,@SMSContent,@IsOpen,@CarCategory)";

            return DbHelp.Execute(sql, entity) > 0;
        }

        /// <summary>
        /// 关闭或打开
        /// </summary>
        /// <returns>执行结果</returns>
        public bool UpdateSendSMSSchedulePlan(int id, string isOpen)
        {
            string sql = "UPDATE SendSMSSchedulePlan SET IsOpen = @IsOpen, UpdateTime = getdate() WHERE Id = @Id";

            return DbHelp.Execute(sql, new { @Id = id, @IsOpen = isOpen }) > 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateSendSMSSchedulePlan(SendSMSSchedulePlan entity)
        {
            string sql = @"UPDATE SendSMSSchedulePlan 
                            SET ServiceTitle = @ServiceTitle,TimeType = @TimeType,ScheduleTime = @ScheduleTime,
	                            ValueTime = @ValueTime,SMSContent = @SMSContent,CarCategory = @CarCategory,UpdateTime = getdate()
                            WHERE Id = @Id";
            return DbHelp.Execute(sql, entity) > 0;
        }

        /// <summary>
        /// 根据条件查询请求列表
        /// </summary>
        /// <param name="start">计划开始时间</param>
        /// <param name="end">计划结束时间</param>
        /// <param name="serviceTitle">任务名称</param>
        /// <param name="timeType">定时类型</param>
        /// <param name="status">状态</param>
        /// <returns>结果列表</returns>
        public IEnumerable<SendSMSSchedulePlan> SelectSendSMSSchedulePlanList(string start, string end, string serviceTitle, string timeType, string status, string carType)
        {
            var sql = @"SELECT * FROM SendSMSSchedulePlan WHERE 1=1 ";

            if (!string.IsNullOrEmpty(start))
            {
                sql += "AND ScheduleTime >= @Start ";
            }

            if (!string.IsNullOrEmpty(end))
            {
                sql += "AND ScheduleTime <= @End ";
            }

            if (!string.IsNullOrEmpty(serviceTitle))
            {
                sql += "AND ServiceTitle LIKE '%" + serviceTitle + "%'";
            }

            if (!string.IsNullOrEmpty(status))
            {
                sql += "AND IsOpen = @Status";
            }

            if (!string.IsNullOrEmpty(timeType))
            {
                sql += "AND TimeType = @TimeType";
            }

            if (!string.IsNullOrEmpty(carType))
            {
                sql += "AND CarCategory = @CarType";
            }

            sql += " ORDER BY CREATETIME DESC";

            return DbHelp.Query<SendSMSSchedulePlan>(sql, new { @Start = start, @End = end, @TimeType = timeType, @Status = status, @CarType = carType });
        }

        /// <summary>
        /// 查询短信发送记录
        /// </summary>
        /// <param name="start">发送时间</param>
        /// <param name="end">发送时间</param>
        /// <param name="userName">用户名</param>
        /// <param name="state">发送状态</param>
        /// <returns>结果列表</returns>
        public IEnumerable<SendSMSSchedulePlanResult> SelectSendSMSSchedulePlanResultList(string start, string end, string userName, string state, string title, string carType)
        {
            var sql = @"SELECT R.*,M.UserName,P.ServiceTitle,P.CarCategory FROM SendSMSSchedulePlanResult R 
                        INNER JOIN MemberShip M ON R.UserId = M.Id
                        INNER JOIN SendSMSSchedulePlan P ON R.PlanId = P.Id
                        WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(start))
            {
                sql += "AND R.SendTime >= @Start ";
            }

            if (!string.IsNullOrEmpty(end))
            {
                sql += "AND R.SendTime <= @End ";
            }

            if (!string.IsNullOrEmpty(userName))
            {
                sql += "AND M.UserName = @UserName ";
            }

            if (!string.IsNullOrEmpty(state))
            {
                sql += "AND R.IsSend = @State ";
            }

            if (!string.IsNullOrEmpty(title))
            {
                sql += "AND P.ServiceTitle LIKE '%" + title + "%'";
            }

            if (!string.IsNullOrEmpty(carType))
            {
                sql += "AND P.CarCategory = @CarType ";
            }

            sql += " ORDER BY R.CREATETIME DESC";

            return DbHelp.Query<SendSMSSchedulePlanResult>(sql, new { @Start = start, @End = end, @UserName = userName, @State = state, @CarType = carType });
        }
    }
}
