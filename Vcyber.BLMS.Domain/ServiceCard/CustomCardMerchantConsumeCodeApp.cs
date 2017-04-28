using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class CustomCardMerchantConsumeCodeApp : ICustomCardMerchantConsumeCodeApp
    {
        public ReturnResult AddCustomCardMerchantConsumeCode(CustomCardMerchantConsumeCode model)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            var addResult = _DbSession.CustomCardMerchantConsumeCodeStorager.AddCustomCardMerchantConsumeCode(model);
            if (!addResult)
            {
                result.IsSuccess = false;
            }
            return result;
        }

        public CustomCardMerchantConsumeCode GetSingleCardMerchantConsumeCode(string cardType)
        {
            return _DbSession.CustomCardMerchantConsumeCodeStorager.GetSingleCardMerchantConsumeCode(cardType);
        }

        public IEnumerable<CustomCardMerchantConsumeCode> GetCardMerchantConsumeCodeList(string cardType, int pageSize, int pageIndex, out int totalCount)
        {
            return _DbSession.CustomCardMerchantConsumeCodeStorager.GetCardMerchantConsumeCodeList(cardType, pageSize, pageIndex, out totalCount);
        }

        public int GetCardMerchantConsumeCodeCount(string cardType, int type)
        {
            return _DbSession.CustomCardMerchantConsumeCodeStorager.GetCardMerchantConsumeCodeCount(cardType, type);
        }


        public CustomCardMerchantConsumeCode GetSingleCardMerchantConsumeCodeByCode(string cardType, string code)
        {
            return _DbSession.CustomCardMerchantConsumeCodeStorager.GetSingleCardMerchantConsumeCodeByCode(cardType, code);
        }
    }
}
