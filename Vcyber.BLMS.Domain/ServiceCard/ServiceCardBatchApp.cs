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
    public class ServiceCardBatchApp : IServiceCardBatchApp
    {
        /// <summary>
        /// 生成卡劵批次，并批量生成卡劵
        /// </summary>
        /// <param name="batch">批次信息</param>
        /// <returns>返回执行结果及出错信息</returns>
        public ReturnResult CreateServiceCardBatch(ServiceCardBatch batch)
        {
            var result = new ReturnResult { IsSuccess = true };

            //1.生成批次号
            string serviceBatchNo = _DbSession.ServiceCardBatchStorager.GetServiceCardBatchNo();
            batch.BatchNo = serviceBatchNo;
            batch.BatchTotalMoney = batch.BatchPrice * batch.BatchQty;

            //2.插入批次
            bool addResult = _DbSession.ServiceCardBatchStorager.AddServiceCardBatch(batch);

            if (!addResult)
            {
                result.IsSuccess = false;
                result.Message = "生成批次信息出错";
                return result;
            }

            //3.生成卡劵
            var card = new ServiceCard { BatchNo = batch.BatchNo, Status = 1, CreateTime = DateTime.Now };

            for (int i = 0; i < batch.BatchQty; i++)
            {
                //3.1.生成卡号
                card.CardNo = _DbSession.ServiceCardStorager.GetServiceCardNo();

                bool addCardResult = _DbSession.ServiceCardStorager.AddServiceCard(card);

                if (!addCardResult)
                {
                    result.IsSuccess = false;
                    result.Message = "生成卡劵信息出错";
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// 查询批次信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ServiceCardBatch> SelectServiceCardBatchList()
        {
            return _DbSession.ServiceCardBatchStorager.SelectServiceCardBatchList();
        }

        /// <summary>
        /// 是存在相同的卡卷名称
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public bool IsExist(string batchName)
        {
            return _DbSession.ServiceCardBatchStorager.IsExist(batchName);
        }

        /// <summary>
        /// 服务卡使用情况统计
        /// </summary>
        /// <param name="condition">统计条件</param>
        /// <returns></returns>
        public ServiceCarUseStatistics serviceCardStatistics(Condition condition)
        {
            return _DbSession.ServiceCardBatchStorager.serviceCardStatistics(condition);
        }

        /// <summary>
        /// 查询服务卡批次信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageData">分页信息</param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<ServiceCardBatch> findCondition(Condition condition, PageData pageData, out int totalCount)
        {
            return _DbSession.ServiceCardBatchStorager.findCondition(condition, pageData, out totalCount);
        }

        #region ==== 服务卡类型 ====

        /// <summary>
        /// 获取服务卡类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ServiceCardType> CardTypeAll()
        {
            return _DbSession.ServiceCardBatchStorager.CardTypeAll();
        }

        /// <summary>
        /// 根据服务卡类型Code,查找服务卡类型
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public ServiceCardType CardTypeOne(string typeCode)
        {
            return _DbSession.ServiceCardBatchStorager.CardTypeOne(typeCode);
        }

        #endregion
    }
}
