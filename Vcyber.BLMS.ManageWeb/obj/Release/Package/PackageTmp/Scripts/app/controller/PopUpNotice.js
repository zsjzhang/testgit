var notice = angular.module('notice', ['ngGrid', 'ngCookies']);

//弹出框公告controller
notice.controller('PopUpNoticeCtrl', function ($scope, $http, $routeParams, $location,$cookieStore) {
    //$cookieStore.put("name", "my name");
    $scope.createData = {};
    $scope.filterOptions = {
        useExternalFilter: true
    };
    $scope.totalServerItems = 0;

    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };

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
            + date.getHours()
            + ":"
            + date.getMinutes();
    }

    $scope.setPagingData = function (data, page, pageSize, total_count) {
        for (i = 0 ; i < data.length ; i++) {
            data[i].CreateTime = $scope.ChangeDateFormat(data[i].CreateTime);
        }
        $scope.myData = data;
        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };

    //创建一条公告
    $scope.create = function () {
        $scope.createData.Summary = encodeURI(getContent());
        var timestamp = Date.parse(new Date());

        //默认显示
        if ($scope.createData.IsDisplay == undefined)
        {
            $scope.createData.IsDisplay = "1";
        }

        //添加
        if ($scope.createData.Id == undefined || $scope.createData.Id == null || $scope.createData.Id == "") {
            $http.post('/PopUpNotice/CreateNotice',
                {
                    Title: $scope.createData.Title,
                    IsDisplay: $scope.createData.IsDisplay,
                    Summary: $scope.createData.Summary
                }).success(function (data) {
                    if (data.success) {
                        alert("添加成功");
                        $('#AddNoticeInfo').modal('hide');
                        $scope.getPagedDataAsyncApply($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    } else {
                        alert("添加失败！ \n" + data.msg);
                        $('#AddNoticeInfo').modal('hide');
                    }
                }).error(function (data) {
                    alert("添加失败！ \n" + data.msg);
                });
        }
        //更新
        else {
            $http.post('/PopUpNotice/UpdateNotice',
            {
                ID: $scope.createData.Id,
                Title: $scope.createData.Title,
                IsDisplay: $scope.createData.IsDisplay,
                Summary: $scope.createData.Summary
            }).success(function (data) {
                if (data.success) {
                    alert("保存成功");
                    $('#AddNoticeInfo').modal('hide');
                    $scope.getPagedDataAsyncApply($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                } else {
                    alert("保存失败！ \n" + data.msg);
                    $('#AddNoticeInfo').modal('hide');
                }
            }).error(function (data) {
                alert("保存失败！ \n" + data.msg);
            });
        }
    }


    //查询
    $scope.getPagedDataAsyncApply = function (pageSize, page, filter) {
        var timestamp = Date.parse(new Date());
        var url = "/PopUpNotice/GetAllList?timestamp=" + timestamp;
        $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function (largeLoad) {
            $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
        });
    };

    //删除
    $scope.delNoticeById = function (id) {
        if (confirm("确定要删除？")) {
            $http.post('/PopUpNotice/DelNoticeById', { ID: id }).success(function (data) {
                    if (data.success) {
                        alert("删除成功");
                        $scope.getPagedDataAsyncApply($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    } else {
                        alert("删除失败！ \n" + data.msg);
                    }
                }).error(function (data) {
                    alert("删除失败！ \n" + data.msg);
                });
        }
    };

    //弹出添加
    $scope.addNotice = function () {
        $scope.createData.Id = "";
        $scope.createData.Title = "";
        $scope.createData.IsDisplay = "1";
        $scope.createData.Summary = "";
        setContent("", false);
        $('#AddNoticeInfo').modal('show');
    };

    //编辑
    $scope.editNoticeById = function (row) {
        $scope.createData.Id = row.entity.Id;
        $scope.createData.Title = row.entity.Title;
        $scope.createData.IsDisplay = row.entity.IsDisplay;
        $scope.createData.Summary = row.entity.Summary;
        setContent(decodeURI(row.entity.Summary), false);
        $('#AddNoticeInfo').modal('show');
    };

    $scope.getPagedDataAsyncApply($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal && newVal.currentPage !== oldVal.currentPage) {
            $scope.getPagedDataAsyncApply($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    }, true);
    $scope.$watch('filterOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.getPagedDataAsyncApply($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
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
            field: 'Id',
            displayName: 'Id',
            visible: false
        }, {
            field: 'Title',
            displayName: '公告标题',
            sortable: true,
            width: 600
        }, {
            field: 'CreateTime',
            displayName: '创建时间',
            sortable: true,
            width: 210,
            cellFilter: 'date: "mediumDate"'
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 100,
            cellTemplate: '<div><a href="javascript:return void()"  ng-click="editNoticeById(row)">编辑</a>  ' +
                '<a href="javascript:return void()" ng-click="delNoticeById(row.getProperty(\'Id\'))">删除</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };


});