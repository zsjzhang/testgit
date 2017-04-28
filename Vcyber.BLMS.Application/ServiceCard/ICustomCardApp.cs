using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface ICustomCardApp
    {
        IEnumerable<CustomCardInfo> GetCarsByCardName(string cardname);
        Remeal GetRemeal(string vin, string cardtype,string cardcode);
        /// <summary>
        /// 添加用户优惠券信息
        /// </summary>
        /// <param name="model">用户优惠券信息</param>
        /// <returns></returns>
        ReturnResult AddCustomCard(CustomCard model);


        ReturnResult AddRepair(Remeal model);


        /// <summary>
        /// 获取用户自定义优惠券 状态（未使用，已使用，已过期）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="source">卡券来源 1：北京现代 ；2：合作商户；</param>
        /// <param name="status">状态（1：未使用，2：已使用，3：已过期）</param>
        /// <returns>优惠券列表信息</returns>
        IEnumerable<ReturnUserCustomCardModel> GetUserCustomCardList(string userId, int source, int status);




        /// <summary>
        /// 根据手机号码，查询用户卡券信息
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="activityName">活动名称</param>
        /// <param name="cardType">卡券ID</param>
        /// <param name="pageIndex">分页号</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="totalCount">返回总行数</param>
        /// <returns></returns>
        IEnumerable<UserCustomCard> GetUserCustomCardListByPhone(string userId, string activityName, string cardType,
            int pageIndex, int pageSize, out int totalCount);

        CustomCard GetUserCustomCardByCardType(string cardType);

        /// <summary>
        /// 根据用户ID和卡券的活动名字获取用户领取的卡券列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="ActivityType">卡券的活动名字</param>
        /// <returns></returns>
        List<CustomCard> getUserCustomCardByUID(string userId, string ActivityType);


        /// <summary>
        /// 获取卡券使用数量
        /// </summary>
        /// <param name="cardType">卡券GUID</param>
        int GetCardUsedCount(string cardType);


        int GetUserReissueCount(string cardType, string phone);

        int GetCountRepar(string vin, string cardType);


        /// <summary>
        /// 根据用户兑奖码 获取该劵码信息
        /// </summary>
        /// <param name="cardCode">用户兑奖码</param>
        /// <returns></returns>
        ReturnCustomCardCodeInfo GetCustomCardCodeInfo(string cardCode);


        /// <summary>
        /// 查询卡券基本信息
        /// </summary>
        /// <param name="cardCode"></param>
        /// <returns></returns>
        ReturnCustomCardCodeInfoByDMS GetCustomCardCodeInfoByDMS(string cardCode);

        /// <summary>
        /// 获取夏季保养活动用户卡券信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        IEnumerable<ReturnCustomCardCodeInfo> GetUserSummerCustomCardListByUserId(string userId);



        /// <summary>
        /// 给用户发送获取卡券信息的短信，目前短信格式只支持兑奖码参数传入，其他短信信息必须在卡券后台短信字段里录入；
        /// </summary>
        /// <param name="model">卡券信息</param>
        /// <param name="cardCode">兑奖码</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        ReturnResult SendCustomCardSms(CustomCardInfo model, string cardCode, string phone);



        /// <summary>
        /// 给用户发送获取卡券信息的短信，目前短信格式只支持兑奖码参数传入，其他短信信息必须在卡券后台短信字段里录入；
        /// </summary>
        /// <param name="model">卡券信息</param>
        /// <param name="sms">卡券短信参数</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        ReturnResult SendCustomCardSms(CustomCardInfo model, CustomCardSms sms, string phone);

        /// <summary>
        /// 获取用户可用的卡券数量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        int GetReceivedCustomCardCount(string userid);
        

        /// <summary>
        /// 根据用户ID和卡券的活动名字获取用户领取的卡券列表
        /// </summary>
        List<CustomCard> GetUserCustomCardByActivityType(string userId, string ActivityType);

        /// <summary>
        /// 获取候机服务券
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="source"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IEnumerable<SNCard> GetTerminalservicevoucherCardList(string userId, int source, int status);
    }
}
