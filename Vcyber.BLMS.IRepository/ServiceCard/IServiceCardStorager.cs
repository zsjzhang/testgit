using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IServiceCardStorager
    {
        /// <summary>
        /// 获取卡劵信息
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
        /// 根据条件查询卡劵信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>卡劵列表</returns>
        IEnumerable<ServiceCard> SelectServiceCardList(ServiceCardSearchCondition condition);

        /// <summary>
        /// 添加卡劵
        /// </summary>
        /// <param name="card">卡劵信息</param>
        /// <returns>执行结果</returns>
        bool AddServiceCard(ServiceCard card);

        /// <summary>
        /// 获取卡劵NO
        /// </summary>
        /// <returns>最新卡劵NO</returns>
        string GetServiceCardNo();

        /// <summary>
        /// 下发卡劵
        /// </summary>
        /// <param name="userId">下发用户</param>
        /// <param name="cardNo">卡劵编号</param>
        /// <returns>执行结果</returns>
        bool SendServiceCard(string userId, string cardNo);

        /// <summary>
        /// 使用卡劵
        /// </summary>
        /// <param name="record">使用卡劵信息</param>
        /// <returns>执行结果</returns>
        bool UseServiceCard(string cardNo);

        /// <summary>
        /// 获取指定批次、指定数量的卡劵
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="qty">数量</param>
        /// <returns>卡劵列表</returns>
        IEnumerable<ServiceCard> GetServiceCard(string batchNo, int qty);

        /// <summary>
        /// 查询服务卡信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="maxValue">查询最新服务卡的个数</param>
        /// <returns></returns>
        IEnumerable<ServiceCard> FindList(Condition condition, int maxValue);

        /// <summary>
        /// 根据卡卷编号查询卡卷信息
        /// </summary>
        /// <param name="cardNo">卡卷编号</param>
        /// <returns></returns>
        ServiceCard FindOne(string cardNo);

    }
}
