using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 服务卡验证
    /// </summary>
    public class ServiceCardValidate
    {

        #region ==== 私有方法 ====

        /// <summary>
        /// 卡卷类型与消费类型映射关系数据源
        /// </summary>
        private static Dictionary<string, string> cardType = null;

        #endregion

        #region ==== 构造函数 ====

        static ServiceCardValidate()
        {
#warning ===== 先这么写吧 ====
            cardType = new Dictionary<string, string>();
            cardType.Add("SD02", "T02");//活动消费类型

            cardType.Add("SD03", "T01");//4s店消费类型
        }

        public ServiceCardValidate() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 根据服务卡批次 验证卡卷是否过期
        /// </summary>
        /// <returns>true:过期</returns>
        public static bool IsBatchNo(string batchNo)
        {
            var data = _DbSession.ServiceCardBatchStorager.FindBatchOne(batchNo);
            return data == null ? false : data.IsOverdue;
        }

        /// <summary>
        /// 验证卡卷是否可以使用
        /// </summary>
        /// <param name="cardNo">卡卷类型</param>
        /// <param name="consumeType">消费类型</param>
        /// <returns>true:可以消费</returns>
        public static bool IsUseCard(string cardNo, string consumeType, out string message)
        {
            message = string.Empty;
            var data = _DbSession.ServiceCardStorager.FindOne(cardNo);

            if (data == null)
            {
                message = "卡卷不存在";
                return false;
            }

            if (data.IsOverdue.Equals("已过期"))
            {
                message = "已过期";
                return false;
            }

            if (data.Status == EServiceCardStatus.YHX.ToInt32())
            {
                message = "已使用";
                return false;
            }

            if (data.TypeCode.Equals("SD01"))
            {
                return true;
            }

            if (cardType[data.TypeCode].Contains(consumeType))
            {
                return true;
            }

            message = "卡卷不能用于此消费。";
            return false;
        }

        #endregion
    }
}
