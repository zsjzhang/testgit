using System;
using Omu.ValueInjecter;
using PetaPoco;

using Vcyber.BLMS.Application.CarService;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Domain.Common;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain.CarService
{

    public class ScheduleServiceApp : IScheduleService
    {
        public Page<ScheduleEntity> QueryOrders(string userId, long page, long itemsPerPage)
        {
            return _DbSession.ScheduleServiceStorager.QueryOrders(userId, page, itemsPerPage);
        }

        public Page<CSSonataServiceV> QueryUserOrdersByType(string userId, long page, long itemsPerPage)
        {
            return _DbSession.ScheduleServiceStorager.QueryUserOrdersByType(userId, page, itemsPerPage);
        }
    }
}
