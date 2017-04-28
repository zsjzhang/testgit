
var vcyberapp = angular.module('vcyberapp',
    ['ngRoute', 'findLoginPassword', 'userQuestion', 'findPayPassword']);

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
                        scope.$eval(clickAction)
                    }
                });
            }
        };
    }]);

//router
vcyberapp.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
            when('/Home/Statistic', {
                templateUrl: '/Home/Statistic',
                controller: 'HomeStatisticCtrl'
            }).when('/Member/Index', {
                templateUrl: '/Member/Index',
                controller: 'MemberCtrl'
            }).when('/MemberBusinessFlow/Index', {
                templateUrl: '/MemberBusinessFlow/Index',
                controller: 'MemberBusinessFlowCtrl'
            }).
            when('/UserPassword/FindLoginPassword', {
                templateUrl: '/UserPassword/FindLoginPassword',
                controller: 'FindLoginPasswordCtrl'
            }).
            when('/UserSecurity/Index', {
                templateUrl: '/UserSecurity/Index',
                controller: 'UserSecurityCtrl'
            }).
            when('/UserQuestion/Create', {
                templateUrl: '/UserQuestion/Create',
                controller: 'CreateUserQuestionCtrl'
            }).
            when('/UserQuestion/Edit', {
                templateUrl: '/UserQuestion/Edit',
                controller: 'EditUserQuestionCtrl'
            }).
            when('/UserPassword/FindPayPassword', {
                templateUrl: '/UserPassword/FindPayPassword',
                controller: 'FindPayPasswordCtrl'
            }).
            //首页
            otherwise({
                redirectTo: '/Home/Statistic'
            });
    }]);