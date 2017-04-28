var homeModule = angular.module('homeModule', []);
homeModule.controller('HomeStatisticCtrl', ['$scope', '$routeParams', '$location', '$route',
	function ($scope, $routeParams, $location, $route, productService) {

	    //productService.query(function (data) {
		//	$scope.products = data;
		//});
		//$scope.deleteProduct = function (product) {
		//	debugger;
		//	productService.delete({ id: product.id }, function () {
		//		$route.reload();
		//	});
		//}
		//$scope.newProduct = function () {
		//	$location.path('/productNew');
		//}
	}
]);