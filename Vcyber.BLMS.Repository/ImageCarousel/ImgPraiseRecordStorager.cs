using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 图片赞美记录操作
    /// </summary>
    public class ImgPraiseRecordStorager : IImgPraiseRecordStorager
    {
        #region ==== 构造函数 ====

        public ImgPraiseRecordStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加图片赞美记录
        /// </summary>
        /// <param name="data"></param>
        public void Add(ImagePraiseRecord data)
        {
            data.Id = Guid.NewGuid().ToString();
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into ImagePraiseRecord(Id,ImgId,MemberId,CreateTime,Remark)");
            sql.Append(" values(@Id,@ImgId,@MemberId,@CreateTime,@Remark)");

            DbHelp.Execute(sql.ToString(), data);

            sql.Clear();

            sql.Append("update ImageCarousel set PraiseCount=ISNULL(PraiseCount,0)+1 where Id=@Id");
            DbHelp.Execute(sql.ToString(), new { Id = data.ImgId });
        }

        /// <summary>
        /// 获取图片赞美个数
        /// </summary>
        /// <param name="imgId"></param>
        /// <returns></returns>
        public int FindCount(int imgId)
        {
            return DbHelp.ExecuteScalar<int>("select COUNT(1) from ImagePraiseRecord where ImagePraiseRecord.ImgId=@ImgId", new { ImgId=imgId });
        }

        #endregion
    }
}
