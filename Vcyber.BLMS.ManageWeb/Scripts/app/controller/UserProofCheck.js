var userProofCheck = angular.module('userProofCheck', ['ngGrid', 'webix']);
//用户凭证审核
userProofCheck.controller('userProofCheckCtrl', function ($scope, $http, $routeParams, $location) {
    //会员等级
    $scope.mLevelDw = {};
    //证件类型
    $scope.mPaperWork = {};    
    //审核状态
    $scope.isProofed = {};

    //$http.get('/Common/GetMembershipLevelJsonResult').success(function (data) {
    //    //data.Delete("索九会员");
    //    $scope.mLevelDw.data = data;
    //});
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
    // 根据 条件 查询
    $scope.search = function () {
        $scope.filterOptions.phoneNumber = $scope.phoneNumber;
        $scope.filterOptions.mLevelValue = $scope.mLevelDw.select;
        $scope.filterOptions.mPaperWork = $scope.mPaperWork.select;
        $scope.filterOptions.identityNumber = $scope.identityNumber;
        $scope.filterOptions.isProofed = $scope.isProofed.select;
        $scope.filterOptions.beginTime = $scope.beginTime;
        $scope.filterOptions.endTime = $scope.endTime;

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
        //var tempStr = "?timtspan=" + Date.parse(new Date());
        var url = "/Membership/GetUserProofList";

        $http.get(url, { params: { phone: filter.phoneNumber, mlevel: filter.mLevelValue, paperwork: filter.mPaperWork, identitynumber: filter.identityNumber, status: filter.isProofed, StrCreateTime: filter.beginTime, StrEnd: filter.endTime, pageCount: (page - 1) * pageSize, currentPage: pageSize * page } }).success(function (largeLoad) {
            if (!largeLoad.IsSuccess) {
                alert(largeLoad.Message)
            }else{
                $scope.setPagingData(largeLoad.Data, page, pageSize, largeLoad.Errors);
            }
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
   
    //更改审核状态
    $scope.updateStatusY = function (membershipId) {
        if (window.confirm("确定凭证审核通过?")) {
            $http.post("/Membership/UpdateProofStatus", { id: membershipId, status: 1 })
                    .success(function (data) {
                        if (data.success) {
                            alert(data.msg);
                        }
                        else
                            alert(data.msg);
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                    });
        };
    };
    //更改审核状态
    $scope.updateStatusN = function (membershipId) {
        if (window.confirm("确定凭证审核未通过?")) {
            $http.post("/Membership/UpdateProofStatus", { id: membershipId, status: 2 })
                    .success(function (data) {
                        if (data.success) {
                            alert(data.msg);
                        }
                        else
                            alert(data.msg);
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    });
        };
    };
    //删除
    $scope.delete = function (Id) {
        var ids = new Array();
        ids.push(Id);
        if (window.confirm("确定要删除此信息？")) {
            $http.post("/Membership/DeleteProofInfos", { ids: ids })
                .success(function (data) {
                    if (data.success) {
                        alert(data.msg);
                    }
                    else {
                        alert(data.msg);
                    }
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                });
        };
    };
    $scope.deleteAll = function () {;
            var chks = document.getElementsByName("chk");
            var ids = new Array();
            for(var i=0;i<chks.length;i++){
                if(chks[i].checked == true){
                    ids.push(chks[i].id);
                }
            }
            if(ids.length == 0){
                alert('请选择要删除的数据');
                return;
            }
            if(confirm("是否确认删除？")){
                $http.post("/Membership/DeleteProofInfos", { ids: ids })
                .success(function (data) {
                    if (data.success) {
                        for (var i = 0; i < chks.length; i++) {
                            chks[i].checked = false;
                        }
                        alert(data.msg);
                    }
                    else {
                        alert(data.msg);
                    }
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                });
            }

    };
    
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
        columnDefs: [
        {
            field: '',
            displayName: '勾选',
            enableCellEdit: false,
            width: 40,
            sortable: true,
            cellTemplate: '<div align=center style="margin-top:5px;" ><input type="checkbox" id="{{row.getProperty(\'Id\')}}"  name="chk" ng-model="proofCheck.Checked"> </div>'
        },
        {
            field: 'action',
            displayName: '删除',
            enableCellEdit: false,
            sortable: false,
            width:40,
            cellTemplate: '<div align="center"><a href="javascript:return void()" ng-click="delete(row.getProperty(\'Id\'))" >删除</a> </div>'
           
        },
        {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 110,
            cellTemplate: '<div><a href="javascript:return void()" data-toggle="modal" data-target="#UserProofInfo" ng-click="userProofInfo(row)" >查看</a>  ' +
                '<a href="javascript:return void()" ng-click="updateStatusY(row.getProperty(\'MembershipId\'))" ng-show="row.getProperty(\'ApproveStatus\')==0">通过</a> ' +
                '<a href="javascript:return void()" ng-click="updateStatusN(row.getProperty(\'MembershipId\'))" ng-show="row.getProperty(\'ApproveStatus\')==0">未通过</a> </div>'
        },
        {
            field: 'PhoneNumber',
            displayName: '手机号',
            enableCellEdit: false,
            sortable: true,
            width: 110
        },
        {
            field: 'UserName',
            displayName: '姓名',
            enableCellEdit: false,
            sortable: true,
            width: 90
        }, {
            field: 'PaperWork',
            displayName: '证件类型',
            enableCellEdit: false,
            sortable: true,
            width: 90
        }, {
            field: 'IdentityNumber ',
            displayName: '证件号',
            enableCellEdit: false,
            sortable: true,
            width: 160
        },{
            field: 'ApproveStatusDiscribe',
            displayName: '审核状态',
            enableCellEdit: false,
            sortable: true,
            width: 80
        },{
            field: 'MLevelDisc',
            displayName: '会员等级',
            enableCellEdit: false,
            sortable: true,
            width: 90
         }, {
             field: 'CreateTime',
             displayName: '提交时间',
             enableCellEdit: false,
             sortable: true,
             width: 150
         }, ],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
    //查看
    $scope.userProofInfo = function (row) {
        $scope.ProofData = {};

        $scope.ProofData.ImageProofFront = row.entity.ImageProofFront;
        $scope.ProofData.ImageProofVerso = row.entity.ImageProofVerso;
        $scope.ProofData.ImageProofByHand = row.entity.ImageProofByHand;
    };



})