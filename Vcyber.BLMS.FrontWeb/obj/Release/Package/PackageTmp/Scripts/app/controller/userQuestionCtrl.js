var userQuestion = angular.module('userQuestion', []);

userQuestion.controller('CreateUserQuestionCtrl', ['$scope', '$http', '$routeParams', '$location', function ($scope, $http, $routeParams, $location) {
    $scope.showId = "f1";
    $scope.formData = { UserGuid: '8cc4e289-9b2b-4c8d-bb6f-db888a268c1f' };
    $scope.questions = new Array();//安全问题

    $scope.userQuestions = new Array(3);
    $scope.userQuestions[0] = { PwId: "", Answer: "" };
    $scope.userQuestions[1] = { PwId: "", Answer: "" };
    $scope.userQuestions[2] = { PwId: "", Answer: "" };

    //加载问题数据
    $scope.load = function () {
        setTimeout(function () {
            var url = "/UserQuestion/GetQuestions";

            $http.post(url).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.questions = largeLoad.Data;
                    $scope.errorMessage = null;
                }
                else {
                    $scope.errorMessage = largeLoad.Message;
                }
            });
        }, 100);
    }

    //验证支付密码
    $scope.clickValidatePayPassword = function () {
        setTimeout(function () {
            var url = "/UserSecurity/ValidatePayPassword";

            $http.post(url, $scope.formData).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.showId = "f2";
                    $scope.errorMessage = null;
                }
                else {
                    $scope.errorPayPassword = largeLoad.Errors.PayPassword;
                    $scope.errorMessage = largeLoad.Message;
                }
            });

        }, 100);
    }

    //创建问题
    $scope.clickCreateUserQuestion = function () {
        setTimeout(function () {
            var url = "/UserQuestion/CreateUserQuestion";

            $http.post(url, $scope.userQuestions).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.showId = "f4";
                    $scope.errorMessage = null;
                }
                else {
                    $scope.errorAnswer1 = largeLoad.Errors.Answer0;
                    $scope.errorAnswer2 = largeLoad.Errors.Answer1;
                    $scope.errorAnswer3 = largeLoad.Errors.Answer2;
                    $scope.errorMessage = largeLoad.Message;
                }
            });

        }, 100);
    }


    //完成
    $scope.clickFinish = function () {
        $location.path("/UserSecurity/Index");
    }




}]);

userQuestion.controller('EditUserQuestionCtrl', ['$scope', '$http', '$routeParams', '$location', function ($scope, $http, $routeParams, $location) {
    $scope.showId = "f1";
    $scope.formData = { UserGuid: '8cc4e289-9b2b-4c8d-bb6f-db888a268c1f' };
    $scope.questions = new Array();//安全问题

    $scope.userQuestions = new Array(3);

    //加载用户密保数据
    $scope.load = function () {
        setTimeout(function () {
            var url = "/UserSecurity/GetSecurityQuestionsByUser";

            $http.post(url).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.userQuestions = largeLoad.Data;
                    $scope.errorMessage = null;
                }
                else {
                    $scope.errorMessage = largeLoad.Message;
                }
            });
        }, 100);

        setTimeout(function () {
            var url = "/UserQuestion/GetQuestions";

            $http.post(url).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.questions = largeLoad.Data;
                    $scope.errorMessage = null;
                }
                else {
                    $scope.errorMessage = largeLoad.Message;
                }
            });
        }, 100);
    }

    //验证支付密码+密保
    $scope.clickValidate = function () {
        setTimeout(function () {
            var url = "/UserSecurity/ValidatePayPasswordAndSecurityQuestion";

            var postData = {
                PayPassword: $scope.formData.PayPassword,
                questions: $scope.userQuestions
            }

            $http.post(url, postData).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.showId = "f2";
                    $scope.errorMessage = null;
                    $scope.errorAnswer1 = '';
                    $scope.errorAnswer2 = '';
                    $scope.errorAnswer3 = '';
                    $scope.userQuestions = new Array(3);
                    $scope.userQuestions[0] = { PwId: "", Answer: "" };
                    $scope.userQuestions[1] = { PwId: "", Answer: "" };
                    $scope.userQuestions[2] = { PwId: "", Answer: "" };
                }
                else {
                    $scope.errorPayPassword = largeLoad.Errors.PayPassword;
                    $scope.errorAnswer1 = largeLoad.Errors.Answer0;
                    $scope.errorAnswer2 = largeLoad.Errors.Answer1;
                    $scope.errorAnswer3 = largeLoad.Errors.Answer2;
                    $scope.errorMessage = largeLoad.Message;
                }
            });

        }, 100);
    }

    //修改问题
    $scope.clickCreateUserQuestion = function () {
        setTimeout(function () {
            var url = "/UserQuestion/CreateUserQuestion";

            $http.post(url, $scope.userQuestions).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.showId = "f3";
                    $scope.errorMessage = null;
                }
                else {
                    $scope.errorAnswer1 = largeLoad.Errors.Answer0;
                    $scope.errorAnswer2 = largeLoad.Errors.Answer1;
                    $scope.errorAnswer3 = largeLoad.Errors.Answer2;
                    $scope.errorMessage = largeLoad.Message;
                }
            });

        }, 100);
    }


    //完成
    $scope.clickFinish = function () {
        $location.path("/UserSecurity/Index");
    }




}]);