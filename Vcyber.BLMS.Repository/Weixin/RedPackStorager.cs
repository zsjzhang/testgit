using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vcyber.BLMS.Entity.Weixin;
using Vcyber.BLMS.IRepository.Weixin;
using Vcyber.BLMS.Repository.Entity.Generated;

namespace Vcyber.BLMS.Repository.Weixin
{
    public class RedPackStorager : IRedPackStorager
    {
        /// <summary>
        /// 添加红包领取记录
        /// </summary>   
        public int AddRecord(RedPackRecord obj)
        {
            var sql = Sql.Builder
                .Append("INSERT INTO dbo.WX_RedPackRecord(RedPackId,CardName,UserId,OpenId,Amount,TradeNo,PaymentNo,ResultCode,ErrorCode,Source,PaymentTime,CreateTime)")
                .Append("VALUES(@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11)", obj.RedPackId,obj.CardName, obj.UserId, obj.OpenId, obj.Amount, obj.TradeNo, obj.PaymentNo,obj.ResultCode,obj.ErrorCode,obj.Source,obj.PaymentTime,obj.CreateTime);
            var id = (int)PocoHelper.DBContext().Execute(sql);
            return id;
        }
        /// <summary>
        /// 修改红包记录
        /// </summary>
        public void UpdateRedPackRecord(RedPackRecord obj) 
        {
            var sql = Sql.Builder
                .Append("UPDATE dbo.WX_RedPackRecord")
                .Append("SET TradeNo = @0,PaymentNo = @1,ResultCode = @2,ErrorCode = @3,Source = @4 ", obj.TradeNo, obj.PaymentNo, obj.ResultCode, obj.ErrorCode, obj.Source)
                .Append("WHERE OpenId = @0 AND YEAR(CreateTime) = @1 AND MONTH(CreateTime) = @2 AND DAY(CreateTime) = @3", obj.OpenId,obj.CreateTime.Year,obj.CreateTime.Month,obj.CreateTime.Day);
            var id = (int)PocoHelper.DBContext().Execute(sql);                  
        }
        /// <summary>
        /// 根据场景值查询红包活动
        /// </summary>
        public RedPack BySceneId(string sceneId)
        {
            var sql = Sql.Builder.Append("SELECT * FROM dbo.WX_RedPack AS wrp WHERE SceneId = @0",sceneId);
            return PocoHelper.DBContext().Query<RedPack>(sql).FirstOrDefault();
        }
        /// <summary>
        /// 根据UserId查询用户领取的一个卡券
        /// </summary>
        /// <param name="redPackId">红包ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>红包记录</returns>
        public RedPackRecord RedPacCardByUserId(int redPackId,string userId) 
        {
            var sql = Sql.Builder
                .Append("SELECT TOP 1 wrpr.Id,wrpr.RedPackId,wrpr.CardName,wrpr.UserId,wrpr.OpenId,wrpr.Amount,")
                .Append("wrpr.TradeNo,wrpr.PaymentNo,wrpr.ResultCode,wrpr.ErrorCode,wrpr.Source,wrpr.PaymentTime,wrpr.CreateTime")
                .Append("FROM dbo.WX_RedPackRecord AS wrpr WHERE RedPackId = @0 AND UserId = @1 AND CardName IS NOT NULL AND CardName <> ''", redPackId, userId);
            return PocoHelper.DBContext().Query<RedPackRecord>(sql).FirstOrDefault();
        }
        /// <summary>
        /// 根据UserId用户今天有没有记录
        /// </summary>
        public int RedPackCountByUserId(int redPackId, string userId, string date)
        {
            var d = DateTime.Parse(date);
            var sql = Sql.Builder
                .Append("SELECT COUNT(0) FROM dbo.WX_RedPackRecord AS wrpr ")
                .Append("WHERE RedPackId = @0 AND UserId = @1 AND YEAR(CreateTime) = @2 AND MONTH(CreateTime) = @3 AND DAY(CreateTime) = @4", redPackId, userId, d.Year, d.Month, d.Day);
            return PocoHelper.DBContext().ExecuteScalar<int>(sql);
        }
    }
}