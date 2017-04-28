using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IWinningInfoStorager
    {
        /// <summary>
        /// 获取指定活动的获奖名单
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        IEnumerable<WinningInfo> GetWinningInfosByActivityId(int activityId);

        /// <summary>
        /// 判断用户是否获奖
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsWinningByUIdAndActicityId(int activityId, string userTel);
        /// <summary>
        /// 根据条件获奖名单信息
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        IEnumerable<WinningInfo> GetWinningsByWhere(int activityId, string where);
        int GetWinningsCount(int activityId);
        IEnumerable<WinningInfo> GetWinningsList(string[] activityIds, string phonenumber);
        /// <summary>
        /// 获取用户获奖信息
        /// </summary>
        /// <param name="activityId">活动编号</param>
        /// <param name="userTel">电话号码</param>
        /// <returns></returns>
        WinningInfo GetWinningByTelAndActicityId(int activityId, string userTel);
        /// <summary>
        /// 获取单条用户获奖信息by userid
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        WinningInfo GetWinningByUserIdAndActicityId(int activityId, string userId);
        /// <summary>
        /// 添加获奖记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddWinningInfo(WinningInfo entity);

        /// <summary>
        /// 分页获取获奖名单
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<WinningInfo> GetWinningInfoByActivityId(int activityId, PageData pageData, out int total);


        /// <summary>
        /// 根据ID获取中奖纪录
        /// 贾锡安2015-09-09 10:57:50
        /// </summary>
        /// <param name="id">中奖纪录ID</param>
        /// <returns></returns>
        WinningInfo GetWinningInfo(int id);

        /// <summary>
        /// 修改中奖纪录
        /// 贾锡安2015-09-09 10:58:41
        /// </summary>
        /// <param name="entity">中奖纪录实体</param>
        /// <returns></returns>
        bool UpdateWinningInfo(WinningInfo entity);

        Membership GetMembershipByNameAndTel(string name, string tel);

        /// <summary>
        /// 判断该微信是否中过奖
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        bool IsWinningByActivity(int activityId, string openId);
        /// <summary>
        /// 获取奖品的每天的使用量
        /// </summary>
        /// <param name="activityId">活动ID</param>        
        /// <returns>奖品使用量列表</returns>
        IEnumerable<PrizesInfo> GetPrizeUse(int activityId);
        /// <summary>
        /// 修改中奖纪录
        /// </summary>
        bool UpdateForUserId(WinningInfo obj);
        /// <summary>
        /// 修改中奖纪录
        /// </summary>
        bool UpdateForId(int id, string userName, string phone, string address);
        /// <summary>
        /// 获取所中的记录
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="prizeId">奖品ID</param>
        /// <returns>中奖记录</returns>
        WinningInfo GetWinPrize(int activityId, string userId, int prizeId);
    }
}
