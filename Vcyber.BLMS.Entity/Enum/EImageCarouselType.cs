using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    public enum EImageCarouselType
    {
        /// <summary>
        /// 首页轮播图
        /// </summary>
        [EnumDescribe("首页")]
        Home=0,

        /// <summary>
        /// 新闻轮播图
        /// </summary>
        [EnumDescribe("新闻")]
        News=1,

        /// <summary>
        /// 活动轮播图
        /// </summary>
        [EnumDescribe("活动")]
        Activities=2,

        /// <summary>
        /// 商品轮播图
        /// </summary>
        [EnumDescribe("商品")]
        Product=3,

        [EnumDescribe("首页6+1")]
        Home7=4
    }
}
