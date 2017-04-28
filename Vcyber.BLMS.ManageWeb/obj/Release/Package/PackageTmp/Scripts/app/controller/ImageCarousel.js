var imgCarousel = angular.module('imageCarousel', ['ngGrid', 'angularFileUpload']);
imgCarousel.controller('ImageCarouselCtrl', function ($scope, $http, $routeParams, $upload) {
    $scope.editData = {};
    $scope.createData = {};
    $scope.approveData = {};
    $scope.selectItem = {};
    $scope.ImageCarouselType = { select: -1 };
    $scope.ImageCarouselCreateType = { select: -1 };
    $scope.ImageCarouselEditType = { select: -1 };
    $http.get('/Common/GetImageCarouselTypeJsonResult').success(function (data) {
        $scope.ImageCarouselType.data = data;
        $scope.ImageCarouselCreateType.data = data;
        $scope.ImageCarouselEditType.data = data;
    });

    //upload
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

                    if (action == "uploadimage") {
                        $scope.createData.ImageUrl = data.url;
                        $scope.createData.image = data.url;
                    }


                    //console.log('file ' + config.file.name + 'uploaded. Response: ' +
                    //            JSON.stringify(data));
                }).error(function (err) {
                    alert('上传失败');
                });
            }
        }
    };

    $scope.uploadEdit = function (files, action) {
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

                    if (action == "uploadimage") {
                        $scope.editData.ImageUrl = data.url;
                        $scope.editData.image = data.url;
                    }


                    //console.log('file ' + config.file.name + 'uploaded. Response: ' +
                    //            JSON.stringify(data));
                }).error(function (err) {
                    alert('上传失败');
                });
            }
        }
    };
    ///prioirty
    $http.get('/ImageCarousel/GetDealerId').success(
       function (data) {
           if (data == null || data == "") {
               $scope.dealerDWShow = 1;
           }
       }
       );

    //$scope.up = function (id) {
    //    $http.post("/ImageCarousel/UpdatePriority",
    //         { id: id, changeValue: -1 })
    //         .success(function () {
    //             $scope.getPagedDataAsync();
    //         });
    //}

    //$scope.down = function (id) {
    //    $http.post("/ImageCarousel/UpdatePriority",
    //        { id: id, changeValue: 1 })
    //        .success(function () {
    //            $scope.getPagedDataAsync();
    //        });
    //}

    ///operation
    $scope.loadEditImageCarousel = function (row) {

        $http.get("/ImageCarousel/ImageCarouselDetail/", { params: { id: row.entity.Id } }).success(function (data) {
            $scope.editData.Id = row.entity.Id;
            $scope.editData.Title = data.Title;
            $scope.editData.ImageUrl = data.ImageUrl;
            $scope.editData.image = data.ImageUrl;
            $scope.editData.LinkUrl = data.LinkUrl;
            $scope.editData.NewPage = data.NewPage;
            $scope.editData.Priority = data.Priority;
            $scope.ImageCarouselEditType.select = data.Type;
        });
    };

    $scope.editImageCarousel = function () {
        if ($scope.ImageCarouselEditType.select < 0) {
            alert("请选择轮播图类型!");
            return;
        }
        $http.post("/ImageCarousel/EditImageCarousel",
            { Id: $scope.editData.Id, Title: $scope.editData.Title, ImageUrl: $scope.editData.ImageUrl, LinkUrl: $scope.editData.LinkUrl, NewPage: $scope.editData.NewPage, Type: $scope.ImageCarouselEditType.select, Priority: $scope.editData.Priority })
            .success(function () {
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                $('#editImageCarouselModal').modal('hide');
            });
    };

    $scope.createImageCarousel = function () {
        if ($scope.ImageCarouselCreateType.select < 0) {
            alert("请选择轮播图类型!");
            return;
        }
        $http.post("/ImageCarousel/CreateImageCarousel",
            { Title: $scope.createData.Title, ImageUrl: $scope.createData.ImageUrl, LinkUrl: $scope.createData.LinkUrl, Type: $scope.ImageCarouselCreateType.select, Priority: $scope.createData.Priority })
            .success(function () {
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                $('#createImageCarouselModal').modal('hide');
            });
    };


    $scope.delete = function (id) {

        if (window.confirm("是否确定删除?")) {
            $http.post("/ImageCarousel/Delete",
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

    }
    //修改权重
    $scope.priorityPage = function (row) {
        $scope.selectItem.Id = row.entity.Id;
        $scope.selectItem.Priority = row.entity.Priority;
    }
    $scope.UpdatePriority = function () {
        $http.post("/ImageCarousel/UpdatePriority",
                   {
                       Id: $scope.selectItem.Id,
                       changeValue: $scope.selectItem.Priority
                   })
                   .success(function (data) {
                       if (data.success) {
                           alert("修改成功!");
                           $('#priorityModal').modal('hide');

                       }
                       else
                           alert(data.msg);
                       $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                   });
    }

    //get the approve list
    $scope.approvePage = function (row) {
        $scope.selectItem.Id = row.entity.Id;
        $scope.selectItem.IsApproved = row.entity.IsApproved;
    }

    $scope.UpdateApproval = function () {
        $http.post("/ImageCarousel/Approval",
                   {
                       id: $scope.selectItem.Id,
                       status: $scope.selectItem.IsApproved,
                       memo: $scope.selectItem.ApprovalMemo

                   })
                   .success(function (data) {
                       if (data.success) {
                           alert("修改成功!");
                           $('#imageCarouselApproveModal').modal('hide');

                       }
                       else
                           alert(data.msg);
                       $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                   });
    }

    $scope.loadApproveList = function (row) {
        setTimeout(function () {
            var data;
            var url = "/ImageCarousel/GetApproveList?";
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
            displayName: '状态',
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

    //operation log
    $scope.loadOperation = function (row) {
        $routeParams.selectId = row.entity.Id;
        $location.path("/ImageCarousel/GetOperationLog");
    }

    //get list

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
        $scope.filterOptions.type = $scope.ImageCarouselType.select;
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

        var url = "/ImageCarousel/GetImageCarouselList?type=" + filter.type;
        $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function(largeLoad) {
            $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
        });

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
            cellTemplate: '<div><img style="width:180px;height:50px" ng-src="{{row.getProperty(\'ImageUrl\')}}" ></div>'

        }, {
            field: 'Title',
            displayName: '标题',
            sortable: false,
            width: 200
        },
            {
                field: 'Priority',
                displayName: '权重',
                sortable: false,
                width: 100
            }, {
                field: 'TypeValue',
                displayName: '类型',
                sortable: false,
                width: 100
            },
        {
            field: 'UpdateTime',
            displayName: '修改日期',
            width: 80, sortable: false
        }, {
            field: 'ApproveStatusDescribe',
            displayName: '审批状态',
            width: 120,
            sortable: false
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            cellTemplate: '<div><a href="" data-toggle="modal" data-target="#editImageCarouselModal" ng-click="loadEditImageCarousel(row)">编辑</a> ' +
                '<a href="javascript:return void()" ng-click="delete(row.getProperty(\'Id\'))">删除</a>  </div> '
            //'<a  href="" data-toggle="modal" data-target="#imageCarouselApproveModal" ng-click="approvePage(row)">审批</a> </div>'

        },
        {
            field: 'action',
            displayName: '权重',
            enableCellEdit: false,
            sortable: false,
            width: 100,
            cellTemplate: '<div ng-show="dealerDWShow"><a href="" data-toggle="modal" data-target="#priorityModal" ng-click="priorityPage(row)">编辑权重</a></div>'
        }, {
            field: 'action',
            displayName: '审批记录',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            cellTemplate: '<div><a href="" data-toggle="modal" data-target="#imageCarouselApproveListModal" ng-click="loadApproveList(row)">审批记录</a></div>'
        }, {
            field: 'action',
            displayName: '操作记录',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            cellTemplate: '<div><a href="#/ImageCarousel/OperationLog/{{row.getProperty(\'Id\')}}">操作记录</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});
imgCarousel.controller('ImageCarouselOperationLogCtrl', function ($scope, $http, $routeParams, $location, $upload) {

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
            var url = "/ImageCarousel/GetOperationLog?Id=" + $routeParams.Id;

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
