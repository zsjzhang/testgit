var memberModule = angular.module('memberModule', ['ngGrid']);
memberModule.controller('MemberBaseInfoListCtrl', function ($scope, $http, $routeParams) {

    $scope.userTotalCount = "";
    //来源下拉框
    $scope.destinationDw = { select: -1 };
    $http.get('/MemberBaseInfo/GetUserTotalCount').success(function (data) {
        $scope.userTotalCount = data;
    });
    $http.get('/Common/GetAllMallJsonResult').success(function (data) {
        $scope.destinationDw.data = data;
    });


    $scope.filterOptions = {
        useExternalFilter: true
    };
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };

    $scope.search = function () {
        $scope.filterOptions.destinationValue = $scope.destinationDw.select;
        $scope.filterOptions.userName = $scope.userName;
        $scope.pagingOptions.currentPage = 1;
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    }

    $scope.setPagingData = function (data, page, pageSize, total_count) {
        //var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
        $scope.myData = data;
        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };
    $scope.getPagedDataAsync = function (pageSize, page, filter) {
        setTimeout(function () {
            var data;
            var url = "/MemberBaseInfo/MemberJsonList?";
            if (filter && filter.destinationValue != undefined && filter.destinationValue != -1) {
                url += "&systemId=" + filter.destinationValue;
            }
            if (filter && filter.userName != undefined) {
                url += "&userName=" + filter.userName;
            }
            $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            });
        }, 100);
    };

    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    }, true);
    $scope.$watch('filterOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    }, true);

    $scope.gridOptions = {
        i18n: 'zh-cn',
        data: 'myData',
        rowTemplate: '<div style="height: 100%"><div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell ">' +
            '<div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }"> </div>' +
            '<div ng-cell></div>' +
            '</div></div>',
        multiSelect: false,
        enableCellSelection: false,
        enableRowSelection: true,
        enableCellEdit: false,
        enablePinning: false,
        columnDefs: [{
            field: 'userName',
            displayName: '会员名称',
            width: '20%',
            sortable: false
        }, {
            field: 'id',
            displayName: '会员id',
            visible: false
        }, {
            field: 'systemName',
            displayName: '商城平台',
            sortable: false,
            width: '60%'
        }, {
            field: 'stateName',
            displayName: '状态',
            sortable: false,
            width: '10%'
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: '10%',
            cellTemplate: '<div><a href="#/MemberBaseInfo/MemberDetails/{{row.getProperty(\'id\')}}">详情</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

memberModule.controller('MemberDetailCtrl', function ($scope, $http, $routeParams) {

    $scope.memberDetail = {};
    $http.get('/MemberBaseInfo/GetMemberDetails/' + $routeParams.id).success(function (data) {
        $scope.memberDetail = data;
    });
});