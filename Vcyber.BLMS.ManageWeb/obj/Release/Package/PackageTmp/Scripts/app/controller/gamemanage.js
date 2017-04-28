var gamemanage = angular.module('gamemanage', ['ngGrid', 'angularFileUpload']);

gamemanage.controller('ActivityInfoCtrl', function ($scope, $http, $routeParams, $location, $upload) {
    $scope.activityInfoData = {};
    $scope.activityinfoListData = new Array();
    $scope.selectactivityitem = {};
    $scope.formData = {};

    $scope.filterOptions = {
        useExternalFilter: true
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

    $scope.setPagingData = function (data) {
        for (i = 0 ; i < data.length ; i++) {
            data[i].StartDate = $scope.ChangeDateFormat(data[i].StartDate);
            data[i].EndDate = $scope.ChangeDateFormat(data[i].EndDate);
            data[i].UpdateDate = $scope.ChangeDateFormat(data[i].UpdateDate);
            data[i].CreateDate = $scope.ChangeDateFormat(data[i].CreateDate);
        }
        $scope.myData = data;

        $scope.totalServerItems = data.length;
        if (!$scope.$$phase) {
            $scope.$apply();
        }

    };

    //加载活动
    $scope.getPagedDataAsync = function () {
        setTimeout(function () {
            var data;
            var url = "/BMGameManage/SelectActivityInfoList";

            $http.post(url).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.result.activityInfos);
            });

        }, 100);
    };

    $scope.getPagedDataAsync();

    //查询活动
    $scope.search = function () {
        var id = $scope.formData.ActivityId;
        if (id == "") {
            $scope.getPagedDataAsync();
            return;
        }
        setTimeout(function () {
            var data;
            var url = "/BMGameManage/SelectActivityInfoListById?id=" + id;

            $http.post(url).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.result.activityInfos);
            });
        }, 100);
    }

    $scope.sendModel = function (row) {
        $scope.selectactivityitem.Id = row.entity.Id;
        $scope.selectactivityitem.Name = row.entity.Name;
        $scope.selectactivityitem.Intro = row.entity.Intro;
        $scope.selectactivityitem.StartDate = row.entity.StartDate;
        $scope.selectactivityitem.EndDate = row.entity.EndDate;
    }

    //修改活动
    $scope.editActivityInfo = function () {

        var id = $scope.selectactivityitem.Id;
        var name = $scope.selectactivityitem.Name;
        var intro = $scope.selectactivityitem.Intro;
        var startDate = $scope.selectactivityitem.StartDate;
        var endDate = $scope.selectactivityitem.EndDate;
        if (name == "" || intro == "" || startDate == "" || endDate == "") {
            alert("数据格式错误");
            return false;
        }

        $http.post("/BMGameManage/EditActivityInfo", {
            Id: id,
            Name: name,
            Intro: intro,
            StartDate: startDate,
            EndDate: endDate
        }).success(function (result) {
            if (result.issuccess) {
                $scope.getPagedDataAsync();
                $("#editActivityInfoModal").modal('hide');
            }
            else {
                alert("修改失败:" + result.message);
                return false;
            }
        }).error(function (data) { alert(data); });
    }

    //添加活动
    $scope.addActivityInfo = function () {
        var name = $scope.activityInfoData.Name;
        var intro = $scope.activityInfoData.Intro;
        var startDate = $scope.activityInfoData.StartDate;
        var endDate = $scope.activityInfoData.EndDate;
        if (name == "" || intro == "" || startDate == "" || endDate == "") {
            alert("数据格式错误");
            return false;
        }

        $http.post("/BMGameManage/AddActivityInfo", {
            Name: name,
            Intro: intro,
            StartDate: startDate,
            EndDate: endDate
        }).success(function (result) {
            if (result.issuccess) {
                $scope.getPagedDataAsync();
                $("#addActivityInfoModal").modal('hide');
            }
            else {
                alert("添加失败:" + result.message);
                return false;
            }
        }).error(function (data) { alert(data); });
    }

    //结束活动
    $scope.endActivityInfo = function () {
        var activityId = $scope.selectactivityitem.Id;
        $http.post("/BMGameManage/EndActivityInfo", {
            id: activityId,
        }).success(function (result) {
            if (result.issuccess) {
                alert("活动已结束！");
                $scope.getPagedDataAsync();
            }
            else {
                alert("操作失败");
                return false;
            }
            $("#deleteActivityInfoModal").modal('hide');
        }).error(function (data) { alert(data); });
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
        columnDefs: [
        {
            field: 'Id',
            displayName: '活动编号',
            sortable: true,
            width: 80
        }
        , {
            field: 'Name',
            displayName: '活动名称',
            sortable: false,
            width: 160
        }, {
            field: 'Intro',
            displayName: '活动简介',
            sortable: false,
            width: 180
        }, {
            field: 'StartDate',
            displayName: '开始时间',
            sortable: false,
            width: 100
        }, {
            field: 'EndDate',
            displayName: '结束时间',
            sortable: false,
            width: 100
        }, {
            field: 'UpdateDate',
            displayName: '最后更新时间',
            sortable: false,
            width: 100
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 160,
            cellTemplate: '<div><button type="button" data-toggle="modal" data-target="#editActivityInfoModal" class="btn btn-link" ng-click="sendModel(row)">修改</button><button type="button" data-toggle="modal" data-target="#deleteActivityInfoModal" ng-click="sendModel(row)" class="btn btn-link">结束活动</button></div>'
        }],
        enablePaging: false,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        //pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

gamemanage.controller('JoinActivityCtrl', function ($scope, $http, $routeParams, $location, $upload) {

    $scope.formData = {};

    $scope.filterOptions = {
        useExternalFilter: true
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

    $scope.setPagingData = function (data, page, pageSize, total_count, count) {
        for (i = 0 ; i < data.length ; i++) {
            data[i].CreateDate = $scope.ChangeDateFormat(data[i].CreateDate);
        }
        $scope.myData = data;

        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }

    };
    //加载参与人员
    $scope.getJoinActivityAsync = function (pageSize, page, filter) {
        setTimeout(function () {
            var data;
            var url = "/BMGameManage/SelectJoinActivityList?";
            if (filter && filter.Id != undefined) {
                url += "&id=" + filter.Id;
            } else {
                url += "&id=0";
            }
            $http.post(url + '&pagesize=' + pageSize + '&pageindex=' + page).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.result.joinactivity, page, pageSize, largeLoad.result.totalnum, largeLoad.count);
            });
        }, 100);
    };

    $scope.search = function () {
        var id = $scope.formData.ActivityId
        if (id && id != undefined)
            $scope.filterOptions.Id = id;
        else {
            $scope.filterOptions.Id = 0;
        }
        $scope.getJoinActivityAsync($scope.pagingOptions.pageSize, 1, $scope.filterOptions);
    }

    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        phoneNumber: null,
        state: 1,
        iscallcenter: "",
        start: null,
        end: null,
        currentPage: 1
    };

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal && newVal.currentPage !== oldVal.currentPage) {
            $scope.getJoinActivityAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    }, true);

    $scope.$watch('filterOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.getJoinActivityAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    }, true);

    $scope.getJoinActivityAsync(10, 1, null);
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
            field: 'ActivityId',
            displayName: '活动编号',
            sortable: true,
            width: 80
        },
        {
            field: 'UserId',
            displayName: '用户编号',
            sortable: true,
            width: 80
        }
        , {
            field: 'Name',
            displayName: '姓名',
            sortable: false,
            width: 100
        }, {
            field: 'Tel',
            displayName: '电话号码',
            sortable: false,
            width: 120
        }, {
            field: 'Province',
            displayName: '省',
            sortable: false,
            width: 80
        }, {
            field: 'City',
            displayName: '市',
            sortable: false,
            width: 80
        }, {
            field: 'Area',
            displayName: '县区',
            sortable: false,
            width: 80
        }, {
            field: 'Address',
            displayName: '详细地址',
            sortable: false,
            width: 155
        }
        , {
            field: 'Email',
            displayName: '邮箱',
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

gamemanage.controller('WinningInfoCtrl', function ($scope, $http, $routeParams, $location, $upload) {
    $scope.formData = {};

    $scope.winningFilter = {};

    $scope.selectItem = {};

    $scope.prizesInfo = {};

    $scope.filterOptions = {
        useExternalFilter: true
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
            data[i].UpdateTime = $scope.ChangeDateFormat(data[i].UpdateTime);
        }
        $scope.myData = data;

        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }

    };
    //加载参与人员
    $scope.getWinningInfosAsync = function (pageSize, page, filter) {
        setTimeout(function () {
            var data;
            var url = "/BMGameManage/SelectWinnintInfos?";
            if (filter && filter.Id != undefined) {
                url += "&id=" + filter.Id;
            } else {
                url += "&id=0";
            }
            $http.post(url + '&pagesize=' + pageSize + '&pageindex=' + page).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.result.winningInfos, page, pageSize, largeLoad.result.totalNum);
            });
        }, 100);
    };

    $scope.search = function () {
        var id = $scope.formData.ActivityId
        if (id && id != undefined)
            $scope.filterOptions.Id = id;
        else {
            $scope.filterOptions.Id = 0;
        }
        $scope.getWinningInfosAsync($scope.pagingOptions.pageSize, 1, $scope.filterOptions);
    }

    $scope.pagingOptions = {
        pageSizes: [10, 5, 2],
        pageSize: 10,
        phoneNumber: null,
        state: 1,
        iscallcenter: "",
        start: null,
        end: null,
        currentPage: 1
    };


    //$scope.ImportWinningInfo = function () {
    //    $http.post("/BMGameManage/ImportWinningInfo",
    //             {
    //                 path: $scope.selectItem.file
    //             })
    //             .success(function (data) {
    //                 if (data.IsSuccess) {
    //                     alert("导入成功!");
    //                     $('#winningInfoImportModal').modal('hide');
    //                 }
    //                 else {
    //                     alert(data.Message);
    //                 }
    //                 $scope.getWinningInfosAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    //             });
    //}

    //$scope.upload = function (files) {
    //    if (files && files.length) {
    //        for (var i = 0; i < files.length; i++) {
    //            var file = files[i];
    //            $upload.upload({
    //                url: '/Scripts/ueditor/net/controller.ashx?action=uploadfile',
    //                file: file,
    //                fileFormDataName: 'upfile'
    //            }).progress(function (evt) {
    //                var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
    //                console.log('progress: ' + progressPercentage + '% ' +
    //                            evt.config.file.name);
    //            }).success(function (data, status, headers, config) {
    //                alert(data.url);
    //                $scope.selectItem.file = data.url;
    //                //console.log('file ' + config.file.name + 'uploaded. Response: ' +
    //                //            JSON.stringify(data));
    //            }).error(function (err) {
    //                alert('上传失败');
    //            });
    //        }
    //    }
    //};

    //$scope.Import = function () {
    //    $("#formToUpdate").ajaxSubmit({
    //        type: 'post',
    //        url: '/BMGameManage/ImportWinningInfo',
    //        success: function (jsonret) {
    //            if (jsonret.issuccess) {
    //                alert("导入成功！");
    //            } else {
    //                alert("导入失败：" + jsonret.msg);
    //            }
    //            //$scope.getWinningInfosAsync(10, 1, null);
    //            //$scope.pagingOptions.currentPage = 1;
    //            //$("#winningInfoImportModal").modal("hide");
    //        },
    //        error: function () {
    //            alert("系统异常");
    //        }
    //    });


        //var url = "/BMGameManage/ImportWinningInfo";
        //setTimeout(function () {
        //    $http.post(url).success(function (jsonret) {
        //        if (jsonret.issuccess) {
        //            alert("导入成功！");
        //        } else {
        //            alert("导入失败：" + jsonret.msg);
        //        }
        //        $scope.getWinningInfosAsync(10, 1, null);
        //        $scope.pagingOptions.currentPage = 1;
        //        $("#winningInfoImportModal").modal("hide");
        //    });
        //}, 100);
    //}

    $scope.sendPrizeModel = function (prizesId) {
        setTimeout(function () {
            var url = "/BMGameManage/GetPrizeInfoById?prizesId=" + prizesId;
            $http.post(url).success(function (result) {
                $scope.prizesInfo = result.prizeinfo;
            });
        }, 100);
    }

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal && newVal.currentPage !== oldVal.currentPage) {
            $scope.getWinningInfosAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    }, true);

    $scope.$watch('filterOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.getWinningInfosAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    }, true);

    $scope.getWinningInfosAsync(10, 1, null);

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
            displayName: '编号',
            sortable: false,
            width: 100
        }, {
            field: 'ActivityId',
            displayName: '活动编号',
            sortable: true,
            width: 80
        },
        {
            field: 'PrizesId',
            displayName: '奖品',
            enableCellEdit: false,
            sortable: false,
            width: 80,
            cellTemplate: '<div><button type="button" data-toggle="modal" data-target="#showPrizeInfoModal" class="btn btn-link btn-block" ng-click="sendPrizeModel(row.entity.PrizesId)">{{row.entity.PrizesId}}</button></div>'
        }
        , {
            field: 'UserName',
            displayName: '用户名',
            sortable: false,
            width: 100
        }, {
            field: 'UserTel',
            displayName: '电话号码',
            sortable: false,
            width: 120
        }, {
            field: 'Province',
            displayName: '省',
            sortable: false,
            width: 80
        }, {
            field: 'City',
            displayName: '市',
            sortable: false,
            width: 80
        }, {
            field: 'Area',
            displayName: '县区',
            sortable: false,
            width: 80
        }, {
            field: 'Address',
            displayName: '详细地址',
            sortable: false,
            width: 180
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

});

gamemanage.controller('PrizesInfoCtrl', function ($scope, $http, $routeParams, $location, $upload) {
    $scope.prizesInfoData = {};
    $scope.prizeInfoListData = new Array();
    $scope.selectdataitem = {};
    $scope.formData = {};

    $scope.filterOptions = {
        useExternalFilter: true
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
            data[i].UpdateTime = $scope.ChangeDateFormat(data[i].UpdateTime);
        }
        $scope.myPrizeInfoData = data;

        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }

    };

    $scope.search = function () {
        var id = $scope.formData.ActivityId;
        if (id && id != undefined) {
            $scope.filterOptions.Id = id;
        } else {
            $scope.filterOptions.Id = 0;
        }
        setTimeout(function () {
            $scope.getPrizeInfoAsync(10, 1, $scope.filterOptions);
        }, 100);
    }

    $scope.pagingOptions = {
        pageSizes: [10, 5, 2],
        pageSize: 10,
        phoneNumber: null,
        state: 1,
        iscallcenter: "",
        start: null,
        end: null,
        currentPage: 1
    };

    //获取奖品
    $scope.getPrizeInfoAsync = function (pageSize, pageIndex, filter) {
        setTimeout(function () {
            var data;
            var url = "/BMGameManage/SelectPrizeInfos?";
            if (filter && filter.Id != undefined) {
                url += "&id=" + filter.Id;
            } else {
                url += "&id=0";
            }
            $http.post(url + '&pagesize=' + pageSize + '&pageindex=' + pageIndex).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.result.prizeInfos, pageIndex, pageSize, largeLoad.result.totalNum);
            });
        }, 100);
    }

    $scope.deletePrizeInfo = function () {
        var id = $scope.selectdataitem.Id;
        setTimeout(function () {
            var url = "/BMGameManage/deletePrizesInfo?";
            if (id && id != undefined) {
                url += "&Id=" + id;
            } else {
                return false;
            }
            $http.post(url).success(function (loaddata) {
                if (loaddata.issuccess) {
                    alert("删除成功！");
                    $scope.getPrizeInfoAsync(10, 1, null);
                    $scope.pagingOptions.currentPage = 1;
                } else {
                    alert("删除失败！");
                    return false;
                }
                $("#deletePrizesInfoModal").modal("hide");
            });
        }, 100);
    }

    //添加奖品信息
    $scope.addPrizesInfo = function () {
        var activityId = $scope.prizesinfoData.ActivityId;
        var title = $scope.prizesinfoData.Title;
        var prizelevel = $scope.prizesinfoData.PrizeLevel;
        var price = $scope.prizesinfoData.Price;
        var total = $scope.prizesinfoData.TotalNum;
        var rate = $scope.prizesinfoData.Rate;
        //var img = $scope.prizesinfoData.Img;
        var cyclesflag = $scope.prizesinfoData.CyclesFlag;
        var cyclesnum = $scope.prizesinfoData.CyclesNum;
        var prizeflag = $scope.prizesinfoData.PrizeFlag;
        var url = "/BMGameManage/addPrizesInfo?";
        if (activityId == "" || title == "" || prizelevel == "" || total == "") {
            alert("数据格式错误"); return false;
        }
        url += "&ActivityId=" + activityId;
        url += "&Title=" + title;
        url += "&PrizeLevel=" + prizelevel;
        url += "&Price=" + price;
        url += "&TotalNum=" + total;
        url += "&Rate=" + rate;
        //url += "&Img=" + img;
        url += "&CyclesFlag=" + cyclesflag;
        url += "&CyclesNum=" + cyclesnum;
        url += "&PrizeFlag=" + prizeflag;
        setTimeout(function () {
            $http.post(url).success(function (loaddata) {
                if (loaddata.issuccess) {
                    $scope.getPrizeInfoAsync(10, 1, null);
                    $("#addPrizesInfoModal").modal("hide");
                    $scope.pagingOptions.currentPage = 1;
                }
                else {
                    alert("添加失败，请重试！");
                    return false;
                }
            });
        }, 100);
    }

    //修改奖品信息
    $scope.editPrizesInfo = function () {
        var id = $scope.selectdataitem.Id;
        var activityId = $scope.selectdataitem.ActivityId;
        var title = $scope.selectdataitem.Title;
        var prizelevel = $scope.selectdataitem.PrizeLevel;
        var price = $scope.selectdataitem.Price;
        var total = $scope.selectdataitem.TotalNum;
        var rate = $scope.selectdataitem.Rate;
        //var img = $scope.selectdataitem.Img;
        var cyclesflag = $scope.selectdataitem.CyclesFlag;
        var cyclesnum = $scope.selectdataitem.CyclesNum;
        var prizeflag = $scope.selectdataitem.PrizeFlag;
        var url = "/BMGameManage/editPrizesInfo?";
        if (title == "" || prizelevel == "" || total == "") {
            alert("数据格式错误"); return false;
        }
        url += "&Id=" + id;
        url += "&ActivityId=" + activityId;
        url += "&Title=" + title;
        url += "&PrizeLevel=" + prizelevel;
        url += "&Price=" + price;
        url += "&TotalNum=" + total;
        url += "&Rate=" + rate;
        //url += "&Img=" + img;
        url += "&CyclesFlag=" + cyclesflag;
        url += "&CyclesNum=" + cyclesnum;
        url += "&PrizeFlag=" + prizeflag;
        setTimeout(function () {
            $http.post(url).success(function (loaddata) {
                if (loaddata.issuccess) {
                    $scope.getPrizeInfoAsync(10, 1, null);
                    $("#editPrizesInfoModal").modal("hide");
                    $scope.pagingOptions.currentPage = 1;
                }
                else {
                    alert("修改失败，请重试！");
                    return false;
                }
            });
        }, 100);
    }

    $scope.getPrizeInfoAsync(10, 1, null);

    $scope.sendModel = function (row) {
        $scope.selectdataitem.Id = row.entity.Id;
        $scope.selectdataitem.ActivityId = row.entity.ActivityId;
        $scope.selectdataitem.Title = row.entity.Title;
        $scope.selectdataitem.Price = row.entity.TotalNum;
        $scope.selectdataitem.TotalNum = row.entity.TotalNum;
        $scope.selectdataitem.PrizeLevel = row.entity.PrizeLevel;

        $scope.selectdataitem.PrizeFlag = row.entity.PrizeFlag;
        //$scope.seledatatyitem.Img = row.entity.Img;
        $scope.selectdataitem.CyclesFlag = row.entity.CyclesFlag;
        $scope.selectdataitem.CyclesNum = row.entity.CyclesNum;
        $scope.selectdataitem.Rate = row.entity.Rate;
    }

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal && newVal.currentPage !== oldVal.currentPage) {
            $scope.getPrizeInfoAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    }, true);

    $scope.$watch('filterOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.getPrizeInfoAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    }, true);

    $scope.gridPrizeInfoOptions = {
        data: 'myPrizeInfoData',
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
            displayName: '编号',
            sortable: false,
            width: 80
        }, {
            field: 'ActivityId',
            displayName: '活动编号',
            sortable: true,
            width: 80
        },
        {
            field: 'PrizeLevel',
            displayName: '奖品等级',
            sortable: false,
            width: 80
        }
        , {
            field: 'Title',
            displayName: '奖品名称',
            sortable: false,
            width: 150
        }, {
            field: 'Price',
            displayName: '价格',
            sortable: false,
            width: 80
        }, {
            field: 'TotalNum',
            displayName: '总数量',
            sortable: false,
            width: 80
        }, {
            field: 'Rate',
            displayName: '中奖概率',
            sortable: false,
            width: 80
        }
        , {
            field: 'CyclesFlag',
            displayName: '周期',
            sortable: false,
            width: 0
        }, {
            field: 'CyclesNum',
            displayName: '每期中奖数量',
            sortable: false,
            width: 100
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 150,
            cellTemplate: '<div><button type="button" data-toggle="modal" data-target="#editPrizesInfoModal" class="btn btn-link" ng-click="sendModel(row)">修改</button><button type="button" data-toggle="modal" data-target="#deletePrizesInfoModal" ng-click="sendModel(row)" class="btn btn-link">删除</button></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

gamemanage.controller('ShareRecordCtrl', function ($scope, $http, $routeParams, $location, $upload) {

    $scope.formData = {};

    $scope.filterOptions = {
        useExternalFilter: true
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

    $scope.search = function () {
        var id = $scope.formData.ActivityId;
        if (id && id != undefined) {
            $scope.filterOptions.Id = id;
        } else {
            $scope.filterOptions.Id = 0;
        }
        setTimeout(function () {
            $scope.getShareRecordAsync(10, 1, $scope.filterOptions);
        }, 100);
    }

    $scope.pagingOptions = {
        pageSizes: [10, 5, 2],
        pageSize: 10,
        phoneNumber: null,
        state: 1,
        iscallcenter: "",
        start: null,
        end: null,
        currentPage: 1
    };

    //获取奖品
    $scope.getShareRecordAsync = function (pageSize, pageIndex, filter) {
        setTimeout(function () {
            var data;
            var url = "/BMGameManage/SelectShareRecordList?";
            if (filter && filter.Id != undefined) {
                url += "&id=" + filter.Id;
            } else {
                url += "&id=0";
            }
            $http.post(url + '&pagesize=' + pageSize + '&pageindex=' + pageIndex).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.result.shareRecords, pageIndex, pageSize, largeLoad.result.totalnum);
            });
        }, 100);
    }

    $scope.getShareRecordAsync(10, 1, null);

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal && newVal.currentPage !== oldVal.currentPage) {
            $scope.getShareRecordAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    }, true);

    $scope.$watch('filterOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.getShareRecordAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
        }
    }, true);

    $scope.gridDataOptions = {
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
            displayName: '编号',
            sortable: false,
            width: 100
        }, {
            field: 'UserId',
            displayName: '用户编号',
            sortable: false,
            width: 120
        }, {
            field: 'ActivityId',
            displayName: '活动编号',
            sortable: false,
            width: 120
        }, {
            field: 'Source',
            displayName: '分享来源',
            sortable: true,
            width: 180
        },
        {
            field: 'ShareType',
            displayName: '分享到',
            sortable: false,
            width: 180
        }
        , {
            field: 'CreateTime',
            displayName: '分享时间',
            sortable: false,
            width: 180
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});