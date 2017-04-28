using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface ICustomCardMerchantConsumeCodeApp
    {
        /// <summary>
        /// 添加商户兑奖码
        /// </summary>
        /// <param name="model">商户兑奖码信息</param>
        /// <returns></returns>
        ReturnResult AddCustomCardMerchantConsumeCode(CustomCardMerchantConsumeCode model);


        /// <summary>
        /// 获取一个商户卡券兑换码
        /// </summary>
        /// <param name="cardType">卡券GUID</param>
        /// <returns>一个商户卡券兑换码</returns>
        CustomCardMerchantConsumeCode GetSingleCardMerchantConsumeCode(string cardType);

        /// <summary>
        /// 获取商户卡券兑换券列表
        /// </summary>
        /// <param name="cardType">卡券GUID</param>
        /// <returns>商户卡券兑换券列表</returns>
        IEnumerable<CustomCardMerchantConsumeCode> GetCardMerchantConsumeCodeList(string cardType, int pageSize, int pageIndex, out int totalCount);

        /// <summary>
        /// 获取商户卡券兑换码
        /// </summary>
        /// <param name="cardType">卡券GUID</param>
        /// <param name="type">使用状态 1：未使用，2：已使用</param>
        /// <returns></returns>
        int GetCardMerchantConsumeCodeCount(string cardType, int type);



        CustomCardMerchantConsumeCode GetSingleCardMerchantConsumeCodeByCode(string cardType, string code);

    }
}
