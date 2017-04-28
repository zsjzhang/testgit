using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Repository.CarService
{
    using PetaPoco;

    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository.CarService;
    using Vcyber.BLMS.Repository.Entity.Generated;

    public class ScheduleServiceStorager : IScheduleServiceStorager
    {
        public Page<ScheduleEntity> QueryOrders(string userId, long page, long itemsPerPage)
        {
            Sql sql =
                new Sql(@"select ScheduleType, ScheduleDate, [State], UpdateTime, UpdateName from (
select	N'线上订车' as ScheduleType, ScheduleDate, [State], UpdateTime, UpdateName
from	CS_OrderCar
where	UserId = @0
union
select	N'预约试驾' as ScheduleType, ScheduleDate, [State], UpdateTime, UpdateName
from	CS_TestDrive
where	UserId = @0
union
select	case	OrderType 
		when	0 then N'上门关怀服务'
		when	1 then N'3年9次面见服务'
		when	2 then N'免费取送车服务'
		when	3 then N'一对一专享服务'
        when	4 then N'预约维保服务'
		end		 as ScheduleType
		, ScheduleDate, [State], UpdateTime, UpdateName
from	CS_SonataService
where	UserId = @0
) a order by UpdateTime desc
",
                    userId);
            return PocoHelper.CurrentDb().Page<ScheduleEntity>(page, itemsPerPage,sql );
        }

        public Page<CSSonataServiceV> QueryUserOrdersByType(string userId, long page, long itemsPerPage)
        {
            Sql sql =
                new Sql(@"select 
[Id]
,[OrderNo]"
//,case	OrderType 
//        when	0 then N'上门关怀服务'
//        when	1 then N'3年9次面见服务'
//        when	2 then N'免费取送车服务'
//        when	3 then N'一对一专享服务'
//        when	4 then N'预约维保服务'
//        when	5 then N'线上订车'
//        when	6 then N'预约试驾'
//end		 as [OrderType]

+ @",OrderType  ,UserId
,isnull([DealerId], '') as[DealerId]
,isnull([DealerName], '') as[DealerName]
,isnull([DealerCity], '') as[DealerCity]
,isnull([DealerProvince], '') as[DealerProvince]
,isnull([PurchaseYear], '') as[PurchaseYear]
,isnull([UserName], '') as[UserName]
,isnull([UserSex], '') as[UserSex]
,isnull([Phone], '') as[Phone]
,isnull([Email], '') as[Email]
,isnull([ContactTime], '') as[ContactTime]
,isnull([Comment], '') as[Comment]
,isnull([State], '') as[State]
,convert(varchar,[ScheduleDate],111) as ScheduleDate
,isnull([TakeAddress], '') as[TakeAddress]
,isnull([TakeLong], '') as[TakeLong]
,isnull([TakeLat], '') as[TakeLat]
,isnull([ReturnAddress], '') as[ReturnAddress]
,isnull([ReturnLong], '') as[ReturnLong]
,isnull([ReturnLat], '') as[ReturnLat]
,convert(varchar,[ReturnDate],111) as[ReturnDate]
,isnull([UpdateId], '') as[UpdateId]
,isnull([UpdateName], '') as[UpdateName]
,convert(varchar,[CreateTime],120) as CreateTime
,convert(varchar,[UpdateTime],120) as UpdateTime
,isnull([ConsultantId], '') as[ConsultantId]
,isnull([ConsultantName], '') as[ConsultantName]
,isnull([VIN], '') as[VIN]
,isnull([CarSeries], '') as[CarSeries]
,isnull([LicensePlate], '') as[LicensePlate]
,isnull([MileAge], '') as[MileAge]"+
//,case [Status]  when 0 then N'系统已受理'
//               when 1 then N'待特约店处理'
//               when 2 then N'服务记录已完成'
//               else N''
//end as Status
@",[Status]
,isnull([ServiceOrderNo], '') as[ServiceOrderNo]
,isnull([MaintainType], '') as[MaintainType]
,convert(varchar,[SyncTime],120) as SyncTime
,isnull([IsExported],0) as[IsExported]
,isnull([PurshaseTimeFrame], '') as[PurshaseTimeFrame]

from (

SELECT [Id]
,[OrderNo]
, "+(int)EOrderType.OrderCar+ @"  as [OrderType]
,[UserId]
,[DealerId]
,[DealerName]
,[DealerCity]
,[DealerProvince]
, '' as [PurchaseYear]
,[UserName]
,[UserSex]
,[Phone]
,[Email]
,[ContactTime]
,[Comment]
,[State]
,[ScheduleDate]
, '' as[TakeAddress]
, '' as[TakeLong]
, '' as[TakeLat]
, '' as[ReturnAddress]
, '' as[ReturnLong]
, '' as[ReturnLat]
, '' as[ReturnDate]
,[UpdateId]
,[UpdateName]
,[CreateTime]
,[UpdateTime]
, '' as[ConsultantId]
, '' as[ConsultantName]
, '' as[VIN]
,[CarSeries]
, '' as[LicensePlate]
, '' as[MileAge]
, '' as[Status]
, '' as[ServiceOrderNo]
, '' as[MaintainType]
, '' as[SyncTime]
,[IsExported]
,'' as [PurshaseTimeFrame]
from	CS_OrderCar
where	UserId = @0

union

SELECT [Id]
,[OrderNo]
, " +  (int)EOrderType.TestDrive+ @" as [OrderType]
,[UserId]
,[DealerId]
,[DealerName]
,[DealerCity]
,[DealerProvince]
, '' as[PurchaseYear]
,[UserName]
,[UserSex]
,[Phone]
,[Email]
,[ContactTime]
,[Comment]
,[State]
,[ScheduleDate]
, '' as[TakeAddress]
, '' as[TakeLong]
, '' as[TakeLat]
, '' as[ReturnAddress]
, '' as[ReturnLong]
, '' as[ReturnLat]
, '' as[ReturnDate]
,[UpdateId]
,[UpdateName]
,[CreateTime]
,[UpdateTime]
, '' as[ConsultantId]
, '' as[ConsultantName]
, '' as[VIN]
,[CarSeries]
, '' as[LicensePlate]
, '' as[MileAge]
, '' as[Status]
, '' as[ServiceOrderNo]
, '' as[MaintainType]
, '' as[SyncTime]
,[IsExported]
,[PurchaseTimeFrame]
from	CS_TestDrive
where	UserId = @0

union

SELECT [Id]
,[OrderNo]
,[OrderType]
,[UserId]
,[DealerId]
,[DealerName]
,[DealerCity]
,[DealerProvince]
,[PurchaseYear]
,[UserName]
,[UserSex]
,[Phone]
,[Email]
,[ContactTime]
,[Comment]
,[State]
,[ScheduleDate]
,[TakeAddress]
,[TakeLong]
,[TakeLat]
,[ReturnAddress]
,[ReturnLong]
,[ReturnLat]
,[ReturnDate]
,[UpdateId]
,[UpdateName]
,[CreateTime]
,[UpdateTime]
,[ConsultantId]
,[ConsultantName]
,[VIN]
,[CarSeries]
,[LicensePlate]
,[MileAge]
,[Status]
,[ServiceOrderNo]
,[MaintainType]
,[SyncTime]
,[IsExported]
,'' as [PurshaseTimeFrame]
from	CS_SonataService
where	UserId = @0
) a order by CreateTime desc
", userId);
            return PocoHelper.CurrentDb().Page<CSSonataServiceV>(page, itemsPerPage, sql);
        }
    }
}
