var accountModule = angular.module('accountModule', []);
accountModule.controller('AccountUpdatePwdCtrl', ['$scope', '$http', '$routeParams', '$location', '$route',
	function ($scope, $http, $routeParams) {

	    $scope.userInfo = {
	        OldPassword: '',
	        Password: '',
	        ConfirmPassword: ''
	    };
	    $scope.updatePwd = function() {
	        $http.post('/Account/UpdatePwd', $scope.userInfo).success(function(data) {
	            if (data.success) {
	                $scope.userInfo = {};
	            }
	            alert(data.message);
	        });

	        //$http({ method: 'POST', url: '/Account/UpdatePwd'}).
	        //success(function (data, status) {
	        //    $scope.status = status;
	        //    $scope.data = data;
	        //}).
	        //error(function (data, status) {
	        //    $scope.data = data || "Request failed";
	        //    $scope.status = status;
	        //});
	    };
	}
]);