using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class CustomCardInfoApp : ICustomCardInfoApp
    {
        /// <summary>
        /// 添加卡券
        /// </summary>
        /// <param name="model">卡券信息</param>
        /// <returns></returns>
        public ReturnResult AddCustomCardInfo(CustomCardInfo model)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            if (string.IsNullOrEmpty(model.CardName))
            {
                result.IsSuccess = false;
                result.Message = "卡劵名称不能为空";
                return result;
            }
            if (string.IsNullOrEmpty(model.ActivityType))
            {
                result.IsSuccess = false;
                result.Message = "活动名称不能为空";
                return result;
            }
            if (string.IsNullOrEmpty(model.CardLogoUrl))
            {
                result.IsSuccess = false;
                result.Message = "卡券logo不能为空";
                return result;
            }
            model.CreateDate = DateTime.Now;
            model.status = 1;
            model.CardType = Guid.NewGuid().ToString();
            var addResult = _DbSession.CustomCardInfoStorager.AddCustomCardInfo(model);
            if (!addResult)
            {
                result.IsSuccess = false;
                result.Message = "添加卡券失败";
                return result;
            }
            return result;
        }

        public CustomCardInfo GetCustomCardInfo(int id)
        {
            return _DbSession.CustomCardInfoStorager.GetCustomCardInfo(id);
        }

        /// <summary>
        /// 更新卡券信息
        /// </summary>
        /// <param name="model">卡券信息</param>
        /// <returns></returns>
        public ReturnResult UpdateCustomCardInfo(CustomCardInfo model)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            if (string.IsNullOrEmpty(model.CardName))
            {
                result.IsSuccess = false;
                result.Message = "卡劵名称不能为空";
                return result;
            }
            if (model.Quantity <= 0)
            {
                result.IsSuccess = false;
                result.Message = "请正确输入卡券库存";
                return result;
            }
            if (model.ReduceCost < 0)
            {
                result.IsSuccess = false;
                result.Message = "请正确输入卡券金额";
                return result;
            }
            //if (string.IsNullOrEmpty(model.CardLogoUrl))
            //{
            //    result.IsSuccess = false;
            //    result.Message = "卡券logo不能为空";
            //    return result;
            //}
            model.UpdateDate = DateTime.Now;
            var updateResult = _DbSession.CustomCardInfoStorager.UpdateCustomCardInfo(model);
            if (!updateResult)
            {
                result.IsSuccess = false;
                result.Message = "更新卡券失败";
                return result;
            }
            return result;
        }

        /// <summary>
        /// 删除一条卡券
        /// </summary>
        /// <param name="id">卡券ID</param>
        /// <returns></returns>
        public ReturnResult DeleteCustomCardById(string cardType)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            var updateResult = _DbSession.CustomCardInfoStorager.DeleteCustomCardById(cardType);
            if (!updateResult)
            {
                result.IsSuccess = false;
                result.Message = "删除卡券失败";
                return result;
            }
            return result;
        }

        /// <summary>
        /// 更新卡券库存
        /// </summary>
        /// <param name="id">卡券ID</param>
        /// <param name="quantity">卡券库存数量</param>
        /// <returns></returns>
        public ReturnResult UpdateCustomCardQuantity(int id, int quantity)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            var updateResult = _DbSession.CustomCardInfoStorager.UpdateCustomCardQuantity(id, quantity);
            if (!updateResult)
            {
                result.IsSuccess = false;
                result.Message = "更新卡券失败";
                return result;
            }
            return result;
        }

        /// <summary>
        /// 获取卡券信息列表；
        /// </summary>
        /// <param name="source">卡券来源</param>
        /// <param name="merchant">合作商户所属的商户名称</param>
        /// <param name="actType">活动名称</param>
        /// <param name="cardName">卡券名称</param>
        /// <param name="status">已领取</param>
        /// <param name="reduceCost">卡券金额</param>
        /// <param name="pageIndex">分页码</param>
        /// <param name="pageSize">分页数字</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>卡券信息列表</returns>
        public IEnumerable<CustomCardInfo> GetCustomCardInfoList(int source, string merchant, string actType, string cardName, int status, string reduceCost, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.CustomCardInfoStorager.GetCustomCardInfoList(source, merchant, actType, cardName, status,
                 reduceCost, pageIndex, pageSize, out totalCount);
        }


        /// <summary>
        /// 返回一张优惠券信息
        /// </summary>
        /// <param name="cardType">优惠券GUID</param>
        /// <returns>返回一张优惠券信息</returns>
        public ReturnCustomCardInfo GetSingleCustomCardInfo(string cardType)
        {
            return _DbSession.CustomCardInfoStorager.GetSingleCustomCardInfo(cardType);
        }


        public CustomCardInfo GetSingleCustomCardInfoByGuid(string cardType)
        {
            return _DbSession.CustomCardInfoStorager.GetSingleCustomCardInfoByGuid(cardType);
        }


        public ReturnUserCustomCardInfo GetSingleUserCustomCardInfoByIdAndUserId(string id, string userId)
        {
            return _DbSession.CustomCardInfoStorager.GetSingleUserCustomCardInfoByIdAndUserId(id, userId);
        }


        public bool UpdateCustomCardQuantityByType(string cardType)
        {
            return _DbSession.CustomCardInfoStorager.UpdateCustomCardQuantityByType(cardType);
        }


        public IEnumerable<CustomCardInfo> GetCustomCardInfoListByActType(string actType, string cardName)
        {
            return _DbSession.CustomCardInfoStorager.GetCustomCardInfoListByActType(actType,cardName);
        }
        public ReturnResult IsExistsCustomCardInfo(string actType, string cardNmae, int source)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            if (string.IsNullOrEmpty(cardNmae))
            {
                result.IsSuccess = false;
                result.Message = "卡劵名称不能为空";
                return result;
            }
            if (string.IsNullOrEmpty(actType))
            {
                result.IsSuccess = false;
                result.Message = "活动名称不能为空";
                return result;
            }
            var cardResult = _DbSession.CustomCardInfoStorager.IsExistsCustomCardInfo(actType, cardNmae, source);
            if (cardResult)
            {
                result.IsSuccess = false;
                result.Message = "该卡券名称已经存在";
                return result;
            }
            return result;
        }



        /// <summary>
        /// 获取当前卡券库存
        /// </summary>
        /// <param name="cardType">卡券类型</param>
        /// <returns></returns>
        public ReturnResult GetCustomCardQuantityByCardType(string cardType)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(cardType);
            if (customCardInfo == null)
            {
                result.Message = "卡券类型不存在";
                result.IsSuccess = false;
                return result;
            }
            if (DateTime.Now < customCardInfo.CardBeginDate || DateTime.Now > customCardInfo.CardEndDate)
            {
                result.Message = "领取的卡券不在有效期内，请联系管理员";
                result.IsSuccess = false;
                return result;
            }
            if (customCardInfo.CardSource == (int)EMerchantType.Bjxd)
            {
                #region  北京现代卡券
                var usedCount = _AppContext.CustomCardApp.GetCardUsedCount(customCardInfo.CardType);
                result.Data = customCardInfo.Quantity - usedCount;
                result.IsSuccess = true;
                return result;
                #endregion
            }
            else
            {
                #region  合作商户 卡券
                var merchant = _AppContext.CustomCardMerchantConsumeCodeApp.GetCardMerchantConsumeCodeCount(cardType, 1);
                result.IsSuccess = true;
                result.Data = merchant;
                return result;
                #endregion
            }
        }

        /// <summary>
        /// 根据cardNO查找卡券信息
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public CustomCardInfo GetSingleCustomCardInfoByCNo(string cardNo)
        {
            return _DbSession.CustomCardInfoStorager.GetSingleCustomCardInfoByCNo(cardNo);
        }
    }
}
