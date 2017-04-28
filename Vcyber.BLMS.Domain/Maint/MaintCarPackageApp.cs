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
    public class MaintCarPackageApp : IMaintCarPackageApp
    {
        public IEnumerable<MaintCarPackage> GetMaintCarPackageList(string carType, string KM)
        {
            return _DbSession.MaintCarPackageStorager.GetMaintCarPackageList(carType, KM);
        }
    }
}
