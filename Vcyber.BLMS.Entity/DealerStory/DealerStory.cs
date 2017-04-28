using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 车主故事--实体
    /// </summary>
    public class DealerStory
    {
        /// <summary>
        /// 帖子ID
        /// </summary>
        public string id
        {
            get;
            set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            set;
            get;
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Contents
        {
            set;
            get;
        }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string image
        {
            set;
            get;
        }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime
        {
            set;
            get;
        } 

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime
        {
            set;
            get;
        }

        /// <summary>
        /// 时间专成string(页面显示的时间)
        /// </summary>
        public string StrCreatetime
        {
            get { return this.UpdateTime.ToString("yyyy-MM-dd HH:mm"); }
        }
    }

}
