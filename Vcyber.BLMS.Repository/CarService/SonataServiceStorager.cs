using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Repository.CarService
{
    using System;

    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository.CarService;
    using Vcyber.BLMS.Repository.Entity.Generated;

    public class SonataServiceStorager : ISonataServiceStorager
    {
        public int Add(CSSonataService entity)
        {
            return (int)PocoHelper.CurrentDb().Insert(entity);
        }

        public Page<CSSonataService> GetPage(QueryParamEntity entity, long page, long itemsPerPage)
        {
            Sql sql = CSSqlBuilder.BuildSql(entity);
            return PocoHelper.CurrentDb().Page<CSSonataService>(page, itemsPerPage, sql);
        }

        public IEnumerable<CSSonataService> QueryOrdersWithUpdate(QueryParamEntity entity)
        {
            throw new NotImplementedException();
        }

        public int UpdateState(int id, EOrderState state, string updateId, string updateName)
        {
            return PocoHelper.CurrentDb().Update<CSSonataService>("set State=@0,UpdateId=@1, UpdateName=@2, UpdateTime=@3 where Id=@4", state, updateId, updateName, DateTime.Now, id);
        }
        public CSSonataService GetEntityById(int id)
        {
            return PocoHelper.CurrentDb().FirstOrDefault<CSSonataService>("where id=@0", id);
        }

        public bool UpdateIsExported(int id, string updateId, string updateName)
        {
            return PocoHelper.CurrentDb().Update<CSSonataService>("set IsExported = 1, UpdateId=@0, UpdateName=@1, UpdateTime=@2 where Id = @3", updateId, updateName, DateTime.Now, id) > 0;
        }


        public DateTime GetSonataBuyTime(string identitynumber)
        {
            string sql = string.Format("SELECT TOP 1 buytime FROM IF_Car WHERE custid IN (SELECT custid FROM IF_Customer WHERE identitynumber='{0}')  ORDER BY buytime DESC;", identitynumber);
            DateTime dTime=PocoHelper.CurrentDb().ExecuteScalar<DateTime>(sql);
            return dTime;
        }

        //public CSMaintenance GetMaintenanceyById(int UserId)
        //{
        //    return PocoHelper.CurrentDb().FirstOrDefault<CSMaintenance>("where UserId=@0", UserId);
        //}
    }
}
