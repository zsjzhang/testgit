using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 省数据集合
    /// </summary>
    public class ProvincesCollectionV
    {
        public ProvincesCollectionV() { }

        /// <summary>
        /// 省数据集
        /// </summary>
        public List<ProvincesV> Datas { get; set; }
    }
}