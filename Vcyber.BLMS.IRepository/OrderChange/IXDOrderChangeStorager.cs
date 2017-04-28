using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IXDOrderChangeStorager
    {
        bool IsCanOrderChange(int activityId, string mobile, int orderChangeType);
        bool IsCanOrderChange(int activityId, string mobile);
        bool AddOrderChange(XDOrderChange xDOrderChange);
        XDOrderChange GetOrderChangeByMobile(int activityId,string mobile);
        bool IsCanOrderChange(string carSeriers, string mobile, string shopCode);
    }
}
