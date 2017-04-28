using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 北现TLc上市活动
    /// 中奖信息表
    /// 2015.08.28 zhangyf
    /// </summary>
    public interface IBlowCarWinningStorager
    {
        /// <summary>
        /// 添加中奖(领取)记录
        /// </summary>
        /// <param name="bcw"></param>
        /// <returns></returns>
        int AddInfo(BlowCarWinning bcw);

        /// <summary>
        /// 更新中奖信息
        /// </summary>
        /// <param name="bcw"></param>
        /// <returns></returns>
        bool UpdateInfo(BlowCarWinning bcw);

        /// <summary>
        /// 根据手机号，获取中奖信息
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        IEnumerable<BlowCarWinning> GetListByTel(String tel);

        /// <summary>
        /// 根据id获取中奖信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BlowCarWinning GetItem(int id);
    }
}
