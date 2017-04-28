using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class AnswerStorager : IAnswerStorager
    {
        /// <summary>
        /// 添加答案记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddAnswer(BLMS.Entity.Answer entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into CS_Answer(MemberId,ParentId,AContent,State,OptionId)");
            sql.Append("values(@MemberId ,@ParentId ,@AContent ,@State ,@OptionId);SELECT @@identity");
            int id = DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
            return id;
        }

        /// <summary>
        /// 添加答案记录集合
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int AddAnswerRang(List<BLMS.Entity.Answer> entities)
        {
            int result = 0;
            if (entities == null || entities.Count == 0)
                return result;
            for (int i = 0; i < entities.Count; i++)
            {
                int tempint = AddAnswer(entities[i]);
                if (tempint > 0)
                    result++;
            }
            return result;
        }

        /// <summary>
        /// 判断用户是否完成过问卷
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool IsAnswer(string memberId, int pid)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(*) from CS_Answer where MemberId='{0}'", memberId);
            sql.Append(" and ParentId in(");
            sql.AppendFormat("select Id from dbo.CS_Question where ParentId='{0}')", pid);
            int tempInt = DbHelp.ExecuteScalar<int>(sql.ToString());
            if (tempInt > 0)
                return true;
            else
                return false;
        }
    }
}
