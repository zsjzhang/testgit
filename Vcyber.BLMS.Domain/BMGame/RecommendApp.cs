using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
namespace Vcyber.BLMS.Domain
{
    public class RecommendApp:IRecommendApp
    {
        /// <summary>
        /// 创建一条推荐
        /// </summary>
        /// <param name="recommend">推荐</param>
        /// <returns>影响的行数</returns>
        public int Add(Recommend recommend)
        {
            string sql = @"INSERT INTO Recommend(OpenId,ActivityType,Name,PhoneNumber,DataSource,Created,UserId,UserName)
VALUES(@OpenId,@ActivityType,@Name,@PhoneNumber,@DataSource,@Created,@UserId,@UserName) SELECT @@IDENTITY";
            return DbHelp.ExecuteScalar<int>(sql, new
            {
                @OpenId = recommend.OpenId,
                @ActivityType = recommend.ActivityType,
                @Name = recommend.Name,
                @PhoneNumber = recommend.PhoneNumber,
                @DataSource = recommend.DataSource,
                @Created = DateTime.Now,
                @UserId = recommend.UserId,
                @UserName = recommend.UserName
            });
        }
        /// <summary>
        /// 查询一条推荐
        /// </summary>
        /// <param name="id">推荐Id</param>
        /// <returns>推荐内容</returns>
        public Recommend Find(int id)
        {
            string sql = @"SELECT * FROM dbo.Recommend AS r WHERE Id = @Id";
            return DbHelp.QueryOne<Recommend>(sql, new { @Id = id });
        }
        /// <summary>
        /// 参与推荐的人数
        /// </summary>
        /// <param name="activityType">活动</param>
        /// <returns>数量</returns>
        public int Count(string activityType)
        {
            string sql = @"SELECT COUNT(0) FROM dbo.Recommend AS r WHERE ActivityType = @ActivityType";
            return DbHelp.ExecuteScalar<int>(sql, new { @ActivityType = activityType });
        }
        /// <summary>
        /// 参与推荐的人数
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns>是否</returns>
        public bool IsPhoneExist(string phone)
        {
            string sql = @"SELECT COUNT(0) FROM dbo.Recommend AS r WHERE r.PhoneNumber = @PhoneNumber";
            return DbHelp.ExecuteScalar<int>(sql, new { PhoneNumber = phone }) > 0;
        }
    }
}
