using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 市信息集合
    /// </summary>
    public class CityCollectionV
    {
        /// <summary>
        /// 市数据集
        /// </summary>
        public List<CityV> Datas { get; set; }
    }
}