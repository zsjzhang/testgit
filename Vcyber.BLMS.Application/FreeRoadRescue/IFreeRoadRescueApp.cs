using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IFreeRoadRescueApp
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>执行结果</returns>
        bool AddFreeRoadRescue(CS_FreeRoadRescue entity);

        /// <summary>
        /// 标记为已处理
        /// </summary>
        /// <returns>执行结果</returns>
        bool FinishFreeRoadRescue(int id);

        /// <summary>
        /// 根据条件查询请求列表
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="status">状态</param>
        /// <returns>结果列表</returns>
        IEnumerable<CS_FreeRoadRescue> SelectFreeRoadRescueList(string start, string end, string phoneNumber, string status);
    }
}
