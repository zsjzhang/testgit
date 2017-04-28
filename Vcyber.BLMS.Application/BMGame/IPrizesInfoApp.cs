using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IPrizesInfoApp
    {
        /// <summary>
        /// 获取活动奖品列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        List<PrizesInfo> GetPrizesInfosByActivityId(int activityId);

        /// <summary>
        /// 获取景区门票中奖记录
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        List<PrizesInfo> GetPrizesUsedNumByActivity(int activityId);

        List<PrizesInfo> GetPrizesInfosByActivityId(int activityId, PageData pageData, out int total);

        /// <summary>
        /// 添加奖品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddPrizesInfo(PrizesInfo entity);

        /// <summary>
        /// 开始活动
        /// 贾锡安2015-09-10 13:57:44
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        ActivityResult StartActivityInfo(string userId, int activityId, string source, string vin);

        /// <summary>
        /// 修改活动记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdatePrizesInfo(PrizesInfo entity);

        PrizesInfo GetPrizeInfoMode(int prizeId);

        bool DelPrizesInfo(int id);
        /// <summary>
        /// 减少奖品未使用的数量
        /// </summary>
        /// <param name="prizeId"></param>
        /// <param name="activityId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        bool CutDownPrizeCyclesUnuseNum(int prizeId, int activityId, int num);
        /// <summary>
        /// 获取奖品的未使用数量
        /// </summary>
        /// <param name="prizeId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        int GetCyclesUnuseNumById(int prizeId, int activityId);
        /// <summary>
        /// 奖品减库存
        /// </summary>
        /// <param name="id">奖品ID</param>
        void PrizeMinus(int id);

       
    }
}
