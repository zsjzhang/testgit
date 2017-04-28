using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 北现TLc上市活动
    /// 中奖信息表
    /// 2015.08.28 zhangyf
    /// </summary>
    public class BlowCarWinning
    {
        public int Id { get; set; }

        /// <summary>
        /// 中奖用户姓名
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 中奖用户联系电话
        /// </summary>
        public String UserTel { get; set; }

        /// <summary>
        /// 用户类型：1-已购车用户，2-未购车用户
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 已购车的Vin码
        /// </summary>
        public String Vin { get; set; }

        /// <summary>
        /// 礼品的邮购的地址
        /// </summary>
        public String Address { get; set; }

        /// <summary>
        /// 发票图片的上传地址
        /// </summary>
        public String imgAddress { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        public String InvoiceNO { get; set; }

        /// <summary>
        /// 状态：0-信息不全，1-待审核，2-审核不通过，3-审核通过
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 奖品名称
        /// </summary>
        public String PrizeName { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
