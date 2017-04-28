using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 图片赞美记录操作
    /// </summary>
    public interface IImgPraiseRecordStorager
    {
        /// <summary>
        /// 添加图片赞美记录
        /// </summary>
        /// <param name="data"></param>
        void Add(ImagePraiseRecord data);

        /// <summary>
        /// 获取图片赞美个数
        /// </summary>
        /// <param name="imgId"></param>
        /// <returns></returns>
        int FindCount(int imgId);
    }
}
