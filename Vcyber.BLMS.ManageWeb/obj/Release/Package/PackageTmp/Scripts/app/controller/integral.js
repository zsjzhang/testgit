var integralModule = angular.module('integralModule', ['ngGrid']);
integralModule.controller('IntegralReleaseListCtrl', function ($scope, $http, $routeParams) {
    $scope.integralData = {};


    //来源下拉框
    $scope.sourceDw = { select: -1 };
    $scope.createSourceDw = { select: -1 };

    $scope.destinationDw = { select: -1 };
    $scope.createDestinationDw = { select: -1 };
    $scope.stateDw = { select: -1 };
    $http.get('/Common/GetSourceAll').success(function (data) {
        $scope.sourceDw.data = data;
        $scope.createSourceDw.data = data;
    });
    $http.get('/Common/GetAllMallJsonResult').success(function (data) {
        $scope.destinationDw.data = data;
        $scope.createDestinationDw.data = data;
    });
    $http.get('/Common/GetIntegralReleaseState').success(function (data) {
        $scope.stateDw.data = data;
    });

    //创建
    $scope.create = function() {
        $http.post("/IntegralRelease/Create",
            { Source: $scope.createSourceDw.select, Destination: $scope.createDestinationDw.select, Integral: $scope.integralData.Integral, Memo: $scope.integralData.Memo })
            .success(function(data) {
                if (data.success) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    $('#createIntegralModal').modal('hide');
                    $scope.integralData = {};
                    $scope.createSourceDw.select = -1;
                    $scope.createDestinationDw.select = -1;
                } else {
                    alert(data.message);
                }

            });
    };

    //发放
    $scope.Release = function (row) {
        if (row.entity.IsRelease) {
            if (window.confirm("是否确定发放?"))
            {
                $http.post("/IntegralRelease/Release",
                    { id: row.entity.ID })
                    .success(function (data) {
                        if (data.success) {
                            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                            alert("发放成功!");
                        }
                        else
                            alert("发放失败!");
                    });
            }
        }
    };

    $scope.Delete = function (row) {
        if (window.confirm("是否确定删除?")) {
            $http.post("/IntegralRelease/Delete",
                    { id: row.entity.ID })
                    .success(function (data) {
                        if (data.success) {                            
                            alert("删除成功!");
                        }
                        else
                            alert("删除失败!");
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    });
        }
    }

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
        $scope.filterOptions.sourceValue = $scope.sourceDw.select;
        $scope.filterOptions.destinationValue = $scope.destinationDw.select;
        $scope.filterOptions.stateValue = $scope.stateDw.select;
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
            var data;
            var url = "/IntegralRelease/IntegralList?";
            if (filter && filter.sourceValue!=undefined && filter.sourceValue != -1)
            {
                url += "&Source=" + filter.sourceValue;
            }
            if (filter && filter.destinationValue != undefined && filter.destinationValue != -1) {
                url += "&SysCode=" + filter.destinationValue;
            }
            if (filter && filter.stateValue != undefined && filter.stateValue != -1) {
                url += "&ReleaseState=" + filter.stateValue;
            }
            $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            });

            //if (filter) {
            //    var ft = filter.toLowerCase();
            //    $http.get(url).success(function (largeLoad) {
            //        data = largeLoad.data.filter(function (item) {
            //            return JSON.stringify(item).toLowerCase().indexOf(ft) != -1;
            //        });
            //        $scope.setPagingData(data, page, pageSize);
            //    });
            //} else {
            //    $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function (largeLoad) {
            //        $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            //    });
            //}
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
        columnDefs: [{
            field: 'ID',
            visible: false
        },{
            field: 'SourceName',
            displayName: '来源',
            width: 120,
            sortable: false
        }, {
            field: 'DestinationName',
            displayName: '去向',
            sortable: false,
            width: 220
        }, {
            field: 'Integral',
            displayName: '积分',
            sortable: false,
            width: 120
        }, {
            field: 'Memo',
            displayName: '备注',
            enableCellEdit: false,
            sortable: false,
            width: 120
        }, {
            field: 'CreateUserName',
            displayName: '创建人',
            enableCellEdit: false,
            sortable: false,           
            width: '20%',
        }, {
            field: 'CreateTime',
            displayName: '创建时间',
            sortable: false,
            width: 120            
        }, {
            field: 'ReleaseState',
            displayName: '发放状态',
            visible: false
        }, {
            field: 'ReleaseStateName',
            displayName: '发放状态',
            sortable: false,
            width: 120
        }, {
            field: 'ReleaseTime',
            displayName: '发放时间',
            sortable: false,
            width: 120
        }, {
            field: 'IsRelease',
            visible: false
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            cellTemplate: '<div><a href="javascript:return void()" ng-show="row.entity.IsRelease" ng-click="Release(row)">发放</a>  <a href="javascript:return void()" ng-show="row.entity.IsRelease" ng-click="Delete(row)">删除</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.CheckIsRelease = function (row) {
        if (row.entity.IsRelease)
            return true;
        else
            return false;
    }
});

//系统账户积分列表
integralModule.controller('IntegralAmountListCtrl', function ($scope, $http, $routeParams) {

    $scope.filterOptions = {
        useExternalFilter: true
    };
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };

    $scope.search = function() {
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
            var data;
            var url = "/IntegralAmount/IntegralAmountList?";

            $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
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
        columnDefs: [{
            field: 'SourceName',
            displayName: '来源',
            width: 120,
            sortable: false
        }, {
            field: 'DestinationName',
            displayName: '商城平台',
            sortable: false,
            width: 220
        }, {
            field: 'Integral',
            displayName: '总积分',
            sortable: false,
            width: 120
        }, {
            field: 'DistributionIntegral',
            displayName: '已分配积分',
            enableCellEdit: false,
            sortable: false,
            width: 120
        }, {
            field: 'AvailableIntegral',
            displayName: '可用积分',
            enableCellEdit: false,
            sortable: false,
            width: '20%',
        }, {
            field: 'CreateTime',
            displayName: '创建时间',
            sortable: false,
            width: 120
        }, {
            field: 'UpdateTime',
            displayName: '更新时间',
            sortable: false,
            width: 120
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

//会员积分管理
integralModule.controller('UserIntegralAmountListCtrl', function ($scope, $http, $routeParams) {
    //来源下拉框
    $scope.sourceDw = { select: -1 };
    $scope.destinationDw = { select: -1 };
    $http.get('/Common/GetSourceAll').success(function (data) {
        $scope.sourceDw.data = data;
    });
    $http.get('/Common/GetAllMallJsonResult').success(function (data) {
        $scope.destinationDw.data = data;
    });

    $scope.userName = "";
    $scope.regStartDate = "";
    $scope.regEndDate = "";

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
        $scope.filterOptions.sourceValue = $scope.sourceDw.select;
        $scope.filterOptions.destinationValue = $scope.destinationDw.select;
        $scope.filterOptions.userName = $scope.userName;
        $scope.filterOptions.regStartDate = $scope.regStartDate;
        $scope.filterOptions.regEndDate = $scope.regEndDate;
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
            var url = "/UserIntegralAmount/UserIntegralAmountList?";
            if (filter && filter.sourceValue != undefined && filter.sourceValue != -1) {
                url += "&Source=" + filter.sourceValue;
            }
            if (filter && filter.destinationValue != undefined && filter.destinationValue != -1) {
                url += "&SysCode=" + filter.destinationValue;
            }
            if (filter && filter.regStartDate != undefined && filter.regStartDate != "") {
                url += "&BeginDate=" + filter.regStartDate;
            }
            if (filter && filter.regEndDate != undefined && filter.regEndDate != "") {
                url += "&EndDate=" + filter.regEndDate;
            }
            if (filter && filter.stateValue != undefined && filter.stateValue != -1) {
                url += "&UserName=" + filter.userName;
            }
            $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
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
        columnDefs: [{
            field: 'Source',
            displayName: '来源',
            width: 120,
            visible: false
        }, {
            field: 'SourceName',
            displayName: '来源',
            width: 120,
            sortable: false
        }, {
            field: 'SysCode',
            displayName: '商城平台',
            visible: false
        }, {
            field: 'SysName',
            displayName: '商城平台',
            sortable: false,
            width: 220
        }, {
            field: 'UserID',
            displayName: '会员名称',
            visible: false
        }, {
            field: 'UserName',
            displayName: '会员名称',
            sortable: false,
            width: 120
        }, {
            field: 'Integral',
            displayName: '会员积分',
            enableCellEdit: false,
            sortable: false,
            width: 120
        }, {
            field: 'CreateTime',
            displayName: '创建时间',
            sortable: false,
            width: 120
        }, {
            field: 'UpdateTime',
            displayName: '更新时间',
            sortable: false,
            width: 120
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            cellTemplate: '<div><a href="#/UserIntegralIssue/Index/{{row.getProperty(\'Source\')}}/{{row.getProperty(\'SysCode\')}}/{{row.getProperty(\'UserID\')}}" >详情</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

//积分详情查询
integralModule.controller('UserIntegralIssueCtrl', function ($scope, $http, $routeParams) {
    $scope.BeginDate = "";
    $scope.EndDate = "";
    $scope.FlowNo = "";
    
    //来源下拉框
    $scope.inorOutTypeDw = { select: -1 };
    $scope.typeCodeDw = { select: -1 };
    $http.get('/Common/GetInorOutTypeJsonResult').success(function (data) {
        $scope.inorOutTypeDw.data = data;
    });
    $http.get('/Common/GetTypeCodeJsonResult').success(function (data) {
        $scope.typeCodeDw.data = data;
    });
        
    $scope.filterOptions = {
        useExternalFilter: true,
        Source: $routeParams.Source,
        SysCode: $routeParams.SysCode,
        UserCode: $routeParams.UserCode
    };
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };

    $scope.search = function () {
        $scope.filterOptions.BeginDate = $scope.BeginDate;
        $scope.filterOptions.EndDate = $scope.EndDate; 
        $scope.filterOptions.FlowNo = $scope.FlowNo;
        $scope.filterOptions.inorOutTypeValue = $scope.inorOutTypeDw.select;
        $scope.filterOptions.typeCodeValue = $scope.typeCodeDw.select;
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
            var url = "/UserIntegralIssue/SelectFromUserIntegralRecord?";
            if (filter && filter.BeginDate != undefined && filter.BeginDate != "") {
                url += "&BeginDate=" + filter.BeginDate;
            }
            if (filter && filter.EndDate != undefined && filter.EndDate != "") {
                url += "&EndDate=" + filter.EndDate;
            }
            if (filter && filter.FlowNo != undefined && filter.FlowNo != "") {
                url += "&FlowNo=" + filter.FlowNo;
            }
            if (filter && filter.Source != undefined && filter.Source != "") {
                url += "&Source=" + filter.Source;
            }
            if (filter && filter.SysCode != undefined && filter.SysCode != "") {
                url += "&SysCode=" + filter.SysCode;
            }
            if (filter && filter.UserCode != undefined && filter.UserCode != "") {
                url += "&UserCode=" + filter.UserCode;
            }
            if (filter && filter.inorOutTypeValue != undefined && filter.inorOutTypeValue != -1) {
                url += "&InorOutType=" + filter.inorOutTypeValue;
            }
            if (filter && filter.typeCodeValue != undefined && filter.typeCodeValue != -1) {
                url += "&TypeCode=" + filter.typeCodeValue;
            }

            $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            });
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
            field: 'Source',
            displayName: '来源',
            width: 120,
            sortable: false
        }, {
            field: 'SysName',
            displayName: '商城平台',
            sortable: false,
            width: 220
        }, {
            field: 'FlowNo',
            displayName: '流水号',
            sortable: false,
            width: 120
        }, {
            field: 'CreateTime',
            displayName: '积分变动时间',
            enableCellEdit: false,
            sortable: false,
            width: 120
        }, {
            field: 'TypeName',
            displayName: '原因',
            enableCellEdit: false,
            sortable: false,
            width: '20%',
        }, {
            field: 'Integral',
            displayName: '积分',
            sortable: false,
            width: 120
        }, {
            field: 'Memo',
            displayName: '备注',
            sortable: false,
            width: 120
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});