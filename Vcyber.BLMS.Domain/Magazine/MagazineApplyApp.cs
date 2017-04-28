using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 资料申请业务
    /// </summary>
    public class MagazineApplyApp : IMagazineApplyApp
    {
        #region ==== 构造函数 ====

        public MagazineApplyApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data">申请表数据</param>
        public void Add(MagazineApply data, out string id)
        {
            _DbSession.MagazineApplyStorager.Add(data, out id);
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id">申请表Id</param>
        /// <returns></returns>
        public bool Success(string id){
            return _DbSession.MagazineApplyStorager.UpdateStatus(id,EMApplyStatus.SHTG);
        }

        /// <summary>
        /// 发送资料
        /// </summary>
        /// <param name="id">申请表Id</param>
        /// <param name="expressNumber">快递单号</param>
        /// <returns></returns>
        public bool SendMagazine(string id, string expressNumber)
        {
            return _DbSession.MagazineApplyStorager.SendMagazine(id, expressNumber);
        }

        /// <summary>
        /// 获取单个申请表信息
        /// </summary>
        /// <param name="id">申请表Id</param>
        /// <returns></returns>
        public MagazineApply GetOne(string id)
        {
            return _DbSession.MagazineApplyStorager.SelectOne(id);
        }

        /// <summary>
        /// 分页获取申请信息
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="totalCount">数据总个数</param>
        /// <returns></returns>
        public IEnumerable<MagazineApply> GetList(PageData pageData, out int totalCount)
        {
            return _DbSession.MagazineApplyStorager.SelectList(pageData, out totalCount);
        }

        #endregion

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
        public IEnumerable<MagazineApply> GetMagazineApplyList(int? status, int? year, int? month, int? day, string title, string phoneNum, int start, int count, out int total)
        {
            try
            {
                return _DbSession.MagazineApplyStorager.GetMagazineApplyListByFilter(status, year, month, day, title, phoneNum, start, count, out total);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 导出纸质杂志申请记录到Excel
        /// </summary>
        /// <param name="status">记录状态</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public IEnumerable<MagazineApply> ExportMagazineApplyList(int? status, int? year, int? month, int? day, string title)
        {
            try
            {
                return _DbSession.MagazineApplyStorager.ExportMagazineApplyListByFilter(status, year, month, day, title);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
