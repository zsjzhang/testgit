using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IPrizesInfoStorager
    {
        /// <summary>
        /// 获取活动奖品列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        IEnumerable<PrizesInfo> GetPrizesInfosByActivity(int activityId);

        /// <summary>
        /// 获取景区门票中奖记录
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        IEnumerable<PrizesInfo> GetPrizesUsedNumByActivity(int activityId);

        IEnumerable<PrizesInfo> GetPrizesInfosByActivity(int activityId, PageData pageData, out int total);
        /// <summary>
        /// 添加奖品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddPrizesInfo(PrizesInfo entity);

        /// <summary>
        /// 修改奖品
        /// 贾锡安2015-09-09 17:13:58
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdatePrizesInfo(PrizesInfo entity);

        // <summary>
        /// 获取有效的奖项
        /// 贾锡安2015-09-09 17:14:02
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        List<PrizesInfo> GetPrizesInfoNotNull(int activityId);

        /// <summary>
        /// 通过编号获取用户信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Membership GetUserInfoById(string guid);

        PrizesInfo GetPrizeInfoMode(int prizeId);


        Membership GetMembershipMode(string phone);
       

        /// <summary>
        /// 删除奖品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DelPrizesInfo(int id);

        bool CutDownPrizeCyclesUnuseNum(int prizeId, int activityId, int num);
        int GetCyclesUnuseNumById(int prizeId, int activityId);

        /// <summary>
        /// 奖品减库存
        /// </summary>
        /// <param name="id">奖品ID</param>
        void PrizeMinus(int id);
    }
}
