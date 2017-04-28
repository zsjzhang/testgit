var freeRoadRescue = angular.module('freeRoadRescue', ['ngGrid', 'angularFileUpload']);
freeRoadRescue.controller('FreeRoadRescueCtrl', function ($scope, $http, $routeParams, $location, $upload) {
    $scope.formData = {};
    $scope.selectItem = {};

    $scope.selectCallCenter = "";

    $scope.colDefs = [];

    $scope.filterOptions = {
        useExternalFilter: true
    };

    $scope.totalServerItems = 0;

    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        start: null,
        end: null,
        phoneNumber: null,
        state: 'N',
        currentPage: 1
    };

    $scope.search = function () {
        $scope.filterOptions.phoneNumber = $scope.formData.PhoneNumber;
        $scope.filterOptions.start = $scope.formData.Start;
        $scope.filterOptions.end = $scope.formData.End;
        $scope.filterOptions.state = $scope.selectCallCenter;
        $scope.pagingOptions.currentPage = 1;
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    };

    function checkTime(i) {
        if (i < 10)
        { i = "0" + i }
        return i
    }

    $scope.ChangeDateFormat = function (jsondate) {
        if (jsondate == null)
            return "";

        jsondate = jsondate.replace("/Date(", "").replace(")/", "");
        if (jsondate.indexOf("+") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("+"));
        }
        else if (jsondate.indexOf("-") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("-"));
        }

        var date = new Date(parseInt(jsondate, 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

        return date.getFullYear()
            + "-"
            + month
            + "-"
            + currentDate
            + " "
            + checkTime(date.getHours())
            + ":"
            + checkTime(date.getMinutes());
    }

    $scope.setPagingData = function (data, page, pageSize, total_count, count) {

        for (i = 0 ; i < data.length ; i++) {
            data[i].CreateTime = $scope.ChangeDateFormat(data[i].CreateTime);
            data[i].UpdateTime = $scope.ChangeDateFormat(data[i].UpdateTime);
        }

        $scope.myData = data;

        $scope.totalServerItems = total_count;

        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };

    $scope.getPagedDataAsync = function (pageSize, page, filter) {
        setTimeout(function () {
            var data;
            var url = "/FreeRoadRescue/List?";
            if (filter && filter.phoneNumber != undefined) {
                url += "&phoneNumber=" + filter.phoneNumber;
            }

            if (filter && filter.state != undefined) {
                url += "&state=" + filter.state;
            }

            if (filter && filter.start != undefined) {
                url += "&start=" + filter.start;
            }

            if (filter && filter.end != undefined) {
                url += "&end=" + filter.end;
            }

            $http.get(url + '&pageindex=' + page + '&pagesize=' + pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count, largeLoad.count);
            });

        }, 100);
    };

    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

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
        columnDefs: [
        {
            field: 'IsFinish',
            displayName: '操作',
            sortable: false,
            width: 150,
            cellTemplate: '<span>&nbsp&nbsp</span><a href="javascript:void()" ng-click="Detail(row)">详情</a><span>&nbsp&nbsp</span><a href="javascript:void()" ng-show="row.entity.IsFinish==\'N\'" ng-click="UpdateStatus(row)" style="color:red"><strong>未处理</strong></a><label ng-show="row.entity.IsFinish==\'Y\'" style="color:gray">已处理</label>'
        }, {
            field: 'Id ',
            displayName: '编号',
            sortable: false,
            width: 50
        }, {
            field: 'PhoneNumber ',
            displayName: '手机号',
            sortable: false,
            width: 120
        }, {
            field: 'CreateTime',
            displayName: '时间',
            sortable: false,
            width: 140
        }, {
            field: 'IsFinish',
            displayName: '已处理',
            sortable: false,
            width: 80
        }, {
            field: 'UpdateTime',
            displayName: '处理时间',
            sortable: false,
            width: 140
        }, {
            field: 'Address',
            displayName: '地址',
            sortable: false,
            width: 300
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.Detail = function (row) {
        //$scope.selectItem.Id = row.entity.Id;
        //$scope.selectItem.CreateTime = row.entity.CreateTime;
        //$scope.selectItem.PhoneNumber = row.entity.PhoneNumber;
        //$scope.selectItem.Position = row.entity.Position;
        window.open("/FreeRoadRescue/Detail?Address=" + row.entity.Address + "&State=" + row.entity.IsFinish + "&PhoneNumber=" + row.entity.PhoneNumber + "&Time=" + row.entity.CreateTime + "&Position=" + row.entity.Position, '详细', 'height=500, width=1000, top=100, left=150, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no');
    }

    $scope.UpdateStatus = function (row) {
        if (confirm("确定要标记为已处理吗？")) {
            $http.post('/FreeRoadRescue/UpdateState', { id: row.entity.Id }).success(function (data) {
                if (data.IsSuccess) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    alert("操作成功！ \n");
                } else {
                    alert("操作失败！ \n" + data.msg);
                }
            }).error(function (data) {
                alert("操作失败！ \n" + data.msg);
            });
        }
    };

    $scope.$watch('myData', function () {

    });

});