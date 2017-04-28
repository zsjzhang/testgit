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
    public class ShareResourcesStorager : IShareResourcesStorager
    {

        public int AddShareRes(BLMS.Entity.ShareResources shareRes)
        {
            string sql = "Insert into ShareResource(Title,SubTitle,Summary,CreateTime,IsDisplay,LinkUrl,FileType,Category,ListImageUrl,PlayImageUrl) values(@Title,@SubTitle,@Summary,@CreateTime,@IsDisplay,@LinkUrl,@FileType,@Category,@ListImageUrl,@PlayImageUrl);SELECT @@identity";
            return DbHelp.Execute(sql, new { @Title = shareRes.Title,@SubTitle=shareRes.SubTitle, @Summary = shareRes.Summary, @CreateTime = DateTime.Now, @IsDisplay = shareRes.IsDisplay, @LinkUrl = shareRes.LinkUrl, @FileType = shareRes.FileType, @Category = shareRes.Category, @ListImageUrl = shareRes.ListImageUrl, @PlayImageUrl = shareRes.PlayImageUrl });
        }

        public int UpdateShareRes(BLMS.Entity.ShareResources shareRes)
        {
            string sql = "Update ShareResource set Title=@Title,SubTitle=@SubTitle,Summary=@Summary,UpdateTime=@UpdateTime,IsDisplay=@IsDisplay,FileType=@FileType,Category=@Category,LinkUrl = @LinkUrl, ListImageUrl=@ListImageUrl,PlayImageUrl=@PlayImageUrl where Id=@Id";
            return DbHelp.Execute(sql, new { @Id = shareRes.Id, @Title = shareRes.Title, @SubTitle = shareRes.SubTitle, @Summary = shareRes.Summary, @UpdateTime = DateTime.Now, @IsDisplay = shareRes.IsDisplay, @FileType = shareRes.FileType, @Category = shareRes.Category, @LinkUrl = shareRes.LinkUrl, @ListImageUrl = shareRes.ListImageUrl, @PlayImageUrl = shareRes.PlayImageUrl });
        }

        public int DelShareResByID(int id)
        {
            string sql = "Update ShareResource set IsDeleted=1 where Id=@Id";
            return DbHelp.Execute(sql, new { @Id = id });
        }

        public BLMS.Entity.ShareResources GetShareResById(int id)
        {
            string sql = "SELECT * FROM ShareResource WHERE Id = @Id";
            return DbHelp.QueryOne<ShareResources>(sql, new { @Id = id });
        }

        public BLMS.Entity.ShareResources GetNewShareRes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BLMS.Entity.ShareResources> GetShareRes(object queryObj, int fileType, string category, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = " SELECT top {0} * FROM ShareResource WHERE isDisplay=1 $where and isDeleted=0 and id not in (select top {1} id from ShareResource WHERE isDisplay=1 $where and isDeleted=0 ORDER BY CreateTime desc) ORDER BY CreateTime desc ";
            sql = string.Format(sql, pageSize, pageIndex);

            string condition = string.Empty;
            if (fileType > 0)
            {
                condition = " AND FileType = @FileType ";
            }

            if (!string.IsNullOrEmpty(category))
            {
                condition += " AND Category = @Category ";
            }

            var totalsql = string.Format("Select count(1) from ShareResource WHERE isDisplay=1 and isDeleted=0 $where ");

            sql = sql.Replace("$where", condition);
            totalsql = totalsql.Replace("$where", condition);

            totalCount = DbHelp.ExecuteScalar<int>(totalsql, new { @FileType = fileType, @Category = category });
            return DbHelp.Query<ShareResources>(sql, new { @FileType = fileType, @Category = category });
        }

        public IEnumerable<BLMS.Entity.ShareResources> GetShareRes(object queryObj)
        {
            throw new NotImplementedException();
        }
    }
}
