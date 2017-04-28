using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 用户地址集合
    /// </summary>
    public class AddressCollectionV
    {
        /// <summary>
        /// 用户地址数据集
        /// </summary>
        public List<AddressV> Datas { get; set; }
    }
}