using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IServiceCardApp
    {
        /// <summary>
        /// 下发卡劵
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="userId">下发用户</param>
        /// <param name="qty">下发数量</param>
        /// <returns>返回执行结果及出错信息</returns>
        ReturnResult SendServiceCard(string batchNo, string userId, int qty);

        /// <summary>
        /// 使用卡劵
        /// </summary>
        /// <param name="record">卡劵使用信息</param>
        /// <returns>返回执行结果及出错信息</returns>
        ReturnResult UseServiceCard(ServiceCardUsedRecord record);

        /// <summary>
        /// 查询所有服务卡劵
        /// </summary>
        /// <returns>卡劵列表</returns>
        IEnumerable<ServiceCard> SelectServiceCardList();

        /// <summary>
        /// 查询服务卡
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<ServiceCard> FindList(Condition condition, PageData pageData, out int totalCount);

        /// <summary>
        /// 查询服务卡信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="maxValue">查询最新服务卡的个数</param>
        /// <returns></returns>
        IEnumerable<ServiceCard> FindList(Condition condition, int maxValue);
    }
}
