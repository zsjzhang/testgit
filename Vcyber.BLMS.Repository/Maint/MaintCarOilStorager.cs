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
    public class MaintCarOilStorager : IMaintCarOilStorager
    {
        public IEnumerable<MaintCarOil> GetMaintCarOilList(string carType)
        {
            string sql = "select * from dbo.MaintCarOil where cartype like @carType";

            return DbHelp.Query<MaintCarOil>(sql, new { @carType = "%" + carType + "%" });
        }
    }
}
