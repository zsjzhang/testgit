namespace Vcyber.BLMS.Repository.CarService
{
    using System;
    using System.Configuration;

    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;

    public class CSSqlBuilder
    {
        public static Sql BuildSql(QueryParamEntity entity, bool isSonata = true)
        {
            Sql sql = new Sql("where 1=1");
            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.UserId)) sql.Append(" and UserId=@0", entity.UserId);
                if (!string.IsNullOrEmpty(entity.CarSeries)) sql.Append(" and Carseries=@0", entity.CarSeries);
                if (!string.IsNullOrEmpty(entity.OrderNo)) sql.Append(" and OrderNo=@0", entity.OrderNo);
                if (!string.IsNullOrEmpty(entity.Phone)) sql.Append(" and Phone=@0", entity.Phone);
                if (entity.State != EOrderState.All) sql.Append(" and state=@0", (int)(entity.State));
                if (!string.IsNullOrEmpty(entity.DealerId)) sql.Append(" and DealerId=@0", entity.DealerId);
                if (!string.IsNullOrEmpty(entity.VIN)) sql.Append(" and VIN=@0", entity.VIN);
                if (!string.IsNullOrEmpty(entity.UpdateName)) sql.Append(" and UpdateName=@0", entity.UpdateName);
                if (entity.IsExported != null && entity.IsExported != EExportSatus.All) sql.Append(" and isnull(IsExported,0)=@0", (int)entity.IsExported);
                if (isSonata)
                {
                    if (!string.IsNullOrEmpty(entity.LicensePlate)) sql.Append(" and LicensePlate=@0", entity.LicensePlate);
                    if (entity.OrderType != null) sql.Append(" and OrderType=@0", (int)(entity.OrderType));
                }
                if (entity.CreateFromDate != null) sql.Append(" and CreateTime>=@0", ((DateTime)entity.CreateFromDate).ToString("yyyy-MM-dd"));
                if (entity.CreateToDate != null) sql.Append(" and CreateTime<@0", ((DateTime)entity.CreateToDate).AddDays(1).ToString("yyyy-MM-dd"));

                if (entity.ScheduleFromDate != null) sql.Append(" and ScheduleDate>=@0", ((DateTime)entity.ScheduleFromDate).ToString("yyyy-MM-dd"));
                if (entity.ScheduleToDate != null) sql.Append(" and ScheduleDate<@0", ((DateTime)entity.ScheduleToDate).AddDays(1).ToString("yyyy-MM-dd"));
                sql.Append(" order by CreateTime desc");
            }
            return sql;
        }

        public static Sql BuildSql4Update(int id, EOrderState state, string updateId, string updateName)
        {
            return new Sql(
                "set State=@0,UpdateId=@1, UpdateName=@2, UpdateTime=@3 where Id=@4",
                state,
                updateId,
                updateName,
                DateTime.Now,
                id);
        }

        public static Sql BuildSql4Consume(ConsumeQueryParamEntity entity)
        {
            Sql sql = new Sql(@"SELECT CS_Consume.[Id]
      ,[UserId]
      ,[OrderNo]
      ,[Phone]
      ,[DealerId]
      ,[DealerName]
      ,[ScheduleOrderNo]
      ,[ConsumeType]
      ,[PartCost]
      ,[MaterialCost]
      ,[LaborCost]
      ,[PurchaseCost]
      ,[PointCost]
      ,[TotalCost]
      ,[ConsumePoints]
      ,[RewardPoints]
      ,'" + ConfigurationManager.AppSettings["ImgPath"] + @"'+[PaperOrder] PaperOrder
      ,[ApproveStatus]
      ,[PointStatus]
      ,[ConsumeDate]
      ,CS_Consume.[CreateTime]
      ,[CreateId]
      ,[CreateName]
      ,CS_Consume.[UpdateTime]
      ,[UpdateId]
      ,CS_Consume.[UpdateName]
      ,[Comment]
      ,CS_Consume.IdentityNumber
      ,CS_Consume.UserName
      ,CS_Consume.VIN
      ,Membership.MLevel
  FROM [dbo].[CS_Consume] join Membership  on [CS_Consume].UserId=Membership.Id where 1=1");
            if (entity != null)
            {
                if (entity.Minpoints != null && entity.Minpoints.HasValue)
                {
                    sql.Append(" and ConsumePoints>=@0 ", entity.Minpoints.Value);
                }
                if (entity.Maxpoints != null && entity.Maxpoints.HasValue)
                {
                    sql.Append(" and ConsumePoints<=@0 ", entity.Maxpoints.Value);
                }
                if (!string.IsNullOrEmpty(entity.OrderNo)) sql.Append(" and OrderNo=@0", entity.OrderNo);
                if (!string.IsNullOrEmpty(entity.VIN)) sql.Append(" and CS_Consume.VIN=@0", entity.VIN);
                if (!string.IsNullOrEmpty(entity.Phone)) sql.Append(" and Phone=@0", entity.Phone);
                if (!string.IsNullOrEmpty(entity.DealerId)) sql.Append(" and DealerId=@0", entity.DealerId);
                if (entity.MinTotalCost > 0) sql.Append(" and TotalCost>=@0", entity.MinTotalCost);
                if (entity.MaxTotalCost > 0) sql.Append(" and TotalCost<@0", entity.MaxTotalCost);
                #region 新增加的
                if (entity.EConsumeType != EConsumeType.ALL) sql.Append(" and ConsumeType=@0", entity.EConsumeType);
                if (!string.IsNullOrEmpty(entity.Start)) sql.Append(" and ConsumeDate>=@0", entity.Start);
                if (!string.IsNullOrEmpty(entity.End)) sql.Append("  and ConsumeDate<@0", Convert.ToDateTime(entity.End).AddDays(1).ToString("yyyy-MM-dd"));
                #endregion
                if (entity.PointApproveStatus != EPointApproveStatus.All) sql.Append(" and ApproveStatus=@0", entity.PointApproveStatus);
                if (entity.PointStatus != EPointStatus.All) sql.Append(" and PointStatus=@0", entity.PointStatus);
                if (entity.HasAttachment == EAttachmentStatus.ToDo) sql.Append(" and isnull(PaperOrder,'')=''");
                if (entity.MLevel != EMLevelType.ALL) sql.Append(" and Membership.MLevel=@0", entity.MLevel);
                else if (entity.HasAttachment == EAttachmentStatus.Done) sql.Append(" and isnull(PaperOrder,'')<>''");
            }
            sql.Append(" order by CS_Consume.UpdateTime desc");
            return sql;
        }
    }
}