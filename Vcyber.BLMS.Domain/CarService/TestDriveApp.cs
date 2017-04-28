using System;
using Omu.ValueInjecter;
using PetaPoco;

using Vcyber.BLMS.Application.CarService;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Domain.Common;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain.CarService
{
    using System.Collections.Generic;
    using System.Security.Principal;

    using Vcyber.BLMS.Application;

    public class TestDriveApp : ITestDrive
    {
        public int Add(TestDriveEntity entity)
        {
            CSTestDrive csEntity = new CSTestDrive();
            csEntity.InjectFrom(entity);
            csEntity.UpdateTime = DateTime.Now;
            csEntity.OrderNo = IdGenerator.GetId(SequenceCategory.SJ);
            csEntity.CreateTime = DateTime.Now;
            csEntity.State = (int)EOrderState.ToBeProcess;
            return _DbSession.TestDriveStorager.Add(csEntity);
        }

       
        public int Add(TestDriveEntity entity, string updateId, string updateName)
        {
            return Add(entity);
        }

        public int Add(CSTestDrive entity, string updateId, string updateName)
        {
            entity.UpdateTime = DateTime.Now;
            entity.UpdateId = updateId;
            entity.UpdateName = updateName;
            entity.OrderNo = IdGenerator.GetId(SequenceCategory.SJ);
            entity.CreateTime = DateTime.Now;
            entity.State = (int) EOrderState.ToBeProcess;

            return _DbSession.TestDriveStorager.Add(entity);
        }

        public Page<CSTestDrive> QueryOrdersByUserId(string userId, long page, long itemsPerPage, string dealerId = null)
        {
            QueryParamEntity entity = new QueryParamEntity { UserId = userId, DealerId = dealerId };
            return this.QueryOrders(entity, page, itemsPerPage);
        }

        public Page<CSTestDrive> QueryOrders(QueryParamEntity entity, long page, long itemsPerPage)
        {
            return _DbSession.TestDriveStorager.GetPage(entity, page, itemsPerPage);
        }

        public IEnumerable<CSTestDrive> QueryOrdersWithUpdate(QueryParamEntity entity)
        {
            return _DbSession.TestDriveStorager.QueryOrdersWithUpdate(entity);
        }

        public int UpdateState(int id, EOrderState state, string updateId, string updateName)
        {
            return _DbSession.TestDriveStorager.UpdateState(id, state,updateId, updateName);
        }

        public CSTestDrive GetEntityById(int id)
        {
            return _DbSession.TestDriveStorager.GetEntityById(id);
        }

        /// <summary>
        /// 更新导出状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateIsExported(int id, string updateId, string updateName)
        {
            return _DbSession.TestDriveStorager.UpdateIsExported(id, updateId, updateName);
        }
    }
}
