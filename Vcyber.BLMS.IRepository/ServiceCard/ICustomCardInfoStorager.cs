using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;


namespace Vcyber.BLMS.IRepository
{
    public interface ICustomCardInfoStorager
    {
        /// <summary>
        /// 添加优惠券
        /// </summary>
        /// <param name="model">优惠券信息</param>
        /// <returns>true 添加成功</returns>
        bool AddCustomCardInfo(CustomCardInfo model);


        /// <summary>
        /// 获取一条卡券信息
        /// </summary>
        /// <param name="id">卡券ID</param>
        /// <returns>卡券信息</returns>
        CustomCardInfo GetCustomCardInfo(int id);


        /// <summary>
        /// 获取一条卡券信息
        /// </summary>
        /// <param name="cardType">卡券Type</param>
        /// <returns></returns>

        CustomCardInfo GetSingleCustomCardInfoByGuid(string cardType);

        /// <summary>
        /// 更新一条卡券信息
        /// </summary>
        /// <param name="model">卡券信息</param>
        /// <returns>true 更新成功</returns>
        bool UpdateCustomCardInfo(CustomCardInfo model);

        /// <summary>
        /// 删除一条卡券信息
        /// </summary>
        /// <param name="id">卡券ID</param>
        /// <returns>true 更新成功</returns>
        bool DeleteCustomCardById(string cardType);

        /// <summary>
        /// 更新卡券库存数量
        /// </summary>
        /// <param name="id">卡券ID</param>
        /// <param name="quantity">库存</param>
        /// <returns></returns>
        bool UpdateCustomCardQuantity(int id, int quantity);



        bool UpdateCustomCardQuantityByType(string cardType);


        /// <summary>
        /// 获取卡券信息列表；
        /// </summary>
        /// <param name="source">卡券来源</param>
        /// <param name="merchant">合作商户所属的商户名称</param>
        /// <param name="actType">活动名称</param>
        /// <param name="cardName">卡券名称</param>
        /// <param name="status">已领取</param>
        /// <param name="reduceCost">卡券金额</param>
        /// <param name="pageIndex">分页码</param>
        /// <param name="pageSize">分页数字</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>卡券信息列表</returns>
        IEnumerable<CustomCardInfo> GetCustomCardInfoList(int source, string merchant, string actType, string cardName, int status, string reduceCost, int pageIndex, int pageSize, out int totalCount);



        /// <summary>
        /// 返回一张优惠券信息
        /// </summary>
        /// <param name="cardType">优惠券GUID</param>
        /// <returns>返回一张优惠券信息</returns>

        ReturnCustomCardInfo GetSingleCustomCardInfo(string cardType);



        /// <summary>
        /// 获取一张用户卡券信息
        /// </summary>
        /// <param name="id">用户卡券信息ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>用户卡券信息</returns>
        ReturnUserCustomCardInfo GetSingleUserCustomCardInfoByIdAndUserId(string id, string userId);




        IEnumerable<CustomCardInfo> GetCustomCardInfoListByActType(string actType, string cardName);


        bool IsExistsCustomCardInfo(string actType, string cardNmae, int source);

        /// <summary>
        /// 根据cardNO查找卡券信息
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        CustomCardInfo GetSingleCustomCardInfoByCNo(string cardNo);

        RecommendCustomer GetRecommendNameByPhone(string phone);

        CustomCardInfo GetCustomTypeByVin(string Vin, string ActivityType);
    }

}
