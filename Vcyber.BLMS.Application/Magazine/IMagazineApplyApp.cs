﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 资料申请业务
    /// </summary>
    public interface IMagazineApplyApp
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data">申请表数据</param>
        void Add(MagazineApply data, out string id);

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id">申请表Id</param>
        /// <returns></returns>
        bool Success(string id);

        /// <summary>
        /// 发送资料
        /// </summary>
        /// <param name="id">申请表Id</param>
        /// <param name="expressNumber">快递单号</param>
        /// <returns></returns>
        bool SendMagazine(string id, string expressNumber);

        /// <summary>
        /// 获取单个申请表信息
        /// </summary>
        /// <param name="id">申请表Id</param>
        /// <returns></returns>
        MagazineApply GetOne(string id);

        /// <summary>
        /// 分页获取申请信息
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="totalCount">数据总个数</param>
        /// <returns></returns>
        IEnumerable<MagazineApply> GetList(PageData pageData, out int totalCount);

        /// <summary>
        /// 获取纸质杂志申请记录
        /// </summary>
        /// <param name="status">记录状态</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="title">标题</param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<MagazineApply> GetMagazineApplyList(int? status, int? year, int? month, int? day, string title, string phoneNum, int start, int count, out int total);

        /// <summary>
        /// 导出纸质杂志申请记录到Excel
        /// </summary>
        /// <param name="status">记录状态</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="title">标题</param>
        /// <returns></returns>
        IEnumerable<MagazineApply> ExportMagazineApplyList(int? status, int? year, int? month, int? day, string title);

        #endregion
    }
}