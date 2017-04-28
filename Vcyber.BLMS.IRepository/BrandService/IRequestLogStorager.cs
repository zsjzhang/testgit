using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IRequestLogStorager
    {
        int Add(IF_RequestLog entity);
        IEnumerable<IF_RequestLog> SelectRequestList();
    }
}
