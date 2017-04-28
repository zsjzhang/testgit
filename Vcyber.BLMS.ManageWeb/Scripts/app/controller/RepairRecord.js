var repairrecord = angular.module('repairrecord', ['ngGrid', 'angularFileUpload']);
repairrecord.controller('RepairRecordCtrl', function ($scope, $http, $routeParams, $location) {

    $scope.serviceTypeDw = { select: -1 };

    $http.post('/Common/GetServiceTypeJsonResult').success(function (data) {
        $scope.serviceTypeDw.data = data;
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
        $scope.filterOptions.serviceType = $scope.serviceTypeDw.select;
        $scope.filterOptions.phoneNumber = $scope.phoneNumber;
        $scope.filterOptions.VIN = $scope.VIN;
        $scope.filterOptions.repairStartTime = $scope.repairStartTime;
        $scope.filterOptions.repairEndTime = $scope.repairEndTime;
        $scope.pagingOptions.currentPage = 1;
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    };
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
          
            var url = "/RepairRecord/GetRepairRecordList?";
            if (filter && filter.phoneNumber != undefined) {
                url += "&phoneNumber=" + filter.phoneNumber;
            }
            if (filter && filter.VIN != undefined) {
                url += "&vin=" + filter.VIN;
            }
            if (filter && filter.serviceType != undefined) {
                url += "&serviceType=" + filter.serviceType;
            }
            if (filter && filter.repairStartTime != undefined) {
                url += "&repairStartTime=" + filter.repairStartTime;
            }
            if (filter && filter.repairEndTime != undefined) {
                url += "&repairEndTime=" + filter.repairEndTime;
            }

            $http.get(url + '&count=' + pageSize + '&skip=' + (page - 1) * pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            });

           //$http.get(eurl, { params: {
           //    vin: filter.VIN,
           //    serviceType: filter.serviceType,
           //    repairStartTime: filter.repairStartTime,
           //    repairEndTime: filter.repairEndTime,
           //    Skip: (page - 1) * pageSize,
           //    Count: pageSize
           //} }).success(function (largeLoad) {
           //     $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
           // });

        }, 100);
    };

    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal && newVal.currentPage !== oldVal.currentPage) {
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
            field: 'RepairReportId',
            displayName: '服务单号',
            sortable: false,
            width: 100
        }, {
            field: 'ServiceType',
            displayName: '服务类型',
            sortable: false,
            width: 150
        }, {
            field: 'ReserveType',
            displayName: '预约方式',
            sortable: false,
            width: 100
        }, {
            field: 'VINCode',
            displayName: 'VIN',
            sortable: false,
            width: 150
        },{
            field: 'DealerName',
            displayName: '经销商',
            sortable: false,
            width: 100
        }, {
            field: 'RepairTime',
            displayName: '服务开始时间',
            sortable: false,
            width: 120
        }, {
            field: 'FinishTime',
            displayName: '服务结束时间',
            sortable: false,
            width: 120
        }, {
            field: 'Status',
            displayName: '服务状态',
            sortable: false,
            width: 100
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});