using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IWeixinMerchantApp
    {
        /// <summary>
        /// 添加微信商户
        /// </summary>
        int Add(WeixinMerchant obj);
        /// <summary>
        /// 查询用户在经销商下是否已添加
        /// </summary>
        bool IsExist(string dealerId, string openId);
    }
}
