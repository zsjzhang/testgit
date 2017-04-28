using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository.Weixin
{
    public interface ICustomerServiceMessageStorager
    {
        /// <summary>
        /// 添加客户消息
        /// </summary>
        /// <param name="obj">客服消息</param>
        /// <returns>Id</returns>
        int Add(CustomerServiceMessage obj);
        /// <summary>
        /// 按时间查询并且按客服分组
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>汇总记录</returns>
        IEnumerable<CustomerServiceRecord> GroupByWorker(string beginTime, string endTime);
        /// <summary>
        /// 按时间汇总
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>汇总记录</returns>
        DataTable Total(string beginTime, string endTime);
        /// <summary>
        /// 生成统计汇总
        /// </summary>
        /// <param name="currDate">记录的时间</param>
        void AddRecord(string currDate = "");

        /// <summary>
        /// 微信多客服绩效统计
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        DataTable PerformanceStatisticsList(string beginTime, string endTime);
    }
}
