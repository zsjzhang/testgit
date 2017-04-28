using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application.Weixin;
using Vcyber.BLMS.Entity.Weixin;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain.Weixin
{
    public class RedPackApp : IRedPackApp
    {
        /// <summary>
        /// 添加红包领取记录
        /// </summary>   
        public int AddRecord(RedPackRecord obj)
        {
            return _DbSession.RedPackStorager.AddRecord(obj);
        }
        /// <summary>
        /// 根据场景值查询红包活动
        /// </summary>
        public RedPack BySceneId(string sceneId)
        {
            return _DbSession.RedPackStorager.BySceneId(sceneId);
        }
        /// <summary>
        /// 根据UserId查询用户领取的一个卡券
        /// </summary>
        /// <param name="redPackId">红包ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>红包记录</returns>
        public RedPackRecord RedPacCardByUserId(int redPackId, string userId)
        {
            return _DbSession.RedPackStorager.RedPacCardByUserId(redPackId, userId);
        }
        /// <summary>
        /// 根据UserId用户今天有没有记录
        /// </summary>
        public int RedPackCountByUserId(int redPackId, string userId, string date) 
        {
            return _DbSession.RedPackStorager.RedPackCountByUserId(redPackId, userId, date);
        }
        /// <summary>
        /// 修改红包记录
        /// </summary>
        public void UpdateRedPackRecord(RedPackRecord obj) 
        {
            _DbSession.RedPackStorager.UpdateRedPackRecord(obj);
        }
    }
}
