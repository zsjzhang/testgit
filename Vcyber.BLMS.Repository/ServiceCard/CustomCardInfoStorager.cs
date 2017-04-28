using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class CustomCardInfoStorager : ICustomCardInfoStorager
    {
        /// <summary>
        /// 添加卡券信息
        /// </summary>
        /// <param name="model">卡券信息</param>
        /// <returns></returns>
        public bool AddCustomCardInfo(BLMS.Entity.CustomCardInfo model)
        {
            string sql = @"INSERT INTO CustomCardInfo (CardLogoUrl ,MerchantLogoUrl,CardSource ,CardColor ,CardName ,ActivityType ,Quantity ,ReduceCost , CardRemark ,status ,UserId ,Instructions ,CreateDate,MerchantName,CardType,CardBeginDate,CardEndDate,CardValidityType ,CardValidity,SmsContent  ) VALUES (@CardLogoUrl ,@MerchantLogoUrl,@CardSource ,@CardColor ,@CardName ,@ActivityType ,@Quantity ,@ReduceCost  ,@CardRemark ,@status ,@UserId ,@Instructions ,@CreateDate,@MerchantName,@CardType,@CardBeginDate,@CardEndDate,@CardValidityType ,@CardValidity,@SmsContent )";
            return DbHelp.Execute(sql, model
               ) > 0;
        }

        /// <summary>
        /// 获取一条有效卡券信息
        /// </summary>
        /// <param name="id">卡卷id</param>
        /// <returns></returns>
        public CustomCardInfo GetCustomCardInfo(int id)
        {
            string sql = "SELECT Id ,CardLogoUrl ,MerchantLogoUrl,CardSource ,CardColor ,CardName ,ActivityType ,CardValidityType ,CardValidity ,CardTimeLimitType ,CardTimeLimit ,Quantity ,Used,ReduceCost ,GetLimit ,CardPutinType ,CardPutin ,CardRemark ,status ,UserId ,Instructions ,CreateDate,CardType  FROM CustomCardInfo WHERE ID=@ID AND status=1";
            return DbHelp.QueryOne<CustomCardInfo>(sql, new
            {
                @ID = id
            });
        }

        /// <summary>
        /// 更新一条卡券信息
        /// </summary>
        /// <param name="model">卡卷信息</param>
        /// <returns></returns>
        public bool UpdateCustomCardInfo(CustomCardInfo model)
        {
            string sql = @" UPDATE CustomCardInfo
                            SET 
                                Quantity = @Quantity, 
                                ReduceCost = @ReduceCost, 
                                CardRemark = @CardRemark, 
                                Instructions = @Instructions, 
                                CardEndDate = @CardEndDate, 
                                SmsContent = @SmsContent,
                                CardName=@CardName 
                            WHERE ID =@ID";

            return DbHelp.Execute(sql, model) > 0;
        }


        /// <summary>
        /// 删除一条卡券信息
        /// </summary>
        /// <param name="id">卡券ID</param>
        /// <returns></returns>
        public bool DeleteCustomCardById(string id)
        {
            string sql = @" UPDATE CustomCardInfo    SET   status = 2  WHERE CardType =@CardType";
            return DbHelp.Execute(sql, new
            {
                CardType = id
            }) > 0;
        }

        /// <summary>
        /// 更新卡券库存数量
        /// </summary>
        /// <param name="id">卡券ID</param>
        /// <param name="quantity">库存数量</param>
        /// <returns></returns>
        public bool UpdateCustomCardQuantity(int id, int quantity)
        {
            string sql = @" UPDATE CustomCardInfo    SET      Quantity = @Quantity   WHERE ID =@ID";
            return DbHelp.Execute(sql, new
            {
                @Quantity = quantity,
                @ID = id,
            }) > 0;
        }


        public IEnumerable<CustomCardInfo> GetCustomCardInfoList(int source, string merchant, string actType, string cardName, int status, string reduceCost, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                "   SELECT * FROM ( SELECT C.CardType,C.Id,C.CardBeginDate,C.CardEndDate ,C.CardLogoUrl,C.MerchantLogoUrl ,C.CardSource ,C.CardColor ,C.CardName ,C.ActivityType ,C.CardValidityType ,C.CardValidity ,C.CardTimeLimitType ,C.CardTimeLimit ,C.Quantity,C.Used ,C.ReduceCost ,C.GetLimit ,C.CardPutinType ,C.CardPutin ,C.CardRemark , (select COUNT(*) from  CustomCard D where D.CardType = C.CardType) AS status,C.UserId ,C.Instructions ,C.CreateDate,C.MerchantName, ROW_NUMBER() over(order by Id desc) as RowNumber   FROM CustomCardInfo C WHERE C.status=1");
            string condtion = string.Empty;
            if (!string.IsNullOrEmpty(merchant))
            {
                merchant = string.Format("%{0}%", merchant);
                condtion += "  AND  C.MerchantName    like @MerchantName";
            }
            if (source > 0)
            {
                condtion += " AND C.CardSource=@CardSource";
            }
            if (!string.IsNullOrEmpty(actType))
            {
                condtion += " AND C.ActivityType=@ActivityType";
            }
            if (!string.IsNullOrEmpty(cardName))
            {
                cardName = string.Format("%{0}%", cardName);
                condtion += "  AND C.CardType  like @CardType";
            }
            if (!string.IsNullOrEmpty(reduceCost))
            {
                condtion += " AND C.ReduceCost=@ReduceCost";
            }
            if (status == (int)ECustomCardReceiveState.Yes)
            {
                condtion += " AND  C.id  in(select   M.CardId  from  CustomCard M where  M.CardId is not null )";
            }
            else if (status == (int)ECustomCardReceiveState.No)
            {
                condtion += " AND  C.id  not in(select   M.CardId  from  CustomCard M where  M.CardId is not null )";
            }
            StringBuilder sqlCount = new StringBuilder("SELECT COUNT(*) FROM CustomCardInfo C WHERE C.status=1");
            sqlCount.Append(condtion);
            sb.AppendFormat(condtion + " ) as RowNumber_Table where RowNumber between {0}  and {1};", (pageIndex - 1) * pageSize + 1, pageSize * pageIndex);
            totalCount = DbHelp.ExecuteScalar<int>(sqlCount.ToString(), new { CardType = cardName, MerchantName = merchant, CardSource = source, ActivityType = actType, ReduceCost = reduceCost });
            return DbHelp.Query<CustomCardInfo>(sb.ToString(), new { CardType = cardName, MerchantName = merchant, CardSource = source, ActivityType = actType, ReduceCost = reduceCost, PageSize = pageSize, PageCount = (pageSize * pageIndex) });

        }
        //YC活动根据手机号获取用户信息
        public RecommendCustomer GetRecommendNameByPhone(string phone)
        {
            string sql = "SELECT TOP 1 * FROM dbo.Recommend WHERE userid=(SELECT Id FROM dbo.Membership WHERE PhoneNumber=@phone)";
            return DbHelp.QueryOne<RecommendCustomer>(sql, new { @phone=phone });
        }

        /// <summary>
        /// 返回一张优惠券信息
        /// </summary>
        /// <param name="cardType">优惠券GUID</param>
        /// <returns>返回一张优惠券信息</returns>
        public ReturnCustomCardInfo GetSingleCustomCardInfo(string cardType)
        {
            string sql = @"SELECT  CardSource,CardLogoUrl,MerchantLogoUrl,CardColor,CardName, ActivityType as ActivityName,ReduceCost,CardRemark,Instructions,CardType,CardBeginDate,CardEndDate,MerchantName FROM  CustomCardInfo  Where  CardType=@CardType;";
            return DbHelp.QueryOne<ReturnCustomCardInfo>(sql, new
            {
                @CardType = cardType
            });
        }

        /// <summary>
        /// 获取一条卡券信息
        /// </summary>
        /// <param name="cardType">卡券Type</param>
        /// <returns></returns>
        public CustomCardInfo GetSingleCustomCardInfoByGuid(string cardType)
        {
            string sql = "SELECT Id ,Used,CardLogoUrl ,MerchantLogoUrl,CardSource ,CardColor ,CardName ,ActivityType ,CardValidityType ,CardValidity ,CardTimeLimitType ,CardTimeLimit ,Quantity ,ReduceCost ,GetLimit ,CardPutinType ,CardPutin ,CardRemark ,status ,UserId ,Instructions ,CreateDate,CardType,CardBeginDate,CardEndDate,SmsContent  FROM CustomCardInfo WHERE CardType=@CardType AND status=1";
            return DbHelp.QueryOne<CustomCardInfo>(sql, new
            {
                @CardType = cardType
            });
        }

        public CustomCardInfo GetCustomTypeByVin(string Vin, string ActivityType)
        {
            string sql = @"WITH T AS (
                         SELECT C.CardType AS CardType FROM  SC_ServiceCardUsedRecord  AS C   LEFT JOIN  CustomCardInfo AS M ON C.CardType=M.CardType
		                 WHERE  VIN=@Vin AND m.ActivityType=@ActivityType AND m.CardName  LIKE '试乘试驾推荐券%') 
                SELECT TOP 1 * FROM CustomCardInfo WHERE  ActivityType=@ActivityType AND Quantity-Used>0 AND  CardName  LIKE '试乘试驾推荐券%' AND CardType  NOT IN (SELECT * FROM T) ORDER BY CreateDate";
            return DbHelp.QueryOne<CustomCardInfo>(sql, new
            {
                @ActivityType = ActivityType,
                @Vin=Vin
            });
        }

        /// <summary>
        /// 获取一张用户卡券信息
        /// </summary>
        /// <param name="cardType">卡券Guid</param>
        /// <param name="userId">用户ID</param>
        /// <returns>用户卡券信息</returns>
        public ReturnUserCustomCardInfo GetSingleUserCustomCardInfoByIdAndUserId(string id, string userId)
        {

            string sql = @"SELECT C.Id, CASE  M.CardSource  WHEN 1 THEN C.CardCode  WHEN 2 THEN  SUBSTRING(C.CardCode, LEN(C.CardType)+2, LEN(C.CardCode)- (LEN(C.CardType)+2))   ELSE C.CardCode  END  AS   CardCode,M.CardSource,M.CardLogoUrl,M.MerchantLogoUrl,M.CardColor,M.CardName, M.ActivityType as ActivityName,M.ReduceCost,M.CardRemark,M.Instructions, M.CardType,M.MerchantName,
		                                CASE M.CardValidityType  WHEN 1  THEN M.CardEndDate   WHEN 2 THEN C.CreateTime +isnull(M.CardValidity,0)    ELSE  M.CardEndDate  end as   CardEndDate,
                                        CASE M.CardValidityType  WHEN 1  THEN M.CardBeginDate   WHEN 2 THEN C.CreateTime    ELSE  M.CardBeginDate  end as   CardBeginDate
                         FROM  CustomCard  AS C   LEFT JOIN  CustomCardInfo AS M ON C.CardType=M.CardType
		                 WHERE C.ID =@ID AND C.UserId=@UserId";
            return DbHelp.QueryOne<ReturnUserCustomCardInfo>(sql, new
            {        

                @ID = id,
                @UserId = userId,
            });
        }


        public bool UpdateCustomCardQuantityByType(string cardType)
        {
            string sql = @" UPDATE CustomCardInfo    SET      Used = Used+1   WHERE CardType =@CardType";
            return DbHelp.Execute(sql, new
            {
                CardType = cardType,
            }) > 0;
        }


        public IEnumerable<CustomCardInfo> GetCustomCardInfoListByActType(string actType,string cardName)
        {            

            var sql = new StringBuilder();
            sql.Append("SELECT Id,Used,CardLogoUrl,MerchantLogoUrl,CardSource,CardColor,CardName,")
                .Append("ActivityType,CardValidityType,CardValidity,CardTimeLimitType,CardTimeLimit,")
                .Append("Quantity,ReduceCost,GetLimit,CardPutinType,CardPutin,CardRemark,status,UserId,")
                .Append("Instructions,CreateDate,CardType,CardBeginDate,CardEndDate,SmsContent")
                .Append(" from CustomCardInfo where (ActivityType=@ActivityType AND CardName = @CardName) AND status=1");
            return DbHelp.Query<CustomCardInfo>(sql.ToString(), new { ActivityType = actType, CardName = cardName });

        }


        public bool IsExistsCustomCardInfo(string actType, string cardNmae, int source)
        {
            string sql = @" SELECT count(*) from  CustomCardInfo     WHERE CardName =@CardName AND  ActivityType=@ActivityType  AND CardSource=@CardSource AND status=1";
            int count = DbHelp.ExecuteScalar<int>(sql, new { CardName = cardNmae, ActivityType = actType, CardSource = source });
            return count > 0;
        }

        /// <summary>
        /// 根据cardNO查找卡券信息
        /// </summary>
        /// <param name="cardType">卡券NO</param>
        /// <returns></returns>
        public CustomCardInfo GetSingleCustomCardInfoByCNo(string cardNo)
        {
            string sql = @"SELECT Id ,Used,CardLogoUrl ,MerchantLogoUrl,CardSource ,CardColor ,CardName ,ActivityType ,
                           CardValidityType ,CardValidity ,CardTimeLimitType ,CardTimeLimit ,Quantity ,ReduceCost ,GetLimit ,CardPutinType ,
                           CardPutin ,CardRemark ,status ,UserId ,Instructions ,CreateDate,CardType,CardBeginDate,CardEndDate,SmsContent  
                           FROM CustomCardInfo where CardType = (select CardType from CustomCard where CardCode = @cardNo) AND status=1";
            return DbHelp.QueryOne<CustomCardInfo>(sql, new
            {
                @cardNo = cardNo
            });
        }
    }
}
