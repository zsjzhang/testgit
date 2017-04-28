using System;
using System.Collections.Generic;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Repository
{
    public class SC_ServiceCardUsedRecordStorager : ISC_ServiceCardUsedRecordStorager
    {
        public bool AddISCServiceCardUsedRecord(SC_ServiceCardUsedRecord record)
        {
            string sql = @" IF NOT EXISTS(select * from SC_ServiceCardUsedRecord where CardType=@CardType AND  CardNo=@CardNo ) INSERT INTO SC_ServiceCardUsedRecord(OpenId,PhoneNumber,CardType,CardNo,UserId,CustName,VIN,Mileage,ConsumeDate,
                        ConsumeMoney,DealerId,CreateTime,CardInfo,ActivityTag,RecommendName,RecommendPhone,DMSOrderNo) 
                        VALUES(@OpenId,@PhoneNumber,@CardType,@CardNo,@UserId,@CustName,@VIN,@Mileage,@ConsumeDate,
                        @ConsumeMoney,@DealerId,GETDATE(),@CardInfo,@ActivityTag,@RecommendName,@RecommendPhone,@DMSOrderNo)";

            return DbHelp.Execute(sql, new
            {
                @OpenId = record.OpenId,
                @PhoneNumber = record.PhoneNumber,
                @CardType = record.CardType,
                @CardNo = record.CardNo,
                @UserId = record.UserId,
                @CustName = record.CustName,
                @VIN = record.VIN,
                @Mileage = record.Mileage,
                @ConsumeDate = record.ConsumeDate,
                @ConsumeMoney = record.ConsumeMoney,
                @DealerId = record.DealerId,
                @CardInfo = record.CardInfo,
                @ActivityTag = record.ActivityTag,
                @RecommendName=record.RecommendName??"",
                @RecommendPhone=record.RecommendPhone??"",
                @DMSOrderNo = record.DMSOrderNo
            }) > 0;
        }

        public SC_ServiceCardUsedRecord GetSCServiceCardUsedRecord(string cardType, string cardNo)
        {
            string sql = "SELECT * FROM dbo.SC_ServiceCardUsedRecord WHERE CardType = @CardType AND CardNo = @CardNo";
            return DbHelp.QueryOne<SC_ServiceCardUsedRecord>(sql, new { @CardType = cardType, @CardNo = cardNo });
        }

        public IEnumerable<SC_ServiceCardUsedRecord> BuySCServiceCardUsedRecord(string cardType)
        {
            string sql = @"WITH t AS (
                        SELECT * FROM  dbo.fnSplitcode(@cardType,','))
                        SELECT * FROM t JOIN dbo.SC_ServiceCardUsedRecord c ON t.c1=c.CardType
                        ";
            return DbHelp.Query<SC_ServiceCardUsedRecord>(sql, new { @CardType = cardType });
        }
         public IEnumerable<CustomCardInfo> BuyCustCard(string cardType,string vin)
        { 
           string sql = @"WITH t AS (
                        SELECT * FROM  dbo.fnSplitcode(@cardType,','))
                        SELECT * FROM t JOIN dbo.CustomCard c ON t.c1=c.CardType
						AND c.OpenId=@vin";
           return DbHelp.Query<CustomCardInfo>(sql, new { cardType = cardType, @vin = vin });
        }

        public IEnumerable<Remeal> SelectRepairList(SC_ServiceCardUsedRecordSearchParam param)
        {

            string sql = @"SELECT re.*, re.CreateTime as CreateTime1 FROM remeal re
                       
                        WHERE 1=1 ";

            if (!string.IsNullOrEmpty(param.PhoneNumber))
                sql += " AND re.PhoneNumber = @PhoneNumber ";

            //if (!string.IsNullOrEmpty(param.CardType))
            //    sql += " AND S.CardType = @CardType ";


            if (!string.IsNullOrEmpty(param.CustName))
                sql += " AND re.CustName = @CustName ";

            if (!string.IsNullOrEmpty(param.DealerId))
                sql += " AND re.DearId = @DealerId ";

            if (!string.IsNullOrEmpty(param.VIN))
                sql += " AND re.VIN = @VIN ";
            if (!string.IsNullOrEmpty(param.CardCategory))
                sql += " and re.CarCategory=@CardCategory";

            if (!string.IsNullOrEmpty(param.isactivity))
                sql += " AND re.CardType = @isactivity ";

            if (!string.IsNullOrEmpty(param.StarCreateTime)) //购车时间
                sql += " AND re.BuyTime = @StarCreateTime ";

            if (!string.IsNullOrEmpty(param.EndCreateTime))//购买时间
            {
                sql += " AND re.CreateTime>=" + param.EndCreateTime;
                param.EndCreateTime = Convert.ToDateTime(param.EndCreateTime).AddDays(1).ToString("yyyy-MM-dd");
                sql += " and re.CreateTime < @EndCreateTime ";
            }

            return DbHelp.Query<Remeal>(sql, param);
        }

        public IEnumerable<SC_ServiceCardUsedRecord> SelectRecordList(SC_ServiceCardUsedRecordSearchParam param)
        {
            
//            string sql = @"SELECT S.*,T.CardTypeName,R.CarCategory,Dear.Name as DealerName,r.BuyTime,r.CarCategory,case m.MLevel when 10 then '普卡'when 11 then'银卡'when 12 then'金卡'when 1 then'注册用户' end MLevel,fk.CreateTime as kqCreateTime
//                            FROM SC_ServiceCardUsedRecord S 
//                            LEFT JOIN IF_CAR R ON S.VIN = R.VIN 
//                            LEFT JOIN SC_ServiceCardType T ON S.CardType = T.CardType
//                            LEFT JOIN BLMS_WX.dbo.WXCard_Records W ON S.CardNo = W.code
//                            left join CS_CarDealerShip Dear on Dear.DealerId=S.DealerId
//                             left join CustomCard fk on s.CardNo=fk.CardCode
//                             left join Membership m on fk.UserId=m.Id
//                             
//                           WHERE 1=1 ";

            string sql = @"SELECT S.*,T.CardTypeName,R.CarCategory,Dear.Name as DealerName,r.BuyTime,r.CarCategory,
case m.MLevel 
when 10 then '普卡'
when 11 then'银卡'
when 12 then'金卡'
when 1 then'注册用户' end MLevel,
fk.CreateTime as kqCreateTime
,jxs.Area 
,jxs.Region 
FROM SC_ServiceCardUsedRecord S 
LEFT JOIN IF_CAR R ON S.VIN = R.VIN 
LEFT JOIN SC_ServiceCardType T ON S.CardType = T.CardType
LEFT JOIN BLMS_WX.dbo.WXCard_Records W ON S.CardNo = W.code
left join CS_CarDealerShip Dear on Dear.DealerId=S.DealerId
left join CustomCard fk on s.CardNo=fk.CardCode
left join Membership m on fk.UserId=m.Id                             
left join CS_CarDealerShip jxs  on s.DealerId=jxs.DealerId
WHERE 1=1 ";

            if (!string.IsNullOrEmpty(param.PhoneNumber))
                sql += " AND S.PhoneNumber = @PhoneNumber ";

            if (!string.IsNullOrEmpty(param.CardType))
                sql += " AND S.CardType = @CardType ";

            if (!string.IsNullOrEmpty(param.CardNo))
                sql += " AND S.CardNo = @CardNo ";

            if (!string.IsNullOrEmpty(param.CustName))
                sql += " AND S.CustName = @CustName ";

            if (!string.IsNullOrEmpty(param.DealerId))
                sql += " AND S.DealerId = @DealerId ";

            if (!string.IsNullOrEmpty(param.VIN))
                sql += " AND S.VIN = @VIN ";

            if (!string.IsNullOrEmpty(param.isactivity))
                sql += " AND S.ActivityTag = @isactivity ";

            if (!string.IsNullOrEmpty(param.StarCreateTime))
                sql += " AND S.CreateTime >= @StarCreateTime ";

            if (!string.IsNullOrEmpty(param.EndCreateTime))
            {

                //param.EndCreateTime = Convert.ToDateTime(param.EndCreateTime).AddDays(1).ToString("yyyy-MM-dd");

               // param.EndCreateTime = Convert.ToDateTime(param.EndCreateTime).AddDays(1).ToString("yyyy-MM-dd");

                sql += " AND S.CreateTime <= @EndCreateTime ";
            }

            return DbHelp.Query<SC_ServiceCardUsedRecord>(sql, param);
        }

        public bool Update(int id, string custName, int Mileage)
        {
            string sql = @"UPDATE SC_ServiceCardUsedRecord SET CustName = @CustName,Mileage = @Mileage WHERE Id = @Id";

            return DbHelp.Execute(sql, new
            {
                @Id = id,
                @CustName = custName,
                @Mileage = Mileage,
            }) > 0;
        }

        public IEnumerable<SC_ServiceCardUsedRecord> SelectRecordByVin(string vin, string activityTag)
        {
            string sql = "SELECT * FROM dbo.SC_ServiceCardUsedRecord WHERE VIN = @VIN AND ActivityTag = @ActivityTag";
            return DbHelp.Query<SC_ServiceCardUsedRecord>(sql, new { @VIN = vin, @ActivityTag = activityTag });
        }
        //检查改卡券是否核销过
        public IEnumerable<SC_ServiceCardUsedRecord> SelectRecordByVinAndCardType(string vin, string CardType)
        {
            string sql = "SELECT * FROM dbo.SC_ServiceCardUsedRecord WHERE VIN = @VIN AND CardType = @CardType";
            return DbHelp.Query<SC_ServiceCardUsedRecord>(sql, new { @VIN = vin, @CardType = CardType });
        }

        public IEnumerable<Remeal> SelectRemealByVin(string Vin, string CardType)
        {
            string sql = "SELECT * FROM remeal WHERE cardtype=@CardType AND vin=@Vin";
            return DbHelp.Query<Remeal>(sql, new { @Vin = Vin, @CardType = CardType });
        }
    }
}
