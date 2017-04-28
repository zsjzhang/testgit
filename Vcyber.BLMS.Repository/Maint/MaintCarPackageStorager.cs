using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class MaintCarPackageStorager : IMaintCarPackageStorager
    {
        public IEnumerable<MaintCarPackage> GetMaintCarPackageList(string carType, string KM)
        {
            string sql = "select * from dbo.MaintCarPackage where cartype like @carType and km = @KM";

            return DbHelp.Query<MaintCarPackage>(sql, new { @carType = "%" + carType + "%", @KM = KM });
        }
    }
}
