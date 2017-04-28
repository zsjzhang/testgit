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
    public class SysConsumePointStorager : ISysConsumePointStorager
    {
        public IEnumerable<SysConsumePoint> SelectSysConsumePointList()
        {
            string sql = "SELECT * FROM SysConsumePoint";

            return DbHelp.Query<SysConsumePoint>(sql);
        }
    }
}
