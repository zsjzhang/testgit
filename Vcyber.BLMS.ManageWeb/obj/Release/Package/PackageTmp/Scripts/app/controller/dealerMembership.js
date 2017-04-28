var dealerMembership = angular.module('dealerMembership', ['ngGrid', 'angularFileUpload']);
dealerMembership.controller('DealerMembershipCtrl', function ($scope, $http, $routeParams, $location, $upload) {
    $scope.formData = {};
    $scope.selectItem = {};
    $scope.status = new Array();// 状态
    $scope.select = 0;

    $scope.selectUserType = "";//银卡会员类型

    $scope.colDefs = [];

    $scope.filterOptions = {
        useExternalFilter: true
    };
    $http.get('/Common/GetMembershipLevelJsonResult').success(function (data) {
        //data.Delete("索九会员");
        $scope.mLevelDw.data = data;
    });
    $scope.totalServerItems = 0;

    $scope.pagingOptions = {
        pageSizes: [10,50,100],
        pageSize: 10,
        currentPage: 1
    };

    $scope.search = function () {
        $scope.filterOptions.phoneNumber = $scope.formData.PhoneNumber;
        $scope.filterOptions.identityNumber = $scope.formData.IdentityNumber;
        $scope.filterOptions.vin = $scope.formData.VIN;
        $scope.filterOptions.startTime = $scope.formData.StartTime;
        $scope.filterOptions.endTime = $scope.formData.EndTime;
        $scope.filterOptions.userType = $scope.selectUserType; 
        $scope.filterOptions.CarCategory = $scope.CarCategory;
        $scope.filterOptions.PaperWork = $scope.PaperWork;
        $scope.pagingOptions.currentPage = 1;
        $scope.filterOptions.dealerId = $scope.formData.DealerId;
       // $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    };

    $scope.setPagingData = function (data, page, pageSize, total_count, count) {
        $scope.myData = data;

        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };

    $scope.getPagedDataAsync = function (pageSize, page, filter) {
        setTimeout(function () {
            var data;
            var url = "/DealerMembership/MemberList?";

            if (filter && filter.phoneNumber != undefined) {
                url += "&phoneNumber=" + filter.phoneNumber;
            }

            if (filter && filter.identityNumber != undefined) {
                url += "&identityNumber=" + filter.identityNumber;
            }

            if (filter && filter.vin != undefined) {
                url += "&vin=" + filter.vin;
            }

            if (filter && filter.startTime != undefined) {
                url += "&startTime=" + filter.startTime;
            }

            if (filter && filter.endTime != undefined) {
                url += "&endTime=" + filter.endTime;
            }

            if (filter && filter.userType != undefined) {
                url += "&userType=" + filter.userType;
            }
            if (filter && filter.CarCategory != undefined) {
                url += "&CarCategory=" + filter.CarCategory;
            }
            if (filter && filter.PaperWork != undefined) {
                url += "&PaperWork=" + filter.PaperWork;
            }
            if (filter && filter.dealerId != undefined) {
                url += "&DealerId=" + filter.dealerId;
            }

            $http.get(url + '&pageindex=' + page + '&pagesize=' + pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count, largeLoad.count);
            });

        }, 100);
    };
    var flag = ($("#ce").val() != "" ? false : true);

    if (!flag) {
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
    }
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
                field: 'DealerId',
                displayName: '店代码',
                sortable: false,
                width: 120
            } ,{
            field: 'UserName ',
            displayName: '手机号',
            sortable: false,
            width: 120
            },
        {
            field: 'CustName',
            displayName: '姓名',
            sortable: false,
            width: 100
        },
        {
            field: 'NickName',
            displayName: '用户名',
            sortable: false,
            width: 120
        }, {
            field: 'MLevel',
            displayName: '会员等级',
            sortable: false,
            width: 100
        },
        //{
        //    field: 'VIN',
        //    displayName: 'VIN',
        //    sortable: false,
        //    width: 150
        //},
        {
            field: 'IdentityNumber',
            displayName: '证件号',
            sortable: false,
            width: 170
        }, {
            field: 'No',
            displayName: '会员卡号',
            sortable: false,
            width: 150
        }, {
            field: 'CreateTime',
            displayName: '入会时间',
            sortable: false,
            width: 150
        }, {
            field: 'CreatedPerson',
            displayName: '创建人',
            sortable: false,
            width: 120
        }, {
            field: 'PayNumber',
            displayName: '付款码',
            sortable: false,
            width: 120
        }
        , {
            field: 'Gender',
            displayName: '性别',
            sortable: false,
            width: 100
        }
        , {
            field: 'Age',
            displayName: '年龄',
            sortable: false,
            width: 100
        }, {
            field: 'Area',
            displayName: '地区',
            sortable: false,
            width: 100
        } , {
            field: 'City',
            displayName: '城市',
            sortable: false,
            width: 100
        } , {
            field: 'action',
             displayName: '车型',
             sortable: false,
             width: 200,
             cellTemplate: '<a  class="screenshot" href="javascript:return void()"   ng-mouseenter="cs(row)"  ng-mouseleave="cs(row)">{{row.getProperty(\'CarCategory\')}}</a><ul class="ulBox" ng-init="aaa=false" ng-show="aaa"><li>{{row.getProperty(\'CarCategory\')}}</li></ul>'
        },
        {
            field: 'TotalIntegral',
            displayName: '积分总数量',
            sortable: false,
            width: 100
        }, {
            field: 'Shiyongintegral',
            displayName: '已使用积分数量',
            sortable: false,
            width: 100
        }, {
            field: 'Shenyuintegral',
            displayName: '剩余积分数量',
            sortable: false,
            width: 100
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.sendPage = function (row) {
        $scope.selectItem.Id = row.entity.Id;
        $scope.selectItem.Code = row.entity.SNCode;
    }

    $scope.cs = function (row) {
        this.aaa = !this.aaa;
        //this.aaa ^= 1;
};

    $scope.Export = function () {
        setTimeout(function () {
            $scope.filterOptions.phoneNumber = $scope.formData.PhoneNumber;
            $scope.filterOptions.state = $scope.select;

            var data;
            var url = "/Card/Export?";
            if ($scope.filterOptions && $scope.filterOptions.phoneNumber != undefined) {
                url += "&phoneNumber=" + $scope.filterOptions.phoneNumber;
            }

            if ($scope.filterOptions && $scope.filterOptions.state != undefined) {
                url += "&state=" + $scope.filterOptions.state;
            }

            $http.get(url).success(function (largeLoad) {
                alert("导出完成!");
            });

        }, 100);
    }


    $scope.$watch('myData', function () {

    });

});

dealerMembership.controller('DealerMembershipNoJoinCtrl', function ($scope, $http, $routeParams, $location, $upload) {
    $scope.formData = {};

    $scope.newMembership = {};//实体

    $scope.selectUserType = "";//银卡会员类型
  
    $scope.selectItem = {};
    $scope.status = [{ Id: 0, Statu: "全部" }, { Id: 1, Statu: "已入会" }, { Id: 2, Statu: "未入会" }];// 状态
    $scope.select = 0;
    $scope.selectCanJoin = "";
    $scope.IsPay = "";
    $scope.Amount = "";
    $scope.PaperWork = "";
    $scope.colDefs = [];

    $scope.filterOptions = {
        useExternalFilter: true
    };

    $scope.totalServerItems = 0;

    $scope.pagingOptions = {
        pageSizes:  [10,50,100],
        pageSize: 10,
        phoneNumber: null,
        state: 1,
        currentPage: 1
    };

    $scope.search = function () {
        $scope.filterOptions.custName = $scope.formData.CustName;
        $scope.filterOptions.phoneNumber = $scope.formData.PhoneNumber;
        $scope.filterOptions.identityNumber = $scope.formData.IdentityNumber;
        $scope.filterOptions.vin = $scope.formData.VIN;
        $scope.filterOptions.startTime = $scope.formData.StartTime;
        $scope.filterOptions.endTime = $scope.formData.EndTime;
        $scope.filterOptions.status = $scope.select;

        $scope.filterOptions.IsPay = $scope.IsPay;
        $scope.filterOptions.Amount = $scope.Amount;
        $scope.filterOptions.PaperWork = $scope.PaperWork;

        $scope.filterOptions.selectCanJoin = $scope.selectCanJoin;//CRM调查
        $scope.filterOptions.userType = $scope.selectUserType;
        $scope.pagingOptions.currentPage = 1;
        $scope.filterOptions.dealerId = $scope.formData.DealerID;
        //$scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    };

    $scope.setPagingData = function (data, page, pageSize, total_count, count) {
        $scope.myData = data;

        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };

    $scope.getPagedDataAsync = function (pageSize, page, filter) {
    
        setTimeout(function () {
            var data;
            var url = "/DealerMembership/MemberListAll?";
            if (filter && filter.custName != undefined) {
                url += "&custName=" + filter.custName;
            }

            if (filter && filter.phoneNumber != undefined) {
                url += "&phoneNumber=" + filter.phoneNumber;
            }

            if (filter && filter.identityNumber != undefined) {
                url += "&identityNumber=" + filter.identityNumber;
            }

            if (filter && filter.vin != undefined) {
                url += "&vin=" + filter.vin;
            }

            if (filter && filter.startTime != undefined) {
                url += "&startTime=" + filter.startTime;
            }

            if (filter && filter.endTime != undefined) {
                url += "&endTime=" + filter.endTime;
            }

            if (filter && filter.status != undefined) {
                url += "&status=" + filter.status;
            }

            if (filter && filter.selectCanJoin != undefined) {
                url += "&selectCanJoin=" + filter.selectCanJoin;
            }

            if (filter && filter.userType != undefined) {
                url += "&userType=" + filter.userType;
            }

            if (filter && filter.dealerId != undefined) {
                url += "&dealerID=" + filter.dealerId;
            }

            if (filter && filter.IsPay != undefined) {
                url += "&IsPay=" + filter.IsPay;
            }

            if (filter && filter.Amount != undefined) {
                url += "&Amount=" + filter.Amount;
            }
            if (filter && filter.PaperWork != undefined) {
                url += "&PaperWork=" + filter.PaperWork;
            }

            $http.get(url + '&pageindex=' + page + '&pagesize=' + pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count, largeLoad.count);
            });

        }, 100);
    };

    var flag = ($("#ce").val() != "" ? false : true);

    if (!flag) {
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
    }
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
            field: 'CarId',
            displayName: '操作',
            sortable: false,
            width: 120,
            cellTemplate: '<span>&nbsp&nbsp</span><a href="javascript:void()" ng-show="row.entity.MembershipId == null" ng-click="Join(row)">点击入会</a>'
        }, {
            field: 'MembershipId',
            displayName: '入会状态',
            sortable: false,
            width: 100,
            cellTemplate: '<span>&nbsp&nbsp{{row.entity.MembershipId == null ? "否" : "是"}}</span>'
        }, {
            field: 'IsCanJoin',
            displayName: '是否同意入会',
            sortable: false,
            width: 130
        }, {
            field: 'No',
            displayName: '会员卡号',
            sortable: false,
            width: 120
        }, {
            field: 'CustMobile',
            displayName: '手机号',
            sortable: false,
            width: 120
        }, {
            field: 'DealerId',
            displayName: '店代码',
            sortable: false,
            width: 120
        }, {
            field: 'CustName ',
            displayName: '姓名',
            sortable: false,
            width: 100
        },
        {
            field: 'VIN',
            displayName: 'VIN',
            sortable: false,
            width: 160
        },
        {
            field: 'IdentityNumber',
            displayName: '证件号',
            sortable: false,
            width: 170
        }, {
            field: 'Gender',
            displayName: '性别',
            sortable: false,
            width: 100
        }, {
            field: 'BuyTime',
            displayName: '购车时间',
            sortable: false,
            width: 150
        }, {
            field: 'City',
            displayName: '城市',
            sortable: false,
            width: 120
        }, {
            field: 'Address',
            displayName: '地址',
            sortable: false,
            width: 300
        }, {
            field: 'Amount',
            displayName: '应付金额',
            sortable: false,
            width: 100
        } , {
            field: 'IsPayState',
            displayName: '缴费状态',
            sortable: false,
            width: 100
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.Join = function (row) {
        if (confirm("确定要帮助客户入会吗？")) {
            $scope.newMembership.PhoneNumber = row.entity.CustMobile;
            $scope.newMembership.IdentityNumber = row.entity.IdentityNumber;
            $scope.newMembership.NickName = row.entity.DealerId + row.entity.CustName;
            $scope.newMembership.VIN = row.entity.VIN;
            $scope.newMembership.City = row.entity.City;
            $scope.newMembership.Address = row.entity.Address;
            $scope.newMembership.RealName = row.entity.CustName;
            $scope.newMembership.IsAutoJoin = "Y";
            $http.post('/Membership/CreateMembership', $scope.newMembership).success(function (data) {
                if (data.success) {
                    $scope.newMembership = {};
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    alert(data.msg);
                } else {
                    alert("添加会员失败！ \n" + data.msg);
                }
            }).error(function (data) {
                alert("添加会员失败！ \n" + data.msg);
            });
        }
    };

    $scope.Export = function () {
        setTimeout(function () {
            $scope.filterOptions.phoneNumber = $scope.formData.PhoneNumber;
            $scope.filterOptions.state = $scope.select;

            var data;
            var url = "/Card/Export?";
            if ($scope.filterOptions && $scope.filterOptions.phoneNumber != undefined) {
                url += "&phoneNumber=" + $scope.filterOptions.phoneNumber;
            }

            if ($scope.filterOptions && $scope.filterOptions.state != undefined) {
                url += "&state=" + $scope.filterOptions.state;
            }

            $http.get(url).success(function (largeLoad) {
                alert("导出完成!");
            });

        }, 100);
    }


    $scope.$watch('myData', function () {

    });

});