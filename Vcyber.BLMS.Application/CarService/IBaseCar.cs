using System.Collections.Generic;

namespace Vcyber.BLMS.Application.CarService
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;

    public interface IBaseCar
    {
        int Add(CSBaseCar car);

        int Delete(int id);
        IEnumerable<CSBaseCar> QueryCars(ECarSeriesType type);

        int Update(CSBaseCar car);

        CSBaseCar GetById(int id);

        IEnumerable<string> GetNamelist();
    }
}
