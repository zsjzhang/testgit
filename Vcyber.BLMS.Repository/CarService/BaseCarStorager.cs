using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Repository.CarService
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository.CarService;
    using Vcyber.BLMS.Repository.Entity.Generated;

    public class BaseCarStorager : IBaseCarStorager
    {
        public int Add(CSBaseCar car)
        {
            return (int)PocoHelper.CurrentDb().Insert(car);
        }

        public int Delete(int id)
        {
           return PocoHelper.CurrentDb().Delete<CSBaseCar>(id);
        }

        public IEnumerable<CSBaseCar> QueryCars(ECarSeriesType type)
        {
            return PocoHelper.CurrentDb().Query<CSBaseCar>("where [Type]=@0 order by SeriesId", (int)type);
        }

        public int Update(CSBaseCar car)
        {
            return PocoHelper.CurrentDb().Update(car);
        }

        public IEnumerable<string> GetNamelist()
        {
            return PocoHelper.CurrentDb().Query<string>(" select distinct SeriesName from CS_BaseCar ");
        }

        public CSBaseCar GetById(int id)
        {
            return PocoHelper.CurrentDb().FirstOrDefault<CSBaseCar>("where [id]=@0", id);
        }

        public CSTestDrive GetByidTryCustList(int UserId)
        {
            return PocoHelper.CurrentDb().FirstOrDefault<CSTestDrive>("where [UserId]=@0", UserId);
        }
    }
}
