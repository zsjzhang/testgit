using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class CustomCardStorager : ICustomCardStorager
    {

        /// <summary>
        /// 添加用户优惠券信息
        /// </summary>
        /// <param name="model">用户优惠券信息</param>
        /// <returns></returns>
        public bool AddCustomCard(CustomCard model)
        {
            string sql = @"INSERT INTO CustomCard(CardCode ,CardType ,CreateTime ,IsCancel ,UserId ,OpenId ,Tel ,IsSend ,IsSave ,CardId ,IsReissue,Source) VALUES (@CardCode,@CardType,@CreateTime,@IsCancel,@UserId,@OpenId,@Tel,@IsSend,@IsSave,@CardId,@IsReissue,@Source)";
            return DbHelp.Execute(sql, model
               ) > 0;
        }

        public bool AddRepair(Remeal model)
        {
            string sql = "INSERT INTO remeal VALUES (@CustName,@PhoneNumber,@Vin,@DearID,@CardType,@Mileage,GETDATE(),@BuyTime,@CarCategory,@DearName,@Mlevel)";
            return DbHelp.Execute(sql,model)>0;
        }

        /// <summary>
        /// 获取用户自定义优惠券 状态（未使用，已使用，已过期）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="source">卡券来源 1：北京现代 ；2：合作商户；</param>
        /// <param name="status">状态（1：未使用，2：已使用，3：已过期）</param>
        /// <returns>优惠券列表信息</returns>
        public IEnumerable<ReturnUserCustomCardModel> GetUserCustomCardList(string userId, int source, int status)
        {
            string sql =
                @"select * from (select C.Id,CASE  M.CardSource  WHEN 1 THEN C.CardCode  WHEN 2 THEN  SUBSTRING(C.CardCode, LEN(C.CardType)+2, LEN(C.CardCode)- (LEN(C.CardType)+2))   ELSE C.CardCode  END  AS   CardCode,
                M.CardSource,M.MerchantName,M.CardLogoUrl,M.MerchantLogoUrl,M.CardColor,M.CardName,M.ActivityType AS ActivityName,M.ReduceCost,M.CardRemark,M.Instructions,M.CardType,
                CASE M.CardValidityType  WHEN 1  THEN M.CardEndDate   WHEN 2 THEN (CONVERT(varchar(100),convert(datetime,convert(varchar,(C.CreateTime +isnull(M.CardValidity,0)),112),112)+1-1.0/3600/24,20))    ELSE  M.CardEndDate  end as   CardEndDate,
                CASE M.CardValidityType  WHEN 1  THEN M.CardBeginDate   WHEN 2 THEN C.CreateTime    ELSE  M.CardBeginDate  end as   CardBeginDate,C.IsSave,C.IsCancel
            from  CustomCard  as  C left  join  CustomCardInfo as M  on  C.CardType = M.CardType
            where C.UserId=@UserId    AND M.CardSource=@CardSource) as dt   where   dt.id is not null   ";

            if (status == 1)
            {
                sql += "  AND  getdate() between dt.CardBeginDate and dt.CardEndDate  AND   dt.IsSave =1   and dt.IsCancel =0";
            }
            else if (status == 2)
            {
                sql += "  AND  getdate() between dt.CardBeginDate and dt.CardEndDate  AND   dt.IsSave =1   and dt.IsCancel =1";
            }
            else if (status == 3)
            {
                sql += " AND   dt.CardEndDate < getdate()   AND   dt.IsSave =1";
            }
            sql += " order by dt.Id  Desc";
            return DbHelp.Query<ReturnUserCustomCardModel>(sql, new { UserId = userId, CardSource = source });
        }

        /// <summary>
        /// 获取候机服务券
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="source"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IEnumerable<SNCard> GetTerminalservicevoucherCardList(string userId, int source, int status)
        {
            string sql =
                @"select *
                   from SNCard
                  where 1=1 ";

            sql += " and UserId=@UserId";

            if (status == 2)
            {
                if (source == 0)//未过期
                    sql += " and Status=2 and dateadd(day,-1,dateadd(month,3,cast(YEAR(SendTime) as varchar(4))+'-'+cast(MONTH(SendTime) as varchar(2))+'-'+cast(DAY(SendTime) as varchar(2))+' 00:00:00'))>=cast(YEAR(GETDATE()) as varchar(4))+'-'+cast(MONTH(GETDATE()) as varchar(2))+'-'+cast(DAY(GETDATE()) as varchar(2))+' 00:00:00'  ";
                else            //已过期
                    sql += " and Status=2 and dateadd(day,-1,dateadd(month,3,cast(YEAR(SendTime) as varchar(4))+'-'+cast(MONTH(SendTime) as varchar(2))+'-'+cast(DAY(SendTime) as varchar(2))+' 00:00:00'))<cast(YEAR(GETDATE()) as varchar(4))+'-'+cast(MONTH(GETDATE()) as varchar(2))+'-'+cast(DAY(GETDATE()) as varchar(2))+' 00:00:00' ";
            }
            else if (status == 3)
            {
                sql += " and Status=3";
            }
            sql += " order by Id";

            return DbHelp.Query<SNCard>(sql, new { @UserId = userId });
        }

        /// <summary>
        /// 根据手机号码，查询用户卡券信息
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="activityName">活动名称</param>
        /// <param name="cardType">卡券ID</param>
        /// <param name="pageIndex">分页号</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="totalCount">返回总行数</param>
        /// <returns></returns>
        public IEnumerable<UserCustomCard> GetUserCustomCardListByPhone(string userId, string activityName, string cardType, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@" SELECT * FROM (  select M.CardBeginDate,M.CardEndDate,CASE  M.CardSource  WHEN 1 THEN C.CardCode  WHEN 2 THEN  SUBSTRING(C.CardCode, LEN(C.CardType)+2, LEN(C.CardCode)- (LEN(C.CardType)+2))   ELSE C.CardCode  END  AS   CardCode,
C.CardType ,C.CreateTime ,C.IsCancel ,C.UserId ,C.OpenId ,C.Tel ,C.IsSend ,C.IsSave ,C.CardId ,C.IsReissue,M.CardSource,M.CardName,M.ActivityType,M.MerchantName , ROW_NUMBER() over(order by C.Id desc) as RowNumber
                      from CustomCard  as C left join  CustomCardInfo   as M on C.CardType = M.CardType
                      WHERE  C.CardType is not null AND C.UserId  =@UserId ");

            string condtion = string.Empty;
            if (!string.IsNullOrEmpty(activityName))
            {
                condtion += "  AND M.ActivityType =@ActivityType";
            }
            if (!string.IsNullOrEmpty(cardType))
            {
                condtion += "  AND C.CardType=@CardType";
            }
            StringBuilder sqlCount = new StringBuilder("select count(*) from CustomCard  as C left join  CustomCardInfo   as M on C.CardType = M.CardType  WHERE  C.CardType is not null AND C.UserId =@UserId  ");
            sqlCount.Append(condtion);
            sb.AppendFormat(condtion + " ) as RowNumber_Table where RowNumber between {0}  and {1};", (pageIndex - 1) * pageSize + 1, pageSize * pageIndex);
            totalCount = DbHelp.ExecuteScalar<int>(sqlCount.ToString(), new { UserId = userId, ActivityType = activityName, CardType = cardType });
            return DbHelp.Query<UserCustomCard>(sb.ToString(), new { UserId = userId, ActivityType = activityName, CardType = cardType, PageSize = pageSize, PageCount = (pageSize * pageIndex) });

        }


        public CustomCard GetUserCustomCardByCardType(string cardType)
        {
            string sql = @"  SELECT C.Id, C.CardCode ,C.CardType ,C.CreateTime ,C.IsCancel ,C.UserId ,C.OpenId ,C.Tel ,C.IsSend ,C.IsSave ,C.CardId ,C.IsReissue from  CustomCard  AS C
               WHERE C.CardType =@CardType";
            return DbHelp.QueryOne<CustomCard>(sql, new
            {
                CardType = cardType
            });
        }

        /// <summary>
        /// 根据用户ID和卡券的活动名字获取用户领取的卡券列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="ActivityType">卡券的活动名字</param>
        /// <returns></returns>
        public List<CustomCard> getUserCustomCardByUID(string userId, string ActivityType)
        {
            string sql = "select c.Id,c.CardCode,c.CardType,c.CreateTime,c.UpdateTime,c.UserId,c.Tel,c.CardId from CustomCard c,CustomCardInfo ci where c.CardType=ci.CardType and c.UserId=@UserId and ci.Type=@Type";
            return DbHelp.Query<CustomCard>(sql, new { UserId = userId, Type = ActivityType }).ToList();
        }
        /// <summary>
        /// 根据用户ID和卡券的活动名字获取用户领取的卡券列表
        /// </summary>
        public List<CustomCard> GetUserCustomCardByActivityType(string userId, string ActivityType)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT cc.* FROM dbo.CustomCardInfo AS cci ")
                .Append(" INNER JOIN dbo.CustomCard AS cc ON cc.CardId = cci.Id")
                .Append(" WHERE cc.UserId = @UserId AND cci.ActivityType = @ActivityType");
            return DbHelp.Query<CustomCard>(sql.ToString(), new { UserId = userId, ActivityType = ActivityType }).ToList();
        }

        /// <summary>
        /// 获取卡券使用数量
        /// </summary>
        /// <param name="cardType">卡券GUID</param>
        public int GetCardUsedCount(string cardType)
        {
            string sql =
                @"select COUNT(*)  from  CustomCard  where  CardType =@CardType ";
            return DbHelp.ExecuteScalar<int>(sql, new { CardType = cardType });
        }

        public IEnumerable<CustomCardInfo> GetCarsByCardName(string cardname)
        {
            string sql = "SELECT * FROM dbo.CustomCardInfo  WHERE CardName=@cardname";
            return  DbHelp.Query<CustomCardInfo>(sql, new {cardname = cardname});
        }



        public int GetUserReissueCount(string cardType, string phone)
        {
            string sql =
               @"select COUNT(*) from CustomCard   where CardType=@CardType  AND Tel=@Tel AND IsReissue=1";
            return DbHelp.ExecuteScalar<int>(sql, new { CardType = cardType, Tel = phone });
        }

        public int GetCountRepar(string vin, string cardType)
        {
            string sql = "SELECT COUNT(1) FROM remeal WHERE cardtype=@cardType AND vin=@vin";
            return DbHelp.ExecuteScalar<int>(sql, new { cardtype = @cardType, vin = @vin });
        }

        public Remeal GetRemeal(string vin, string cardtype,string cardCode )
        {
            string sql = @"WITH t AS (
                        SELECT * FROM  dbo.fnSplitcode(@cardType,','))
                        SELECT *,c.CardType as CardType1 FROM t JOIN dbo.CustomCard c ON t.c1=c.CardType 
                        join ReMeal on ReMeal.vin=c.OpenId  
						AND c.OpenId=@vin
                        AND c.CardCode=@cardCode";
            return DbHelp.QueryOne<Remeal>(sql, new { vin = @vin, cardtype = @cardtype, @cardCode = cardCode });
        }


        public ReturnCustomCardCodeInfo GetCustomCardCodeInfo(string cardCode)
        {
            string sql = @"  SELECT  CASE  M.CardSource  WHEN 1 THEN C.CardCode  WHEN 2 THEN  SUBSTRING(C.CardCode, LEN(C.CardType)+2, LEN(C.CardCode)- (LEN(C.CardType)+2))   ELSE C.CardCode  END  AS   CardCode ,C.CardType ,C.IsCancel ,C.IsSend ,C.IsSave,
                                CASE M.CardValidityType  WHEN 1  THEN M.CardBeginDate   WHEN 2 THEN C.CreateTime     ELSE  M.CardBeginDate  end as   CardBeginDate,                   
                                CASE M.CardValidityType  WHEN 1  THEN M.CardEndDate   WHEN 2 THEN C.CreateTime +isnull(M.CardValidity,0)    ELSE  M.CardEndDate  end as   CardEndDate  from  CustomCard  AS C
                            LEFT JOIN  CustomCardInfo AS M ON C.CardType=M.CardType
                            WHERE C.CardCode =@CardCode";
            return DbHelp.QueryOne<ReturnCustomCardCodeInfo>(sql, new
            {
                CardCode = cardCode
            });
        }


        public ReturnCustomCardCodeInfoByDMS GetCustomCardCodeInfoByDMS(string cardCode)
        {
            string sql = @"  SELECT  CASE  M.CardSource  WHEN 1 THEN C.CardCode  WHEN 2 THEN  SUBSTRING(C.CardCode, LEN(C.CardType)+2, LEN(C.CardCode)- (LEN(C.CardType)+2))   ELSE C.CardCode  END  AS   CardCode ,C.CardType ,C.IsCancel ,C.IsSend ,C.IsSave,
                               C.Tel,M.ReduceCost,M.CardRemark,M.CardCategory,
                             CASE M.CardValidityType  WHEN 1  THEN M.CardBeginDate   WHEN 2 THEN C.CreateTime     ELSE  M.CardBeginDate  end as   CardBeginDate,                   
                                CASE M.CardValidityType  WHEN 1  THEN M.CardEndDate   WHEN 2 THEN C.CreateTime +isnull(M.CardValidity,0)    ELSE  M.CardEndDate  end as   CardEndDate  from  CustomCard  AS C
                            LEFT JOIN  CustomCardInfo AS M ON C.CardType=M.CardType
                            WHERE C.CardCode =@CardCode";
            return DbHelp.QueryOne<ReturnCustomCardCodeInfoByDMS>(sql, new
            {
                CardCode = cardCode
            });
        }


        public IEnumerable<ReturnCustomCardCodeInfo> GetUserSummerCustomCardListByUserId(string userId)
        {
            string sql = @"select   CASE  M.CardSource  WHEN 1 THEN C.CardCode  WHEN 2 THEN  SUBSTRING(C.CardCode, LEN(C.CardType)+2, LEN(C.CardCode)- (LEN(C.CardType)+2))   ELSE C.CardCode  END  AS   CardCode ,C.CardType ,C.IsCancel ,C.IsSend ,C.IsSave,M.CardBeginDate,M.CardEndDate   from CustomCard  C left join  CustomCardInfo  M ON C.CardType=M.CardType
                        where M.Type=2   and  C.UserId=@UserId";
                        return DbHelp.Query<ReturnCustomCardCodeInfo>(sql, new { UserId = userId});
        }

        public int GetReceivedCustomCardCount(string UserId)
        {
            string sql = @"select count(*) from (select C.UserId,C.Id,CASE  M.CardSource  WHEN 1 THEN C.CardCode  WHEN 2 THEN  SUBSTRING(C.CardCode, LEN(C.CardType)+2, LEN(C.CardCode)- (LEN(C.CardType)+2))   ELSE C.CardCode  END  AS   CardCode,
                M.CardSource,M.MerchantName,M.CardLogoUrl,M.MerchantLogoUrl,M.CardColor,M.CardName,M.ActivityType AS ActivityName,M.ReduceCost,M.CardRemark,M.Instructions,M.CardType,
                CASE M.CardValidityType  WHEN 1  THEN M.CardEndDate   WHEN 2 THEN C.CreateTime +isnull(M.CardValidity,0)    ELSE  M.CardEndDate  end as   CardEndDate,
                CASE M.CardValidityType  WHEN 1  THEN M.CardBeginDate   WHEN 2 THEN C.CreateTime    ELSE  M.CardBeginDate  end as   CardBeginDate,C.IsSave,C.IsCancel
            from  CustomCard  as  C left  join  CustomCardInfo as M  on  C.CardType = M.CardType
            where C.UserId=@UserId ) as dt   where   dt.id is not null AND  getdate() between dt.CardBeginDate and dt.CardEndDate  AND   dt.IsSave =1   and dt.IsCancel =0";
            return DbHelp.ExecuteScalar<int>(sql, new { UserId = UserId });
        }
    }
}
