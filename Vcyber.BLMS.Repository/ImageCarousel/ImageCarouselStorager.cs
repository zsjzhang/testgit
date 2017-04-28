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
    public class ImageCarouselStorager : IImageCarouselStorager
    {
        #region ==== 构造函数 ====

        public ImageCarouselStorager()
        { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 根据Id，获取一个轮播图片实体
        /// </summary>
        /// <param name="id">图片Id</param>
        /// <returns></returns>
        public ImageCarousel GetImgCarouselById(int id)
        {
            string sql = "SELECT * FROM ImageCarousel " +
                         "where Id = @Id and IsDeleted=@IsDeleted "; 
            return DbHelp.QueryOne<ImageCarousel>(sql, new { @Id = id, @IsDeleted = 0 });

        }

        public IEnumerable<ImageCarousel> GetImageCarouselList(int? approveStatus,int? type, int? start, int? count, out int totalCount)
        {
            StringBuilder conditionStr = new StringBuilder();
            
            if (approveStatus != null && approveStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsApproved={0}", approveStatus);

            }

            if (type != null && type >= 0)
            {

                conditionStr.AppendFormat(" and type={0}", type);

            }

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from ImageCarousel where IsDeleted=0 ");
            sql.Append(conditionStr);
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();

            sql.AppendFormat("SELECT top {0} * FROM ImageCarousel WHERE IsDeleted=0", count);
            sql.Append(conditionStr);
            sql.AppendFormat(" and id not in (select top {0} id from ImageCarousel WHERE IsDeleted=0", start);
            sql.Append(conditionStr);
            sql.Append(" ORDER BY Priority desc,CreateTime DESC) ORDER BY Priority desc,CreateTime DESC");
            return DbHelp.Query<ImageCarousel>(sql.ToString());
     
        }
        /// <summary>
        /// 创建一个轮播图片
        /// </summary>
        /// <param name="entity">图片实体</param>
        /// <returns></returns>
        public int CreateImgCarousel(ImageCarousel entity)
        {
            string sql = "Insert into ImageCarousel" +
                         "(Title,ImageUrl,LinkUrl,Priority,NewPage," +
                         "CreateBy,CreateTime,UpdateBy,UpdateTime,IsDeleted,IsApproved,Type) " +
                         "values" +
                         "(@Title,@ImageUrl,@LinkUrl,@Priority,@NewPage," +
                         "@CreateBy,@CreateTime,@UpdateBy,@UpdateTime,@IsDeleted,@IsApproved,@Type);SELECT @@identity";
            return DbHelp.ExecuteScalar<int>(sql, new
            {
                entity.Title, 
                entity.ImageUrl, 
                entity.LinkUrl, 
                entity.Priority,
                entity.NewPage, 
                entity.CreateBy,
                @CreateTime = DateTime.Now, 
                entity.UpdateBy,
                @UpdateTime = DateTime.Now,
                @IsDeleted = 0,
                @IsApproved = (int)EApproveStatus.NoBegin,
                @Type = entity.Type
            });

        }

        /// <summary>
        /// 更新一个轮播图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateImgCarousel(ImageCarousel entity)
        {
            string sql = "UPDATE ImageCarousel SET Title=@Title,ImageUrl=@ImageUrl,LinkUrl=@LinkUrl,NewPage= @NewPage,Priority=@Priority,UpdateTime=@UpdateTime,UpdateBy=@UpdateBy,IsApproved=@IsApproved,ApprovedBy=@ApprovedBy,ApprovedTime=@ApprovedTime,Type=@Type WHERE Id=@Id";
            var approvestatus = entity.IsApproved;
            var apporveBy = entity.ApprovedBy;
            var approveTime = entity.ApprovedTime;
            
            if (approvestatus != (int)EApproveStatus.NoBegin)
            {
                approvestatus = (int)EApproveStatus.NoBegin;
                apporveBy = entity.UpdateBy;
                approveTime = DateTime.Now;
            }

            if (approveTime<=DateTime.MinValue)
            {
                approveTime = (DateTime)SqlDateTime.MinValue;
            }
            return DbHelp.Execute(sql, new
            {
                entity.Title,
                entity.ImageUrl,
                entity.LinkUrl,
                entity.NewPage,
                entity.Id,
                entity.Priority,
                @UpdateTime = DateTime.Now,
                entity.UpdateBy,
                @IsApproved = approvestatus,
                @ApprovedBy = apporveBy,
                @ApprovedTime = approveTime,
                @Type = entity.Type
            }) > 0;

        }

        /// <summary>
        /// 通过Id，删除响应的轮播图片
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteImgCarousel(int Id,string name)
        {
            try
            {
                string sql = "Update ImageCarousel set IsDeleted=1,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime " +
                             "WHERE ImageCarousel.Id=@Id AND IsDeleted=0";
                return DbHelp.Execute(sql, new { @Id = Id, @UpdateBy=name,@UpdateTime=DateTime.Now}) > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetMaxPriority()
        {
            string sql = "SELECT Max(Priority) FROM IMAGECAROUSEL WHERE IsDeleted=0";
            return DbHelp.ExecuteScalar<int>(sql);

        }

        public int GetMinPriority()
        {
            string sql = "SELECT Min(Priority) FROM IMAGECAROUSEL WHERE IsDeleted=0";
            return DbHelp.ExecuteScalar<int>(sql);

        }

        public ImageCarousel GetPreEntity(ImageCarousel entity)
        {
            string sql = "SELECT top 1 * FROM IMAGECAROUSEL WHERE Priority<@Priority and IsDeleted=0 order by Priority desc";
            return DbHelp.QueryOne<ImageCarousel>(sql, new { @Priority = entity.Priority });
        }
        public ImageCarousel GetNextEntity(ImageCarousel entity)
        {
            string sql = "SELECT top 1 * FROM IMAGECAROUSEL WHERE Priority>@Priority and IsDeleted=0 order by Priority";
            return DbHelp.QueryOne<ImageCarousel>(sql, new { @Priority = entity.Priority });
        }

        public bool ApprovedImageCarousel(int id, string userId, int status)
        {
            string sql = "Update IMAGECAROUSEL set IsApproved = @IsApproved,ApprovedBy=@ApprovedBy,ApprovedTime=@ApprovedTime where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @IsApproved = status, @ApprovedBy = userId, @ApprovedTime = DateTime.Now }) > 0;

        }

        /// <summary>
        /// 获取轮播图审批列表
        /// </summary>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<ImageCarousel> GetImageCarousel(int status, int pageIndex, int pageSize, out int totalCount)
        {
            var whereStr = "";
            if (status >= 0)
            {
                whereStr = "IsApproved=" + status + " and";
            }
            string sql = "SELECT top {0} * FROM ImageCarousel WHERE " + whereStr +
                         " IsDeleted=0 and id not in (select top {1} id from ImageCarousel WHERE " + whereStr + " IsDeleted=0 ORDER BY Priority,CreateTime desc) ORDER BY Priority,CreateTime desc";
            sql = string.Format(sql, pageSize, pageIndex, status);
            var totalsql = string.Format("Select count(*) from ImageCarousel WHERE {0} IsDeleted=0", whereStr);
            totalCount = DbHelp.ExecuteScalar<int>(totalsql);
            return DbHelp.Query<ImageCarousel>(sql);
        }

        public bool UpdatePriority(int id, int priority, string operatorName)
        {
            string sql = "Update ImageCarousel set Priority = @Priority,UpdateTime=@UpdateTime,UpdateBy=@UpdateBy where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @Priority = priority, @UpdateBy = operatorName, @UpdateTime = DateTime.Now }) > 0;

        }
        #endregion
    }
}
