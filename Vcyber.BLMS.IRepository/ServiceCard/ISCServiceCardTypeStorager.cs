using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface ISCServiceCardTypeStorager
    {
        /// <summary>
        /// 添加优惠券卡类型信息
        /// </summary>
        /// <param name="model">优惠券卡类型信息</param>
        /// <returns></returns>
        bool AddSCServiceCardType(SCServiceCardType model);

        IEnumerable<SCServiceCardType> GetActiveTagName();

        /// <summary>
        /// 获取卡券列表
        /// </summary>
        /// <param name="type">1：卡券活动类型，2：卡券名称；</param>
        /// <param name="source">卡券来源 1：北京现代 ；2：合作商户；</param>
        /// <returns></returns>
        IEnumerable<SCServiceCardType> GetSCServiceCardTypeList(int type, int source, int iswx = 0);


        /// <summary>
        /// 获取活动名称所属的卡券名称
        /// </summary>
        /// <param name="name">活动名称</param>
        /// <returns></returns>
        IEnumerable<SCServiceCardType> GetScServiceCardTypeNameListByActivityType(String name);

        /// <summary>
        /// 获取所有优惠券信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<ReturnCustomCardTypeModel> GetSCServiceCardTypeList();


        /// <summary>
        /// 获取商户卡券列表信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<SCServiceCardType> GetMerchantCardTypeList();



        /// <summary>
        /// 获取夏季保养卡券
        /// </summary>
        /// <returns></returns>
        IEnumerable<ReturnCustomCardTypeModel> GetSummerCardList();

        
    }
}
