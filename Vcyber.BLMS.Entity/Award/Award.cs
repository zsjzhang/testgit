using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 奖品
    /// </summary>
    public class Award
    {
        public int Id { get; set; }

        /// <summary>
        /// 奖品名称
        /// </summary>
        public string Name { get; set; }

        public string ImgUrl { get; set; }

        /// <summary>
        /// 奖品分类（实物、虚拟）
        /// </summary>
        public AwardType AwardType { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public VisualProductType VisualProductType { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
    }

    /// <summary>
    /// 奖品类型（实物、虚拟）
    /// </summary>
    public enum AwardType
    {
        /// <summary>
        /// 实物
        /// </summary>
        physical = 1,

        /// <summary>
        /// 虚拟
        /// </summary>
        visual
    }

    /// <summary>
    /// 奖池
    /// </summary>
    public class LotteryDrawPool
    {
        public int Id { get; set; }

        public int AwardId { get; set; }

        public Award Award { get; set; }

        public LotteryDrawPoolType PoolType { get; set; }

        /// <summary>
        /// 如果是虚拟商品，需要有微信卡券ID
        /// </summary>
        public string CardId { get; set; }

        public int Amount { get; set; }

        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 是否为私有券（特殊券）,私有券不参入到公共抽奖池中，使用特殊发放方式
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// 单个商品中奖几率区间
        /// </summary>
        public string Probability { get; set; }

        public int? ProbabilityLeft { get; set; }

        public int? ProbabilityRight { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int VersionNumber { get; set; }

        public DateTime Created { get; set; }

    }

    public enum VisualProductType 
    {
        /// <summary>
        /// 机场券
        /// </summary>
        AirportTicket=1,

        /// <summary>
        /// 微信卡券
        /// </summary>
        WxCard
    }

    /// <summary>
    /// 奖池类型
    /// </summary>
    public enum LotteryDrawPoolType
    {
        /// <summary>
        /// 试驾有礼
        /// </summary>
        LingDong_TestDeive=1,

        /// <summary>
        /// 推荐有礼
        /// </summary>
        LingDong_Recommend,

        /// <summary>
        /// 试驾有礼 wap
        /// </summary>
        LingDong_TestDeive_Wap,

        /// <summary>
        /// 推荐有礼 wap
        /// </summary>
        LingDong_Recommend_Wap
    }

    public class AwardSendRecord
    {
        public int Id { get; set; }

        public string OpenId { get; set; }

        /// <summary>
        /// 奖品
        /// </summary>
        public Award Award { get; set; }

        /// <summary>
        /// 奖品ID
        /// </summary>
        public int AwardId { get; set; }

        /// <summary>
        /// 奖池ID
        /// </summary>
        public int LotteryDrawPoolId { get; set; }

        /// <summary>
        /// 邮寄地址
        /// </summary>
        public PostAddress PostAddress { get; set; }

        /// <summary>
        /// 邮寄地址ID
        /// </summary>
        public int PostAddressId { get; set; }

        /// <summary>
        /// 虚拟卡券发放状态
        /// </summary>
        public int SendState { get; set; }

        /// <summary>
        /// 奖品发放状态描述
        /// </summary>
        public string SendStateMemo { get; set; }

        public DateTime Created { get; set; }

    }

    /// <summary>
    /// 邮寄地址
    /// </summary>
    public class PostAddress
    {
        public int Id { get; set; }

        /// <summary>
        /// 中奖ID
        /// </summary>
        public int AwardSendRecordId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string OpenId { get; set; }

        public string Address { get; set; }

        public DateTime Created { get; set; }
    }

    public class LotteryModel
    {
        public string PhoneNumber { get; set; }

        public string AwardName { get; set; }

        public AwardType AwardType { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public VisualProductType VisualProductType { get; set; }

    }
}
