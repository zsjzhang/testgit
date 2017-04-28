using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class OweAggregationsApp : IOweAggregationsApp
    {
        public void Add(OweAggregations obj)
        {
            _DbSession.OweAggregationsStorager.Add(obj);
        }
    }
}
