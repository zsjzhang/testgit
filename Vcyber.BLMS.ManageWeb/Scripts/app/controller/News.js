var news = angular.module('news', ['ngGrid', 'angularFileUpload']);
news.controller('NewsCtrl', function ($scope, $http, $routeParams, $location) {
    $scope.title = "";

    $scope.approveData = {};
    $scope.selectItem = {};


    $scope.provinceDw = { select: -1 };
    $scope.cityDw = { select: -1 };
    $scope.dealerDw = { select: -1 };//4s店
    $scope.dealerDWShow = 0;

    $http.post('/News/GetDealerId').success(
        function (data) {
            if (data == null || data == "") {
                $scope.dealerDWShow = 1;
               // $scope.dealerDw.select = data;
            }
        }
        );

    $http.post('/Common/GetDealerProvinceListJsonResult').success(function (data) {
        $scope.provinceDw.data = data;
        $scope.provinceChange();
        $scope.cityChange();
    });

    $scope.provinceChange = function () {
        $http.post('/Common/GetDealerCityListJsonResult', {  province: $scope.provinceDw.select}).success(function (data) {
            $scope.cityDw.select = -1;
            $scope.cityDw.data = data;
            $scope.cityChange();
        });
    };

    $scope.cityChange = function () {
        $http.post('/Common/GetDealerListJsonResult', {province: $scope.provinceDw.select, city: $scope.cityDw.select  }).success(function (data) {
            $scope.dealerDw.select = -1;
            $scope.dealerDw.data = data;
        });
    };

    $scope.create = function () {
        $location.path("/News/CreateNews");
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
        $scope.filterOptions.title = $scope.title;
        $scope.filterOptions.dealer = $scope.dealerDw.select;
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
        console.log($("#gstart").val());
         var url = "/News/GetNewsList?gstart="+$("#gstart").val()+"&gend="+$("#gend").val();
            if (filter && filter.title != undefined) {
                url += "&title=" + filter.title;
            }
            if (filter && filter.dealer != undefined) {
                url += "&dealer=" + filter.dealer;
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

    $scope.loadEditNews = function (row) {
        $routeParams.selectId = row.entity.Id;
        $location.path("/News/EditNews");
    }

    $scope.delete = function (id) {
        if (window.confirm("是否确定删除?")) {
            $http.post("/News/Delete",
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

    $scope.updateDisplay = function (id, status) {
        if (window.confirm("是否确定更改显示状态?")) {
            $http.post("/News/UpdateIsDisplay",
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

    $scope.priorityPage= function(row) {
        $scope.selectItem.Id = row.entity.Id;
        $scope.selectItem.Priority = row.entity.Priority;
        $scope.selectItem.IsDisplay = row.entity.IsDisplay;
        $scope.selectItem.IsHot = row.entity.IsHot;
    }
    $scope.UpdatePriority = function () {
        $http.post("/News/UpdateAllDisplay",
                   {
                       id: $scope.selectItem.Id,
                       priority: $scope.selectItem.Priority,
                       display: $scope.selectItem.IsDisplay,
                       isHot: $scope.selectItem.IsHot
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

    $scope.loadApproveList = function (row) {
        setTimeout(function () {
            var url = "/News/GetApproveList?";
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
            width: 100,
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
            width: 100
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
            displayName: '编号',
            width: 80,
            sortable: true
        }, {
            field: 'Title',
            displayName: '标题',
            width: 200,
            sortable: false
        }, {
            field: 'action',
            displayName: '缩略图',
            width: 200,
            height: 150,
            cellTemplate: '<div><img ng-src="{{row.getProperty(\'MajorImageUrl\')}}"  style="width:100px;height:50px" ></div>'

        }, {
            field: 'IsDisplay',
            displayName: '是否显示',
            enableCellEdit: false,
            sortable: true,
            width: 80,
            cellTemplate: '<div ng-show="row.entity.IsDisplay">显示</div><div ng-show="!row.entity.IsDisplay">不显示</div>'
        }, //{
        //    field: 'IsHot',
        //    displayName: '是否热点',
        //    enableCellEdit: false,
        //    sortable: true,
        //    width: 80,
        //    cellTemplate: '<div ng-show="row.entity.IsHot">是</div><div ng-show="!row.entity.IsHot">否</div>'
        //}, 
        {
            field: 'Priority',
            displayName: '权值',
            enableCellEdit: false,
            sortable: true,
            width: 80
        },
    //    {
    //        field: 'DealerName',
    //displayName: '经销商',
    //enableCellEdit: false,
    //sortable: true,
    //width: 80
    //    },
    {
        field: 'ApprovedTime',
        displayName: '创建时间',
        sortable: true,
        width: 120
    },
    
//{
//            field: 'CreateTime',
//            displayName: '创建时间',
//            sortable: true,
//            width: 120
//},
        //{
        //    field: 'CreateBy',
        //    displayName: '创建人',
        //    sortable: true,
        //    width: 120
        //},
        {
            field: 'IsApproved',
            visible: false
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
            cellTemplate: '<div><a href="#/News/EditNews/{{row.getProperty(\'Id\')}}">编辑</a>  ' +
                '<a href="javascript:return void()" ng-click="delete(row.getProperty(\'Id\'))">删除</a>  ' +
                //'<a href="" data-toggle="modal" data-target="#newsApproveModal" ng-click="approvePage(row)">审批</a>  ' +
                '<a href="https://www.bluemembers.com.cn/News/NewsDetail/{{row.getProperty(\'Id\')}}" ng-click="">预览</a></div>'
        },
        //{
        //    field: 'action',
        //    displayName: '显示',
        //    enableCellEdit: false,
        //    sortable: false,
        //    width: 100,
        //    cellTemplate: '<div ng-show="row.entity.IsDisplay"><a href="javascript:return void()" ng-click="updateDisplay(row.getProperty(\'Id\'),0)">隐藏</a>'
        //        + '</div><div ng-show="!row.entity.IsDisplay"><a href="javascript:return void()" ng-click="updateDisplay(row.getProperty(\'Id\'),1)">显示</a></div>'
        //},
        {
            field: 'action',
            displayName: '显示',
            enableCellEdit: false,
            sortable: false,
            width: 100,
            cellTemplate: '<div><a href="" data-toggle="modal" data-target="#priorityModal" ng-click="priorityPage(row)">编辑显示</a></div>'
        }, {
            field: 'action',
            displayName: '审批记录',
            enableCellEdit: false,
            sortable: false,
            width: 100,
            cellTemplate: '<div><a href="" data-toggle="modal" data-target="#newsApproveListModal" ng-click="loadApproveList(row)">审批记录</a></div>'
        }, {
            field: 'action',
            displayName: '操作记录',
            enableCellEdit: false,
            sortable: false,
            width: 100,
            cellTemplate: '<div><a href="#/News/OperationLog/{{row.getProperty(\'Id\')}}">操作记录</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});


//新闻创建界面
news.controller('CreateNewsCtrl', function ($scope, $http, $upload, $location) {

    $scope.createData = {};
    $scope.createData.IsDisplay = 0;

    $scope.$watch('files', function () {
        $scope.upload($scope.files);
    });

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
                    console.log('progress: ' + progressPercentage + '% ' +
                                evt.config.file.name);
                }).success(function (data, status, headers, config) {
                    //$scope.createData.Path = data.FilePath;
                    $scope.createData.MajorImageUrl = data.url;
                    $scope.createData.image = data.url;
                    //console.log('file ' + config.file.name + 'uploaded. Response: ' +
                    //            JSON.stringify(data));
                }).error(function (err) {
                    alert('上传失败');
                });
            }
        }
    };

    $scope.create = function () {
        $scope.createData.Content = encodeURI(getContent());
        $http.post("/News/CreateNews",
            {
                Title: $scope.createData.Title,
                MajorImageUrl: $scope.createData.MajorImageUrl,
                Content: $scope.createData.Content,
               IsDisplay: $scope.createData.IsDisplay,
                Summary: $scope.createData.Summary,
                Priority: $scope.createData.Priority
            }).success(function (data) {
                if (data.success) {
                    $location.path("/News/Index");
                }
                else
                    alert(data.msg);
            });
    }
});

//新闻编辑界面
news.controller('EditNewsCtrl', function ($scope, $http, $location, $routeParams, $upload) {

    $scope.editData = {};
    $scope.$watch('files', function () {
        $scope.upload($scope.files);
    });

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
                    console.log('progress: ' + progressPercentage + '% ' +
                                evt.config.file.name);
                }).success(function (data, status, headers, config) {

                    $scope.editData.MajorImageUrl = data.url;
                    $scope.editData.image = data.url;
                    //console.log('file ' + config.file.name + 'uploaded. Response: ' +
                    //            JSON.stringify(data));
                }).error(function (err) {
                    alert('上传照片出错');
                });
            }
        }
    };
    $http.get("/News/LoadNews/" + $routeParams.Id).success(function (data) {
        $scope.editData.Title = data.Title;
        $scope.editData.MajorImageUrl = data.MajorImageUrl;
        $scope.editData.image = data.MajorImageUrl;
        $scope.editData.IsHot = data.IsHot;
        $scope.editData.Content = decodeURI(data.Content);
        $scope.editData.Priority = data.Priority;
        $scope.editData.IsDisplay = data.IsDisplay;
        $scope.editData.Summary = data.Summary;
        setContent(decodeURI(data.Content));
    }
   );
    $scope.edit = function () {

        if (window.confirm("编辑会进入审核状态，如果是首页新闻，会取消首页的显示，是否确定编辑?")) {
            var test = $scope.editData.path;
            $http.post("/News/EditNews",
                {
                    Id: $routeParams.Id,
                    Title: $scope.editData.Title,
                    MajorImageUrl: $scope.editData.MajorImageUrl,
                    Content: encodeURI(getContent()),
                    IsDisplay: $scope.editData.IsDisplay,
                    Summary: $scope.editData.Summary
                }).success(function (data) {
                    if (data.success) {
                        $location.path("/News/Index");
                    }
                    else
                        alert(data.msg);
                });
        }
    };

});

//新闻操作记录界面
news.controller('NewsOperationLogCtrl', function ($scope, $http, $routeParams) {

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
            var url = "/News/GetOperationLog?Id=" + $routeParams.Id;

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
