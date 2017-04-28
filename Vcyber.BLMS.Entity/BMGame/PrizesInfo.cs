using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class PrizesInfo
    {
        public PrizesInfo() { }
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 活动编号
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 奖品价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 奖品数量
        /// </summary>
        public int TotalNum { get; set; }
        /// <summary>
        /// 使用数量
        /// </summary>
        public int UsedNum { get; set; }
        /// <summary>
        /// 奖品等级（ >=1）数值表示几等奖
        /// </summary>
        public int PrizeLevel { get; set; }
        /// <summary>
        /// 奖品类型(0-虚拟；1-实物)
        /// </summary>
        public int PrizeFlag { get; set; }
        /// <summary>
        /// 奖品图片
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// 奖品周期（0-无周期，1-天，2-周，3-月，4-季度，5-年）
        /// </summary>
        public int CyclesFlag { get; set; }
        /// <summary>
        /// 周期奖品数(奖品周期大于0起作用)
        /// </summary>
        public int CyclesNum { get; set; }
        /// <summary>
        /// 本周期剩余数(奖品周期大于0起作用)
        /// </summary>
        public int CyclesUnuseNum { get; set; }
        /// <summary>
        /// 获奖几率
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// 最后修改日期
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 机率开始区间
        /// </summary>
        public int BeginSection { get; set; }
        /// <summary>
        /// 机率结束区间
        /// </summary>
        public int EndSection { get; set; }
        /// <summary>
        /// 中奖ID
        /// </summary>
        public int WinningInfoId { get; set; }
        /// <summary>
        /// 奖品变化操作
        /// </summary>
        public void WinningPrize()
        {
            this.UsedNum++;
            if (this.CyclesFlag > 0 && this.CyclesNum > 0)
            {
                this.CyclesUnuseNum--;
            }
            UpdateTime = DateTime.Now;
        }
    }
}
