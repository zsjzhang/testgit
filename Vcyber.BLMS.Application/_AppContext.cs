using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;


namespace Vcyber.BLMS.Application
{
    using Vcyber.BLMS.Application.Common;
    using Vcyber.BLMS.Application.CarService;
    using Vcyber.BLMS.Application.User;
    using Vcyber.BLMS.Application.Invoice;
    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Application.Weixin;

    /// <summary>
    /// 外观模式，封装对象
    /// </summary>
    public class _AppContext
    {
        #region 权限功能
        public static IFunctionApp FunctionApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IFunctionApp>();
            }
        }

        #endregion

        #region 用户

        public static IUserInfoApp UserInfoApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserInfoApp>();
            }
        }

        public static IUserRecordApp UserRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserRecordApp>();
            }
        }

        public static IValidateCodeApp ValidateCodeApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IValidateCodeApp>();
            }
        }

        public static IUserAccountRelevanceApp UserAccountRelevanceApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserAccountRelevanceApp>();
            }
        }

        public static IUserVerifyApp UserVerifyApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserVerifyApp>();
            }
        }

        public static IUserSecurityApp UserSecurityApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserSecurityApp>();
            }
        }

        public static IUserStateApp UserStateApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserStateApp>();
            }
        }

        public static IUserWXBindApp UserWxBindApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserWXBindApp>();
            }
        }

        #endregion

        #region ==== OperationLog ====

        public static ILogOperatorApp LogOperatorApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ILogOperatorApp>();
            }
        }

        public static ILoginMemRecordApp LoginMemRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ILoginMemRecordApp>();
            }
        }

        #endregion

        #region ==== Car Service ====

        public static ITestDrive TestDriveApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ITestDrive>();
            }
        }
        public static IOrderCar OrderCarApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IOrderCar>();
            }
        }
        //public static IScheduleMaint ScheduleMaintApp
        //{
        //    get
        //    {
        //        return AppServiceLocator.Instance.GetInstance<IScheduleMaint>();
        //    }
        //}

        public static ICarServiceUser CarServiceUserApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ICarServiceUser>();
            }
        }

        public static IConsume ConsumeApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IConsume>();
            }
        }

        public static IScheduleService ScheduleServiceApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IScheduleService>();
            }
        }

        public static IRepairReportApp RepairRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IRepairReportApp>();
            }
        }

        public static ICSConsultantApp CSConsultantApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ICSConsultantApp>();
            }
        }
        #endregion

        #region ==== product ====

        public static IOrderApp OrderApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IOrderApp>();
            }
        }

        public static ICategoryApp CategoryApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ICategoryApp>();
            }
        }

        public static IProductApp ProductApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IProductApp>();
            }
        }

        public static IAddressApp AddressApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IAddressApp>();
            }
        }

        public static IShoppingApp ShoppingApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IShoppingApp>();
            }
        }

        public static IBaseCar BaseCarApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IBaseCar>();
            }
        }

        public static ISonataService SonataServiceApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ISonataService>();
            }
        }
        #endregion

        #region ==== BLMSMoney ====

        public static ITradeFlowApp TradeFlowApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ITradeFlowApp>();
            }
        }

        public static IUserBlueBeanApp UserBlueBeanApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserBlueBeanApp>();
            }
        }

        public static IUserIntegralApp UserIntegralApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserIntegralApp>();
            }
        }

        public static ITradePort TradePort
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ITradePort>();
            }
        }

        public static IBreadApp BreadApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IBreadApp>();
            }
        }

        public static IUserEmpiricApp UserEmpiricApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserEmpiricApp>();
            }
        }

        #endregion

        #region==== Common ====
        public static IDealer DealerApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IDealer>();
            }
        }
        #endregion

        #region ==== ImageCarousel ====
        public static IImageCarouselApp ImageCarouselApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IImageCarouselApp>();
            }
        }
        #endregion

        #region ==== News ====
        public static INewsApp NewsApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<INewsApp>();
            }
        }
        #endregion

        #region ==== Activities ====
        public static IActivitiesApp ActivitiesApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IActivitiesApp>();
            }
        }
        public static IActivitiesSignUpApp ActivitiesSignUpApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IActivitiesSignUpApp>();
            }
        }
        #endregion

        #region ==== ApproveRecord ====
        public static IApproveRecordApp ApproveRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IApproveRecordApp>();
            }
        }
        #endregion

        #region ==== Magazine ====

        public static IMagazineApp MagazineApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IMagazineApp>();
            }
        }

        public static IMagazineApplyApp MagazineApplyApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IMagazineApplyApp>();
            }
        }
        #endregion

        #region ==== UserGuide ====
        public static IUserGuideApp UserGuideApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserGuideApp>();
            }
        }
        #endregion

        #region Maintain
         public static IMaintainServiceAPP MaintainServiceAPP
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IMaintainServiceAPP>();
            }
        }
        #endregion

        #region ==== Card ====
        public static IAirportServiceApp AirportServiceApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IAirportServiceApp>();
            }
        }

        public static ICustomCardInfoApp CustomCardInfoApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ICustomCardInfoApp>();
            }
        }

        public static ICustomCardApp CustomCardApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ICustomCardApp>();
            }
        }

        public static ISCServiceCardTypeApp SCServiceCardTypeApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ISCServiceCardTypeApp>();
            }
        }

        public static ICustomCardMerchantConsumeCodeApp CustomCardMerchantConsumeCodeApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ICustomCardMerchantConsumeCodeApp>();
            }
        }
        #endregion

        #region ==== Member ====

        public static IMemberNumberApp MemberNumberApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IMemberNumberApp>();
            }
        }
        public static IReceiveRecordApp ReceiveRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IReceiveRecordApp>();
            }
        }
        public static IOweAggregationsApp OweAggregationsApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IOweAggregationsApp>();
            }
        }
        #endregion

        #region ==== SMS ====

        public static ISmsApp SMSApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ISmsApp>();
            }
        }

        #endregion

        #region ==== Maint ====

        public static IMaintCarOilApp MaintCarOilApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IMaintCarOilApp>();
            }
        }

        public static IMaintCarPackageApp MaintCarPackageApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IMaintCarPackageApp>();
            }
        }


        #endregion

        #region ==== Report ====
        public static IReportApp ReportApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IReportApp>();
            }
        }
        #endregion

        public static IBluebeanWinRecordApp BluebeanWinRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IBluebeanWinRecordApp>();
            }
        }
        public static IDealerMembershipApp DealerMembershipApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IDealerMembershipApp>();
            }
        }

        #region ==== NoticeInfosApp ====
        public static INoticeInfosApp NoticeInfosApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<INoticeInfosApp>();
            }
        }
        #endregion

        #region ==== ShareResourcesApp ====
        public static IShareResourcesApp ShareResourcesApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IShareResourcesApp>();
            }
        }
        #endregion

        #region ==== QuestionnaireApp ====

        public static IQuestionnaireApp QuestionnaireApp
        {
            get { return AppServiceLocator.Instance.GetInstance<IQuestionnaireApp>(); }
        }
        #endregion

        #region ==== ServiceCard ====


        public static IServiceCardApp ServiceCardApp
        {
            get { return AppServiceLocator.Instance.GetInstance<IServiceCardApp>(); }
        }

        public static IServiceCardBatchApp ServiceCardBatchApp
        {
            get { return AppServiceLocator.Instance.GetInstance<IServiceCardBatchApp>(); }
        }

        #endregion


        #region ==== QuestionApp ====

        public static IQuestionApp QuestionApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IQuestionApp>();
            }
        }

        #endregion

        #region ==== OptionApp ====

        public static IOptionApp OptionApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IOptionApp>();
            }
        }

        #endregion

        #region ==== AnswerApp ====

        public static IAnswerApp AnswerApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IAnswerApp>();
            }
        }

        #endregion

        #region ==== QuestionnaireRecord ====

        public static IQuestionnaireRecordApp QuestionnaireRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IQuestionnaireRecordApp>();
            }
        }

        #endregion

        #region ==== QuestionnaireVisitor ====

        public static IQuestionnaireVisitorApp QuestionnaireVisitorApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IQuestionnaireVisitorApp>();
            }
        }

        #endregion

        #region ==== QuestionnaireWinningApp ====

        public static IQuestionnaireWinningApp QuestionnaireWinningApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IQuestionnaireWinningApp>();
            }
        }

        #endregion

        #region=========BMGame===========

        public static IActivityInfoApp ActivityInfoApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IActivityInfoApp>();
            }
        }

        public static IPrizesInfoApp PrizesInfoApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IPrizesInfoApp>();
            }
        }

        public static IWinningInfoApp WinningInfoApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IWinningInfoApp>();
            }
        }

        public static IJoinActivityApp JoinActivityApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IJoinActivityApp>();
            }
        }
        public static IShareRecordApp ShareRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IShareRecordApp>();
            }
        }
        public static IRecommendApp RecommendApp 
        {
            get { return AppServiceLocator.Instance.GetInstance<IRecommendApp>(); }
        }

        #endregion

        #region 紧急救援
        public static IFreeRoadRescueApp FreeRoadRescueApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IFreeRoadRescueApp>();
            }
        }
        #endregion

        public static ISendSMSSchedulePlanApp SendSMSSchedulePlanApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ISendSMSSchedulePlanApp>();
            }
        }
        public static IInvoiceForReserveApp InvoiceForReserveApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IInvoiceForReserveApp>();
            }
        }


        public static IBrandServiceApp BrandServiceApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IBrandServiceApp>();
            }
        }

        public static IRequestLogApp RequestLogApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IRequestLogApp>();
            }
        }

        public static ISC_ServiceCardUsedRecordApp ServiceCardUsedRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<ISC_ServiceCardUsedRecordApp>();
            }
        }


        public static IGetVinCustTimeApp GetVinCustTime
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IGetVinCustTimeApp>();
            }
        }

        public static IFittingValidateApp FittingValidateApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IFittingValidateApp>();
            }
        }

        public static IUserMessageRecordApp UserMessageRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IUserMessageRecordApp>();
            }
        }

        public static IPaymentRecordApp PaymentRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IPaymentRecordApp>();
            }
        }

        #region OrderChange
        public static IXDActivityApp XDActivityApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IXDActivityApp>();
            }
        }
        public static IXDInviterApp XDInviterApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IXDInviterApp>();
            }
        }
        public static IXDLotteryApp XDLotteryApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IXDLotteryApp>();
            }
        }
        public static IXDLotteryRecordApp XDLotteryRecordApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IXDLotteryRecordApp>();
            }
        }
        public static IXDOrderChangeApp XDOrderChangeApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IXDOrderChangeApp>();
            }
        }
        public static IXDSendInfoApp XDSendInfoApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IXDSendInfoApp>();
            }
        }

        #endregion

        #region 结算列表数据
        public static ISettlementApp SettlementApp
        {
            
            get
            {
              return  AppServiceLocator.Instance.GetInstance<ISettlementApp>();
            }
        }
        #endregion
        #region 卡券核销索赔
        public static IXDCarClaimInformationApp CarClaimInformationApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IXDCarClaimInformationApp>();
            }
        }
        #endregion

        #region --微信相关-----
        public static ICustomerServiceMessageApp CustomerServiceMessageApp
        {
            get { return AppServiceLocator.Instance.GetInstance<ICustomerServiceMessageApp>(); }
        }
        public static IRedPackApp RedPackApp
        {
            get { return AppServiceLocator.Instance.GetInstance<IRedPackApp>(); }
        }
        public static IWeixinMerchantApp WeixinMerchantApp
        {
            get { return AppServiceLocator.Instance.GetInstance<IWeixinMerchantApp>(); }
        }
        #endregion
        #region OnlineService
        public static IOnlineServiceApp OnlineServiceApp
        {
            get
            {
                return AppServiceLocator.Instance.GetInstance<IOnlineServiceApp>();
            }
        }
        #endregion
    }
}
