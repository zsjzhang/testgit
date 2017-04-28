var shareRes = angular.module('shareRes', ['ngGrid', 'angularFileUpload']);

//共享资源文件controller
shareRes.controller('ShareResourceCtrl', function ($scope, $http, $routeParams, $location,$upload) {
    $scope.createData = {};

    $scope.selectFileType = "1";

    $scope.filterOptions = {
        useExternalFilter: true
    };
    $scope.totalServerItems = 0;

    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };
    $scope.$watch('files', function () {
        $scope.upload($scope.files);
    });

    $scope.upload = function (files, action, type) {
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
                            if (type == 'list') {
                                $scope.createData.ListImageUrl = data.url;
                                $scope.createData.ListImage = data.url;
                            }

                            if (type == 'play') {
                                $scope.createData.PlayImageUrl = data.url;
                                $scope.createData.PlayImage = data.url;
                            }
                        }
                        if (action == "uploadSource") {
                            $scope.createData.LinkUrl = data.url;
                        }
                        alert('上传成功');
                    }

                    //console.log('file ' + config.file.name + 'uploaded. Response: ' +
                    //            JSON.stringify(data));
                }).error(function (err) {
                    alert('上传失败');
                });
            }
        }
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

    //创建一个资源文件
    $scope.create = function () {
        var timestamp = Date.parse(new Date());

        //默认显示
        if ($scope.createData.IsDisplay == undefined) {
            $scope.createData.IsDisplay = "1";
        }

        //添加
        if ($scope.createData.Id == undefined || $scope.createData.Id == null || $scope.createData.Id == "") {
            $http.post('/ShareResource/AddResourceFile',
                {
                    Title: $scope.createData.Title,
                    SubTitle:$scope.createData.SubTitle,
                    LinkUrl: $scope.createData.LinkUrl,
                    IsDisplay: $scope.createData.IsDisplay,
                    Summary: $scope.createData.Summary,
                    FileType: $scope.selectFileType,
                    Category: $scope.createData.Category,
                    ListImageUrl: $scope.createData.ListImageUrl,
                    PlayImageUrl: $scope.createData.PlayImageUrl
                }).success(function (data) {
                    if (data.success) {
                        alert("添加成功");
                        $('#AddShareResource').modal('hide');
                        $scope.getPagedDataAsyncApply($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                    } else {
                        alert("添加失败！ \n" + data.msg);
                        $('#AddShareResource').modal('hide');
                    }
                }).error(function (data) {
                    alert("添加失败！ \n" + data.msg);
                });
        }
            //更新
        else {
            $http.post('/ShareResource/UpdateResourceFile',
            {
                ID: $scope.createData.Id,
                Title: $scope.createData.Title,
                SubTitle: $scope.createData.SubTitle,
                LinkUrl: $scope.createData.LinkUrl,
                IsDisplay: $scope.createData.IsDisplay,
                Summary: $scope.createData.Summary,
                FileType: $scope.selectFileType,
                Category: $scope.createData.Category,
                ListImageUrl: $scope.createData.ListImageUrl,
                PlayImageUrl: $scope.createData.PlayImageUrl
            }).success(function (data) {
                if (data.success) {
                    alert("保存成功");
                    $('#AddShareResource').modal('hide');
                    $scope.getPagedDataAsyncApply($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                } else {
                    alert("保存失败！ \n" + data.msg);
                    $('#AddShareResource').modal('hide');
                }
            }).error(function (data) {
                alert("保存失败！ \n" + data.msg);
            });
        }
    }


    //查询
    $scope.getPagedDataAsyncApply = function (pageSize, page, filter) {
        var timestamp = Date.parse(new Date());
        var url = "/ShareResource/GetAllList?timestamp=" + timestamp;
        $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize + '&fileType=0&category=').success(function (largeLoad) {
            $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
        });
    };

    //删除
    $scope.delShareResById = function (id) {
        if (confirm("确定要删除？")) {
            $http.post('/ShareResource/DelResourceFile', { ID: id }).success(function (data) {
                if (data.success) {
                    alert("删除成功");
                    $scope.getPagedDataAsyncApply($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                } else {
                    alert("删除失败！ \n" + data.msg);
                }
            }).error(function (data) {
                alert("删除失败！ \n" + data.msg);
            });
        }
    };

    //弹出添加
    $scope.addShareRes = function () {
        //$scope.createData = {
        //    ListImage : null,
        //    PlayImage : null
        //};

        $scope.createData.Id = "";
        $scope.createData.Title = "";
        $scope.createData.SubTitle = "";
        $scope.createData.IsDisplay = "1";
        $scope.createData.Summary = "";
        $scope.createData.LinkUrl = "";
        $scope.selectFileType = "1";
        $scope.createData.Category = "";
        $scope.createData.ListImageUrl = "";
        $scope.createData.ListImage = "";
        $scope.createData.PlayImageUrl = "";
        $scope.createData.PlayImage = "";
        $('#AddShareResource').modal('show');
    };

    //编辑
    $scope.editShareResById = function (row) {
        //$scope.createData = {
        //    ListImage: null,
        //    PlayImage: null
        //};

        $scope.createData.Id = row.entity.Id;
        $scope.createData.Title = row.entity.Title;
        $scope.createData.SubTitle = row.entity.SubTitle;
        $scope.createData.IsDisplay = row.entity.IsDisplay;
        $scope.createData.Summary = row.entity.Summary;
        $scope.createData.LinkUrl = row.entity.LinkUrl;
        $scope.selectFileType = row.entity.FileType;
        $scope.createData.Category = row.entity.Category;
        $scope.createData.ListImageUrl = row.entity.ListImageUrl;
        $scope.createData.ListImage = row.entity.ListImageUrl;
        $scope.createData.PlayImageUrl = row.entity.PlayImageUrl;
        $scope.createData.PlayImage = row.entity.PlayImageUrl;
        $('#AddShareResource').modal('show');


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
            field: 'Title',
            displayName: '资源名称',
            sortable: true,
            width: 200
        }, {
            field: 'Summary',
            displayName: '简介',
            sortable: true,
            width: 410
        }, {
            field: 'CreateTime',
            displayName: '创建时间',
            sortable: true,
            width: 200,
            cellFilter: 'date: "mediumDate"'
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 100,
            cellTemplate: '<div><a href="javascript:return void()"  ng-click="editShareResById(row)">编辑</a>  ' +
                '<a href="javascript:return void()" ng-click="delShareResById(row.getProperty(\'Id\'))">删除</a></div>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };


});

//下载资源文件controller
shareRes.controller('ResDownloadCtrl', function ($scope, $http, $routeParams, $location) {
    $scope.createData = {};
    $scope.filterOptions = {
        useExternalFilter: true
    };
    $scope.totalServerItems = 0;

    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
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

    //查询
    $scope.getPagedDataAsyncApply = function (pageSize, page, filter) {
        var timestamp = Date.parse(new Date());
        var url = "/ShareResource/GetAllList?timestamp=" + timestamp;
        $http.get(url + '&count=' + pageSize + '&start=' + (page - 1) * pageSize + '&fileType=1&category=').success(function (largeLoad) {
            $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
        });
    };

    //下载资源文件
    $scope.downloadShareRes = function (row) {
        //$http.post('/ShareResource/DownloadResourceFile', { ID: row.entity.Id });
        window.open("https://" + location.host + "/" + row.entity.LinkUrl);
        return null;
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
            field: 'Title',
            displayName: '资源名称',
            sortable: true,
            width: 200
        }, {
            field: 'Summary',
            displayName: '简介',
            sortable: true,
            width: 410
        }, {
            field: 'CreateTime',
            displayName: '创建时间',
            sortable: true,
            width: 200,
            cellFilter: 'date: "mediumDate"'
        }, {
            field: 'LinkUrl',
            displayName: '资源路径',
            visible: false
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 100,
            cellTemplate: '<div><a href="javascript:return void()"  ng-click="downloadShareRes(row)">下载</a>  ' 
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };


});