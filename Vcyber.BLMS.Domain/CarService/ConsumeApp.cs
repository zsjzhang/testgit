using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Domain.CarService
{
    using System.Web;

    using Omu.ValueInjecter;

    using PetaPoco;

    using Vcyber.BLMS.Application.CarService;
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Domain.Common;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository;
    using Entity;

    public class ConsumeApp : IConsume
    {
        public int Add(ConsumeEntity entity, string userId, string userName)
        {
            CSConsume consume = new CSConsume();
            consume.InjectFrom(entity);
            consume.ConsumeType = entity.ConsumeType;
            consume.CreateTime = DateTime.Now;
            consume.UpdateTime = DateTime.Now;
            consume.CreateId = userId;
            consume.UpdateId = userId;
            consume.CreateName = userName;
            consume.UpdateName = userName;
            consume.ApproveStatus = (int)EPointApproveStatus.AutoApproved;
            consume.PointStatus = (int)EPointStatus.ToDo;
            consume.OrderNo = IdGenerator.GetId(SequenceCategory.XF);
            consume.DMSOrderNo = entity.DMSOrderNo;
            return _DbSession.ConsumeStorager.Add(consume);
        }

        public Page<CSConsume> QueryOrders(ConsumeQueryParamEntity entity, long page, long itemsPerPage)
        {
            return _DbSession.ConsumeStorager.QueryOrders(entity, page, itemsPerPage);
        }

        public int UpdateStatus(int id, EPointApproveStatus status, string updateId, string updateName)
        {
            return _DbSession.ConsumeStorager.UpdateStatus(id, status, updateId, updateName);
        }

        public int UpdatePointStatus(int id, EPointStatus status, string updateId, string updateName)
        {
            return _DbSession.ConsumeStorager.UpdatePointStatus(id, status, updateId, updateName);
        }

        public IEnumerable<CSConsume> GetUserConsume(string userId, int pageIndex, int pageSize, out int total)
        {
            return _DbSession.ConsumeStorager.GetUserConsume(userId, pageIndex, pageSize, out total);
        }

        public CSConsume GetById(int id)
        {
            return _DbSession.ConsumeStorager.GetById(id);
        }

        public int UpdatePaperOrder(int id, string paperOrder)
        {
            return _DbSession.ConsumeStorager.UpdatePaperOrder(id, paperOrder);
        }

        public int UpdateConsume(CSConsume entity)
        {
            return _DbSession.ConsumeStorager.UpdateConsume(entity);
        }

        public void ReleasePoints()
        {
            throw new NotImplementedException();
        }

        public int BatchUpdateStatus(string ids, EPointApproveStatus status, string userId, string userName)
        {
            return _DbSession.ConsumeStorager.BatchUpdateStatus(ids, status, userId, userName);
        }

        public void AddAndProcess(string userId, int id)
        {
            //数据操作
            _DbSession.ConsumeStorager.AddAndProcess(userId, id);
        }

        public void Settlement(string dealerId, string startDate, string endDate, int SettlementState, string consumeType)
        {
            _DbSession.ConsumeStorager.Settlement(dealerId, startDate, endDate, SettlementState,consumeType);
        }
       

        public IEnumerable<CSConsume> GetUserConsumeList(string userId, DateTime bgDate, DateTime endDate)
        {
            return _DbSession.ConsumeStorager.GetUserConsumeList(userId, bgDate, endDate);
        }

        /// <summary>
        /// 获取蓝缤会员积分使用（消费/新增）明细
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="bgDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public IEnumerable<ResUserintegral> GetUserIntegraldetail(string userId, DateTime bgDate, DateTime endDate)
        {
            return _DbSession.ConsumeStorager.GetUserIntegraldetail(userId, bgDate, endDate);
        }

        /// <summary>
        /// 根据经销商名称获取经销商ID
        /// </summary>
        /// <param name="DealerName"></param>
        /// <returns></returns>
        public string GetDealerIdByname(string DealerName)
        {
            return _DbSession.ConsumeStorager.GetDealerIdByname(DealerName);
        }

        /// <summary>
        /// 查找一键入会返积分/经销商入会返积分的经销商ID
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public string GetDeaByUId(string UserId)
        {
            return _DbSession.ConsumeStorager.GetDeaByUId(UserId);
        }

        /// <summary>
        /// 查找缴费返积分的经销商ID
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string GetDIdByUId(string UserId)
        {
            return _DbSession.ConsumeStorager.GetDIdByUId(UserId);
        }

        /// <summary>
        /// 根据消费工单ID查找NO
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        public string GetOrderNoById(int id)
        {
            //数据操作
           return _DbSession.ConsumeStorager.GetOrderNoById(id);
        }
        
        /// <summary>
        /// 根据DMS的消费单号查找工单信息
        /// </summary>
        /// <param name="DMSOrderNo"></param>
        /// <returns></returns>
        public Consuem GetUserConsumeByDmsNo(string DMSOrderNo)
        {
            return _DbSession.ConsumeStorager.GetUserConsumeByDmsNo(DMSOrderNo);
        }

        /// <summary>
        /// DMS消费工单，只执行返还积分的操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        public void AddAndProcessReward(string userId, int id)
        {
            //数据操作
            _DbSession.ConsumeStorager.AddAndProcessReward(userId, id);
        }
    }
}
