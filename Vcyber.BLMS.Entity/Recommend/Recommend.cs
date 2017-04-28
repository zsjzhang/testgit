using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class Recommend
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        [Required]
        public string OpenId { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        [Required]
        public string ActivityType { get; set; }


        //被推荐人的信息
        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 昵称/用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }
    }

    public class RecommendViewModel
    {
        public string OpenId { get; set; }

        //被推荐人的信息
        public string Name { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class RecommendString
    {
        public string OpenId { get; set; }

        //被推荐人的信息
        public string Name1 { get; set; }

        public string PhoneNumber1 { get; set; }

        public string Name2 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string Name3 { get; set; }

        public string PhoneNumber3 { get; set; }
    }
}
