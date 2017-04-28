using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Weixin;

namespace Vcyber.BLMS.Application.Weixin
{
    public interface IRedPackApp
    {
        /// <summary>
        /// 添加红包领取记录
        /// </summary>   
        int AddRecord(RedPackRecord obj);
        /// <summary>
        /// 根据场景值查询红包活动
        /// </summary>
        RedPack BySceneId(string sceneId);
        /// <summary>
        /// 根据UserId查询用户领取的一个卡券
        /// </summary>
        /// <param name="redPackId">红包ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>红包记录</returns>
        RedPackRecord RedPacCardByUserId(int redPackId, string userId);
        /// <summary>
        /// 根据UserId用户今天有没有记录
        /// </summary>
        int RedPackCountByUserId(int redPackId, string userId, string date);
        /// <summary>
        /// 修改红包记录
        /// </summary>
        void UpdateRedPackRecord(RedPackRecord obj);
    }
}
