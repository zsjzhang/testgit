using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Application.CarService
{
    using System.Security.Principal;

    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Entity;

    public interface IConsume
    {
        int Add(ConsumeEntity entity, string userId, string userName);

        //int Add(T2 entity);
        //Page<CSConsume> QueryOrdersByUserId(string userId, long page, long itemsPerPage, string dealerId=null);
        Page<CSConsume> QueryOrders(ConsumeQueryParamEntity entity, long page, long itemsPerPage);

        IEnumerable<CSConsume> GetUserConsume(string userId, int pageIndex, int pageSize, out int total);

        int UpdateStatus(int id, EPointApproveStatus status, string userId, string userName);

        int UpdatePointStatus(int id, EPointStatus status, string userId, string userName);

        CSConsume GetById(int id);

        int UpdatePaperOrder(int id, string paperOrder);

        int UpdateConsume(CSConsume entity);

        void ReleasePoints();
        /// <summary>
        /// 批量审核，并发放积分
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        int BatchUpdateStatus(string ids, EPointApproveStatus status, string userId, string userName);

        /// <summary>
        /// 直接发放积分和短信
        /// </summary>
        /// <param name="id"></param>
        void AddAndProcess(string userId, int id);

        void Settlement(string dealerId, string startDate, string endDate, int SettlementState, string consumeType);


        /// <summary>
        /// 蓝缤会员消费积分明细
        /// </summary>
        /// <param name="userId">会员ID</param>
        /// <param name="bgDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>蓝缤会员消费积分明细</returns>
        IEnumerable<CSConsume> GetUserConsumeList(string userId, DateTime bgDate, DateTime endDate);

        /// <summary>
        /// 获取蓝缤会员积分使用（消费/新增）明细
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="bgDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        IEnumerable<ResUserintegral> GetUserIntegraldetail(string userId, DateTime bgDate, DateTime endDate);

        /// <summary>
        /// 根据经销商名称获取经销商ID
        /// </summary>
        /// <param name="DealerName"></param>
        /// <returns></returns>
        string GetDealerIdByname(string DealerName);

        /// <summary>
        /// 查找一键入会返积分/经销商入会返积分的经销商ID
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        string GetDeaByUId(string UserId);

        /// <summary>
        /// 查找缴费返积分的经销商ID
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        string GetDIdByUId(string UserId);

        /// <summary>
        /// 根据消费工单ID查找NO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetOrderNoById(int id);

        /// <summary>
        /// 根据DMS的消费单号查找工单信息
        /// </summary>
        /// <param name="DMSOrderNo"></param>
        /// <returns></returns>
        Consuem GetUserConsumeByDmsNo(string DMSOrderNo);

        /// <summary>
        /// DMS消费工单，只执行返还积分的操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        void AddAndProcessReward(string userId, int id);
        
    }
}
