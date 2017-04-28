using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
namespace Vcyber.BLMS.Entity.Generated
{
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Entity.Enum;

    public partial class CSConsume
    {
        public string ConsumeTypeString
        {
            get
            {
                return ((EConsumeType)ConsumeType).DisplayName();
            }
        }
        [Column] public decimal? PaperOrderCost { get; set; }

        [Column]
        public string DMSOrderNo { get; set; }
    }

    public partial class CSCarDealerShip
    {
        /// <summary>
        /// 纬度
        /// </summary>
        public string Y { get { return (!string.IsNullOrEmpty(this.Position) && this.Position.Contains(','))? this.Position.Split(',')[0]:"" ; } }

        /// <summary>
        /// 精度
        /// </summary>
        public string X { get { return (!string.IsNullOrEmpty(this.Position) && this.Position.Contains(',')) ? this.Position.Split(',')[1] : ""; } }
        /// <summary>
        /// 微信账户 
        /// </summary>
        [Column]
        public string WeixinPayAccount { get; set; }
    }
}
