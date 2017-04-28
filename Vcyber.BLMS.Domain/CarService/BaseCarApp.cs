using System;
namespace Vcyber.BLMS.Domain.CarService
{
    using System.Collections.Generic;

    using Omu.ValueInjecter;

    using PetaPoco;

    using Vcyber.BLMS.Application.CarService;
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Domain.Common;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository;

    public class BaseCarApp : IBaseCar
    {
        public int Add(CSBaseCar car)
        {
            return _DbSession.BaseCarStorager.Add(car);
        }

        public int Delete(int id)
        {
            return _DbSession.BaseCarStorager.Delete(id);

        }

        public IEnumerable<CSBaseCar> QueryCars(ECarSeriesType type)
        {
            return _DbSession.BaseCarStorager.QueryCars(type);
        }

        public int Update(CSBaseCar car)
        {
            return _DbSession.BaseCarStorager.Update(car);
        }

        public IEnumerable<string> GetNamelist()
        {
            return _DbSession.BaseCarStorager.GetNamelist();
        }


        public CSBaseCar GetById(int id)
        {
            return _DbSession.BaseCarStorager.GetById(id);
        }
    }
}
