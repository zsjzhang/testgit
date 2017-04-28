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

    public class TestDriveStorager : ITestDriveStorager
    {
        public int Add(CSTestDrive entity)
        {
            return (int)PocoHelper.CurrentDb().Insert(entity);
        }

        public Page<CSTestDrive> GetPage(QueryParamEntity entity, long page, long itemsPerPage)
        {
            Sql sql = CSSqlBuilder.BuildSql(entity,false);
            return PocoHelper.CurrentDb().Page<CSTestDrive>(page, itemsPerPage, sql);
        }

        public IEnumerable<CSTestDrive> QueryOrdersWithUpdate(QueryParamEntity entity)
        {
            throw new NotImplementedException();
        }

        public int UpdateState(int id, EOrderState state, string updateId, string updateName)
        {
            return PocoHelper.CurrentDb().Update<CSTestDrive>("set State=@0,UpdateId=@1, UpdateName=@2, UpdateTime=@3 where Id=@4", state, updateId, updateName, DateTime.Now, id);
        }

      

        public bool UpdateIsExported(int id, string updateId, string updateName)
        {
            return PocoHelper.CurrentDb().Update<CSTestDrive>("set IsExported = 1, State=1,UpdateId=@0, UpdateName=@1, UpdateTime=@2  where Id = @3", updateId, updateName, DateTime.Now, id) > 0;
        }
        public DateTime GetSonataBuyTime(string identitynumber)
        {
            string sql = string.Format("SELECT TOP 1 buytime FROM IF_Car WHERE custid IN (SELECT custid FROM IF_Customer WHERE identitynumber='{0}') AND  (CarCategory = '第九代索纳塔' or CarCategory = '索纳塔9' or CarCategory = '全新途胜') ORDER BY buytime DESC;", identitynumber);
            DateTime dTime = PocoHelper.CurrentDb().ExecuteScalar<DateTime>(sql);
            return dTime;
        }

        public CSTestDrive GetEntityById(int id)
        {
            return PocoHelper.CurrentDb().FirstOrDefault<CSTestDrive>("where id=@0", id);
        }      
    }
}
