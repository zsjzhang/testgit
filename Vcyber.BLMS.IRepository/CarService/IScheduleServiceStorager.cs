namespace Vcyber.BLMS.IRepository.CarService
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Generated;

    public interface IScheduleServiceStorager
    {
        Page<ScheduleEntity> QueryOrders(string userId, long page, long itemsPerPage);

        Page<CSSonataServiceV> QueryUserOrdersByType(string userId, long page, long itemsPerPage);
    }
}