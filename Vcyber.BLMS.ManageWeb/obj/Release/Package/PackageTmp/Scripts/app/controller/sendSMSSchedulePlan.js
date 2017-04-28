var sendSMSSchedulePlan = angular.module('sendSMSSchedulePlan', ['ngGrid', 'angularFileUpload']);
freeRoadRescue.controller('SendSMSSchedulePlanCtrl', function ($scope, $http, $routeParams, $location, $upload) {
    $scope.formData = {};
    $scope.selectItem = {};

    $scope.addPlanData = {
        SelectTimeType: "",
        SelectCarType: "",
        IsScheduleDate: true,
        IsValueDate: true
    };

    $scope.editPlanData = {
        SelectTimeType: "",
        SelectCarType: "",
        IsScheduleDate: true,
        IsValueDate: true
    };

    $scope.selectIsOpen = "";
    $scope.selectTimeType = "";
    $scope.selectCarType = "";

    $scope.colDefs = [];

    $scope.filterOptions = {
        useExternalFilter: true
    };

    $scope.totalServerItems = 0;

    $scope.pagingOptions = {
        pageSizes: [10,50,100],
        pageSize: 10,
        start: null,
        end: null,
        serviceTitle: null,
        timeType: null,
        state: null,
        carType: null,
        currentPage: 1
    };

    $scope.search = function () {
        $scope.filterOptions.serviceTitle = $scope.formData.ServiceTitle;
        $scope.filterOptions.start = $scope.formData.Start;
        $scope.filterOptions.end = $scope.formData.End;
        $scope.filterOptions.state = $scope.selectIsOpen;
        $scope.filterOptions.timeType = $scope.selectTimeType;
        $scope.filterOptions.carType = $scope.selectCarType;
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
    }

    $scope.setPagingData = function (data, page, pageSize, total_count, count) {

        for (i = 0 ; i < data.length ; i++) {
            data[i].CreateTime = $scope.ChangeDateFormat(data[i].CreateTime);
            data[i].ScheduleTime = $scope.ChangeDateFormat(data[i].ScheduleTime);
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
            var url = "/SendSMSSchedulePlan/List?";
            if (filter && filter.serviceTitle != undefined) {
                url += "&serviceTitle=" + filter.serviceTitle;
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

            if (filter && filter.timeType != undefined) {
                url += "&timeType=" + filter.timeType;
            }

            if (filter && filter.carType != undefined) {
                url += "&carType=" + filter.carType;
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
            field: 'Id ',
            displayName: '编号',
            sortable: false,
            width: 40
        }, {
            field: 'ServiceTitle ',
            displayName: '任务名称',
            sortable: false,
            width: 320
        }, {
            field: 'CarCategory',
            displayName: '适用车型',
            sortable: false,
            width: 130
        }, {
            field: 'TimeTypeName',
            displayName: '定时类型',
            sortable: false,
            width: 130
        }, {
            field: 'ScheduleTime',
            displayName: '计划执行日期',
            sortable: false,
            width: 100
        }, {
            field: 'ValueTime',
            displayName: '距购车之日时限',
            sortable: false,
            width: 120
        }, {
            field: 'IsOpen',
            displayName: '是否开启',
            sortable: false,
            width: 70
        }, {
            field: 'IsFinish',
            displayName: '操作',
            sortable: false,
            width: 150,
            cellTemplate: '<span>&nbsp&nbsp</span><a href="javascript:void()" data-toggle="modal" data-target="#editPlanModal" ng-click="edit(row)">编辑</a><span>&nbsp&nbsp</span><a href="javascript:void()" ng-show="row.entity.IsOpen==\'N\'" ng-click="updateStatus(row)" style="color:red"><strong>开启</strong></a><a href="javascript:void()" ng-show="row.entity.IsOpen==\'Y\'" ng-click="UpdateStatus(row)" style="color:blue"><strong>关闭</strong></a>'
        }, {
            field: 'SMSContent',
            displayName: '短信内容',
            sortable: false,
            width: 300
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.edit = function (row) {
        $scope.editPlanData = {
            SelectTimeType: "",
            SelectCarType: "",
            IsScheduleDate: true,
            IsValueDate: true
        };

        $scope.editPlanData.ServiceTitle = row.entity.ServiceTitle;
        $scope.editPlanData.SelectTimeType = row.entity.TimeType;
        $scope.editPlanData.SelectCarType = row.entity.CarCategory;
        $scope.editPlanData.Id = row.entity.Id;

        if (row.entity.TimeType == "1") {
            $scope.editPlanData.ValueTime = row.entity.ValueTime;
            $scope.editPlanData.IsValueDate = false;
            $scope.editPlanData.IsScheduleDate = true;
        }

        if (row.entity.TimeType == "2") {
            $scope.editPlanData.ScheduleTime = row.entity.ScheduleTime;
            $scope.editPlanData.IsValueDate = true;
            $scope.editPlanData.IsScheduleDate = false;
        }

        if (row.entity.TimeType == "3") {
            $scope.editPlanData.IsValueDate = true;
            $scope.editPlanData.IsScheduleDate = true;
        }

        $scope.editPlanData.SMSContent = row.entity.SMSContent;
    }

    $scope.timeTypeChange_edit = function () {
        if ($scope.editPlanData.SelectTimeType == "1") {
            $scope.editPlanData.IsValueDate = false;
            $scope.editPlanData.IsScheduleDate = true;
            $scope.editPlanData.ScheduleTime = null;
        } else if ($scope.editPlanData.SelectTimeType == "2") {
            $scope.editPlanData.IsValueDate = true;
            $scope.editPlanData.IsScheduleDate = false;
            $scope.editPlanData.ValueTime = null;
        } else {
            $scope.editPlanData.ValueTime = null;
            $scope.editPlanData.ScheduleTime = null;
            $scope.editPlanData.IsValueDate = true
            $scope.editPlanData.IsScheduleDate = true
        }
    }

    $scope.editPlanSave = function () {
        if ($scope.editPlanData.ServiceTitle == "" || $scope.editPlanData.ServiceTitle == undefined) {
            alert("请输入任务名称");
            return false;
        } else if ($scope.editPlanData.SelectTimeType == "" || $scope.editPlanData.SelectTimeType == undefined) {
            alert("请选择定时类型");
            return false;
        } else if ($scope.editPlanData.SelectTimeType == "1" && ($scope.editPlanData.ValueTime == "" || $scope.editPlanData.ValueTime == undefined)) {
            alert("请设定购车日起多少日");
            return false;
        } else if ($scope.editPlanData.SelectTimeType == "2" && ($scope.editPlanData.ScheduleTime == "" || $scope.editPlanData.ScheduleTime == undefined)) {
            alert("请设定计划执行日期");
            return false;
        } else if ($scope.editPlanData.SMSContent == "" || $scope.editPlanData.SMSContent == undefined) {
            alert("请设定短信内容");
            return false;
        } else if ($scope.editPlanData.SelectCarType == "" || $scope.editPlanData.SelectCarType == undefined) {
            alert("请设定适用车型");
            return false;
        } else {
            $http.post('/SendSMSSchedulePlan/Update',
                {
                    Id: $scope.editPlanData.Id,
                    ServiceTitle: $scope.editPlanData.ServiceTitle,
                    TimeType: $scope.editPlanData.SelectTimeType,
                    ScheduleTime: $scope.editPlanData.ScheduleTime,
                    ValueTime: $scope.editPlanData.ValueTime,
                    SMSContent: $scope.editPlanData.SMSContent,
                    CarCategory: $scope.editPlanData.SelectCarType
                }
            ).success(function (data) {
                if (data.IsSuccess) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    alert("操作成功！ \n");
                } else {
                    alert("操作失败！ \n" + data.msg);
                }
            }).error(function (data) {
                alert("操作失败！ \n" + data.msg);
            });

            $scope.editPlanData = {
                SelectTimeType: "",
                SelectCarType: "",
                IsScheduleDate: true,
                IsValueDate: true
            };
        }
    }

    $scope.timeTypeChange = function () {
        if ($scope.addPlanData.SelectTimeType == "1") {
            $scope.addPlanData.IsValueDate = false;
            $scope.addPlanData.IsScheduleDate = true;
        } else if ($scope.addPlanData.SelectTimeType == "2") {
            $scope.addPlanData.IsValueDate = true;
            $scope.addPlanData.IsScheduleDate = false;
        } else {
            $scope.addPlanData.IsValueDate = true
            $scope.addPlanData.IsScheduleDate = true
        }
    }

    $scope.addPlanSave = function () {
        if ($scope.addPlanData.ServiceTitle == "" || $scope.addPlanData.ServiceTitle == undefined) {
            alert("请输入任务名称");
            return false;
        } else if ($scope.addPlanData.SelectTimeType == "" || $scope.addPlanData.SelectTimeType == undefined) {
            alert("请选择定时类型");
            return false;
        } else if ($scope.addPlanData.SelectTimeType == "1" && ($scope.addPlanData.ValueTime == "" || $scope.addPlanData.ValueTime == undefined)) {
            alert("请设定购车日起多少日");
            return false;
        } else if ($scope.addPlanData.SelectTimeType == "2" && ($scope.addPlanData.ScheduleTime == "" || $scope.addPlanData.ScheduleTime == undefined)) {
            alert("请设定计划执行日期");
            return false;
        } else if ($scope.addPlanData.SMSContent == "" || $scope.addPlanData.SMSContent == undefined) {
            alert("请设定短信内容");
            return false;
        } else if ($scope.addPlanData.SelectCarType == "" || $scope.addPlanData.SelectCarType == undefined) {
            alert("请设定适用车型");
            return false;
        } else {
            $http.post('/SendSMSSchedulePlan/Add',
                {
                    ServiceTitle: $scope.addPlanData.ServiceTitle,
                    TimeType: $scope.addPlanData.SelectTimeType,
                    ScheduleTime: $scope.addPlanData.ScheduleTime,
                    ValueTime: $scope.addPlanData.ValueTime,
                    SMSContent: $scope.addPlanData.SMSContent,
                    IsOpen: "Y",
                    CarCategory: $scope.addPlanData.SelectCarType
                }
            ).success(function (data) {
                if (data.IsSuccess) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    alert("操作成功！ \n");
                } else {
                    alert("操作失败！ \n" + data.msg);
                }
            }).error(function (data) {
                alert("操作失败！ \n" + data.msg);
            });

            $scope.addPlanData = {
                SelectTimeType: "",
                SelectCarType: "",
                IsScheduleDate: true,
                IsValueDate: true
            };
        }
    }

    $scope.updateStatus = function (row) {
        if (confirm("确定要更改状态吗？")) {

            var isOpen = "Y";
            if (row.entity.IsOpen == "N") {
                isOpen = "Y";
            } else {
                isOpen = "N";
            }

            $http.post('/SendSMSSchedulePlan/Edit', { id: row.entity.Id, isOpen: isOpen }).success(function (data) {
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

freeRoadRescue.controller('SendSMSSchedulePlanResultCtrl', function ($scope, $http, $routeParams, $location, $upload) {
    $scope.formData = { SelectCarType: "" };
    $scope.selectItem = {};

    $scope.selectIsOpen = "";

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
        userName: null,
        state: null,
        title: null,
        carType: null,
        currentPage: 1
    };

    $scope.search = function () {
        $scope.filterOptions.userName = $scope.formData.UserName;
        $scope.filterOptions.start = $scope.formData.Start;
        $scope.filterOptions.end = $scope.formData.End;
        $scope.filterOptions.state = $scope.selectIsOpen;
        $scope.filterOptions.title = $scope.formData.ServiceTitle;
        $scope.filterOptions.carType = $scope.formData.SelectCarType;
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
            + date.getHours()
            + ":"
            + date.getMinutes();
    }

    $scope.setPagingData = function (data, page, pageSize, total_count, count) {

        for (i = 0 ; i < data.length ; i++) {
            data[i].CreateTime = $scope.ChangeDateFormat(data[i].CreateTime);
            data[i].SendTime = $scope.ChangeDateFormat(data[i].SendTime);
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
            var url = "/SendSMSSchedulePlan/SendList?";
            if (filter && filter.userName != undefined) {
                url += "&userName=" + filter.userName;
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

            if (filter && filter.title != undefined) {
                url += "&title=" + filter.title;
            }

            if (filter && filter.carType != undefined) {
                url += "&carType=" + filter.carType;
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
            field: 'Id ',
            displayName: '编号',
            sortable: false,
            width: 80
        }, {
            field: 'UserName',
            displayName: '接收用户',
            sortable: false,
            width: 120
        }, {
            field: 'ServiceTitle ',
            displayName: '任务名称',
            sortable: false,
            width: 320
        }, {
            field: 'CarCategory ',
            displayName: '适用车型',
            sortable: false,
            width: 130
        }, {
            field: 'IsSend',
            displayName: '是否已发送',
            sortable: false,
            width: 100
        }, {
            field: 'SendTime',
            displayName: '短信发送时间',
            sortable: false,
            width: 130
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.$watch('myData', function () {

    });

});