var menuModule = angular.module('menuModule', []);
menuModule.controller('TopmenuCtrl', ['$scope', '$http', '$routeParams', '$location', '$route',
    function ($scope, $http, $routeParams, $location) {        
        $scope.topmenus = {};
        $scope.activeMenu = {
        };

        $http.get('/Menu/GetMenus').success(function (data) {
            $scope.topmenus = data.Menus;
            angular.forEach($scope.topmenus, function (value, key) {
                $http.get('/Menu/LeftMenu?parentId=' + value.id).success(function (data) {
                    value.items = data.Menus;
                    var location = $location.path();
                    $http.get('/Menu/GetFunByActionAndController?location='+location).success(function(funData) {
                        for (var i = 0; i < $scope.topmenus.length;i++)
                        {
                            if ($scope.topmenus[i].id == funData.Id) {
                                $scope.activeMenu.items = $scope.topmenus[i].items;
                                return;
                            }
                        }
                    });
                    //if ($scope.topmenus.length > 0 && $scope.topmenus[0].items != null && $scope.topmenus[0].items.length > 0 && $scope.activeMenu.items == undefined) {
                    //    $scope.activeMenu.items = $scope.topmenus[0].items;
                    //}
                });
            });
        });

        //$http.get('/Menu/GetTopMenus').success(function (data) {
        //    $scope.topmenus = data;
        //    angular.forEach($scope.topmenus, function (value, key) {
        //        $http.get('/Menu/LeftMenu?parentId=' + value.id).success(function (data) {
        //            value.items = data.Menus;
        //            if ($scope.topmenus.length >0 && $scope.topmenus[0].items != null && $scope.topmenus[0].items.length > 0 && $scope.activeMenu.items==undefined) {
        //                $scope.activeMenu.items = $scope.topmenus[0].items;
        //            }
        //        });
        //    });
        //});


        $scope.ActiveLeftMenu = function (index) {
            $scope.activeMenu.items = $scope.topmenus[index].items;
        }

        $scope.isActive = function (viewLocation) {

            return viewLocation === $location.path();
        };

    }
]);

menuModule.controller('MenuActive',
    function ($scope, $location) {
        $scope.isActive = function (viewLocation) {
            return viewLocation === $location.path();
        };
    });