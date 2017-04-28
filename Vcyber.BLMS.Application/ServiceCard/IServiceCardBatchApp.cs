﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IServiceCardBatchApp
    {
        /// <summary>
        /// 生成卡劵批次，并批量生成卡劵
        /// </summary>
        /// <param name="batch">批次信息</param>
        /// <returns>返回执行结果及出错信息</returns>
        ReturnResult CreateServiceCardBatch(ServiceCardBatch batch);

        /// <summary>
        /// 是存在相同的卡卷名称
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        bool IsExist(string batchName);

        /// <summary>
        /// 查询批次信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<ServiceCardBatch> SelectServiceCardBatchList();

        /// <summary>
        /// 服务卡使用情况统计
        /// </summary>
        /// <param name="condition">统计条件</param>
        /// <returns></returns>
        ServiceCarUseStatistics serviceCardStatistics(Condition condition);

        /// <summary>
        /// 查询服务卡批次信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageData">分页信息</param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<ServiceCardBatch> findCondition(Condition condition, PageData pageData, out int totalCount);

        #region ==== 服务卡类型 ====

        /// <summary>
        /// 获取服务卡类型
        /// </summary>
        /// <returns></returns>
        IEnumerable<ServiceCardType> CardTypeAll();

        /// <summary>
        /// 根据服务卡类型Code,查找服务卡类型
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        ServiceCardType CardTypeOne(string typeCode);

        #endregion
    }
}
