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
    public class XDOrderChangeApp : IXDOrderChangeApp
    {
        public XDOrderChangeApp() { }
        public bool IsCanOrderChange(int activityId, string mobile, int orderChangeType)
        {
            return _DbSession.XDOrderChangeStorager.IsCanOrderChange(activityId, mobile, orderChangeType);
        }

        public bool AddOrderChange(XDOrderChange xDOrderChange)
        {
            return _DbSession.XDOrderChangeStorager.AddOrderChange(xDOrderChange);
        }

        public XDOrderChange GetOrderChangeByMobile(int activityId, string mobile)
        {
            return _DbSession.XDOrderChangeStorager.GetOrderChangeByMobile(activityId, mobile);
        }


        public bool IsCanOrderChange(int activityId, string mobile)
        {
            return _DbSession.XDOrderChangeStorager.IsCanOrderChange(activityId, mobile);
        }
        public bool IsCanOrderChange(string carSeriers, string mobile, string shopCode)
        {
            return _DbSession.XDOrderChangeStorager.IsCanOrderChange(carSeriers, mobile, shopCode);
        }
    }
}
