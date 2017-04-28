using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Repository
{
    public class RequestLogStorager : IRequestLogStorager
    {
        public int Add(IF_RequestLog entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into IF_RequestLog(UserId,RequestData,ResponseData)values(@UserId,@RequestData,@ResponseData)");
            return DbHelp.Execute(sql.ToString(), entity);
        }

        public IEnumerable<IF_RequestLog> SelectRequestList()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from IF_RequestLog");
            return DbHelp.Query<IF_RequestLog>(sql.ToString());
        }
    }
}
