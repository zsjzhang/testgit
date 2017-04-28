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
    public class OptionStorager : IOptionStorager
    {
        /// <summary>
        /// 获取全部选项信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BLMS.Entity.Option> GetAllOptions()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from CS_Option");
            return DbHelp.Query<BLMS.Entity.Option>(sql.ToString());
        }

        /// <summary>
        /// 根据题号获取选项列表
        /// </summary>
        /// <param name="qid">题号</param>
        /// <returns></returns>
        public IEnumerable<BLMS.Entity.Option> GetOptionsByQId(int qid)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from CS_Option where State=1 and PartentId={0} and (OType is null or OType<>3) order by Sort asc", qid);
            return DbHelp.Query<BLMS.Entity.Option>(sql.ToString());
        }

        /// <summary>
        /// 根据题号和题目类型获取选项列表
        /// </summary>
        /// <param name="qid">题号</param>
        /// <returns></returns>
        public IEnumerable<BLMS.Entity.Option> GetOptionsByQIdAndOType(int qid, int oType)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from CS_Option where State=1 and PartentId={0} and OType={1} order by Sort asc", qid, oType);
            return DbHelp.Query<BLMS.Entity.Option>(sql.ToString());
        }
        #region ==== 后台 ====

        public int Create(Option entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into CS_Option (PartentId,OContent,Sort,State,OType)");
            sql.Append(" values (@PartentId,@OContent,@Sort,@State,@OType);select @@identity;");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
        }

        public bool Edit(Option entity)
        {
            string sql = "update CS_Option set PartentId=@PartentId,OContent=@OContent,Sort=@Sort,State=@State where Id=@Id";
            return DbHelp.Execute(sql, entity) > 0;
        }

        public bool Delete(int id)
        {
            return DbHelp.Execute("update CS_Option set State=@State where Id=@Id", new { State = 0, Id = id }) > 0;
        }

        public bool DelBatchSort(int sort, int parentId)
        {
            bool result = true;
            string sqlStr = "select * from CS_Option where PartentId=@PartentId and State = 1 and Sort > @Sort";
            IEnumerable<Option> oList = DbHelp.Query<Option>(sqlStr, new { PartentId = parentId, Sort = sort });
            if (oList.Count() > 0)
            {
                StringBuilder updateStr = new StringBuilder();
                foreach (Option o in oList)
                {
                    updateStr.AppendFormat("update CS_Option set Sort={0} where Id={1}", (o.Sort - 1), o.Id);
                }
                result = DbHelp.Execute(updateStr.ToString()) > 0;
            }
            return result;
        }

        public bool EditContent(string content, int id)
        {
            return DbHelp.Execute("update CS_Option set OContent=@OContent where Id=@Id", new { OContent = content, Id = id }) > 0;
        }

        public bool EditSort(int sort, int id, int parentId, int ySort)
        {
            string sqlStr = "update CS_Option set Sort=@Sort where PartentId=@PartentId and State = 1 and Sort=@ySort";
            bool result = false;
            if (DbHelp.Execute(sqlStr, new { Sort = ySort, PartentId = parentId, ySort = sort }) > 0)
            {
                result = DbHelp.Execute("update CS_Option set Sort=@Sort where Id=@Id", new { Sort = sort, Id = id }) > 0;
            }
            return result;
        }

        public int MaxOption(int parentId)
        {
            string sql = "select MAX(CAST(OContent AS int)) from CS_Option where PartentId = @PartentId and State = 1 and OType = 0";
            int result = DbHelp.ExecuteScalar<int>(sql, new { PartentId = parentId });
            return result;
        }

        public bool DeleteMaxOption(int parentId)
        {
            return DbHelp.Execute("update CS_Option set State = 0 where PartentId = @PartentId and State = 1 and OType = 0 and OContent = (select MAX(CAST(OContent AS int)) from CS_Option where PartentId = @PartentId and State = 1 and OType = 0)", new { PartentId = parentId }) > 0;
        }

        public bool EditType(int type,int id)
        {
            return DbHelp.Execute("update CS_Option set OType=@OType where Id=@Id", new { OType = type, Id = id }) > 0;
        }

        public bool EditValueType(int vType, int id)
        {
            return DbHelp.Execute("update CS_Option set OValueType=@OValueType where Id = @Id", new { OValueType = vType, Id = id }) > 0;
        }
        #endregion
    }
}
