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
    /// 吹气游戏记录
    /// </summary>
    public interface IBlowCarRecordStorager
    {
        /// <summary>
        /// 添加游戏参与游戏记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        int AddRecord(BlowCarRecord record);



        /// <summary>
        /// 根据行驶的距离，击败百分比
        /// </summary>
        /// <param name="distance">本次比赛的击败百分比</param>
        /// <returns></returns>
        decimal RecordRanking(decimal distance);

    }
}
