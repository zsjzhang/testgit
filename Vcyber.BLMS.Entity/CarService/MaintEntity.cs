using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.CarService
{
    using System.ComponentModel.DataAnnotations;

    public class MaintEntity
    {
        ///<summary>
        ///用户Id
        ///</summary>
        public string UserId { get; set; }

        /// <summary>
        /// 试驾车系
        /// </summary>
        [Required]
        public string CarSeries { get; set; }

        /// <summary>
        /// 经销商Id
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 经销商所在城市
        /// </summary>
        public string DealerCity { get; set; }

        /// <summary>
        /// 经销商所在省份
        /// </summary>
        public string DealerProvince { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string LicensePlate { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public int MileAge { get; set; }

        /// <summary>
        /// 购车年份
        /// </summary>
        public string PurchaseYear { get; set; }

        /// <summary>
        /// 服务项目：0:维修， 1:保养
        /// </summary>
        public byte ServiceType { get; set; }

        /// <summary>
        /// 到店时间
        /// </summary>
        public DateTime? ScheduleDate { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户性别(0: 女, 1:男)
        /// </summary>
        public int UserSex { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 补充说明
        /// </summary>
        public string Comment { get; set; }

    }
}
