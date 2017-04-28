using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.CarService
{
    using System.ComponentModel.DataAnnotations;

    public class OrderCarEntity
    {
        ///<summary>
        ///用户Id
        ///</summary>
       
        public string UserId { get; set; }

        ///<summary>
        ///试驾车系
        ///</summary>
        [Required]
       
        public string CarSeries { get; set; }

        ///<summary>
        ///经销商Id
        ///</summary>
       
        public string DealerId { get; set; }

        ///<summary>
        ///经销商名称
        ///</summary>
       
        public string DealerName { get; set; }

        ///<summary>
        ///经销商所在城市
        ///</summary>
       
        public string DealerCity { get; set; }

        ///<summary>
        ///经销商所在省份
        ///</summary>
       
        public string DealerProvince { get; set; }

        ///<summary>
        ///用户姓名
        ///</summary>
        [Required]
        public string UserName { get; set; }

        ///<summary>
        ///用户性别(0: 女, 1:男)
        ///</summary>
        [Required]
       
        public int UserSex { get; set; }

        ///<summary>
        ///电话
        ///</summary>
        [Required]
       
        public string Phone { get; set; }

        ///<summary>
        ///电子邮箱
        ///</summary>
       
        public string Email { get; set; }

        ///<summary>
        ///可以联系用户的时间
        ///</summary>
       
        public string ContactTime { get; set; }

        ///<summary>
        ///注释
        ///</summary>
       
        public string Comment { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 记录外部系统ID
        /// </summary>
        public string ForeignId { get; set; }
       
    }
}
