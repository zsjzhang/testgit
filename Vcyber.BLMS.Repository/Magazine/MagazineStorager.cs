using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;   
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class MagazineStorager : IMagazineStorager
    {
        #region ==== 构造函数 ====

        public MagazineStorager()
        { }

        #endregion



        /// <summary>
        /// 根据Id，获取一个杂志实体
        /// </summary>
        /// <param name="id">杂志Id</param>
        /// <returns></returns>
        public Magazine GetMagazineById(int id)
        {
            string sql = "SELECT * FROM Magazine where Id = @Id and IsDeleted=@IsDeleted";
            return DbHelp.QueryOne<Magazine>(sql, new { @Id = id, @IsDeleted = 0 });

        }

        public IEnumerable<Magazine> GetMagazineList(int? approveStatus, int? year, int? month, string name, int start, int count, out int total)
        {
            string appendStr = "";

            if (year != null && year > 0)
            {
                appendStr += " and Year=" + year;
            }
            if (month != null  && month > 0)
            {
                appendStr += " and Month=" + month;
            }

            if (approveStatus != null && approveStatus >=0)
            {
                appendStr += " and IsApproved=" + approveStatus;
            }

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT top {0} * FROM Magazine WHERE TITLE LIKE '%{1}%' AND ISDELETED=0",count,name);
            sql.Append(appendStr);
            sql.AppendFormat(" and id not in (select top {0} id from Magazine WHERE TITLE LIKE '%{1}%' AND ISDELETED=0",start,name);
            sql.Append(appendStr);
            sql.Append(" ORDER BY Year desc,Month desc,CreateTime desc) ORDER BY Year desc,Month desc,CreateTime desc");


            StringBuilder totalSql = new StringBuilder();
            totalSql.AppendFormat("Select count(*) from Magazine WHERE Title like '%{0}%' and IsDeleted=0",name);
            totalSql.Append(appendStr);

            total = DbHelp.ExecuteScalar<int>(totalSql.ToString());
            return DbHelp.Query<Magazine>(sql.ToString());
        }

        public int Create(Magazine entity)
        {
            string sql = "Insert into Magazine" +
                         "(Title,ImageUrl,LinkUrl,Summary,Year," +
                         "Month,CreateTime,CreateBy,UpdateTime,UpdateBy,I" +
                         "sDeleted,IsApproved,IsDisplay,QuestionUrl,ResultUrl,ReadLink) " +
                         "values" +
                         "(@Title,@ImageUrl,@LinkUrl,@Summary,@Year," +
                         "@Month,@CreateTime,@CreateBy,@UpdateTime," +
                         "@UpdateBy,@IsDeleted,@IsApproved,@IsDisplay,@QuestionUrl,@ResultUrl,@ReadLink);SELECT @@identity";
            return DbHelp.ExecuteScalar<int>(sql, new
            {
                entity.Title,
                entity.ImageUrl, 
                entity.LinkUrl,
                entity.Summary, 
                entity.Year, 
                entity.Month, 
                @CreateTime = DateTime.Now, 
                entity.CreateBy, 
                @UpdateTime = DateTime.Now, 
                entity.UpdateBy, 
                @IsDeleted = 0,
                entity.IsApproved, 
                entity.IsDisplay,
                entity.QuestionUrl,
                entity.ResultUrl,
                entity.ReadLink
            });

        }

        /// <summary>
        /// 更新一个报刊
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(Magazine entity)
        {
            try
            {
                string sql = "UPDATE Magazine SET Title=@Title,ImageUrl=@ImageUrl,LinkUrl=@LinkUrl," +
                             "Summary=@Summary,Year=@Year,Month=@Month,UpdateTime=@UpdateTime," +
                             "UpdateBy=@UpdateBy,IsDeleted=@IsDeleted,IsDisplay=@IsDisplay," +
                             "IsApproved=@IsApproved,ApprovedBy=@ApprovedBy,ApprovedTime=@ApprovedTime,QuestionUrl=@QuestionUrl,ResultUrl=@ResultUrl,ReadLink=@ReadLink " +
                             "WHERE Id=@Id";
                var approvestatus = entity.IsApproved;
                var apporveBy = entity.ApprovedBy;
                var approveTime = entity.ApprovedTime;

                if (approvestatus != (int)EApproveStatus.NoBegin)
                {
                    approvestatus = (int)EApproveStatus.NoBegin;
                    apporveBy = entity.UpdateBy;
                    approveTime = DateTime.Now;
                } 
                if (approveTime <= DateTime.MinValue)
                {
                    approveTime = (DateTime)SqlDateTime.MinValue;
                }
                return DbHelp.Execute(sql, new
                {
                   entity.Title,
                   entity.ImageUrl, 
                   entity.LinkUrl,
                   entity.Summary, 
                   entity.Year, 
                   entity.Month, 
                   entity.Id, 
                   entity.UpdateTime,
                   entity.UpdateBy, 
                   entity.IsDeleted,
                   entity.IsDisplay,
                   @IsApproved = approvestatus,
                   @ApprovedBy = apporveBy,
                   @ApprovedTime = approveTime,
                   entity.QuestionUrl,
                   entity.ResultUrl,
                   entity.ReadLink
                }) > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 通过Id，删除响应的轮播图片
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Delete(int Id,string name)
        {
            try
            {
                string sql = "Update Magazine set IsDeleted=1,IsDisplay=0,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime WHERE Id=@Id AND IsDeleted=0";
                return DbHelp.Execute(sql, new { @Id = Id,@UpdateBy=name,@UpdateTime=DateTime.Now  }) > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取报刊审批列表
        /// </summary>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<Magazine> GetMagazine(int? status, int pageIndex, int pageSize, out int totalCount)
        {
            var whereStr = "";
            if (status!=null &&status >= 0)
            {
                whereStr = "IsApproved=" + status + " and";
            }
            string sql = "SELECT top {0} * FROM Magazine WHERE " + whereStr +
                         " IsDeleted=0 and id not in (select top {1} id from Magazine WHERE " + whereStr + " IsDeleted=0 ORDER BY CreateTime desc) ORDER BY CreateTime desc";
            sql = string.Format(sql, pageSize, pageIndex, status);
            var totalsql = string.Format("Select count(*) from Magazine WHERE {0} IsDeleted=0", whereStr);
            totalCount = DbHelp.ExecuteScalar<int>(totalsql);
            return DbHelp.Query<Magazine>(sql);
        }
        public bool ApprovedMagazine(int id, string userId, int status)
        {
            string sql = "Update Magazine set IsApproved = @IsApproved,ApprovedBy=@ApprovedBy,ApprovedTime=@ApprovedTime,UpdateTime=@UpdateTime where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @IsApproved = status, @ApprovedBy = userId, @ApprovedTime = DateTime.Now, @UpdateTime = DateTime.Now }) > 0;
        }



        public IEnumerable<Magazine> GetMagazineAll()
        {
            StringBuilder sql = new StringBuilder("select * from Magazine where IsApproved=1 and IsDisplay=1 order by year desc, month desc ");
            return DbHelp.Query<Magazine>(sql.ToString());
        }
    }
}