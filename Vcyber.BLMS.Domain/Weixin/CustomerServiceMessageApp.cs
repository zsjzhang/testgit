using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application.Weixin;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain.Weixin
{
    public class CustomerServiceMessageApp : ICustomerServiceMessageApp
    {
        /// <summary>
        /// 添加客户消息
        /// </summary>
        /// <param name="obj">客服消息</param>
        /// <returns>Id</returns>
        public int Add(CustomerServiceMessage obj)
        {
            return _DbSession.CustomerServiceMessageStorager.Add(obj);
        }
        /// <summary>
        /// 按时间查询并且按客服分组
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>汇总记录</returns>
        public IEnumerable<CustomerServiceRecord> GroupByWorker(string beginTime, string endTime)
        {
            if (string.IsNullOrEmpty(beginTime))
            {
                beginTime = DateTime.Now.Date.ToString();
            }
            else
            {
                beginTime = DateTime.Parse(beginTime).Date.ToString();
            }
            if (string.IsNullOrEmpty(endTime))
            {
                endTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString();
            }
            else
            {
                endTime = DateTime.Parse(endTime).Date.AddDays(1).AddSeconds(-1).ToString();
            }
            return _DbSession.CustomerServiceMessageStorager.GroupByWorker(beginTime, endTime);
        }
        /// <summary>
        /// 按时间汇总
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>汇总记录</returns>
        public DataTable Total(string beginTime, string endTime)
        {
            if (string.IsNullOrEmpty(beginTime))
            {
                beginTime = DateTime.Now.Date.ToString();
            }
            else
            {
                beginTime = DateTime.Parse(beginTime).Date.ToString();
            }
            if (string.IsNullOrEmpty(endTime))
            {
                endTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString();
            }
            else
            {
                endTime = DateTime.Parse(endTime).Date.AddDays(1).AddSeconds(-1).ToString();
            }
            return _DbSession.CustomerServiceMessageStorager.Total(beginTime, endTime);
        }
        /// <summary>
        /// 生成统计汇总
        /// </summary>
        /// <param name="currDate">记录的时间</param>
        public void AddRecord(string currDate = "") 
        {
            _DbSession.CustomerServiceMessageStorager.AddRecord(currDate);
        }

        /// <summary>
        /// 微信多客服绩效统计
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataTable PerformanceStatisticsList(string beginTime, string endTime)
        {
            if (string.IsNullOrEmpty(beginTime))
            {
                beginTime = DateTime.Now.Date.ToString();
            }
            else
            {
                beginTime = DateTime.Parse(beginTime).Date.ToString();
            }
            if (string.IsNullOrEmpty(endTime))
            {
                endTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString();
            }
            else
            {
                endTime = DateTime.Parse(endTime).Date.AddDays(1).AddSeconds(-1).ToString();
            }
            return _DbSession.CustomerServiceMessageStorager.PerformanceStatisticsList(beginTime, endTime);
        }
    }
}
