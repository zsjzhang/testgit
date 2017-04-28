using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IRecommendApp
    {
        /// <summary>
        /// 创建一条推荐
        /// </summary>
        /// <param name="recommend">推荐</param>
        /// <returns>影响的行数</returns>
        int Add(Recommend recommend);
        /// <summary>
        /// 查询一条推荐
        /// </summary>
        /// <param name="id">推荐Id</param>
        /// <returns>推荐内容</returns>
        Recommend Find(int id);
        /// <summary>
        /// 参与推荐的人数
        /// </summary>
        /// <param name="activityType">活动</param>
        /// <returns>数量</returns>
        int Count(string activityType);
        /// <summary>
        /// 参与推荐的人数
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns>是否</returns>
        bool IsPhoneExist(string phone);
    }
}