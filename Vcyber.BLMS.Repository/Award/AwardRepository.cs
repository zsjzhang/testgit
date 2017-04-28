using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Repository
{
    public class AwardRepository
    {
        public Award GetByAlias(string alias)
        {
            string sql = @"
select * from Award
where alias=@alias
";
            return DbHelp.QueryOne<Award>(sql, new { @alias = alias });
        }
    }
}
