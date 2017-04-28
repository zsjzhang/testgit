var contentapprove = angular.module('contentapprove', ['ngGrid']);
contentapprove.controller('ContentApproveCtrl', function ($scope, $http, $routeParams, $location) {
    $scope.selectItem = {};
    $scope.approveType = { select: 1 };
    $scope.approveStatus = { select: 0 };
    $scope.filterOptions = {
        useExternalFilter: true
    };
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };
    //活动状态下拉表/// 0:未开始，1：已开始，未结束，2：已结束
    $http.get("/ContentApprove/ApprovedType").success(function(data) {
        $scope.approveType.data = data;
    });
    $http.get("/ContentApprove/ApprovedStatus").success(function (data) {
        $scope.approveStatus.data = data;
    });
    $scope.search = function () {
        $scope.filterOptions.approveType = $scope.approveType.select;
        $scope.filterOptions.approveStatus = $scope.approveStatus.select;
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
       var url = "/ContentApprove/GetList?status=" + filter.approveStatus + "&type=" + filter.approveType;

            $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            });
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

   
    $scope.approvePage = function (row) {
        $scope.selectItem.Id = row.entity.SouceId;
        $scope.selectItem.type = row.entity.Type;
        $scope.selectItem.IsApproved = row.entity.IsApproved;
    }

    $scope.UpdateApproval = function () {
        if ($scope.selectItem.IsApproved == null) {
            $scope.alert = 1;
        }
        $http.post("/ContentApprove/Approval",
                   {
                       id: $scope.selectItem.Id,
                       type:$scope.selectItem.type,
                       status: $scope.selectItem.IsApproved,
                       memo: $scope.selectItem.ApprovalMemo

                   })
                   .success(function (data) {
                       if (data.success) {
                           alert("修改成功!");
                           $('#approveModal').modal('hide');

                       }
                       else
                           alert(data.msg);
                       $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                   });
    }

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
            field: 'SouceId',
            displayName: '编号',
            width: 80,
            sortable: true
        }, {
            field: 'Title',
            displayName: '标题',
            width: 120,
            sortable: false
        }, {
            field: 'Status',
            visible: false
        }, {
            field: 'StatusValue',
            displayName: '状态',
            enableCellEdit: false,
            width: 80
        }, {
            field: 'UpdateTime',
            displayName: '修改日期',
            enableCellEdit: false,
            sortable: true,
            width: 100
        }, {
            field: 'UpdateBy',
            displayName: '修改人',
            sortable: true,
            width: 100
        },{
            field: 'ApproveTime',
            displayName: '审批日期',
            enableCellEdit: false,
            sortable: true,
            width: 100
        }, {
            field: 'ApproveBy',
            displayName: '审批人',
            sortable: true,
            width: 180
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 100,
            cellTemplate: '<div><a  href="" data-toggle="modal" data-target="#approveModal" ng-click="approvePage(row)">审批</a> ' +
                '</div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});
