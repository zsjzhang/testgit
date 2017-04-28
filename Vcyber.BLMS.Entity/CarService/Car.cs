using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class Car
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 车种
        /// </summary>
        public string CarCategory { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 经销商ID
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string LicencePlate { get; set; }

        /// <summary>
        /// VIN码
        /// </summary>
        public string VIN { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public string Mileage { get; set; }

        /// <summary>
        /// 所属客户
        /// </summary>
        public string CustId { get; set; }

        /// <summary>
        /// 购车日期
        /// </summary>
        public DateTime? BuyTime { get; set; }

        /// <summary>
        /// 购车日期
        /// </summary>
        public string BuyTimeYear
        {
            get
            {
                return BuyTime != null ? BuyTime.Value.ToString("yyyy-MM-dd") : string.Empty;
            }
        }

        /// <summary>
        /// 车辆图片
        /// </summary>
        public string PictureUrl { get; set; }

        public string PictureUrl2 { get; set; }


        public string Userintegral { get; set; }

        public string accnttype { get; set; }

    }
}
