using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class CustomCardMerchantConsumeCodeStorager : ICustomCardMerchantConsumeCodeStorager
    {
        /// <summary>
        /// 添加商户兑奖码
        /// </summary>
        /// <param name="model">商户兑奖码信息</param>
        /// <returns></returns>
        public bool AddCustomCardMerchantConsumeCode(CustomCardMerchantConsumeCode model)
        {
            string sql = "INSERT INTO CustomCardMerchantConsumeCode (CardType ,CardCode ,IsDel ) VALUES (@CardType,@CardCode,@IsDel )";
            return DbHelp.Execute(sql, model) > 0;
        }



        /// <summary>
        /// 获取一个商户卡券兑换码
        /// </summary>
        /// <param name="cardType">卡券GUID</param>
        /// <returns>一个商户卡券兑换码</returns>
        public CustomCardMerchantConsumeCode GetSingleCardMerchantConsumeCode(string cardType)
        {
            string sql = @"select TOP 1 C.Id, SUBSTRING(C.CardCode, LEN(C.CardType)+2, LEN(C.CardCode)- (LEN(C.CardType)+2)) as CardCode, C.CardType   from  CustomCardMerchantConsumeCode  C where C.CardType=@CardType   AND isDel =1
                            and C.CardCode  NOT in (select M.CardCode  FROM  CustomCard  M WHERE C.CardType=@CardType )  order by C.Id desc;";
            return DbHelp.QueryOne<CustomCardMerchantConsumeCode>(sql, new
            {
                CardType = cardType
            });
        }
        /// <summary>
        /// 获取商户卡券兑换券列表
        /// </summary>
        /// <param name="cardType">卡券GUID</param>
        /// <returns>商户卡券兑换券列表</returns>
        public IEnumerable<CustomCardMerchantConsumeCode> GetCardMerchantConsumeCodeList(string cardType, int pageSize, int pageIndex, out int totalCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@" SELECT * FROM (  SELECT C.ID,C.CardType, SUBSTRING(C.CardCode, LEN(C.CardType)+2, LEN(C.CardCode)- (LEN(C.CardType)+2)) as CardCode,C.isDel,C.CreateDate,M.CardName,M.ActivityType,M.MerchantName,  ROW_NUMBER() over(order by C.Id desc) as RowNumber,
                       CASE WHEN SendCard.id IS NULL THEN '未领取' ELSE '已领取' END SendStatus 
                         FROM  CustomCardMerchantConsumeCode AS  C   left join  CustomCardInfo AS M  ON C.CardType= M.CardType 
                         LEFT JOIN dbo.CustomCard AS SendCard
							 ON C.CardCode=SendCard.CardCode
                WHERE C.CardType=@CardType ");

            StringBuilder sqlCount = new StringBuilder("select count(*) from CustomCardMerchantConsumeCode    WHERE CardType=@CardType");
            sb.AppendFormat(" ) as RowNumber_Table where RowNumber between {0}  and {1};", (pageIndex - 1) * pageSize + 1, pageSize * pageIndex);
            totalCount = DbHelp.ExecuteScalar<int>(sqlCount.ToString(), new { CardType = cardType });
            return DbHelp.Query<CustomCardMerchantConsumeCode>(sb.ToString(), new { CardType = cardType, PageSize = pageSize, PageCount = (pageSize * pageIndex) });
        }
        /// <summary>
        /// 获取商户卡券兑换码
        /// </summary>
        /// <param name="cardType">卡券GUID</param>
        /// <param name="type">使用状态 1：未使用，2：已使用</param>
        /// <returns></returns>
        public int GetCardMerchantConsumeCodeCount(string cardType, int type)
        {
            string sql =
                @"select count(*) from  CustomCardMerchantConsumeCode  C where C.CardType=@CardType   AND C.isDel =1 ";
            if (type == 1)
            {
                sql += "and C.CardCode  NOT in (select M.CardCode  FROM  CustomCard  M WHERE C.CardType=@CardType ) ";
            }
            else if (type == 2)
            {
                sql += "and C.CardCode   in (select M.CardCode  FROM  CustomCard  M WHERE C.CardType=@CardType ) ";
            }
            return DbHelp.ExecuteScalar<int>(sql, new { CardType = cardType });
        }





        public CustomCardMerchantConsumeCode GetSingleCardMerchantConsumeCodeByCode(string cardType, string code)
        {
            string sql =
                @"select * from  CustomCardMerchantConsumeCode  where CardType=@CardType   AND isDel =1 AND  CardCode=@CardCode   ";
            return DbHelp.QueryOne<CustomCardMerchantConsumeCode>(sql, new
            {
                CardType = cardType,
                CardCode = code
            });
        }
    }
}
