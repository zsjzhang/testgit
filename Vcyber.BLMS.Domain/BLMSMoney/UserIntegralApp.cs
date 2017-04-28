using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.BLMSMoney;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 用户积分业务
    /// </summary>
    public class UserIntegralApp : IUserIntegralApp
    {
        public int AddVin(List<Car> Vins, string identityNumber, string userid)
        {
            return _DbSession.UserIntegralStorager.AddVin(Vins, identityNumber, userid);

            // return _DbSession.UserIntegralStorager.AddVin(Vins, identityNumber, userid);
        }
        #region ==== 构造函数 =====

        public UserIntegralApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加用户积分
        /// </summary>
        /// <param name="data"></param>
        public void Add(UserIntegral data)
        {
            _DbSession.UserIntegralStorager.Add(data);
        }

        public void AddUserIntegralRecord(UserIntegral data)
        {
            _DbSession.UserIntegralStorager.AddUserIntegralRecord(data);
        }

        /// <summary>
        /// 添加用户积分
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ruleType">积分获取规则类型</param>
        public void Add(UserIntegral data, EIRuleType ruleType)
        {
            data.integralSource = ruleType.ToInt32().ToString();
            _DbSession.UserIntegralStorager.Add(data);
        }

        /// <summary>
        /// 获取用户总积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetTotalIntegral(string userId)
        {
            return _DbSession.UserIntegralStorager.GetTotalIntegral(userId);
        }

        /// <summary>
        /// 获取用户积分（总，使用，剩余）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<IntegraInfo> GetIntegralIn(string userId)
        {
            return _DbSession.UserIntegralStorager.GetIntegralIn(userId);
        }

        /// <summary>
        /// 获取用户全部积分记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<UserIntegral> GetAll(string userId)
        {
            return _DbSession.UserIntegralStorager.SelectAll(userId);
        }

        /// <summary>
        /// 分页获取用户积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<UserIntegral> GetAll(string userId, PageData pageData, out int total)
        {
            return _DbSession.UserIntegralStorager.SelectAll(userId, pageData, out total);
        }

        /// <summary>
        /// 根据用户UserId获取消费积分明细分页（旧方法）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        //public IEnumerable<UserIntegral> SelectUserIntegral(string userId, int pageIndex, int pageSize, out int total)
        //{

        //    return _DbSession.UserIntegralStorager.SelectUserIntegral(userId, pageIndex, pageSize, out total);
        //}

        public IEnumerable<UserIntegral> SelectIntegralRecordPage(string userId, int pageIndex, int pageSize,
            out int total)
        {

            return _DbSession.UserIntegralStorager.SelectIntegralRecordPage(userId, pageIndex, pageSize, out total);
        }
        /// <summary>
        /// 获取用户有效积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<UserIntegral> GetList(string userId)
        {
            return _DbSession.UserIntegralStorager.SelectList(userId);
        }

        /// <summary>
        /// 删除用户积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteUserIntegral(string userId)
        {
            return _DbSession.UserIntegralStorager.DeleteUserIntegral(userId);
        }

        #endregion


        
        static bool IsUserBirthDay(string identitynumber)
        {
            Regex reg = new Regex(@"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
            if (!reg.IsMatch(identitynumber))
            {
                return false;
            }
            if (identitynumber.Length == 15)
            {
                return Convert.ToInt32(identitynumber.Substring(8, 2)) == DateTime.Now.Month;
            }
            if (identitynumber.Length == 18)
            {
                return Convert.ToInt32(identitynumber.Substring(10, 2)) == DateTime.Now.Month;
            }
            return true;
        }

        public double GetUserIntegralDiscount(string userId)
        {
            return _DbSession.UserIntegralStorager.GetUserIntegralDiscount(userId);
        }

        /// <summary>
        /// 蓝缤会员积分明细
        /// </summary>
        /// <param name="userId">会员ID</param>
        /// <param name="bgDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>蓝缤会员积分明细列表</returns>
        public IEnumerable<UserIntegral> GetUserIntegralList(string userId, DateTime bgDate, DateTime endDate)
        {
            return _DbSession.UserIntegralStorager.GetUserIntegralList(userId, bgDate, endDate);
        }

        //积分明细接口 
        public UserIntegralRecordDetail UserIntegralDetailByUserID(string userid)
        {
            return _DbSession.UserIntegralStorager.UserIntegralDetailByUserID(userid);
        }

        /// <summary>
        /// 根据userId获取该用户的总积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserIntegral GetUserintegralByUserId(string userId)
        {
            return _DbSession.UserIntegralStorager.GetUserintegralByUserId(userId);
        }

        /// <summary>
        /// 根据用户UserId获取消费积分明细分页
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<UserIntegralRecord> SelectUserIntegral(string userId, int pageIndex, int pageSize, out int total, out int pageSurplus)
        {
            return _DbSession.UserIntegralStorager.SelectUserIntegral(userId, pageIndex, pageSize, out total, out pageSurplus);
        }

        /// <summary>
        /// DMS修改消费工单
        /// </summary>
        /// <param name="data"></param>
        public void DmsUpdateOrder(Consuem data)
        {
            _DbSession.UserIntegralStorager.DmsUpdateOrder(data);
        }

        /// <summary>
        /// DMS取消消费工单
        /// </summary>
        /// <param name="data"></param>
        public void DmsCancelOrder(string OrderNo)
        {
            _DbSession.UserIntegralStorager.DmsCancelOrder(OrderNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderNo"></param>
        public bool SubIntegral(int id,string userid,int integral)
        {
           return _DbSession.UserIntegralStorager.SubIntegral(id,userid,integral);
        }
    }
}
