using System;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IBrandServiceCodeStorager
    {
        /// <summary>
        /// 查询指定品牌的用户权益码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="brandName">品牌名称</param>
        /// <returns></returns>
        BrandServiceCode SelectBrandServiceCodeByUserId(string userId,string brandName);

        /// <summary>
        /// 查询权益码信息
        /// </summary>
        /// <param name="serviceCode">权益码</param>
        /// <returns></returns>
        BrandServiceCode SelectBrandServiceCodeByCode(string serviceCode);

        /// <summary>
        /// 获取一个指定品牌的权益码
        /// </summary>
        /// <param name="brandName"></param>
        /// <returns></returns>
        BrandServiceCode GetBrandServiceCode(string brandName);

        /// <summary>
        /// 将指定的权益码发放给指定的用户
        /// </summary>
        /// <param name="serviceCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int SendServiceCode(string serviceCode, string userId);


    }
}
