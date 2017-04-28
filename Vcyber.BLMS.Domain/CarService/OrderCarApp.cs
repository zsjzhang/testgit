using System;
namespace Vcyber.BLMS.Domain.CarService
{
    using System.Collections.Generic;
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

    public class OrderCarApp : IOrderCar
    {
        public int Add(OrderCarEntity entity)
        {
            CSOrderCar csEntity = new CSOrderCar();
            csEntity.InjectFrom(entity);
            csEntity.UpdateTime = DateTime.Now;
            csEntity.OrderNo = IdGenerator.GetId(SequenceCategory.DC);
            csEntity.CreateTime = DateTime.Now;
            csEntity.State = (int)EOrderState.ToBeProcess;
            return _DbSession.OrderCarStorager.Add(csEntity);
        }

        public int Add(OrderCarEntity entity, string updateId, string updateName)
        {
            return Add(entity);
        }

        public int Add(CSOrderCar entity, string updateId, string updateName)
        {
            entity.UpdateTime = DateTime.Now;
            entity.UpdateId = updateId;
            entity.UpdateName = updateName;
            entity.OrderNo = IdGenerator.GetId(SequenceCategory.DC);
            entity.CreateTime = DateTime.Now;
            entity.State = (int)EOrderState.ToBeProcess;
            return _DbSession.OrderCarStorager.Add(entity);
        }

        public Page<CSOrderCar> QueryOrdersByUserId(string userId, long page, long itemsPerPage, string dealerId = null)
        {
            QueryParamEntity entity = new QueryParamEntity { UserId = userId, DealerId = dealerId};
            return this.QueryOrders(entity, page, itemsPerPage);
        }

        public Page<CSOrderCar> QueryOrders(QueryParamEntity entity, long page, long itemsPerPage)
        {
            return _DbSession.OrderCarStorager.GetPage(entity, page, itemsPerPage);
        }

        public IEnumerable<CSOrderCar> QueryOrdersWithUpdate(QueryParamEntity entity)
        {
            return _DbSession.OrderCarStorager.QueryOrdersWithUpdate(entity);
        }

        public int UpdateState(int id, EOrderState state, string updateId, string updateName)
        {
            return _DbSession.OrderCarStorager.UpdateState(id, state, updateId, updateName);
        }


        public CSOrderCar GetEntityById(int id)
        {
            return _DbSession.OrderCarStorager.GetEntityById(id);
        }

        /// <summary>
        /// 更新导出状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateIsExported(int id, string updateId, string updateName)
        {
            return _DbSession.OrderCarStorager.UpdateIsExported(id, updateId, updateName);
        }
    }
}
