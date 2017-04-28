var vcyberapp = angular.module('vcyberapp',
    ['ngRoute', 'homeModule', 'ngGrid', 'accountModule', 'membershipModule', 'menuModule', 'integralModule', 'serviceCard', 'memberModule', 'bussinessModule', 'systemManage', 'webix', 'imageCarousel', 'news', 'activities', 'XDActivities', 'userProofCheck', 'magazine', 'card', 'contentapprove', 'userguide', 'repairrecord', 'consultantModule', 'dealerMembership', 'notice', 'shareRes', 'freeRoadRescue', 'sendSMSSchedulePlan', 'gamemanage', 'afterSaleService']);
vcyberapp.directive('ngConfirmClick', [
    function () {
        return {
            priority: 1,
            terminal: true,
            link: function (scope, element, attr) {
                var msg = attr.ngConfirmClick || "Are you sure?";
                var clickAction = attr.ngClick;
                element.bind('click', function (event) {
                    if (window.confirm(msg)) {
                        scope.$eval(clickAction);
                    }
                });
            }
        };
    }]);
vcyberapp.directive('ngFileSelect', ['$rootScope', '$http', function ($rootScope, $http) {
    return function (scope, ele, attr) {
        ele.bind('change', function (e) {
            //上传
            var fn = attr.ngFileSelect;//回调方法，本例为$scope中的upload()方法
            var file = e.target.files[0];
            if (file == undefined) {//没选择文件
                return false;
            }
            var form = new FormData();
            //form.append('token', $rootScope.upload.token);//设置上传token
            form.append("upfile", file);
            //$rootScope.loading = true;
            //$http.post('/News/PostImg', form, {
            //    headers: {
            //        'Content-Type': undefined//如果不设置Content-Type,默认为application/json,七牛会报错
            //    }
            //}).success(function (data) {
            //    $rootScope.loading = false;
            //    scope[fn]($rootScope.upload.cdn + data.key);//上传回调，将url传到upload方法中
            //});
        });
    };
}]);
//router
vcyberapp.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
            when('/Home/Index', {
                templateUrl: '/Home/Statistic',
                controller: 'HomeStatisticCtrl'
            })
            //会员管理
            .when('/Home/Statistic', {
                templateUrl: '/Home/Statistic',
                controller: 'HomeStatisticCtrl'
            })
            //会员管理
            .when('/Report/GetCreatedPerson', {
                templateUrl: '/Report/GetCreatedPerson',
                controller: 'GetCreatedPersonCtrl'
            })
            .when('/Membership/Index', {
                  templateUrl: '/Membership/Index',
                  controller: 'MembershipCtrl'
              }).
            when('/Membership/Create', {
                templateUrl: '/Membership/Create',
                controller: 'MembershipCreateCtrl'
            }).
            when('/Membership/CreateNormal', {
                templateUrl: '/Membership/CreateNormal',
                controller: 'MembershipCreateNormalCtrl'
            }).
            when('/Membership/Active', {
                templateUrl: '/Membership/Active',
                controller: 'MembershipActiveCtrl'
            }).
            when('/Membership/Approving', {
                templateUrl: '/Membership/Approving',
                controller: 'MembershipApprovingListCtrl'
            }).
            when('/Membership/Detail/:id', {
                templateUrl: '/Membership/Detail',
                controller: 'MembershipDetailCtrl'
            }).
            when('/Account/UpdatePwd', {
                templateUrl: '/Account/UpdatePwd',
                controller: 'AccountUpdatePwdCtrl'
            }).
             when('/Membership/MembershipRequestFailed', {
                 templateUrl: '/Membership/MembershipRequestFailed',
                 controller: 'MembershipRequestFailedCtrl'
             }).
            //用户凭证审核
            when('/Membership/UserProofCheck', {
                templateUrl: '/Membership/UserProofCheck',
                controller: 'userProofCheckCtrl'
            }).
            //交易 //收支明细
            when('/Bussiness/Index', {
                templateUrl: '/Bussiness/Index',
                controller: 'incomeAndExpenditureCtrl'
            }).
            //交易 //交易记录
            when('/MemberOrder/Index', {
                templateUrl: '/MemberOrder/Index',
                controller: 'tradeRecordInfoCtrl'
            }).
            //交易 //退款记录
            when('/MemberOrderDrawback/Index', {
                templateUrl: '/MemberOrderDrawback/Index',
                controller: 'orderDrawbackCtrl'
            }).
            when('/OrderInfoAndDetail/Index/:tradeType/:mallCode/:orderCode/:flowNumber', {
                templateUrl: '/OrderInfoAndDetail/Index',
                controller: 'orderDetailInfoCtrl'
            }).
            //用户管理
            when('/Users/User', {
                templateUrl: '/Users/User',
                controller: 'UserManagmentCtrl'
            }).
            //用户角色管理
            when('/UserRoles/Index/:id', {
                templateUrl: '/UserRoles/Index',
                controller: 'AddUser2RoleCtrl'
            }).
            //角色管理
            when('/Roles/Index', {
                templateUrl: '/Roles/Index',
                controller: 'RoleManageCtrl'
            }).
            //权限管理
            when('/Function/Index', {
                templateUrl: '/Function/Index',
                controller: 'FunctionManageCtrl'
            }).
            //轮播图管理
            when('/ImageCarousel/Index', {
                templateUrl: '/ImageCarousel/Index',
                controller: 'ImageCarouselCtrl'
            }).
             when('/ImageCarousel/OperationLog/:Id', {
                 templateUrl: '/ImageCarousel/OperationLog',
                 controller: 'ImageCarouselOperationLogCtrl'
             }).
            //新闻管理
             when('/News/Index', {
                 templateUrl: '/News/Index',
                 controller: 'NewsCtrl'
             })
            .when('/News/CreateNews', {
                templateUrl: '/News/CreateNews',
                controller: 'CreateNewsCtrl'
            }).when('/News/EditNews/:Id', {
                templateUrl: '/News/EditNews',
                controller: 'EditNewsCtrl'
            }).
            when('/News/OperationLog/:Id', {
                templateUrl: '/News/OperationLog',
                controller: 'NewsOperationLogCtrl'
            }).
            //报刊管理
             when('/Magazine/Index', {
                 templateUrl: '/Magazine/Index',
                 controller: 'MagazinelCtrl'
             })
            .when('/Magazine/CreateMagazine', {
                templateUrl: '/Magazine/CreateMagazine',
                controller: 'CreateMagazineCtrl'
            }).when('/Magazine/EditMagazine/:Id', {
                templateUrl: '/Magazine/EditMagazine',
                controller: 'EditMagazineCtrl'
            }).
            when('/Magazine/OperationLog/:Id', {
                templateUrl: '/Magazine/OperationLog',
                controller: 'MagazineOperationLogCtrl'
            }).
            when('/Magazine/MagazineApplyManage', {
                templateUrl: '/Magazine/MagazineApplyManage',
                controller: 'MagazineApplyManageCtrl'
            }).
            //活动管理
            when('/Activities/Index', {
                templateUrl: '/Activities/Index',
                controller: 'ActivitiesCtrl'
            }).when('/Activities/CreateActivities', {
                templateUrl: '/Activities/CreateActivities',
                controller: 'CreateActivitiesCtrl'
            }).when('/Activities/EditActivities/:Id', {
                templateUrl: '/Activities/EditActivities',
                controller: 'EditActivitiesCtrl'
            }).when('/Activities/GetSignUpActivities/:Id', {
                templateUrl: '/Activities/GetSignUpActivities',
                controller: 'SignUpActivitiesCtrl'
            }).when('/Activities/ApprovedActivities/:Id', {
                templateUrl: '/Activities/ApprovedActivities',
                controller: 'ApprovedActivitiesCtrl'
            }).
            when('/Activities/OperationLog/:Id', {
                templateUrl: '/Activities/OperationLog',
                controller: 'ActivitiesOperationLogCtrl'
            }).
            //置换活动管理
            //置换活动首页
            when('/Activities/PermuteIndex', {
                templateUrl: '/Activities/PermuteIndex',
                controller: 'PermuteActivitiesCtrl'
            }).
            //创建置换活动
            when('/Activities/CreatePermuteActivity', {
                templateUrl: '/Activities/CreatePermuteActivity',
                controller: 'CreatePermuteActivitiesCtrl'
            }).
            //编辑置换活动
            when('/Activities/EditPermuteActivity/:Id', {
                templateUrl: '/Activities/EditPermuteActivity',
                controller: 'EditPermuteActivitiesCtrl'
            }).
            //卡劵管理
             when('/Card/Index', {
                 templateUrl: '/Card/Index',
                 controller: 'CardCtrl'
             }).
            //侯机机场管理
             when('/Card/Airport', {
                 templateUrl: '/Card/Airport',
                 controller: 'AirportCtrl'
             })
            .when('/Activities/OperationLog/:Id', {
                templateUrl: '/Activities/OperationLog',
                controller: 'ActivitiesOperationLogCtrl'
            }).
            //电子手册管理
             //报刊管理
             when('/UserGuide/Index', {
                 templateUrl: '/UserGuide/Index',
                 controller: 'UserGuideCtrl'
             })
            .when('/UserGuide/CreateUserGuide', {
                templateUrl: '/UserGuide/CreateUserGuide',
                controller: 'CreateUserGuideCtrl'
            }).when('/UserGuide/EditUserGuide/:Id', {
                templateUrl: '/UserGuide/EditUserGuide',
                controller: 'EditUserGuideCtrl'
            }).
            when('/UserGuide/OperationLog/:Id', {
                templateUrl: '/UserGuide/OperationLog',
                controller: 'UserGuideOperationLogCtrl'
            }).
            //弹出框公告管理
            when('/PopUpNotice/Index', {
                templateUrl: '/PopUpNotice/Index',
                controller: 'PopUpNoticeCtrl'
            }).
            //共享资源文件管理
            when('/ShareResource/Index', {
                templateUrl: '/ShareResource/Index',
                controller: 'ShareResourceCtrl'
            }).when('/ShareResource/ResDownload', {
                templateUrl: '/ShareResource/ResDownload',
                controller: 'ResDownloadCtrl'
            }).
            //审批管理
             when('/ContentApprove/Index', {
                 templateUrl: '/ContentApprove/Index',
                 controller: 'ContentApproveCtrl'
             }).
            when('/RepairRecord/Index', {
                templateUrl: '/RepairRecord/Index',
                controller: 'RepairRecordCtrl'
            }).
            when('/Consultant/Index', {
                templateUrl: '/Consultant/Index',
                controller: 'consultantListCtrl'
            }).
            when('/Consultant/Add', {
                templateUrl: '/Consultant/Add',
                controller: 'consultantCreateCtrl'
            }).
            //App内容管理
            when('/AppManage/Index', {
                templateUrl: '/AppManage/Index',
                controller: 'AppManageCtrl'
            }).
            when('/DealerMembership/Index', {
                templateUrl: '/DealerMembership/Index',
                controller: 'DealerMembershipCtrl'
            }).
            when('/DealerMembership/NoJoin', {
                templateUrl: '/DealerMembership/NoJoin',
                controller: 'DealerMembershipNoJoinCtrl'
            }).
            //报表管理-权益使用分析
            when('/Equity/Index', {
                templateUrl: '/Equity/Index',
                controller: 'EquityCtrl'
            }).
            //报表管理-权益使用分析
            when('/Equity/ServiceUse', {
                templateUrl: '/Equity/ServiceUse',
                controller: 'EquityCtrl'
            }).
            //服务管理-紧急道路救援
            when('/FreeRoadRescue/Index', {
                templateUrl: '/FreeRoadRescue/Index',
                controller: 'FreeRoadRescueCtrl'
            }).
            //系统管理-发送短信任务管理
            when('/SendSMSSchedulePlan/Index', {
                templateUrl: '/SendSMSSchedulePlan/Index',
                controller: 'SendSMSSchedulePlanCtrl'
            }).
            //系统管理-发送短信历史记录
            when('/SendSMSSchedulePlan/Send', {
                templateUrl: '/SendSMSSchedulePlan/Send',
                controller: 'SendSMSSchedulePlanResultCtrl'
            }).
             //游戏部分-活动管理
            when('/BMGameManage/ActivityInfo', {
                templateUrl: '/BMGameManage/ActivityInfo',
                controller: 'ActivityInfoCtrl'
            }).
             //游戏部分-奖品设置
            when('/BMGameManage/JoinActivity', {
                templateUrl: '/BMGameManage/JoinActivity',
                controller: 'JoinActivityCtrl'
            }).
            //游戏部分-获奖名单
            when('/BMGameManage/WinningInfo', {
                templateUrl: '/BMGameManage/WinningInfo',
                controller: 'WinningInfoCtrl'
            }).
             //游戏部分-奖品名单
            when('/BMGameManage/PrizeInfo', {
                templateUrl: '/BMGameManage/PrizeInfo',
                controller: 'PrizesInfoCtrl'
            }).
            //游戏部分-分享名单
            when('/BMGameManage/ShareRecord', {
                templateUrl: '/BMGameManage/ShareRecord',
                controller: 'ShareRecordCtrl'
            }).
            //游戏部分-分享名单
            when('/BMGameManage/ImportWinningInfo', {
                templateUrl: '/BMGameManage/ImportWinningInfo',
                controller: 'ImportWinningInfoCtrl'
            }).
            //售后服务
            when('/AfterSaleService/Index', {
                templateUrl: '/AfterSaleService/Index',
                controller: 'AfterSaleServiceCtrl'
            }).
            //保养套餐
            when('/AfterSaleService/Repair', {
                templateUrl: '/AfterSaleService/Repair',
                controller: 'AfterSaleRepairCtrl'
            }).
            //保养套餐查询
             when('/AfterSaleService/RepairSelect', {
                 templateUrl: '/AfterSaleService/RepairSelect',
                 controller: 'AfterSaleRepairSelectCtrl'
             }).
            //售后服务
            when('/AfterSaleService/RecordList', {
                templateUrl: '/AfterSaleService/RecordList',
                controller: 'AfterSaleServiceListCtrl'
            }).
            //首页
            otherwise({
                redirectTo: '/Home/Index'
            });
    }]);