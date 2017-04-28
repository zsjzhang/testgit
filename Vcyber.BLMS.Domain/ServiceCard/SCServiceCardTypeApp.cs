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
    public class SCServiceCardTypeApp : ISCServiceCardTypeApp
    {

        public ReturnResult AddSCServiceCardType(SCServiceCardType model)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            model.CreateTime = DateTime.Now;
            var addResult = _DbSession.SCServiceCardTypeStorager.AddSCServiceCardType(model);
            if (!addResult)
            {
                result.IsSuccess = false;
                result.Message = "添加卡券类型失败";
                return result;
            }
            return result;
        }

        /// <summary>
        /// 获取卡券列表
        /// </summary>
        /// <param name="type">1：卡券活动类型，2：卡券名称；</param>
        /// <returns></returns>
        public IEnumerable<SCServiceCardType> GetSCServiceCardTypeList(int type, int source, int iswx = 0)
        {
            return _DbSession.SCServiceCardTypeStorager.GetSCServiceCardTypeList(type,source,iswx);
        }

        public IEnumerable<SCServiceCardType> GetActiveTagName()
        {
            return _DbSession.SCServiceCardTypeStorager.GetActiveTagName();
        }


        /// <summary>
        /// 获取活动名称所属的卡券名称
        /// </summary>
        /// <param name="name">活动名称</param>
        /// <returns></returns>
        public IEnumerable<SCServiceCardType> GetScServiceCardTypeNameListByActivityType(string name)
        {
            return _DbSession.SCServiceCardTypeStorager.GetScServiceCardTypeNameListByActivityType(name);
        }

        /// <summary>
        /// 获取所有优惠券信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReturnCustomCardTypeModel> GetSCServiceCardTypeList()
        {
            return _DbSession.SCServiceCardTypeStorager.GetSCServiceCardTypeList();
        }


        public IEnumerable<SCServiceCardType> GetMerchantCardTypeList()
        {
            return _DbSession.SCServiceCardTypeStorager.GetMerchantCardTypeList();
        }


        public IEnumerable<ReturnCustomCardTypeModel> GetSummerCardList()
        {
            return _DbSession.SCServiceCardTypeStorager.GetSummerCardList();
        }
    }
}
