using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 北现TLc上市活动
    /// 中奖信息表
    /// 2015.08.28 zhangyf
    /// </summary>
    public class BlowCarWinningStorager : IBlowCarWinningStorager
    {
        /// <summary>
        /// 添加中奖(领取)记录
        /// </summary>
        /// <param name="bcw"></param>
        /// <returns></returns>
        public int AddInfo(BlowCarWinning bcw)
        {
            return 0;
        }

        /// <summary>
        /// 更新中奖信息
        /// </summary>
        /// <param name="bcw"></param>
        /// <returns></returns>
        public bool UpdateInfo(BlowCarWinning bcw)
        {
            return false;
        }

        /// <summary>
        /// 根据手机号，获取中奖信息
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public IEnumerable<BlowCarWinning> GetListByTel(String tel)
        {
            return null;
        }

        /// <summary>
        /// 根据id获取中奖信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BlowCarWinning GetItem(int id)
        {
            return null;
        }
    }
}
