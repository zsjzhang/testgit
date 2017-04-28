using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class WeixinMerchantApp : IWeixinMerchantApp
    {
        /// <summary>
        /// 添加微信商户
        /// </summary>
        public int Add(Entity.WeixinMerchant obj)
        {
            return _DbSession.WeixinMerchantStorager.Add(obj);
        }
        /// <summary>
        /// 查询用户在经销商下是否已添加
        /// </summary>
        public bool IsExist(string dealerId, string openId)
        {
            return _DbSession.WeixinMerchantStorager.IsExist(dealerId, openId);
        }
    }
}
