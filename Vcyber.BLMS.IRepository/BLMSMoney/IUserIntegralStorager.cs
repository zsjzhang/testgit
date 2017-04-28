using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.BLMSMoney;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 用户积分操作
    /// </summary>
    public interface IUserIntegralStorager
    {
        #region ==== 公共方法 ====

        int AddVin(List<Car> Vins, string identityNumber, string userid);
        /// <summary>
        /// 添加用户积分
        /// </summary>
        /// <param name="data"></param>
        void Add(UserIntegral data);

        double GetUserIntegralDiscount(string userId);
        void AddUserIntegralRecord(UserIntegral data);

        IEnumerable<UserIntegral> SelectIntegralRecordPage(string userId, int pageIndex, int pageSize, out int total);
        /// <summary>
        /// 获取用户总积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetTotalIntegral(string userId);

        /// <summary>
        /// 获取用户总积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<IntegraInfo> GetIntegralIn(string userId);

        /// <summary>
        /// 获取用户全部积分记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<UserIntegral> SelectAll(string userId);

        /// <summary>
        /// 分页获取用户积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<UserIntegral> SelectAll(string userId, PageData pageData, out int total);


        /// <summary>
        /// 分页获取用户积分分页
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<UserIntegralRecord> SelectUserIntegral(string userId, int pageIndex, int pageSize, out int total, out int pageSurplus);

        /// <summary>
        /// 获取用户有效积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<UserIntegral> SelectList(string userId);

        /// <summary>
        /// 减去用户积分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool SubIntegral(int id, string userId, int subValue);

        /// <summary>
        /// 清理用户过期积分
        /// </summary>
        /// <param name="userId"></param>
        void CleanIntegral(string userId);

        /// <summary>
        /// 删除用户积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int DeleteUserIntegral(string userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="buyCarType"></param>
        /// <param name="paymentMoney"></param>
        /// <returns></returns>
        int GetUserIntegralBybuyCarType(string idNumber, BuyCarType buyCarType, int paymentMoney);


        /// <summary>
        /// 蓝缤会员积分明细
        /// </summary>
        /// <param name="userId">会员ID</param>
        /// <param name="bgDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>蓝缤会员积分明细列表</returns>
        IEnumerable<UserIntegral> GetUserIntegralList(string userId, DateTime bgDate, DateTime endDate);

        //积分明细接口 
        UserIntegralRecordDetail UserIntegralDetailByUserID(string userid);

        #endregion

        /// <summary>
        /// 根据userId获取该用户的总积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserIntegral GetUserintegralByUserId(string userId);

        /// <summary>
        /// DMS修改消费工单
        /// </summary>
        /// <param name="data"></param>
        void DmsUpdateOrder(Consuem data);

        /// <summary>
        /// DMS取消消费工单
        /// </summary>
        /// <param name="data"></param>
        void DmsCancelOrder(string OrderNo);
    }
}
