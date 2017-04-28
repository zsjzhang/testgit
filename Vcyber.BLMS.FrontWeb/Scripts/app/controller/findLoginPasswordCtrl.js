var findLoginPassword = angular.module('findLoginPassword', []);

findLoginPassword.controller('FindLoginPasswordCtrl', function ($scope, $http, $routeParams, $location) {
    $scope.showId = "f1";
    $scope.formData = {};
    $scope.questions = new Array(3);//安全问题

    //单击重新改变验证码
    $scope.clickRemoveChangeCode = function () {
        var code = $("#imgCode").attr("src");
        $("#imgCode").attr("src", code + "1");
    }

    //发送手机验证码
    $scope.clickSendValidateCode = function () {
        $scope.mySwitch = true;
        setTimeout(function () {

            var url = "/ValidateCode/Send";

            $http.post(url, $scope.formData).success(function (largeLoad) {
                $scope.errorMessage = largeLoad.Message;

                $scope.mySwitch = false;
            });

        }, 100);
    }


    //验证手机号
    $scope.clickValidatePhoneNumber = function () {
        setTimeout(function () {

            var url = "/UserSecurity/ValidatePhoneNumber";

            $http.post(url, $scope.formData).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.showId = "f2";

                    //处理手机号
                    $scope.formData.PassPhoneNumber = $scope.formData.PhoneNumber.substring(0, 3) + "****" + $scope.formData.PhoneNumber.slice(-4);
                    $scope.errorMessage = null;
                }
                else {
                    $scope.errorPhoneNumber = largeLoad.Errors.PhoneNumber;
                    $scope.errorValidateCode = largeLoad.Errors.ValidateCode;
                    $scope.errorMessage = largeLoad.Message;
                }
            });

        }, 100);
    }

    //选择验证方式
    $scope.clickSelectValidateFunction = function (select) {
        $scope.showId = "f2" + select;

        //密保问题
        if (select == 2 || select == 4)
        {
            setTimeout(function () {
                var url = "/UserSecurity/GetSecurityQuestions";

                $http.post(url, $scope.formData).success(function (largeLoad) {
                    $scope.questions = largeLoad.Data;
                });

            }, 100);
        }
    }

    //身份验证
    $scope.clickValidate = function (select) {
        switch (select) {
            case 1:
                setTimeout(function () {

                    var url = "/UserSecurity/ValidateByPhoneAndIdentity";

                    $http.post(url, $scope.formData).success(function (largeLoad) {
                        if (largeLoad.IsSuccess) {
                            $scope.showId = "f3";
                            $scope.errorMessage = null;
                            $scope.formData.UserGuid = largeLoad.Data.UserGuid;
                        }
                        else {
                            $scope.errorPhoneValidateCode = largeLoad.Errors.PhoneValidateCode;
                            $scope.errorIdentityNumber = largeLoad.Errors.IdentityNumber;
                            $scope.errorMessage = largeLoad.Message;
                        }
                    });

                }, 100);
                break;
            case 2:
                setTimeout(function () {

                    var url = "/UserSecurity/ValidateByPhoneAndSecurityQuestion";

                    var postData = {
                        PhoneNumber: $scope.formData.PhoneNumber,
                        PhoneValidateCode: $scope.formData.PhoneValidateCode,
                        questions : $scope.questions
                    };

                    $http.post(url, postData).success(function (largeLoad) {
                        if (largeLoad.IsSuccess) {
                            $scope.showId = "f3";
                            $scope.errorMessage = null;
                            $scope.formData.UserGuid = largeLoad.Data.UserGuid;
                        }
                        else {
                            $scope.errorPhoneValidateCode = largeLoad.Errors.PhoneValidateCode;
                            $scope.errorMessage = largeLoad.Message;
                            $scope.errorAnswer1 = largeLoad.Errors.Answer0;
                            $scope.errorAnswer2 = largeLoad.Errors.Answer1;
                            $scope.errorAnswer3 = largeLoad.Errors.Answer2;
                        }
                    });

                }, 100);
                break;
            case 3:
                setTimeout(function () {

                    var url = "/UserSecurity/ValidateByEmailAndIdentity";

                    $http.post(url, $scope.formData).success(function (largeLoad) {
                        if (largeLoad.IsSuccess) {
                            $scope.showId = "f3";
                            $scope.errorMessage = null;
                            $scope.formData.UserGuid = largeLoad.Data.UserGuid;
                        }
                        else {
                            $scope.errorEmail = largeLoad.Errors.Email;
                            $scope.errorIdentityNumber = largeLoad.Errors.IdentityNumber;
                            $scope.errorMessage = largeLoad.Message;
                        }
                    });

                }, 100);
                break;
            case 4:
                setTimeout(function () {

                    var url = "/UserSecurity/ValidateByIdentityAndSecurityQuestion";

                    var postData = {
                        IdentityNumber: $scope.formData.IdentityNumber,
                        questions: $scope.questions
                    };

                    $http.post(url, postData).success(function (largeLoad) {
                        if (largeLoad.IsSuccess) {
                            $scope.showId = "f3";
                            $scope.errorMessage = null;
                            $scope.formData.UserGuid = largeLoad.Data.UserGuid;
                        }
                        else {
                            $scope.errorIdentityNumber = largeLoad.Errors.IdentityNumber;
                            $scope.errorMessage = largeLoad.Message;
                            $scope.errorAnswer1 = largeLoad.Errors.Answer0;
                            $scope.errorAnswer2 = largeLoad.Errors.Answer1;
                            $scope.errorAnswer3 = largeLoad.Errors.Answer2;
                        }
                    });

                }, 100);
                break;
            default:
                return;
        }


    }

    //修改密码
    $scope.clickModifyPassword = function () {
        setTimeout(function () {

            var url = "/UserPassword/ResetLoginPassword";

            $http.post(url, $scope.formData).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.showId = "f4";
                    $scope.errorMessage = null;
                }
                else {
                    $scope.errorLoginPassword = largeLoad.Errors.LoginPassword;
                    $scope.errorConfirmLoginPassword = largeLoad.Errors.ConfirmLoginPassword;
                    $scope.errorMessage = largeLoad.Message;
                }
            });

        }, 100);
    }

    //完成，并重新登录
    $scope.clickFinish = function () {
        $location.path('/Account/Login');
    }

});