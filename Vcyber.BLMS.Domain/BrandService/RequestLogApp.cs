using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Newtonsoft.Json;

namespace Vcyber.BLMS.Domain
{
    public class RequestLogApp : IRequestLogApp
    {

        public bool Add(IF_RequestLog entity)
        {
            return _DbSession.RequestLogStorager.Add(entity) > 0;
        }

        public IEnumerable<IF_RequestLog> SelectRequestList()
        {
            return _DbSession.RequestLogStorager.SelectRequestList();
        }
        
        public string ConverterToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
