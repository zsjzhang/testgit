using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Member
{
    [TableName("ReceiveRecord")]
    [PrimaryKey("Id")]
    public class ReceiveRecord
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 服务券码
        /// </summary>
        public string ServiceCode { get; set; }
        /// <summary>
        /// 是否会员,是：1，否0
        /// </summary>
        public int IsMember { get; set; }
        /// <summary>
        /// 是否完成，是：1，否0
        /// </summary>
        public int Ifinish { get; set; }
        /// <summary>
        /// 加入时间
        /// </summary>
        public DateTime JoinTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
