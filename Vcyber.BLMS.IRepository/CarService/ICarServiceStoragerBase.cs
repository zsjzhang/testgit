using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.IRepository.CarService
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;

    public interface ICarServiceStoragerBase<T>
    {
        int Add(T entity);

        Page<T> GetPage(QueryParamEntity entity, long page=1, long itemsPerPage=10);
        IEnumerable<T> QueryOrdersWithUpdate(QueryParamEntity entity);
        int UpdateState(int id, EOrderState state, string updateId, string updateName);

        T GetEntityById(int id);

        bool UpdateIsExported(int id, string updateId, string updateName);

        DateTime GetSonataBuyTime(string identitynumber);

      //  T GetMaintenanceyById(int UserId);


            
    }
}
