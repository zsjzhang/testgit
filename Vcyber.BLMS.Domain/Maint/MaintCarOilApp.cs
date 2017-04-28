using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Domain
{
    public class MaintCarOilApp : IMaintCarOilApp
    {
        public IEnumerable<MaintCarOil> GetMaintCarOilList(string carType)
        {
            return _DbSession.MaintCarOilStorager.GetMaintCarOilList(carType);
        }
    }
}
