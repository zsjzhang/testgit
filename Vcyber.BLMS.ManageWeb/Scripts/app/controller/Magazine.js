var magazine = angular.module('magazine', ['ngGrid', 'angularFileUpload']);
magazine.controller('MagazinelCtrl', function ($scope, $http, $routeParams, $location) {
    $scope.editData = {};
    $scope.createData = {};
    $scope.create = function () {
        $location.path("/Magazine/CreateMagazine");
    }
    $scope.delete = function (id) {

        if (window.confirm("是否确定删除?")) {
            $http.post("/Magazine/Delete",
                    { id: id })
                    .success(function (data) {
                        if (data.success) {
                            alert("删除成功!");
                        }
                        else
                            alert(data.msg);
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    });
        }

    };

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
        $scope.filterOptions.year = $scope.year;
        $scope.filterOptions.month = $scope.month;
        $scope.filterOptions.title = $scope.title;
        $scope.pagingOptions.currentPage = 1;
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    };

    $scope.setPagingData = function (data, page, pageSize, total_count) {
        $scope.myData = data;
        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };
    //get list
    $scope.getPagedDataAsync = function (pageSize, page, filter) {
        var url = "/Magazine/GetMagazinelList?";
        if (filter && filter.year != undefined) {
            url += "&year=" + filter.year;
        }
        if (filter && filter.month != undefined) {
            url += "&month=" + filter.month;
        }
        if (filter && filter.title != undefined) {
            url += "&name=" + filter.title;
        }
        $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function (largeLoad) {
            $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);

        });
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

    $scope.updateDisplay = function (id, status) {
        if (window.confirm("是否确定更改显示状态?")) {
            $http.post("/Magazine/UpdateIsDisplay",
                    { id: id, status: status })
                    .success(function (data) {
                        if (data.success) {
                            alert("修改成功!");
                        }
                        else
                            alert(data.msg);
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                    });
        }
    }
    $scope.loadApproveList = function (row) {
        setTimeout(function () {
            var data;
            var url = "/Magazine/GetApproveList?";
            $http.get(url + '&id=' + row.entity.Id).success(function (largeLoad) {
                $scope.approveData = largeLoad;
            });

        }, 100);
    }

    $scope.approveGridOptions = {
        i18n: 'zh-cn',
        data: 'approveData',
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
            field: 'UpdateTime',
            displayName: '更新时间',
            width: 200,
            sortable: false

        }, {
            field: 'ApproveStatusDescribe',
            displayName: '审批状态',
            enableCellEdit: false,
            sortable: false,
            width: 120
        }, {
            field: 'ApprovalMemo ',
            displayName: '原因',
            sortable: false,
            width: 120
        }, {
            field: 'OperatorId',
            displayName: '操作人ID',
            sortable: false,
            width: 120
        }, {
            field: 'OperatorName',
            displayName: '操作人名称',
            sortable: false,
            width: 120
        }],
        enablePaging: false,
        showFooter: true
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
            field: 'ImageUrl',
            visible: false
        }, {
            field: 'action',
            displayName: '图片缩略图',
            width: 200,
            height: 150,
            cellTemplate: '<div><img ng-src="{{row.getProperty(\'ImageUrl\')}}" style="width:100px;height:50px" ></div>'

        }, {
            field: 'Title',
            displayName: '标题',
            sortable: true,
            width: 100
        }, {
            field: 'Date',
            displayName: '报刊时间',
            sortable: true,
            width: 100
        }, {
            field: 'IsDisplay',
            displayName: '是否显示',
            enableCellEdit: false,
            sortable: true,
            width: 80,
            cellTemplate: '<div ng-show="row.entity.IsDisplay">显示</div><div ng-show="!row.entity.IsDisplay">不显示</div>'
        }, {
            field: 'Date',
            displayName: '报刊时间',
            sortable: true,
            width: 100
        }, {
            field: 'Summary',
            displayName: '简介',
            sortable: false,
            width: 200
        }, {
            field: 'UpdateTime',
            displayName: '修改日期',
            sortable: true,
            width: 100
        }, {
            field: 'ApproveStatusDescribe',
            displayName: '状态',
            enableCellEdit: false,
            sortable: true,
            width: 120
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            cellTemplate: '<div><a href="#/Magazine/EditMagazine/{{row.getProperty(\'Id\')}}">编辑</a>  ' +
                '<a href="javascript:return void()" ng-click="delete(row.getProperty(\'Id\'))">删除</a></div>'
        }, {
            field: 'action',
            displayName: '显示',
            enableCellEdit: false,
            sortable: false,
            width: 100,
            cellTemplate: '<div ng-show="row.entity.IsDisplay"><a href="javascript:return void()" ng-click="updateDisplay(row.getProperty(\'Id\'),0)">隐藏</a>'
                + '</div><div ng-show="!row.entity.IsDisplay"><a href="javascript:return void()" ng-click="updateDisplay(row.getProperty(\'Id\'),1)">显示</a></div>'
        }, {
            field: 'action',
            displayName: '审批记录',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            cellTemplate: '<div><a href="" data-toggle="modal" data-target="#magazineApproveListModal" ng-click="loadApproveList(row)">审批记录</a></div>'
        }, {
            field: 'action',
            displayName: '操作记录',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            cellTemplate: '<div><a href="#/Magazine/OperationLog/{{row.getProperty(\'Id\')}}">操作记录</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

//纸质杂志申请查询controller
magazine.controller('MagazineApplyManageCtrl', function ($scope, $http, $routeParams, $location) {

    $scope.filterOptions = {
        useExternalFilter: true
    };
    $scope.totalServerItems = 0;

    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };

    $scope.carDefaultType = "请选择";
    $http.get("/Magazine/CarTypeView").success(function (resultData) {
        $scope.carTypes = $(resultData).toArray();
    });

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

    //查询纸质杂志申请记录
    $scope.searchApply = function () {
        $scope.filterOptions.datetime = $scope.datetime;
        $scope.filterOptions.title = $scope.title;
        $scope.filterOptions.phoneNum = $scope.phoneNum;
        $scope.pagingOptions.currentPage = 1;
        $scope.getPagedDataAsyncApply($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    };

    $scope.exportApply = function () {
        var url = "/Magazine/ExportMagazineApplyData?";
        if ($scope.datetime != undefined) {
            url += "datetime=" + $scope.datetime;
        } else {
            url += "datetime=";
        }
        if ($scope.title != undefined) {
            url += "&title=" + $scope.title;
        } else {
            url += "&title=";
        }
        $http.get(url).success(function (args) {
            alert("导出成功！");
        });
    };

    //查询纸质杂志申请记录
    $scope.getPagedDataAsyncApply = function (pageSize, page, filter) {
        var url = "/Magazine/GetMagazinelApplyList?";
        if (filter && filter.datetime != undefined) {
            url += "&datetime=" + filter.datetime;
        }
        if (filter && filter.title != undefined) {
            url += "&title=" + filter.title;
        }
        if (filter && filter.phoneNum != undefined) {
            url += "&phoneNum=" + filter.phoneNum;
        }
        $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function (largeLoad) {
            $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
        });
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
            field: 'ReceiveName',
            displayName: '姓名',
            sortable: true,
            width: 100
        }, {
            field: 'Phone',
            displayName: '手机号',
            sortable: true,
            width: 140
        }, {
            field: 'Detail',
            displayName: '车型',
            sortable: true,
            width: 300
        }
        , {
            field: 'CreateTime',
            displayName: '申请时间',
            sortable: true,
            width: 371,
            cellFilter: 'date: "mediumDate"'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };


});


magazine.controller('CreateMagazineCtrl', function ($scope, $http, $location, $upload) {

    $scope.createData = {};

    $scope.$watch('files', function () {
        $scope.upload($scope.files);
    });

    $scope.upload = function (files, action) {
        if (files && files.length) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                $upload.upload({
                    url: '/Scripts/ueditor/net/controller.ashx?action=' + action,
                    file: file,
                    fileFormDataName: 'upfile'
                }).progress(function (evt) {
                    var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                    console.log('progress: ' + progressPercentage + '% ' +
                                evt.config.file.name);
                }).success(function (data, status, headers, config) {
                    //$scope.createData.Path = data.FilePath;
                    if (data.state != "SUCCESS") {
                        alert(data.state);
                    } else {
                        if (action == "uploadimage") {
                            $scope.createData.ImageUrl = data.url;
                            $scope.createData.image = data.url;
                        }
                        if (action == "uploadSource") {
                            $scope.createData.LinkUrl = data.url;
                        }
                    }
                    //console.log('file ' + config.file.name + 'uploaded. Response: ' +
                    //            JSON.stringify(data));
                }).error(function (err) {
                    alert(err);
                    alert('上传失败');
                });
            }
        }
    };


    $scope.createMagazine = function () {
        $scope.createData.Summary = encodeURI(getContent());
        $http.post("/Magazine/CreateMagazine",
        { Title: $scope.createData.Title, ImageUrl: $scope.createData.ImageUrl, LinkUrl: $scope.createData.LinkUrl, Year: $scope.createData.Year, Month: $scope.createData.Month, Summary: $scope.createData.Summary, IsDisplay: $scope.createData.IsDisplay, QuestionUrl: $scope.createData.QuestionUrl, ResultUrl: $scope.createData.ResultUrl, ReadLink: $scope.createData.ReadLink })
        .success(function (data) {
            if (data.success) {
                $location.path("/Magazine/Index");
                $('#createMagazineModal').modal('hide');
            }
            else

                alert(data.msg);
        });
    }
});

magazine.controller('EditMagazineCtrl', function ($scope, $http, $location, $routeParams, $upload) {

    $scope.editData = {};
    $scope.$watch('files', function () {
        $scope.upload($scope.files);
    });

    $scope.upload = function (files, action) {
        if (files && files.length) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                $upload.upload({
                    url: '/Scripts/ueditor/net/controller.ashx?action=' + action,//uploadimage
                    file: file,
                    fileFormDataName: 'upfile'
                }).progress(function (evt) {
                    var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                    console.log('progress: ' + progressPercentage + '% ' +
                                evt.config.file.name);
                }).success(function (data, status, headers, config) {
                    if (data.state != "SUCCESS") {
                        alert(data.state);
                    } else {
                        if (action == "uploadimage") {
                            $scope.editData.ImageUrl = data.url;
                            $scope.editData.image = data.url;
                        }
                        if (action == "uploadSource") {
                            $scope.editData.LinkUrl = data.url;
                        }
                    }
                    //console.log('file ' + config.file.name + 'uploaded. Response: ' +
                    //            JSON.stringify(data));
                }).error(function (err) {
                    alert('上传出错');
                });
            }
        }
    };

    $http.get("/Magazine/MagazineDetail/" + $routeParams.Id).success(function (data) {
        $scope.editData.Id = $routeParams.Id;
        $scope.editData.Title = data.Title;
        $scope.editData.ImageUrl = data.ImageUrl;
        $scope.editData.image = data.ImageUrl;
        $scope.editData.LinkUrl = data.LinkUrl;
        $scope.editData.Summary = decodeURI(data.Summary); 
        $scope.editData.Year = data.Year;
        $scope.editData.Month = data.Month;
        $scope.editData.QuestionUrl = data.QuestionUrl;
        $scope.editData.ResultUrl = data.ResultUrl;
        $scope.editData.ReadLink = data.ReadLink;
        setContent(decodeURI(data.Summary));
    }
   );
    $scope.edit = function () {
        $scope.editData.Summary = encodeURI(getContent());
        $http.post("/Magazine/EditMagazine",
            {
                Id: $scope.editData.Id,
                Title: $scope.editData.Title,
                ImageUrl: $scope.editData.ImageUrl,
                LinkUrl: $scope.editData.LinkUrl,
                Year: $scope.editData.Year,
                Month: $scope.editData.Month,
                Summary: $scope.editData.Summary,
                QuestionUrl: $scope.editData.QuestionUrl,
                ResultUrl: $scope.editData.ResultUrl,
                ReadLink:$scope.editData.ReadLink
            })
            .success(function (data) {
                if (data.success) {
                    $location.path("/Magazine/Index");
                }
                else
                    alert(data.msg);
            });
    }


});

magazine.controller('MagazineOperationLogCtrl', function ($scope, $http, $routeParams) {

    $scope.selectItem = {};

    $scope.filterOptions = {
        useExternalFilter: true
    };
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };


    $scope.setPagingData = function (data, page, pageSize, total_count) {
        $scope.logData = data;
        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };
    $scope.getPagedDataAsync = function (pageSize, page, filter) {
        setTimeout(function () {
            var url = "/Magazine/GetOperationLog?Id=" + $routeParams.Id;

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

    $scope.logOptions = {
        data: 'logData',
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
            field: 'TypeValue',
            displayName: '类型',
            width: 120,
            sortable: false
        }, {
            field: 'SourceId',
            displayName: '数据ID',
            enableCellEdit: false,
            sortable: true,
            width: 100
        }, {
            field: 'OriginalValue',
            displayName: '原始值',
            width: 100
        }, {
            field: 'CurrentValue',
            displayName: '当前值',
            width: 100
        }, {
            field: 'OperateTime',
            displayName: '操作时间',
            enableCellEdit: false,
            width: 80
        }, {
            field: 'OperaterName',
            displayName: '操作人',
            enableCellEdit: false,
            width: 130
        },
        {
            field: 'Remark',
            displayName: '备注',
            enableCellEdit: false,
            width: 280
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };


});