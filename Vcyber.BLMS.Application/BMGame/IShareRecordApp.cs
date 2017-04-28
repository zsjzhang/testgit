﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IShareRecordApp
    {
        /// <summary>
        /// 根据编号获取活动的分享列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        List<ShareRecord> GetShareRecordsByActivity(int activityId);

        /// <summary>
        /// 根据编号分页获取活动的分享列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="page"></param>
        /// <param name="totalNum"></param>
        /// <returns></returns>
        List<ShareRecord> GetShareRecordsByActivity(int activityId, PageData page, out int totalNum);

        /// <summary>
        /// 添加分享
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddShareRecord(ShareRecord entity);

        /// <summary>
        /// 获取全部的分享信息
        /// </summary>
        /// <returns></returns>
        List<ShareRecord> GetShareRecordsAll();

        /// <summary>
        /// 分页获取活动的分享列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="totalNum"></param>
        /// <returns></returns>
        List<ShareRecord> GetShareRecordsAll(PageData page, out int totalNum);
    }
}
