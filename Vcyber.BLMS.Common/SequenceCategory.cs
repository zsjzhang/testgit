using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    public enum SequenceCategory
    {
        /// <summary>
        /// 保养
        /// </summary>
        WB = 0,
        /// <summary>
        /// 上门关怀
        /// </summary>
        GH=1,

        /// <summary>
        /// home to home
        /// </summary>
        HH=2,
        /// <summary>
        /// 免检
        /// </summary>
        MJ = 3,      /// <summary>
        /// 试乘试驾
        /// </summary>
        SJ = 4,
        /// <summary>
        /// 订车
        /// </summary>
        DC=5,

        /*******************纯id*********************/
        /// <summary>
        /// 新闻
        /// </summary>
        News = 6,
        /// <summary>
        /// 新闻类别
        /// </summary>
        Newscategory = 7,
        /// <summary>
        /// 服务项目
        /// </summary>
        ServiceItem = 8,
        /// <summary>
        /// 活动
        /// </summary>
        Activity = 9,
        /// <summary>
        /// 活动分类
        /// </summary>
        ActivityCategory = 10,
        /// <summary>
        /// 车主
        /// </summary>
        User = 11,
        /// <summary>
        /// 车辆
        /// </summary>
        Car = 12,
        /// <summary>
        /// 车主级别
        /// </summary>
        CarSeries = 13,
        /// <summary>
        /// 服务分类
        /// </summary>
        ServiceCategory = 14,
        /// <summary>
        /// 服务范围
        /// </summary>
        ServiceRegion = 15,
        /// <summary>
        /// 系统用户
        /// </summary>
        SystemUser = 16,
        /// <summary>
        /// 索九服务
        /// </summary>
        SN = 17,
        /// <summary>
        /// 消费记录
        /// </summary>
        XF = 18,

        /// <summary>
        /// 车系
        /// </summary>
        CX = 19,
        /// <summary>
        /// 缴费获取积分
        /// </summary>
        JF = 20,
        /// <summary>
        /// 北现自有卡券
        /// </summary>
        BH = 21

    }
}
