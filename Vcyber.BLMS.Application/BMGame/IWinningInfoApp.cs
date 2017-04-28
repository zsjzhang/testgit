using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IWinningInfoApp
    {
        /// <summary>
        /// 获取指定活动的获奖名单
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        List<WinningInfo> GetWinningInfosByActivityId(int activityId);

        /// <summary>
        /// 获取指定活动的获奖名单(针对前端显示要求整理的模型)
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        List<WinningModel> GetWinningModelsByActivityId(int activityId);

        /// <summary>
        /// 判断用户是否获奖
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsWinningByUIdAndActicityId(int activityId, string userTel);
        /// <summary>
        /// 获取活动获奖名单列表（where为条件，如：Id=1 and Tel='135***'）
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<WinningInfo> GetWinningsByWhere(int activityId, string where);
        /// <summary>
        /// 获取中奖数量
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <returns>数量</returns>
        int GetWinningsCount(int activityId);
        List<WinningInfo> GetWinningsList(string[] activityIds, string phonenumber);
        /// <summary>
        /// 根据电话号码获取获奖信息
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userTel"></param>
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
        bool AddWinningInfo(WinningInfo entity);

        /// <summary>
        /// 分页获取获奖名单
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<WinningInfo> GetWinningInfoByActivityId(int activityId, PageData pageData, out int total);

        /// <summary>
        /// 根据ID获取中奖信息
        /// 贾锡安2015-09-09 11:00:11
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WinningInfo GetWinningInfo(int id);

        /// <summary>
        /// 修改中奖纪录
        /// 贾锡安2015-09-09 11:13:06
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateWinningInfo(WinningInfo entity);

        Membership GetMembershipByNameAndTel(string name, string tel);

        ReturnResult ImportWinningInfoData(string path);

        /// <summary>
        /// 判断当前设备是否中过奖
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
