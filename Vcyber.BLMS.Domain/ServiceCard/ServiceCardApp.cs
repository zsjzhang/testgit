using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Domain
{
    public class ServiceCardApp : IServiceCardApp
    {
        /// <summary>
        /// 下发卡劵
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="userId">下发用户</param>
        /// <param name="qty">下发数量</param>
        /// <returns>返回执行结果及出错信息</returns>
        public ReturnResult SendServiceCard(string batchNo, string userId, int qty)
        {
            var result = new ReturnResult { IsSuccess = true };

            if (ServiceCardValidate.IsBatchNo(batchNo))
            {
                result.IsSuccess = false;
                result.Message = "服务卡批次已经过期。";
                return result;
            }

            //1.获取指定批次、指定数量的卡劵
            var cardList = _DbSession.ServiceCardStorager.GetServiceCard(batchNo, qty);

            if (cardList.Count() < qty)
            {
                result.IsSuccess = false;
                result.Message = "卡劵库存不足，下发失败";
                return result;
            }

            foreach (var item in cardList)
            {
                //2.修改卡劵状态
                bool updateResult = _DbSession.ServiceCardStorager.SendServiceCard(userId, item.CardNo);

                if (!updateResult)
                {
                    result.IsSuccess = false;
                    result.Message = "下发失败";
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// 使用卡劵
        /// </summary>
        /// <param name="record">卡劵使用信息</param>
        /// <returns>返回执行结果及出错信息</returns>
        public ReturnResult UseServiceCard(ServiceCardUsedRecord record)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            string message;

            if (!ServiceCardValidate.IsUseCard(record.CardNo,record.ConsumeType,out message))
            {
                result.Message = message;
                result.IsSuccess = false;
                return result;
            }

            //1.修改卡劵状态
            bool updateResult = _DbSession.ServiceCardStorager.UseServiceCard(record.CardNo);

            if (!updateResult)
            {
                result.IsSuccess = false;
                result.Message = "修改卡劵状态失败";
                return result;
            }

            //2.保存使用信息
            bool addResult = _DbSession.ServiceCardUsedRecordStorager.AddServiceCardUsedRecord(record);

            if (!addResult)
            {
                result.IsSuccess = false;
                result.Message = "保存使用信息失败";
                return result;
            }

            return result;
        }

        /// <summary>
        /// 查询所有服务卡劵
        /// </summary>
        /// <returns>卡劵列表</returns>
        public IEnumerable<ServiceCard> SelectServiceCardList()
        {
            return _DbSession.ServiceCardStorager.SelectServiceCardList();
        }

        /// <summary>
        /// 查询服务卡
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<ServiceCard> FindList(Condition condition, PageData pageData, out int totalCount)
        {
            return _DbSession.ServiceCardStorager.FindList(condition, pageData, out totalCount);
        }

        /// <summary>
        /// 查询服务卡信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="maxValue">查询最新服务卡的个数</param>
        /// <returns></returns>
        public IEnumerable<ServiceCard> FindList(Condition condition, int maxValue)
        {
            return _DbSession.ServiceCardStorager.FindList(condition, maxValue);
        }
    }
}
