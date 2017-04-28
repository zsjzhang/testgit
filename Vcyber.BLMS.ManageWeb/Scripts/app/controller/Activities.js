var activities = angular.module('activities', ['ngGrid', 'angularFileUpload']);
activities.controller('ActivitiesCtrl', function ($scope, $http, $routeParams, $location) {
    $scope.title = "";
    $scope.approveData = {};
    $scope.selectItem = {};
    $scope.signUpData = {};


    $scope.provinceDw = { select: -1 };
    $scope.cityDw = { select: -1 };
    $scope.dealerDw = { select: -1 };//4s店
    $scope.dealerDWShow = 0;

    $http.post('/Activities/GetDealerId').success(
        function (data) {
            if (data == null || data == "") {
                $scope.dealerDWShow = 1;

                //$scope.dealerDw.select = data;
            }
        }
        );
    
    $http.post('/Common/GetDealerProvinceListJsonResult').success(function (data) {
        $scope.provinceDw.data = data;
        $scope.provinceChange();
        $scope.cityChange();
    });

    $scope.provinceChange = function () {
        $http.post('/Common/GetDealerCityListJsonResult', {  province: $scope.provinceDw.select }).success(function (data) {
            $scope.cityDw.select = -1;
            $scope.cityDw.data = data;
            $scope.cityChange();
        });
    };

    $scope.cityChange = function () {
        $http.post('/Common/GetDealerListJsonResult', {  province: $scope.provinceDw.select, city: $scope.cityDw.select  }).success(function (data) {
            $scope.dealerDw.select = -1;
            $scope.dealerDw.data = data;
        });
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

    //活动状态下拉表/// 0:未开始，1：已开始，未结束，2：已结束
    $scope.statuslist =
       [{
           id: 1,
           Name: '未开始'
       }, {
           id: 2,
           Name: '进行中'
       }, {
           id: 3,
           Name: '已结束'
       }];
    $scope.search = function () {
        $scope.filterOptions.Status = $scope.selected;
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
        var data;
        var url = "/Activities/GetActivitiesList?status=" + filter.Status;
        if (filter && filter.dealer != undefined) {
            url += "&dealer=" + filter.dealer;
        }
        $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize).success(function (largeLoad) {
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
    //跳转到编辑视图
    $scope.loadEditNews = function (row) {
        $routeParams.selectId = row.entity.Id;
        $location.path("/Activities/EditActivities");
    }
    //跳转到创建视图
    $scope.create = function () {
        $location.path("/Activities/CreateActivities");
    }
    $scope.delete = function (id) {

        if (window.confirm("是否确定删除?")) {
            $http.post("/Activities/Delete",
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

    //加载审批记录
    $scope.loadApproveList = function (row) {
           var url = "/Activities/GetApproveList?";
            $http.get(url + '&id=' + row.entity.Id).success(function (largeLoad) {
                $scope.approveData = largeLoad;
            });

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

    //更新显示字段
    $scope.updateDisplay = function (id, status) {
        if (window.confirm("是否确定更改显示状态?")) {
            $http.post("/Activities/UpdateIsDisplay",
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
    $scope.priorityPage = function (row) {
        $scope.selectItem.Id = row.entity.Id;
        $scope.selectItem.Priority = row.entity.Priority;
        $scope.selectItem.IsDisplay = row.entity.IsDisplay;
        $scope.selectItem.IsHot = row.entity.IsHot;
    }
    $scope.UpdatePriority = function () {
        $http.post("/Activities/UpdateAllDisplay",
                   {
                       id: $scope.selectItem.Id,
                       priority: $scope.selectItem.Priority,
                       dispaly: $scope.selectItem.IsDisplay,
                       isHot:$scope.selectItem.IsHot
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
            width: 120,
            sortable: false
        }, {
            field: 'IsApproved',
            visible: false
        }, {
            field: 'StatusValue',
            displayName: '状态',
            enableCellEdit: false,
            width: 80
        }, {
            field: 'action',
            displayName: '缩略图',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            heigth: 80,
            cellTemplate: '<div><img ng-src="{{row.getProperty(\'MajorImageUrl\')}}" style="width:100px;height:50px" ></div>'

        },{
            field: 'DealerName',
            displayName: '经销商',
            enableCellEdit: false,
            width: 80
        },
        {
            field: 'BeginTime',
            displayName: '开始日期',
            enableCellEdit: false,
            sortable: true,
            width: 100
        }, {
            field: 'EndTime',
            displayName: '结束时间',
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
            field: 'IsHotStatus',
            displayName: '是否热点',
            enableCellEdit: false,
            sortable: true,
            width: 80
        }, {
            field: 'SignUpStatus',
            displayName: '支持在线报名',
            sortable: true,
            width: 100
        }, {
            field: 'IsUrlStatus',
            displayName: '支持url跳转',
            sortable: true,
            width: 100
        }, {
            field: 'ApproveStatusDescribe',
            displayName: '审批状态',
            enableCellEdit: false,
            sortable: true,
            width: 80
        }, {
            field: 'Priority',
            displayName: '权重',
            enableCellEdit: false,
            sortable: true,
            width: 80
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 180,
            cellTemplate: '<div><a href="#/Activities/EditActivities/{{row.getProperty(\'Id\')}}" ng-show="row.getProperty(\'IsApproved\')==0 ">编辑</a>  ' +
                '<a href="javascript:return void()" ng-click="delete(row.getProperty(\'Id\'))" ng-show="row.getProperty(\'Status\')==0 || row.getProperty(\'Status\')==2">删除</a> ' +
                '<a href="https://www.bluemembers.com.cn/Mavement/Detail/{{row.getProperty(\'Id\')}}" ng-click="">预览</a></div>'
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
            width: 120,
            cellTemplate: '<div><a href="" data-toggle="modal" data-target="#activitiesApproveListModal" ng-click="loadApproveList(row)">审批记录</a></div>'
        }, {
            field: 'action',
            displayName: '查看报名名单',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            cellTemplate: '<div><a href="#/Activities/GetSignUpActivities/{{row.getProperty(\'Id\')}}" ng-show="row.getProperty(\'SignUp\')==1 ">查看报名名单</a></div>'
        }, {
            field: 'action',
            displayName: '操作记录',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            cellTemplate: '<div><a href="#/Activities/OperationLog/{{row.getProperty(\'Id\')}}">操作记录</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});



activities.controller('CreateActivitiesCtrl', function ($scope, $http, $location, $upload) {

    $scope.createData = {};
    $scope.createData.SupportWay = 0;
    $scope.createData.IsDisplay = 0;
    $scope.IsCarOwner = 0;
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
        var url = 0;
        var signUp = 0;
        var supportway = $scope.createData.SupportWay;
        if (supportway == 0) {
            signUp = 1;
        }
        if (supportway == 1) {
            url = 1;
        }
        $http.post("/Activities/CreateActivities",
        {
            Title: $scope.createData.Title,
            MajorImageUrl: $scope.createData.MajorImageUrl,
            SignUp: signUp,
            IsUrl: url,
            IsCarOwner: $scope.createData.IsCarOwner,
            Priority: $scope.createData.Priority,
            IsDisplay: $scope.createData.IsDisplay,
            Summary: $scope.createData.Summary,
            Url: $scope.createData.Url,
            Content: $scope.createData.Content,
            BeginTime: $scope.createData.BeginTime,
            EndTime: $scope.createData.EndTime
        }).success(function (data) {
            if (data.success) {
                $location.path("/Activities/Index");
            }
            else
                alert("创建失败");
        });
    }
});

activities.controller('EditActivitiesCtrl', function ($scope, $http, $routeParams, $location, $upload) {

    $scope.editData = {};
    $http.get("/Activities/LoadActivities/" + $routeParams.Id).success(function (data) {
        $scope.editData.Title = data.Title;
        $scope.editData.MajorImageUrl = data.MajorImageUrl;
        $scope.editData.image = data.MajorImageUrl;
        $scope.editData.SignUp = data.SignUp;
        $scope.editData.Content = decodeURI(data.Content);
        $scope.editData.BeginTime = data.BeginTime;
        $scope.editData.EndTime = data.EndTime;
        $scope.editData.IsCarOwner = data.IsCarOwner;
        $scope.editData.isUrl = data.IsUrl;
        $scope.editData.IsHot = data.IsHot;
        $scope.editData.Summary = data.Summary;
        $scope.editData.Url = data.Url;
        $scope.editData.Priority = data.Priority;
        $scope.editData.SupportWay = data.SupportWay;
        setContent(decodeURI(data.Content));
    }
    );

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

    $scope.edit = function () {
        if (window.confirm("编辑会进入审核状态，如果是首页活动，会取消首页的显示，是否确定编辑?")) {
            var url = 0;
            var signUp = 0;
            var supportway = $scope.editData.SupportWay;
            if (supportway == 0) {
                signUp = 1;
            }
            if (supportway == 1) {
                url = 1;
            }
            $http.post("/Activities/EditActivities",
                {
                    Id: $routeParams.Id,
                    Title: $scope.editData.Title,
                    MajorImageUrl: $scope.editData.MajorImageUrl,
                    SignUp: signUp,
                    Content: encodeURI(getContent()),
                    BeginTime: $scope.editData.BeginTime,
                    EndTime: $scope.editData.EndTime,
                    Priority: $scope.createData.Priority,
                    IsCarOwner: $scope.editData.IsCarOwner,
                    IsUrl: url,
                    Summary: $scope.editData.Summary,
                    Url: $scope.editData.Url
                }).success(function (data) {
                    if (data.success) {
                        $location.path("/Activities/Index");
                    }
                    else
                        alert("修改失败");
                });
        }
    }

});

activities.controller('SignUpActivitiesCtrl', function ($scope, $http, $routeParams) {
    $scope.userName = "";

    $scope.filterOptions = {
        useExternalFilter: true
    };
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };

    $scope.searchSignUp = function () {
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
            var url = "/Activities/LoadSignUpList?id=" + $routeParams.Id;
            if (filter && filter.userName != undefined) {
                url += "&userName=" + filter.userName;
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
            field: 'Id',
            displayName: '编号',
            width: 80,
            sortable: true
        }, {
            field: 'UserId',
            displayName: '报名人ID',
            width: 200,
            sortable: false
        }, {
            field: 'UserName',
            displayName: '报名人名称',
            width: 200,
            sortable: true
        }, {
            field: 'CreateTime',
            displayName: '报名时间',
            enableCellEdit: false,
            sortable: true,
            width: 80
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

});
activities.controller('ActivitiesOperationLogCtrl', function ($scope, $http, $routeParams, $location, $upload) {

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
            var url = "/Activities/GetOperationLog?Id=" + $routeParams.Id;

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



