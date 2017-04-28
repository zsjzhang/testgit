using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Application.CarService
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Generated;

    public interface IScheduleService
    {
        Page<ScheduleEntity> QueryOrders(string userId, long page, long itemsPerPage);

        Page<CSSonataServiceV> QueryUserOrdersByType(string userId, long page, long itemsPerPage);
    }
}
