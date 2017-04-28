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
    public class QuestionStorager : IQuestionStorager
    {
        public IEnumerable<BLMS.Entity.Question> GetQuestionsByPId(int pid)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from CS_Question where State=1 and ParentId={0} order by Sort asc", pid);
            return DbHelp.Query<BLMS.Entity.Question>(sql.ToString());
        }

        public IEnumerable<BLMS.Entity.Question> GetAllQuestion()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from CS_Question");
            return DbHelp.Query<BLMS.Entity.Question>(sql.ToString());
        }

        public int Create(Question entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into CS_Question (QContent,Type,State,ParentId,Sort,IsRequired,Cycle)");
            sql.Append(" values (@QContent,@Type,@State,@ParentId,@Sort,@IsRequired,@Cycle);select @@identity;");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
        }

        public bool Edit(Question entity)
        {
            string sql = "update CS_Question set QContent=@QContent,Type=@Type,State=@State,ParentId=@ParentId,Sort=@Sort,IsRequired=@IsRequired where Id=@Id";
            bool result = DbHelp.Execute(sql, entity) > 0;
            return result;
        }

        public bool Delete(int id)
        {
            return DbHelp.Execute("update CS_Question set State=@State where Id=@Id", new { State = 0, Id = id }) > 0;
        }


        public bool EditContent(string content, int id)
        {
            return DbHelp.Execute("update CS_Question set QContent=@QContent where Id=@Id", new { QContent = content, Id = id }) > 0;
        }

        public bool EditSort(int sort, int id)
        {
            return DbHelp.Execute("update CS_Question set Sort=@Sort where Id=@Id", new { Sort = sort, Id = id }) > 0;
        }


        public bool EditBatchSort(int sort, int ySort, int parentId)
        {
            bool result = true;
            string sqlStr = "select * from CS_Question where ParentId=@ParentId and State = 1 and ";
            string sqlHz = string.Empty;
            bool isUp = false;
            if (sort < ySort)
            {
                sqlHz = "Sort<@ySort and Sort>=@sort";
                isUp = true;
            }
            else
            {
                sqlHz = "Sort<=@sort and Sort>@ySort";
                isUp = false;
            }
            IEnumerable<Question> qList = DbHelp.Query<Question>(sqlStr + sqlHz, new { ParentId = parentId, ySort = ySort, sort = sort });
            if (qList.Count() > 0)
            {
                StringBuilder updateStr = new StringBuilder();
                foreach (Question q in qList)
                {
                    if (isUp)
                    {
                        updateStr.AppendFormat("update CS_Question set Sort={0} where Id={1}", (q.Sort + 1), q.Id);
                    }
                    else
                    {
                        updateStr.AppendFormat("update CS_Question set Sort={0} where Id={1}", (q.Sort - 1), q.Id);
                    }
                }
                result = DbHelp.Execute(updateStr.ToString()) > 0;
            }
            return result;
        }

        public bool DelBatchSort(int sort, int parentId)
        {
            bool result = true;
            string sqlStr = "select * from CS_Question where ParentId=@ParentId and State = 1 and Sort > @Sort";
            IEnumerable<Question> qList = DbHelp.Query<Question>(sqlStr, new { ParentId = parentId, Sort = sort });
            if (qList.Count() > 0)
            {
                StringBuilder updateStr = new StringBuilder();
                foreach (Question q in qList)
                {
                    updateStr.AppendFormat("update CS_Question set Sort={0} where Id={1}", (q.Sort - 1), q.Id);
                }
                result = DbHelp.Execute(updateStr.ToString()) > 0;
            }
            return result;
        }

        public bool EditIsRequired(bool isRequired, int id)
        {
            return DbHelp.Execute("update CS_Question set IsRequired=@IsRequired where Id=@Id", new { IsRequired = isRequired, Id = id }) > 0;
        }


        public Question GetById(int id)
        {
            string sql = "select * from CS_Question where Id=@Id";
            Question tempData = DbHelp.QueryOne<Question>(sql, new { Id = id });
            return tempData;
        }

        public IEnumerable<Question> GetJzChildrenQuestion(int parentId)
        {
            string sqlStr = "select * from CS_Question where ParentId=@ParentId and State = 1 and Type=@Type order by Sort asc";
            return DbHelp.Query<Question>(sqlStr, new { ParentId = parentId, Type = Convert.ToInt32(EQuestionType.JzChildren) });
        }

        public bool AddCycleOption(int id, int count)
        {
            string sqlStr = "update CS_Question set Cycle=@Cycle where Id=@Id";
            return DbHelp.Execute(sqlStr, new { Cycle = count, Id = id }) > 0;
        }

        public bool EditQuestionTextIsBefore(bool textIsBefore, int id)
        {
            return DbHelp.Execute("update CS_Question set TextIsBefore=@TextIsBefore where Id=@Id", new { TextIsBefore = textIsBefore, Id = id }) > 0;
        }

        public bool EditQuestionIsChecked(bool isChecked, int id)
        {
            return DbHelp.Execute("update CS_Question set IsChecked=@IsChecked where Id=@Id", new { IsChecked = isChecked, Id = id }) > 0;
        }
    }
}
