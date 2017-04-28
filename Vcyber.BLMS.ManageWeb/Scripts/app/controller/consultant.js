var consultantModule = angular.module('consultantModule', ['ngGrid', 'angularFileUpload']);
consultantModule.controller('consultantListCtrl', function ($scope, $http, $routeParams) {
    $scope.dealerDw = { select: -1 };
    $scope.selectItem = {};

    $scope.queryModel = {};

    $scope.filterOptions = {
        useExternalFilter: true,
        identity: $scope.identity
    };
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };

    $scope.search = function () {
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

    $scope.getPagedDataAsync = function (pageSize, page, filter)
    {
        var url = "/Consultant/Select";
        $http.get(url, { params: { name: $scope.queryModel.Name, dealerId: $scope.queryModel.DealerId, Skip: (page - 1) * pageSize, Count: pageSize } }).success(function (largeLoad) {
            $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            //判断管理员or经销商（显示搜索条件输入框）
            if (largeLoad != null && (largeLoad.DealerId == null || largeLoad.DealerId == ""))
            {
                $("#from_search_info").show();
            }
        });
    };

    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

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

    $scope.detail = function (row) {
        $scope.selectDetail = {};
        $scope.selectDetail.Name = row.entity.Name;
        $scope.selectDetail.DealerName = row.entity.DealerName;
        $scope.selectDetail.Photo = row.entity.Photo;
        $scope.selectDetail.Tel = row.entity.Tel;
        $scope.selectDetail.SexName = row.entity.SexName;
        $scope.selectDetail.Age = row.entity.Age;
        $scope.selectDetail.Comment = row.entity.Comment;
    };

    $scope.deleteConsultant = function (row) {
        if (window.confirm("确定要删除?")) {
            $http.post("/Consultant/DeleteConsultant", { id: row.entity.Id })
            .success(function (data) {
                if (data.success)
                    alert("操作成功!");
                else
                    alert("操作失败!" + data.msg);
            });
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    };

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
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 150,
            cellTemplate: '<a href="javascript:return void()" data-toggle="modal" data-target="#ConsulantDetailModal" ng-click="detail(row)">详情</a> '
                + '<a href="javascript:return void()" ng-click="deleteConsultant(row)">删除</a> '
        }, {
            field: 'Name',
            displayName: '姓名',
            sortable: false,
            width: 130
        }, {
            field: 'DealerName',
            displayName: '经销商名称',
            sortable: false,
            width: 260
        }, {
            field: 'DealerId',
            displayName: '特约店代码',
            sortable: false,
            width: 260
        }, {
            field: 'Tel',
            displayName: '电话',
            sortable: false,
            width: 200
        }, {
            field: 'SexName',
            displayName: '性别',
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

consultantModule.controller('consultantCreateCtrl', function ($scope, $http, $routeParams, $upload, $location) {
    $scope.isShowDealerDw = false;

    $scope.newConsultant = {};

    $scope.provinceDw = { select: -1 };
    $scope.cityDw = { select: -1 };
    $scope.dealerDw = { select: -1 };//4s店

    //$http.get('/Membership/GetMembershipDetail', { params: { id: $routeParams.id } }).success(function (data) {
    //    $scope.dealerDw.data = data;
    //});

    $http.post('/Consultant/GetDealerIdForClient').success(function (data) {
        if (data.DealerId == "")
            $scope.isShowDealerDw = true;
    });

    $http.post('/Common/GetDealerProvinceListJsonResult').success(function (data) {
        $scope.provinceDw.data = data;
        $scope.provinceChange();
        $scope.cityChange();
    });

    $scope.provinceChange = function () {
        $http.post('/Common/GetDealerCityListJsonResult', {  province: $scope.provinceDw.select  }).success(function (data) {
            $scope.cityDw.select = -1;
            $scope.cityDw.data = data;
            $scope.cityChange();
        });
    };
    $scope.myOptions = $scope.dealerDw[0];
    $scope.cityChange = function () {
        $http.post('/Common/GetDealerListJsonResult', {  province: $scope.provinceDw.select, city: $scope.cityDw.select }).success(function (data) {
            $scope.dealerDw.select = -1;
            $scope.dealerDw.data = data;
        });
    };

    $scope.create = function () {
        var dealerSelected = eval("(" + $scope.myOptions + ")");
        if (dealerSelected!=undefined && dealerSelected != "undefined") {
            $scope.newConsultant.DealerId = dealerSelected.id;
            $scope.newConsultant.DealerName = dealerSelected.value;
        }
        $http.post('/Consultant/AddConsultant', $scope.newConsultant).success(function (data) {
            if (data.success) {
                $scope.newConsultant = {};
                alert("添加经销商顾问成功");
                $location.path("/Consultant/Index");
            } else {
                alert("添加经销商顾问失败！ \n" + data.msg);
                $location.path("/Consultant/Index");
            }
        }).error(function (data) {
            alert("添加经销商顾问失败！ \n" + data.msg);
            $location.path("/Consultant/Index");
        });

    };

    $scope.upload = function (files) {
        if (files && files.length) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                $upload.upload({
                    url: '/Scripts/ueditor/net/controller.ashx?action=uploadimage',
                    file: file,
                    fileFormDataName: 'upfile'
                }).progress(function (evt) {
                    var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                }).success(function (data, status, headers, config) {
                    $scope.newConsultant.Photo = data.url;
                }).error(function (err) {
                    alert('上传失败');
                });
            }
        }
    };
});