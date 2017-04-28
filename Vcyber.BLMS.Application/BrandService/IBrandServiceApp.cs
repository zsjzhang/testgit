using System;
using System.Collections.Generic;
using Vcyber.BLMS.Entity;


namespace Vcyber.BLMS.Application
{
    public interface IBrandServiceApp
    {
        /// <summary>
        /// 用户领取权益码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="brandName">品牌名称</param>
        /// <returns></returns>
        ReturnResult GetBrandServiceCode(string userId, string phoneNumber, string brandName);
        
        /// <summary>
        /// 插入用户在品牌完成注册后的记录信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ReturnResult AddMembershipBrand(MembershipBrand item);

        /// <summary>
        /// 根据用户查询品牌合作相关信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<MembershipBrand> SelectMembershipBrandByUserId(string userId);

        BrandServiceCode SelectBrandServiceCodeByUserId(string userId, string brandName);
    }
}
