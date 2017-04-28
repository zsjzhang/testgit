//交易
var bussinessModule = angular.module('bussinessModule', ['ngGrid']);

//收支明细
bussinessModule.controller('incomeAndExpenditureCtrl', function ($scope, $http, $routeParams) {

    $scope.startDate = "";
    $scope.endDate = "";
    $scope.financeFlowDw = { select: -1 };
    $scope.destinationDw = { select: -1 };
    $scope.orderCode = "";

    $http.get('/Common/GetAllMallJsonResult').success(function (data) {
        $scope.destinationDw.data = data;
    });
    //资金流向
    $http.get('/Common/GetTradeTypeJsonResult').success(function (data) {
        $scope.financeFlowDw.data = data;
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

    $scope.search = function() {
        $scope.filterOptions.startDate = $scope.startDate;
        $scope.filterOptions.endDate = $scope.endDate;
        $scope.filterOptions.financeFlowDw = $scope.financeFlowDw.select;
        $scope.filterOptions.destinationDw = $scope.destinationDw.select;
        $scope.filterOptions.orderCode = $scope.orderCode;
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
            var url = "/Bussiness/BusinessFlow?";
            if (filter && filter.userName != undefined) {
                url += "&StartTime=" + filter.StartTime;
            }
            if (filter && filter.EndTime != undefined) {
                url += "&EndTime=" + filter.EndTime;
            }
            if (filter && filter.destinationDw != undefined && filter.destinationDw != -1) {
                url += "&MallCode=" + filter.destinationDw;
            }
            if (filter && filter.financeFlowDw != undefined && filter.financeFlowDw != -1) {
                url += "&TradeType=" + filter.financeFlowDw;
            }
            if (filter && filter.orderCode != undefined) {
                url += "&OrderCode=" + filter.orderCode;
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
        data: 'myData',
        i18n: 'zh-cn',
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
            field: 'MallName',
            displayName: '商城平台',
            sortable: false,
            width: 100
        }, {
            field: 'MallCode',
            displayName: '商城平台',
            visible: false
        }, {
            field: 'OrderCode',
            displayName: '订单',
            visible: false
        }, {
            field: 'FlowNumber',
            displayName: '流水号',
            width: 200,
            sortable: false
        }, {
            field: 'CreateTime',
            displayName: '日期',
            sortable: false,
            width: 100
        }, {
            field: 'TradeType',
            displayName: '收支类型',
            visible: false
        }, {
            field: 'TradeTypeName',
            displayName: '收支类型',
            sortable: false,
            width: 200
        }, {
            field: 'TradeIntegral',
            displayName: '积分',
            enableCellEdit: false,
            sortable: false,
            width: 80
        }, {
            field: 'Remark',
            displayName: '备注',
            sortable: false,
            width: '40%'
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 80,
            cellTemplate: '<div><a href="#/OrderInfoAndDetail/Index/{{row.getProperty(\'TradeType\')}}/{{row.getProperty(\'MallCode\')}}/{{row.getProperty(\'OrderCode\')}}/{{row.getProperty(\'FlowNumber\')}}">详情</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

//订单详情
bussinessModule.controller('orderDetailInfoCtrl', function ($scope, $http, $routeParams) {
    $scope.orderDetail = {};
    $http.get('/OrderInfoAndDetail/OrderDetail?tradeType=' + $routeParams.tradeType + "&mallCode=" + $routeParams.mallCode + "&orderCode=" + $routeParams.orderCode + "&flowNumber=" + $routeParams.flowNumber).success(function (data) {
        $scope.orderDetail = data;
    });
});

//交易记录
bussinessModule.controller('tradeRecordInfoCtrl', function ($scope, $http, $routeParams) {

    $scope.startDate = "";
    $scope.endDate = "";
    $scope.userName = "";
    $scope.tradeStateDw = { select: -1 };
    $scope.destinationDw = { select: -1 };
    $scope.orderCode = "";

    $http.get('/Common/GetAllMallJsonResult').success(function (data) {
        $scope.destinationDw.data = data;
    });
    //交易状态
    $http.get('/Common/GetOrderTradeStatusJsoinResult').success(function (data) {
        $scope.tradeStateDw.data = data;
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

    $scope.search = function() {
        $scope.filterOptions.startDate = $scope.startDate;
        $scope.filterOptions.endDate = $scope.endDate;
        $scope.filterOptions.tradeStateDw = $scope.tradeStateDw.select;
        $scope.filterOptions.destinationDw = $scope.destinationDw.select;
        $scope.filterOptions.orderCode = $scope.orderCode;
        $scope.filterOptions.userName = $scope.userName;
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
            var url = "/MemberOrder/SelectFromOrderRecord?";
            if (filter && filter.startDate != undefined) {
                url += "&StartTime=" + filter.startDate;
            }
            if (filter && filter.endDate != undefined) {
                url += "&EndTime=" + filter.endDate;
            }
            if (filter && filter.destinationDw != undefined && filter.destinationDw != -1) {
                url += "&MallCode=" + filter.destinationDw;
            }
            if (filter && filter.tradeStateDw != undefined && filter.tradeStateDw != -1) {
                url += "&Status=" + filter.tradeStateDw;
            }
            if (filter && filter.orderCode != undefined) {
                url += "&OrderCode=" + filter.orderCode;
            }
            if (filter && filter.userName != undefined) {
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
            field: 'MallName',
            displayName: '商城平台',
            sortable: false,
            width: 100
        }, {
            field: 'MallCode',
            displayName: '商城平台',
            visible: false
        }, {
            field: 'MallUserName',
            displayName: '会员名称',
            sortable: false,
            width: 200
        }, {
            field: 'OrderCode',
            displayName: '订单号',
            width: '20%'
        }, {
            field: 'FlowNumber',
            displayName: '流水号',
            width: 200,
            sortable: false
        },{
            field: 'UseIntegral',
            displayName: '积分',
            enableCellEdit: false,
            sortable: false,
            width: 80
        }, {
            field: 'TradeStatusName',
            displayName: '交易状态',
            enableCellEdit: false,
            sortable: false,
            width: 80
        }, {
            field: 'CreateTime',
            displayName: '日期',
            sortable: false,
            width: 100
        }, {
            field: 'TradeType',
            displayName: '收支类型',
            visible: false
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 80,
            cellTemplate: '<div><a href="#/OrderInfoAndDetail/Index/{{row.getProperty(\'TradeType\')}}/{{row.getProperty(\'MallCode\')}}/{{row.getProperty(\'OrderCode\')}}/{{row.getProperty(\'FlowNumber\')}}">详情</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

//退款记录
bussinessModule.controller('orderDrawbackCtrl', function ($scope, $http, $routeParams) {

    $scope.startDate = "";
    $scope.endDate = "";
    $scope.userName = "";
    $scope.orderCode = "";
    $scope.drawbackCode = "";
    $scope.statusDw = { select: -1 };
    $scope.destinationDw = { select: -1 };

    $http.get('/Common/GetAllMallJsonResult').success(function (data) {
        $scope.destinationDw.data = data;
    });
    //退款状态
    $http.get('/Common/GetDrawbackStatusJsonResult').success(function (data) {
        $scope.statusDw.data = data;
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

    $scope.search = function() {
        $scope.filterOptions.startDate = $scope.startDate;
        $scope.filterOptions.endDate = $scope.endDate;
        $scope.filterOptions.userName = $scope.userName;
        $scope.filterOptions.statusDw = $scope.statusDw.select;
        $scope.filterOptions.destinationDw = $scope.destinationDw.select;
        $scope.filterOptions.orderCode = $scope.orderCode;
        $scope.filterOptions.drawbackCode = $scope.drawbackCode;
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
            var url = "/MemberOrderDrawback/SelectFromDrawbackRecord?";
            if (filter && filter.startDate != undefined) {
                url += "&StartTime=" + filter.startDate;
            }
            if (filter && filter.endDate != undefined) {
                url += "&EndTime=" + filter.endDate;
            }
            if (filter && filter.userName != undefined) {
                url += "&UserName=" + filter.userName;
            }
            if (filter && filter.destinationDw != undefined && filter.destinationDw != -1) {
                url += "&MallCode=" + filter.destinationDw;
            }
            if (filter && filter.statusDw != undefined && filter.statusDw != -1) {
                url += "&Status=" + filter.statusDw;
            }
            if (filter && filter.OrderCode != undefined) {
                url += "&OrderCode=" + filter.OrderCode;
            }
            if (filter && filter.drawbackCode != undefined) {
                url += "&DrawbackCode=" + filter.drawbackCode;
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
            field: 'MallName',
            displayName: '商城平台',
            sortable: false,
            width: 200
        }, {
            field: 'MallCode',
            displayName: '商城平台',
            visible: false
        }, {
            field: 'MallUserName',
            displayName: '会员名称',
            sortable: false,
            width: 200
        }, {
            field: 'OrderCode',
            displayName: '订单号',
            width: '20%'
        }, {
            field: 'DrawbackCode',
            displayName: '退款编号',
            width: '20%'
        }, {
            field: 'DrawbackMoney',
            displayName: '退款金额',
            width: '120'
        }, {
            field: 'DrawbackIntegral',
            displayName: '退还积分',
            enableCellEdit: false,
            sortable: false,
            width: 80
        }, {
            field: 'DrawbackStatusName',
            displayName: '交易状态',
            enableCellEdit: false,
            sortable: false,
            width: 80
        }, {
            field: 'CreateTime',
            displayName: '日期',
            sortable: false,
            width: 100
        }, {
            field: 'TradeType',
            displayName: '收支类型',
            visible: false
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 80,
            cellTemplate: '<div><a href="#/OrderInfoAndDetail/Index/{{row.getProperty(\'TradeType\')}}/{{row.getProperty(\'MallCode\')}}/{{row.getProperty(\'OrderCode\')}}/0">详情</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});