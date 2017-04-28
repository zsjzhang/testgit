using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class BlowCarRecordStorager : IBlowCarRecordStorager
    {
        /// <summary>
        /// 添加游戏参与游戏记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public int AddRecord(BlowCarRecord record)
        {
            if (record==null)
            {
                return 0;
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO [dbo].[BlowCarRecord] ([UserId],[Duration],[Distance],[DeviceType],[CreateTime]) ");
            sql.Append(" VALUES (@UserId,@Duration,@Distance,@DeviceType,@CreateTime); select @@identity;");
            int id = DbHelp.ExecuteScalar<int>(sql.ToString(), record);

            return id;
        }

        /// <summary>
        /// 根据行驶的距离，击败百分比
        /// </summary>
        /// <param name="distance">本次比赛的击败百分比</param>
        /// <returns></returns>
        public decimal RecordRanking(decimal distance)
        {
            return 0;
        }
    }
}
