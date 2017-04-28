using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 资料申请表数据
    /// </summary>
    public class MagazineApply
    {
        #region ==== 构造函数 ====

        public MagazineApply() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 申请Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 杂志Id
        /// </summary>
        public int MagazineId { get; set; }

        /// <summary>
        /// 用户Id(暂时不用)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 杂志名称
        /// </summary>
        public string MagazineTitle { get; set; }

        /// <summary>
        /// 接受人姓名
        /// </summary>
        public string ReceiveName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 省Code
        /// </summary>
        public int ProvinceCode { get; set; }

        /// <summary>
        /// 市Code
        /// </summary>
        public int CityCode { get; set; }

        /// <summary>
        /// 县Code
        /// </summary>
        public int CountyCode { get; set; }

        /// <summary>
        /// 省市县中文
        /// </summary>
        public string PCC { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 申请状态（0：待审核；1：审核通过；3：已经邮寄；13：审核失败）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime { get; set; }

        #endregion
    }
}
