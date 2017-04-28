//置换活动
var XDActivities = angular.module('XDActivities', ['ngGrid', 'angularFileUpload']);
//置换活动首页
XDActivities.controller('PermuteActivitiesCtrl', function ($scope, $http, $routeParams, $location) {
    $scope.title = "";
    $scope.approveData = {};
    $scope.selectItem = {};
    $scope.signUpData = {};

    $scope.filterOptions = {
        useExternalFilter: true
    };
    //数据总条数
    $scope.totalServerItems = 0;
    //设置每页显示条数
    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };

    //活动状态下拉表/// 0：进行中，已开始，未结束；1：已结束；2：未开始
    $scope.statuslist =
       [{
           id: 0,
           Name: '进行中'
       }, {
           id: 1,
           Name: '已结束'
       }, {
           id: 2,
           Name: '未开始'
       }];
    // 根据 条件 查询
    $scope.search = function () {
        $scope.filterOptions.Status = $scope.selected;
        //$scope.filterOptions.dealer = $scope.dealerDw.select;
        $scope.pagingOptions.currentPage = 1;
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    };
    //分页显示数据 翻页
    $scope.setPagingData = function (data, page, pageSize, total_count) {
        //var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
        $scope.myData = data;
        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };

    //分页查询
    $scope.getPagedDataAsync = function (pageSize, page, filter) {
        var data;
        var url = "/Activities/GetPermuteActivitiesList?status=" + filter.Status;
        //if (filter && filter.dealer != undefined) {
        //    url += "&dealer=" + filter.dealer;
        //}
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
    //点击编辑时跳转到编辑视图 查询当前行的数据 
    $scope.loadEditNews = function (row) {
        $routeParams.selectId = row.entity.Id;
        $location.path("/Activities/EditPermuteActivity");
    }
    // 跳转到创建视图 创建 置换活动 
    $scope.createPermute = function () {
        $location.path("/Activities/CreatePermuteActivity");
    }
    //删除置换活动
    $scope.delete = function (id) {

        if (window.confirm("是否确定删除?")) {
            $http.post("/Activities/deletePermute",
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



    //设置是否显示字段
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

    //
    $scope.UpdatePriority = function () {
        $http.post("/Activities/UpdateAllDisplay",
                   {
                       id: $scope.selectItem.Id,
                       priority: $scope.selectItem.Priority,
                       dispaly: $scope.selectItem.IsDisplay,
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
    //列表绑定数据
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
            displayName: '编号',
            width: 80,
            sortable: true
        }, {
            field: 'XDActivityName',
            displayName: '标题',
            width: 120,
            sortable: false
        }, {
            field: 'StatusValue',
            displayName: '活动状态',
            enableCellEdit: false,
            width: 80
        }, {
            field: 'action',
            displayName: '缩略图',
            enableCellEdit: false,
            sortable: false,
            width: 120,
            heigth: 80,
            cellTemplate: '<div><img ng-src="{{row.getProperty(\'ActivitySubImage\')}}" style="width:100px;height:50px" ></div>'

        },
        {
            field: 'ActivityStartTime',
            displayName: '开始日期',
            enableCellEdit: false,
            sortable: true,
            width: 150
        }, {
            field: 'ActivityEndTime',
            displayName: '结束时间',
            sortable: true,
            width: 150
        }, {
            field: 'IsValid ',
            displayName: '是否显示',
            enableCellEdit: false,
            sortable: true,
            width: 80,
            cellTemplate: '<div ng-show="row.getProperty(\'IsValid\')==1">显示</div><div ng-show="row.getProperty(\'IsValid\')==0">不显示</div>'
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 180,
            cellTemplate: '<div><a href="#/Activities/EditPermuteActivity/{{row.getProperty(\'ActivityId\')}}" >编辑</a>  ' +
                '<a href="javascript:return void()" ng-click="delete(row.getProperty(\'ActivityId\'))" ng-show="row.getProperty(\'IsValid\')==0 || row.getProperty(\'IsValid\')==1">删除</a> ' +
                '<a href="https://blmsweb.pxene.com/OrderChange/ExChangeActivityDetail?activityId={{row.getProperty(\'ActivityId\')}}" ng-click="">预览</a></div>'
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
        //{
        //    field: 'action',
        //    displayName: '显示',
        //    enableCellEdit: false,
        //    sortable: false,
        //    width: 100,
        //    cellTemplate: '<div><a href="" data-toggle="modal" data-target="#priorityModal" ng-click="priorityPage(row)">编辑显示</a></div>'
        //}, 
        ],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

})
//创建置换活动
XDActivities.controller('CreatePermuteActivitiesCtrl', function ($scope, $http, $location, $upload) {

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
        
        $http.post("/Activities/CreatePermuteActivity",
        {
            XDActivityName: $scope.createData.Title,
            ActivityImage: $scope.createData.MajorImageUrl,
            ActivitySubImage: $scope.createData.MajorImageUrl,            
            IsValid: $scope.createData.IsDisplay,
            ActivityTitle: $scope.createData.Summary,            
            ActivityContent: $scope.createData.Content,
            ActivityStartTime: $scope.createData.BeginTime,
            ActivityEndTime: $scope.createData.EndTime
        }).success(function (data) {
            if (data.success) {
                $location.path("/Activities/PermuteIndex");
            }
            else
                alert("创建失败");
        });
    }
});
//修改置换活动
XDActivities.controller('EditPermuteActivitiesCtrl', function ($scope, $http, $routeParams, $location, $upload) {

    $scope.editData = {};
    $http.get("/Activities/LoadPermuteActivityById/" + $routeParams.Id).success(function (data) {
        $scope.editData.Title = data.XDActivityName;
        $scope.editData.MajorImageUrl = data.ActivitySubImage;
        $scope.editData.image = data.ActivitySubImage;
        $scope.editData.IsDisplay = data.IsValid,
        $scope.editData.Content = decodeURI(data.ActivityContent);
        $scope.editData.BeginTime = data.ActivityStartTime;
        $scope.editData.EndTime = data.ActivityEndTime;        
        $scope.editData.Summary = data.ActivityTitle;
       
        setContent(decodeURI(data.ActivityContent));
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
            //var url = 0;
            //var signUp = 0;
            //var supportway = $scope.editData.SupportWay;
            //if (supportway == 0) {
            //    signUp = 1;
            //}
            //if (supportway == 1) {
            //    url = 1;
            //}
            $http.post("/Activities/EditPermuteActivity",
                {
                    ActivityId: $routeParams.Id,
                    XDActivityName: $scope.editData.Title,
                    ActivityImage: $scope.editData.MajorImageUrl,
                    ActivitySubImage: $scope.editData.MajorImageUrl,
                    IsValid: $scope.editData.IsDisplay,
                    ActivityTitle: $scope.editData.Summary,
                    ActivityContent: encodeURI(getContent()),
                    ActivityStartTime: $scope.editData.BeginTime,
                    ActivityEndTime: $scope.editData.EndTime
                }).success(function (data) {
                    if (data.success) {
                        $location.path("/Activities/PermuteIndex");
                    }
                    else
                        alert("修改失败");
                });
        }
    }

});