using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.IRepository.CarService;
using Vcyber.BLMS.IRepository.Common;

namespace Vcyber.BLMS.IRepository
{
    using Vcyber.BLMS.IRepository;
    using Vcyber.BLMS.IRepository.User;
    using Vcyber.BLMS.IRepository.Weixin;

    public static class _DbSession
    {
        #region ==== User ====

        public static IUserStorager UserStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserStorager>(); }
        }

        public static IUserAccountRelevanceStorager UserAccountRelevanceStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserAccountRelevanceStorager>(); }
        }

        public static IValidateCodeStorager ValidateCodeStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IValidateCodeStorager>(); }
        }

        public static IUserVerifyStorager UserVerifyStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserVerifyStorager>(); }
        }

        public static IUserRecordStorager UserRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserRecordStorager>(); }
        }

        public static IUserStateStorager UserStateStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserStateStorager>(); }
        }

        public static IUserWXStorager UserWxStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserWXStorager>(); }
        }

        #endregion

        #region ==== Function ====

        public static IFunctionStorager FunctionStorager { get { return AppServiceLocator.Instance.GetInstance<IFunctionStorager>(); } }
        public static IFunctionUrlStorager FunctionUrlStorager { get { return AppServiceLocator.Instance.GetInstance<IFunctionUrlStorager>(); } }

        #endregion

        #region ==== OpeartionLog ====
        public static ILogOperatorStorager LogStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ILogOperatorStorager>(); }
        }

        public static ILoginMemRecordStorager LoginMemRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ILoginMemRecordStorager>(); }
        }

        #endregion

        #region ==== Common ====

        public static IDictionary Dictionary
        {
            get { return AppServiceLocator.Instance.GetInstance<IDictionary>(); }
        }

        public static IIdGeneratorStorager IdGeneratorStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IIdGeneratorStorager>(); }
        }

        public static IDealerStorager DealerStorager
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IDealerStorager>();
            }
        }

        #endregion

        #region ==== UserSecurity ====
        public static IPwQuestionStorager PwQuestionStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IPwQuestionStorager>(); }
        }

        public static IUserPwQuestionStorager UserPwQuestionStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserPwQuestionStorager>(); }
        }
        #endregion

        #region ==== Product ====

        public static ICategoryStorager CategoryStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ICategoryStorager>(); }
        }

        public static IProductStorager ProductStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IProductStorager>(); }
        }

        public static IShoppingStorager ShoppingStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IShoppingStorager>(); }
        }

        #endregion

        #region ==== Order ====

        public static IOrderStorager OrderStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IOrderStorager>(); }
        }


        public static IBaseCarStorager BaseCarStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IBaseCarStorager>(); }
        }

        public static ISonataServiceStorager SonataServiceStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ISonataServiceStorager>(); }
        }

        public static IAddressStorager AddressStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IAddressStorager>(); }
        }

        #endregion

        #region ==== BLMSMoney ====

        public static IUserIntegralStorager UserIntegralStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserIntegralStorager>(); }
        }

        public static IUserBlueBeanStorager UserBlueBeanStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserBlueBeanStorager>(); }
        }

        public static ITradeFlowStorager TradeFlowStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ITradeFlowStorager>(); }
        }

        public static IBlueBeanRuleStorager BlueBeanRuleStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IBlueBeanRuleStorager>(); }
        }

        public static IUserEmpiricStorager UserEmpiricStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserEmpiricStorager>(); }
        }

        public static IUserEmpiricRuleStorager UserEmpiricRuleStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserEmpiricRuleStorager>(); }
        }

        #endregion

        #region ==== Car Service ====
        public static ITestDriveStorager TestDriveStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ITestDriveStorager>(); }
        }

        public static IOrderCarStorager OrderCarStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IOrderCarStorager>(); }
        }

        public static IScheduleMaintStorager ScheduleMaintStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IScheduleMaintStorager>(); }
        }

        public static ICarServiceUserStorager CarServiceUserStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ICarServiceUserStorager>(); }
        }

        public static IConsumeStorager ConsumeStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IConsumeStorager>(); }
        }

        public static IScheduleServiceStorager ScheduleServiceStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IScheduleServiceStorager>(); }
        }

        public static IRepairRecordStorager RepairRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IRepairRecordStorager>(); }
        }

        public static ICSConsultantStorager CSConsultantStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ICSConsultantStorager>(); }
        }

        #endregion

        #region ====AirpoortService
        public static ISNCardStorager SNCardStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ISNCardStorager>(); }
        }


        #endregion

        #region ==== ImageCarousel ====
        public static IImageCarouselStorager ImageCarouselStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IImageCarouselStorager>(); }
        }

        public static IImgPraiseRecordStorager ImgPraiseRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IImgPraiseRecordStorager>(); }
        }
        #endregion

        #region ==== News ====
        public static INewsStorager NewsStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<INewsStorager>(); }
        }
        #endregion

        #region ==== Activities ====
        public static IActivitiesStorager ActivitiesStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IActivitiesStorager>(); }
        }
        public static IActivitiesSignUpStorager ActivitiesSignUpStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IActivitiesSignUpStorager>(); }
        }
        #endregion

        #region ==== ApproveRecord ====
        public static IApproveRecordStorager ApproveRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IApproveRecordStorager>(); }
        }
        #endregion

        #region ==== Magazine ====
        public static IMagazineStorager MagazineStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IMagazineStorager>(); }
        }

        public static IMagazineApplyStorager MagazineApplyStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IMagazineApplyStorager>(); }
        }
        #endregion

        #region Maintain
         public static IMaintainServiceRepository MaintainServiceRepository
        {
            get { return AppServiceLocator.Instance.GetInstance<IMaintainServiceRepository>(); }
        }
        #endregion

        #region ==== UserGuide ====
        public static IUserGuideStorager UserGuideStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserGuideStorager>(); }
        }
        #endregion

        #region ==== Member ====

        public static IMemberNumberRepository MemberNumberRepository
        {
            get { return AppServiceLocator.Instance.GetInstance<IMemberNumberRepository>(); }
        }
        public static IReceiveRecordRepository ReceiveRecordRepository
        {
            get { return AppServiceLocator.Instance.GetInstance<IReceiveRecordRepository>(); }
        }
        public static IOweAggregationsStorager OweAggregationsStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IOweAggregationsStorager>(); }
        }
        #endregion

        #region ==== Maint ====

        public static IMaintCarOilStorager MaintCarOilStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IMaintCarOilStorager>(); }
        }

        public static IMaintCarPackageStorager MaintCarPackageStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IMaintCarPackageStorager>(); }
        }

        #endregion

        #region ==== Report ====
        public static IReportStorager ReportStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IReportStorager>(); }
        }

        #endregion

        #region ==== NoticeInfo ====
        public static INoticeInfoStorager NoticeInfoStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<INoticeInfoStorager>(); }
        }

        #endregion

        #region ==== ShareRes ====
        public static IShareResourcesStorager ShareResourcesStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IShareResourcesStorager>(); }
        }

        #endregion

        #region ==== Questionnaire ====
        public static IQuestionnaireStorager QuestionnaireStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IQuestionnaireStorager>(); }
        }
        #endregion

        public static ISmsStorager SmsStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ISmsStorager>(); }
        }

        public static IDealerMembershipStorager DealerMembershipStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IDealerMembershipStorager>(); }
        }

        public static IQuestionStorager QuesStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IQuestionStorager>(); }
        }

        public static IOptionStorager OptionStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IOptionStorager>(); }
        }

        public static IAnswerStorager AnswerStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IAnswerStorager>(); }
        }

        public static IQuestionnaireRecordStorager QuestionnaireRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IQuestionnaireRecordStorager>(); }
        }

        public static IQuestionnaireVisitorStorager QuestionnaireVisitorStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IQuestionnaireVisitorStorager>(); }
        }

        #region 游戏功能部分

        public static IActivityInfoStorager ActivityInfoStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IActivityInfoStorager>(); }
        }

        public static IJoinActivityStorager JoinActivityStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IJoinActivityStorager>(); }
        }

        public static IPrizesInfoStorager PrizesInfoStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IPrizesInfoStorager>(); }
        }

        public static IWinningInfoStorager WinningInfoStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IWinningInfoStorager>(); }
        }

        public static IShareRecordStorager ShareRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IShareRecordStorager>(); }
        }
        #endregion

        #region ==== QuestionnaireManageStorager ====
        public static IQuestionnaireManageStorager QuestionnaireManageStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IQuestionnaireManageStorager>(); }
        }
        #endregion

        #region ==== QuestionnaireWinningStorager ====
        public static IQuestionnaireWinningStorager QuestionnaireWinningStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IQuestionnaireWinningStorager>(); }
        }
        #endregion

        #region 卡劵

        public static ISysConsumePointStorager SysConsumePointStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ISysConsumePointStorager>(); }
        }

        public static IServiceCardBatchStorager ServiceCardBatchStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IServiceCardBatchStorager>(); }
        }

        public static IServiceCardStorager ServiceCardStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IServiceCardStorager>(); }
        }

        public static IServiceCardUsedRecordStorager ServiceCardUsedRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IServiceCardUsedRecordStorager>(); }
        }

        public static IServiceCardBatchConsumeStorager ServiceCardBatchConsumeStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IServiceCardBatchConsumeStorager>(); }
        }

        public static ICustomCardInfoStorager CustomCardInfoStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ICustomCardInfoStorager>(); }
        }


        public static ICustomCardStorager CustomCardStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ICustomCardStorager>(); }
        }

        public static ISCServiceCardTypeStorager SCServiceCardTypeStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ISCServiceCardTypeStorager>(); }
        }
        public static ICustomCardMerchantConsumeCodeStorager CustomCardMerchantConsumeCodeStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ICustomCardMerchantConsumeCodeStorager>(); }
        }

        #endregion

        #region 紧急救援
        public static IFreeRoadRescueStorager FreeRoadRescueStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IFreeRoadRescueStorager>(); }
        }
        #endregion

        public static ISendSMSSchedulePlanStorager SendSMSSchedulePlanStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ISendSMSSchedulePlanStorager>(); }
        }
        public static IInvoiceForReserveRepository InvoiceForReserveRepository
        {
            get { return AppServiceLocator.Instance.GetInstance<IInvoiceForReserveRepository>(); }
        }


        public static IBrandServiceCodeStorager BrandServiceCodeStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IBrandServiceCodeStorager>(); }
        }

        public static IMembershipBrandStorager MembershipBrandStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IMembershipBrandStorager>(); }
        }

        public static IRequestLogStorager RequestLogStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IRequestLogStorager>(); }
        }

        public static ISC_ServiceCardUsedRecordStorager SC_ServiceCardUsedRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<ISC_ServiceCardUsedRecordStorager>(); }
        }
        public static IGetVinCustTimeStorager GetVinCustTimeStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IGetVinCustTimeStorager>(); }
        }

        public static IFittingValidateStorager FittingValidateStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IFittingValidateStorager>(); }
        }

        public static IUserMessageRecordStorager UserMessageRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IUserMessageRecordStorager>(); }
        }
        public static IMembershipLoginNotifyStorager MembershipLoginNotifyStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IMembershipLoginNotifyStorager>(); }
        }

        public static IPaymentRecordStorager PaymentRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IPaymentRecordStorager>(); }
        }

        #region OrderChange
        public static IXDActivityStorager XDActivityStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IXDActivityStorager>(); }
        }
        public static IXDInviterStorager XDInviterStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IXDInviterStorager>(); }
        }
        public static IXDLotteryRecordStorager XDLotteryRecordStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IXDLotteryRecordStorager>(); }
        }
        public static IXDLotteryStorager XDLotteryStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IXDLotteryStorager>(); }
        }
        public static IXDOrderChangeStorager XDOrderChangeStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IXDOrderChangeStorager>(); }
        }
        public static IXDSendInfoStorager XDSendInfoStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IXDSendInfoStorager>(); }
        }
        #endregion

        #region 结算列表
        public static IXDSettlementStorager XDSettlementStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IXDSettlementStorager>(); }
        }
        #endregion

        #region 卡券核销索赔
        public static IXDCarClaimInformationStorager XDCarClaimInformationStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IXDCarClaimInformationStorager>(); }
        }
        #endregion

        #region --微信相关-----
        public static ICustomerServiceMessageStorager CustomerServiceMessageStorager 
        {
            get { return AppServiceLocator.Instance.GetInstance<ICustomerServiceMessageStorager>(); }
        }
        public static IRedPackStorager RedPackStorager 
        {
            get { return AppServiceLocator.Instance.GetInstance<IRedPackStorager>(); }
        }
        public static IWeixinMerchantStorager WeixinMerchantStorager
        {
            get { return AppServiceLocator.Instance.GetInstance<IWeixinMerchantStorager>(); }
        }
        #endregion
        #region OnlineService
        public static IOnlineServiceStorager OnlineServiceStorager {
            get {
                return AppServiceLocator.Instance.GetInstance<IOnlineServiceStorager>();
            }
        }
        #endregion
    }
}
