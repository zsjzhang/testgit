using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class Airport
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 机场名称
        /// </summary>
        public string AirportName { get; set; }

        /// <summary>
        /// 机场候机室详细地址
        /// </summary>
        public string AirportAddress { get; set; }

        /// <summary>
        /// 机场候机室类型
        /// </summary>
        public string AirportRoomType { get; set; }

        /// <summary>
        /// 机场候机室名称
        /// </summary>
        public string AirportRoomName { get; set; }

        /// <summary>
        /// 机场候机室全称
        /// </summary>
        public string AirportAllName
        {
            get
            {
                return AirportRoomType + "-" + AirportRoomName;
            }
        }
    }
}
