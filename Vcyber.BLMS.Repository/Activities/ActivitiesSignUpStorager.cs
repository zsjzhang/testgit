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
    public class ActivitiesSignUpStorager : IActivitiesSignUpStorager
    {
        public int SignUpActivities(ActivitiesSignUp entity)
        {
            string sql = "Insert into ActivitiesSignUp(ActivitiesId ,UserId ,UserName ,CreateTime ,IsDeleted) values(@ActivitiesId,@UserId,@UserName,@CreateTime,@IsDeleted);SELECT @@identity";
            return DbHelp.ExecuteScalar<int>(sql, new { @ActivitiesId = entity.ActivitiesId, @UserId = entity.UserId, @UserName = entity.UserName, @CreateTime = DateTime.Now, @IsDeleted = 0 });

        }
        public IEnumerable<ActivitiesSignUp> GetSignUpList(int id, string userName, int? start, int? count, out int total)
        {

            string sql = "SELECT top {0} * FROM ActivitiesSignUp WHERE IsDeleted=0 and ActivitiesId={3} and UserName like '%{2}%' and id not in (select top {1} id from ActivitiesSignUp where IsDeleted=0 and ActivitiesId={3} and UserName like '%{2}%') ORDER BY CreateTime DESC";
            sql = string.Format(sql, count??0, start ?? 0,userName,id);
            total = DbHelp.ExecuteScalar<int>(string.Format("Select count(*) from ActivitiesSignUp where IsDeleted=0 and userName like '%{0}%' and ActivitiesId={1}", userName, id));

            return DbHelp.Query<ActivitiesSignUp>(sql);
        }

        public IEnumerable<ActivitiesSignUp> GetSignUpListByUserId(string userId, int? start, int? count, out int total)
        {

            string sql = "SELECT top {0} * FROM ActivitiesSignUp WHERE UserId= '{2}' and id not in (select top {1} id from ActivitiesSignUp WHERE UserId= '{2}') ORDER BY CreateTime DESC";
            sql = string.Format(sql, count ?? 0, start ?? 0, userId);
            total = DbHelp.ExecuteScalar<int>(string.Format("Select count(*) from ActivitiesSignUp where UserId= '{0}'", userId));

            return DbHelp.Query<ActivitiesSignUp>(sql);
        }


        public bool CancelSignUp(int id, int userId)
        {
            string sql = "Update ActivitiesSignUp set IsDeleted =@IsDeleted where ActivitiesId=@ActivitiesId and UserId=@UserId";
            return DbHelp.Execute(sql, new
            {
                @IsDeleted=1,
                @ActivitiesId = id,
                @UserId=userId
            }) > 0;
        }

        public ActivitiesSignUp GetItemByUserIdAndActivitiesId(string userId, int activitiesId)
        {
            string sql = "SELECT * FROM ActivitiesSignUp WHERE ActivitiesId=@ActivitiesId and UserId=@UserId";

            return DbHelp.QueryOne<ActivitiesSignUp>(sql, new { @ActivitiesId =activitiesId,@UserId=userId});
        }
    }
}
