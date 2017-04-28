using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.IRepository.CarService
{
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;

    public interface IBaseCarStorager
    {
        int Add(CSBaseCar car);

        int Delete(int id);

        IEnumerable<CSBaseCar> QueryCars(ECarSeriesType type);

        int Update(CSBaseCar car);

        IEnumerable<string> GetNamelist();

        CSBaseCar GetById(int id);

    }
}
