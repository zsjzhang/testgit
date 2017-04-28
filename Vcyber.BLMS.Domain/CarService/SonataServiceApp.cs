using System;
namespace Vcyber.BLMS.Domain.CarService
{
    using System.Collections.Generic;

    using Omu.ValueInjecter;

    using PetaPoco;

    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Application.CarService;
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Domain.Common;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository;

    public class SonataServiceApp : ISonataService
    {
        public int Add(SonataServiceEntity entity)
        {
            CSSonataService csEntity = new CSSonataService();
            csEntity.InjectFrom(entity);
            csEntity.OrderType = (int)entity.OrderType; //nullable
            csEntity.ScheduleDate = entity.ScheduleDate;//nullable
            //普通维保不属于索九服务
            csEntity.OrderNo = IdGenerator.GetId(entity.OrderType == EBMServiceType.CommonMaintain ? SequenceCategory.WB : SequenceCategory.SN);
            csEntity.CreateTime = DateTime.Now;
            csEntity.Status = (int)EOrderState.ToBeProcess;
            csEntity.MaintainType = (int)entity.ServiceType;

            return _DbSession.SonataServiceStorager.Add(csEntity);
        }

        public int Add(SonataServiceEntity entity, string updateId, string updateName)
        {
            return Add(entity);
        }

        public int Add(CSSonataService entity, string updateId, string updateName)
        {
            entity.UpdateTime = DateTime.Now;
            entity.UpdateId = updateId;
            entity.UpdateName = updateName;
            entity.OrderNo = IdGenerator.GetId(SequenceCategory.SN);
            entity.CreateTime = DateTime.Now;
            entity.State = (int)EOrderState.ToBeProcess;

            return _DbSession.SonataServiceStorager.Add(entity);
        }

        public Page<CSSonataService> QueryOrdersByUserId(string userId, EBMServiceType type, long page, long itemsPerPage, string dealerId = null)
        {
            QueryParamEntity entity = new QueryParamEntity { UserId = userId, OrderType = (EOrderType)type, DealerId = dealerId };
            return this.QueryOrders(entity, page, itemsPerPage);
        }

        public Page<CSSonataService> QueryOrders(QueryParamEntity entity, long page, long itemsPerPage)
        {
            return _DbSession.SonataServiceStorager.GetPage(entity, page, itemsPerPage);
        }

        public IEnumerable<CSSonataService> QueryOrdersWithUpdate(QueryParamEntity entity)
        {
            return _DbSession.SonataServiceStorager.QueryOrdersWithUpdate(entity);
        }

        public int UpdateState(int id, EOrderState state, string updateId, string updateName)
        {
            return _DbSession.SonataServiceStorager.UpdateState(id, state, updateId, updateName);
        }

        public CSSonataService GetEntityById(int id)
        {
            return _DbSession.SonataServiceStorager.GetEntityById(id);
        }

        public bool IsSonataUser(string identity)
        {
            return _DbSession.CarServiceUserStorager.IsSonataUser(identity);
        }

        public bool IsTlcUser(string identityNumber)
        {
            return _DbSession.CarServiceUserStorager.IsTlcUser(identityNumber);
        }

        public int GetServiceCount(string identity, EDMSServiceType type)
        {
            return _DbSession.CarServiceUserStorager.GetFreeServiceCount(identity, type);
        }

        public Dictionary<int, int> GetServiceCount(string identity)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            int count = 0;
            foreach (var e in Enum.GetValues(typeof(EDMSServiceType)))
            {
                int c = GetServiceCount(identity, (EDMSServiceType)e);
                result.Add((int)e, c);
                count += c;
            }
            result.Add(-1, count);
            return result;
        }

        /// <summary>
        /// 更新导出状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateIsExported(int id, string updateId, string updateName)
        {
            return _DbSession.SonataServiceStorager.UpdateIsExported(id,updateId, updateName);
        }


        public DateTime GetSonataBuyTime(string identitynumber)
        {
            return _DbSession.SonataServiceStorager.GetSonataBuyTime(identitynumber);
        }



        //public CSMaintenance GetMaintenanceyById(int UserId)
        //{
        //    return _DbSession.SonataServiceStorager.GetMaintenanceyById(UserId);
        //}
    }
}
