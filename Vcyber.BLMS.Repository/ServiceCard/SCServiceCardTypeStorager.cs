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
    public class SCServiceCardTypeStorager : ISCServiceCardTypeStorager
    {
        /// <summary>
        /// 添加优惠券卡类型信息
        /// </summary>
        /// <param name="model">优惠券卡类型信息</param>
        /// <returns></returns>
        public bool AddSCServiceCardType(SCServiceCardType model)
        {
            string sql = @"INSERT INTO SC_ServiceCardType (CardType ,CardTypeName ,CreateTime ,ActivityType) VALUES (@CardType ,@CardTypeName ,@CreateTime ,@ActivityType)";
            return DbHelp.Execute(sql, model
               ) > 0;
        }

        /// <summary>
        /// 获取卡券列表
        /// </summary>
        /// <param name="type">1：卡券活动类型，2：卡券名称；</param>
        /// <returns></returns>
        public IEnumerable<SCServiceCardType> GetSCServiceCardTypeList(int type, int source, int iswx = 0)
        {
            string sql = "";
            if (type == 1)
            {
                sql =
                    @"select * from (select count(*) as id,S.ActivityType as ActivityType,MAX(s.CreateTime) as CreateTime  FROM SC_ServiceCardType as S where  S.ActivityType is not null ";
                if (source > 0)
                {
                    sql += " and S.CardType in ( select C.CardType from CustomCardInfo C  WHERE  C.CardType= S.CardType AND CardSource=@CardSource AND status=1 ";
                    if (iswx == 0)
                    {
                        sql += " and IsWeixin=@IsWeixin";
                    }
                    sql += ")";
                }
                else
                {
                    sql += " and S.CardType in ( select C.CardType from CustomCardInfo C  WHERE  C.CardType= S.CardType  AND status=1 ";
                    if (iswx == 0)
                    {
                        sql += " and IsWeixin=@IsWeixin";
                    }
                    sql += ")";
                }
                sql += " group by ActivityType ) as result order by result.CreateTime desc ";
            }
            else if (type == 2)
            {
                sql =
                    " select * from (select count(*) as id,S.ActivityType as ActivityType,MAX(s.CreateTime) as CreateTime  FROM SC_ServiceCardType  AS S  where S.CardTypeName is not null  ";

                if (source > 0)
                {
                    sql += "  and S.CardType in ( select C.CardType from CustomCardInfo C  WHERE  C.CardType= S.CardType  CardSource=@CardSource   AND status=1 ";
                    if (iswx == 0)
                    {
                        sql += " and IsWeixin=@IsWeixin";
                    }
                    sql += ")";

                }
                sql += "  group by ActivityType ) as result order by result.CreateTime desc";
            }
            return DbHelp.Query<SCServiceCardType>(sql, new { CardSource = source, IsWeixin = iswx });

        }

        public IEnumerable<SCServiceCardType> GetActiveTagName()
        {
            string sql = "SELECT * FROM dbo.CustomCardInfo WHERE CardSource=1 and  [status]=1 ORDER BY CreateDate DESC ";
            return DbHelp.Query<SCServiceCardType>(sql);
        }

        /// <summary>
        /// 获取活动名称所属的卡券名称
        /// </summary>
        /// <param name="name">活动名称</param>
        /// <returns></returns>
        public IEnumerable<SCServiceCardType> GetScServiceCardTypeNameListByActivityType(string name)
        {
            string sql = "Select S.CardType,S.CardTypeName,S.ActivityType,s.code from  SC_ServiceCardType   AS S   WHERE   S.CardType IN ( SELECT C.CardType FROM CustomCardInfo  AS C   where C.status=1   )     AND S.ActivityType=@ActivityType  order by S.CreateTime asc ";
            return DbHelp.Query<SCServiceCardType>(sql, new { ActivityType = name });
        }

        

        /// <summary>
        /// 获取所有优惠券信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReturnCustomCardTypeModel> GetSCServiceCardTypeList()
        {
            string sql =
                "select s.CardType,s.CardTypeName as CardName,s.ActivityType as ActivityName from  SC_ServiceCardType S inner join  CustomCardInfo M    on   S.CardType = M.CardType  where m.status=1 and IsWeixin=0";
            return DbHelp.Query<ReturnCustomCardTypeModel>(sql);
        }


        public IEnumerable<SCServiceCardType> GetMerchantCardTypeList()
        {
            string sql =
                "select s.CardType,s.CardTypeName,s.ActivityType  from  SC_ServiceCardType S inner join  CustomCardInfo M    on   S.CardType = M.CardType  where m.status=1   and M.CardSource=2";
            return DbHelp.Query<SCServiceCardType>(sql);
        }


        public IEnumerable<ReturnCustomCardTypeModel> GetSummerCardList()
        {
            string sql =
                 "select s.CardType,s.CardTypeName as CardName,s.ActivityType as ActivityName from  SC_ServiceCardType S inner join  CustomCardInfo M    on   S.CardType = M.CardType  where m.status=1 and IsWeixin=0  and Type=2";
            return DbHelp.Query<ReturnCustomCardTypeModel>(sql);
        }
    }
}
