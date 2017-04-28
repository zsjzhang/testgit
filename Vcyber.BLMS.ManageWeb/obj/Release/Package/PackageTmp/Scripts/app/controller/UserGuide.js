var userguide = angular.module('userguide', ['ngGrid', 'angularFileUpload']);
userguide.controller('UserGuideCtrl', function ($scope, $http, $routeParams, $location) {
    $scope.editData = {};
    $scope.createData = {};
    $scope.createUserGuide = function () {
        $location.path("/UserGuide/CreateUserGuide");
    }
    $scope.delete = function (id) {

        if (window.confirm("是否确定删除?")) {
            $http.post("/UserGuide/Delete",
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
        var url = "/UserGuide/GetUserGuideList?";

            if (filter && filter.title != undefined) {
                url += "&title=" + filter.title;
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
            $http.post("/UserGuide/UpdateIsDisplay",
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
            var url = "/UserGuide/GetApproveList?";
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
            field: 'IsDisplay',
            displayName: '是否显示',
            enableCellEdit: false,
            sortable: true,
            width: 80,
            cellTemplate: '<div ng-show="row.entity.IsDisplay">显示</div><div ng-show="!row.entity.IsDisplay">不显示</div>'
        }, {
            field: 'DownloadTimes',
            displayName: '下载次数',
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
            cellTemplate: '<div><a href="#/UserGuide/EditUserGuide/{{row.getProperty(\'Id\')}}">编辑</a>  ' +
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
            cellTemplate: '<div><a href="" data-toggle="modal" data-target="#userGuideApproveListModal" ng-click="loadApproveList(row)">审批记录</a></div>'
        }, {
            field: 'action',
            displayName: '操作记录',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            cellTemplate: '<div><a href="#/UserGuide/OperationLog/{{row.getProperty(\'Id\')}}">操作记录</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});
userguide.controller('CreateUserGuideCtrl', function ($scope, $http, $location, $upload) {

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
                    alert('上传失败');
                });
            }
        }
    };


    $scope.createUserGuide = function () {
        $http.post("/UserGuide/CreateUserGuide",
        { Title: $scope.createData.Title, ImageUrl: $scope.createData.ImageUrl, LinkUrl: $scope.createData.LinkUrl, Summary: $scope.createData.Summary, IsDisplay: $scope.createData.IsDisplay })
        .success(function (data) {
            if (data.success) {
                $location.path("/UserGuide/Index");
                $('#createUserGuideModal').modal('hide');
            }
            else

                alert(data.msg);
        });
    }
});

userguide.controller('EditUserGuideCtrl', function ($scope, $http, $location, $routeParams, $upload) {

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

    $http.get("/UserGuide/UserGuideDetail/" + $routeParams.Id).success(function (data) {
        $scope.editData.Id = $routeParams.Id;
        $scope.editData.Title = data.Title;
        $scope.editData.ImageUrl = data.ImageUrl;
        $scope.editData.image = data.ImageUrl;
        $scope.editData.LinkUrl = data.LinkUrl;
        $scope.editData.Summary = data.Summary;
        $scope.editData.IsDisplay = data.IsDisplay;
    }
   );
    $scope.editUserGuide = function () {

        $http.post("/UserGuide/EditUserGuide",
            {
                Id: $scope.editData.Id,
                Title: $scope.editData.Title,
                ImageUrl: $scope.editData.ImageUrl,
                LinkUrl: $scope.editData.LinkUrl,
                Summary: $scope.editData.Summary,
                IsDisplay: $scope.editData.IsDisplay
            })
            .success(function (data) {
                if (data.success) {
                    $location.path("/UserGuide/Index");
                }
                else
                    alert(data.msg);
            });
    }


});

userguide.controller('UserGuideOperationLogCtrl', function ($scope, $http, $routeParams) {

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
            var url = "/UserGuide/GetOperationLog?Id=" + $routeParams.Id;

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