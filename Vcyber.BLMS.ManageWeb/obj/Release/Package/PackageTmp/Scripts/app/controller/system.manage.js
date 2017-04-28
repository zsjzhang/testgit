var systemManage = angular.module('systemManage', ['ngGrid', 'webix']);
systemManage.controller('UserManagmentCtrl', function ($scope, $http, $routeParams) {
    $scope.newUser = {};
    $scope.editData = {};
    $scope.resetData = {};

    $scope.provinceDw = { select: -1 };
    $scope.cityDw = { select: -1 };
    $scope.dealerDw = { select: -1 };//4s店

    $scope.roles = new Array();//用户类型
    $scope.select = "";

    //加载用户类型列表
    $http.get('/Roles/RoleList?start=0&count=100').success(function (largeLoad) {
            $scope.roles = largeLoad.data;
    });

    
    $http.get('/Common/GetDealerProvinceListJsonResult').success(function (data) {
        $scope.provinceDw.data = data;
    });

    $scope.provinceChange = function() {
        $http.get('/Common/GetDealerCityListJsonResult', { params: { province: $scope.provinceDw.select } }).success(function (data) {
            $scope.cityDw.data = data;
        });
    };
    
    $scope.cityChange = function () {
        $http.get('/Common/GetDealerListJsonResult', { params: { province: $scope.provinceDw.select, city: $scope.cityDw.select } }).success(function (data) {
            $scope.dealerDw.data = data;
        });
    };

    $scope.selectid = "";

	$scope.addUser = function () {
	    $http.post("/Users/AddUser",
            { UserName: $scope.newUser.UserName, Email: $scope.newUser.Email, Password: $scope.newUser.Password, RepeatPassword: $scope.newUser.RepeatPassword, Phone: $scope.newUser.Phone, Status: $scope.newUser.Status, DealerId: $scope.dealerDw.select })
			.success(function (data) {
			    if (data.success) {
			        $scope.newUser = {};
			        $('#addUserModal').modal('hide');
			        alert("添加用户成功！");
			    }
			    else {
			        alert("添加用户失败！ \n" + data.msg);
			    }
			    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
			})
            .error(function (data) {
                alert("添加用户失败！ \n" + data.msg);
            });
	};

    $scope.loadEditUser = function(row) {
        $scope.selectid = "";
        $scope.selectid = row.entity.Id;
        $http.get("/Users/UserJsonResult/", { params: { id: row.entity.Id } }).success(function(data) {
            $scope.editData.Id = row.entity.Id;
            $scope.editData.UserName = data.UserName;
            $scope.editData.Email = data.Email;
            $scope.editData.Phone = data.Phone;
            $scope.editData.Department = data.Department;
            $scope.editData.Status = data.Status;
        });
    };
    $scope.loadResetData = function(row) {
        $scope.resetData = {};
        $scope.resetData.Id = row.entity.Id;
        $scope.resetData.UserName = row.entity.UserName;
    };
    $scope.resetUserPw = function() {
        $http.post("/Account/ResetPassword", { Id: $scope.resetData.Id, UserName: $scope.resetData.UserName, Password: $scope.resetData.Password, ConfirmPassword: $scope.resetData.ConfirmPassword })
            .success(function(data) {
                if (data.IsSuccess) {
                    $('#resetpwModal').modal('hide');
                    alert("重置密码成功！");
                } else {
                    alert("重置密码失败！ \n" + data.message);
                }
            })
            .error(function(data) {
                alert("重置密码失败！ \n" + data.message);
            });
    };

    $scope.editUser = function() {
        $http.post("/Users/EditUser",
            { Id: $scope.editData.Id, Email: $scope.editData.Email, Phone: $scope.editData.Phone, Department: $scope.editData.Department, Status: $scope.editData.Status })
            .success(function() {
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                $('#editModal').modal('hide');
            });
    };

    $scope.deleteUser = function(row) {
        console.log("call delete user");
        $http.post("/Users/DelUser", { Id: row.entity.Id })
            .success(function(data) {
                if (data) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                    alert("删除成功！");
                } else
                    alert("删除失败！");
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

	$scope.searchUser = function () {
	    $scope.filterOptions.userName = $scope.userName;
	    $scope.filterOptions.userRole = $scope.select;
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
			var url = "/Users/UserList";
			var filterStr = "";
			if (filter && filter.userName != undefined) {
			    filterStr += filter.userName;
			}

			var uRole = "";
			if (filter && filter.userRole != undefined) {
			    uRole = filter.userRole;
			}

			$http.get(url, { params: { start: (page - 1) * pageSize, count: pageSize, userName: filterStr, userRole: uRole } }).success(function (largeLoad) {
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
			displayName: 'Id',
			visible: false
		}, {
			field: 'UserName',
			displayName: '账户名称',
			width: 200,
			sortable: false
		}, {
			field: 'Email',
			displayName: '邮箱',
			sortable: false,
			width: 200
		}, {
			field: 'Phone',
			displayName: '手机号',
			sortable: false,
			width: 200
		}, {
			field: 'CreateTime',
			displayName: '创建时间',
			visible: false
		}, {
			field: 'RoleName',
			displayName: '角色类型',
			sortable: false,
			width: 120
		}, {
			field: 'Department',
			displayName: '行政部门',
			sortable: false,
			width: 120
		}, {
			field: 'StatusName',
			displayName: '状态',
			sortable: false,
			width: 120
		}, {
			field: 'action',
			displayName: '操作',
			enableCellEdit: false,
			sortable: false,
			width: 200,
			cellTemplate: '<a href="javascript:return void()" data-toggle="modal" data-target="#editModal" ng-click="loadEditUser(row)">编辑</a> <a href="javascript:return void()" data-toggle="modal" data-target="#resetpwModal" ng-click="loadResetData(row)">重置密码</a> <a href="#/UserRoles/Index/{{row.getProperty(\'Id\')}}">角色分配</a> <a href="javascript:return void()" ng-click="deleteUser(row)" ng-confirm-click="确定删除此用户?">删除</a>'
		}],
		enablePaging: true,
		showFooter: true,
		totalServerItems: 'totalServerItems',
		pagingOptions: $scope.pagingOptions,
		filterOptions: $scope.filterOptions
	};
});

systemManage.controller('AddUser2RoleCtrl', function ($scope, $http, $routeParams, $location) {
    $scope.User = {
        id: $routeParams.id
    };

    $http.get('/UserRoles/AddToRole', { params: { userId: $scope.User.id } }).success(function (data) {
        $scope.UserRoles = data;
    });

    $scope.addRoles = function() {
        $http.post("/UserRoles/AddToRole",
            { UserRoles: $scope.UserRoles })
            .success(function(data) {
                if (data.success)
                    $location.url("Users/User");
                else
                    alert(data.msg);
            });
    };

    $scope.returnUp = function () {
        $location.url("Users/User");
    }
});

systemManage.controller('RoleManageCtrl', function ($scope, $http, TreeData) {
    $scope.Functions = {};

    $scope.loadEditRole = function (row) {
        $scope.roleData = {};
        $scope.roleData.Id = row.entity.Id;
        $scope.roleData.Name = row.entity.Name;
        $scope.roleData.Describe = row.entity.Describe;
    };
    $scope.addRole = function () {
        $http.post("/Roles/AddRole",
            { Name: $scope.roleData.Name, Describe: $scope.roleData.Describe })
            .success(function () {
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                $scope.roleData = {};
                $('#addRoleModal').modal('hide');
            });
    };

    $scope.editRole = function () {
        $http.post("/Roles/EditRole",
            { Id: $scope.roleData.Id, Name: $scope.roleData.Name, Describe: $scope.roleData.Describe })
            .success(function () {
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
                $('#editRoleModal').modal('hide');
            });
    };

    $scope.deleteRole = function (row) {
        $http.post("/Roles/DelRole", { Id: row.entity.Id })
            .success(function (data) {
                if (data) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    alert("删除成功！");
                } else
                    alert("删除失败！");
            });
    };

    $scope.getFuncList = function (row) {
        var vm = $scope.vm = {};
        $scope.roleid = row.entity.Id;
        $http.get("/Roles/BindFunctionJsonResult/", { params: { id: row.entity.Id } }).success(function (data) {
            vm.Functions = data;
            vm.tree = new TreeData(vm.Functions);

        });
    };
    $scope.getCheckedIdList = function (functions) {

        if (functions != null) {
            $.each(functions, function (index) {
                if (functions[index].IsChecked == true) {
                    $scope.IdList.push(functions[index].id);
                }
                $scope.getCheckedIdList(functions[index].data);
            });
        }
    };


    $scope.functionSave = function () {
        $scope.IdList = [];
        $scope.getCheckedIdList($scope.vm.Functions);
        $http.post("/Roles/BindFunction", { things: $scope.IdList, roleId: $scope.roleid })
            .success(function (data) {
                if (data) {
                    $('#eidtRoleFunctionModal').modal('hide');
                    alert("保存成功！");
                } else
                    alert("保存失败！");
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

    $scope.search = function () {
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
            var timestamp = (new Date()).valueOf();
            var url = "/Roles/RoleList?timestamp=" + timestamp;
            $http.get(url, { params: { start: (page - 1) * pageSize, count: pageSize } }).success(function (largeLoad) {
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
            displayName: 'Id',
            visible: false
        }, {
            field: 'Name',
            displayName: '角色名称',
            width: '40%',
            sortable: false
        }, {
            field: 'Describe',
            displayName: '描述',
            sortable: false,
            width: '45%',
        }, {
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 160,
            cellTemplate: '<a href="javascript:return void()" data-toggle="modal" data-target="#editRoleModal" ng-click="loadEditRole(row)">编辑</a>  <a href="javascript:return void()" ng-click="deleteRole(row)" ng-confirm-click="确定删除此角色?">删除</a>  <a href="javascript:return void()" data-toggle="modal" data-target="#eidtRoleFunctionModal" ng-click="getFuncList(row)">权限设置</a>'
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

systemManage.controller('FunctionManageCtrl', function ($scope, $http, $routeParams, $location) {
    $scope.Functions = {};
    $scope.AddFunctionData = {};
    $scope.EditFunctionData = {};
    $scope.routeDw = { select: 0 };

    $scope.getFuncList = function () {
        $http.get("/Function/FunctionListJsonResult/").success(function (data) {
            $scope.Functions.data = data;
        });
    };

    $scope.getFuncList();

    $scope.SendSelectId = function(id) {
        $scope.selectid = id;
    };
    $scope.loadEditFunction = function (id) {
        $http.get("/Function/FunctionAndUrlJsonResult/", { params: { id: id } }).success(function (data)
        {
            $scope.functionData = {};
            $scope.functionData.Id = id;

            $scope.functionData.Name = data.Name;
            $scope.functionData.Describe = data.Describe;
            $scope.functionData.Action = data.Action;
            $scope.functionData.Controller = data.Controller;
            //$scope.functionData.Url = data.Url;
            $scope.functionData.UrlDescibe = data.UrlDescibe;
        });

    };

    //$scope.loadEditFunction = function(id) {
    //    $scope.EditFunctionData = {};

    //    $http.get("/Function/FunctionJsonResult/", { params: { id: id } }).success(function(data) {
    //        $scope.EditFunctionData = data;
    //    });
    //};

    $scope.addFunction = function() {
        $http.post("/Function/AddRootFunction",
        { Name: $scope.AddFunctionData.name, Describe: $scope.AddFunctionData.description, Action: $scope.AddFunctionData.action, Controller: $scope.AddFunctionData.controller, UrlDescibe: $scope.AddFunctionData.urlDesc, RouteSelection: $scope.routeDw.select })
            .success(function(data) {
                if (data.success) {
                    $scope.AddFunctionData = {};
                    $scope.getFuncList();
                    $('#addFunctionModal').modal('hide');
                } else {
                    alert(data.message);
                }
            });
    };
    $scope.operatorFun = function (id, details) {
        if (id.column == "add") {
            $scope.SendSelectId(id.row);
        }
        if (id.column == "edit") {
            $scope.loadEditFunction(id.row);
        }
        if (id.column == "delete") {
            $scope.deleteFunction(id.row);
        }
    };   


    $scope.addChildFunction = function (id) {
        $http.post("/Function/AddChildFunction",
           { parentId: $scope.selectid, Name: $scope.AddFunctionData.name, Describe: $scope.AddFunctionData.description, Action: $scope.AddFunctionData.action, Controller: $scope.AddFunctionData.controller, UrlDescibe: $scope.AddFunctionData.urlDesc, RouteSelection: $scope.routeDw.select })
           .success(function (data) {
               if (data.success) {
                   $scope.AddFunctionData = {};
                   $scope.getFuncList();
                   $('#addChildFunctionModal').modal('hide');
               } else {
                   alert(data.message);
               }
           });
    };

    $scope.editFunction = function() {
        $http.post("/Function/EditFunction",
            { id: $scope.functionData.Id, Name: $scope.functionData.Name, Describe: $scope.functionData.Describe, Action: $scope.functionData.Action, Controller: $scope.functionData.Controller, UrlDescibe: $scope.functionData.UrlDescibe, RouteSelection: $scope.routeDw.select })
            .success(function (data) {
                if (data.success) {
                    $scope.AddFunctionData = {};
                    $scope.getFuncList();
                    $('#editFunctionModal').modal('hide');
                } else {
                    alert(data.message);
                }
            });
    };

    $scope.deleteFunction = function (id) {
        if (window.confirm("确定删除此功能?"))
            {
        $http.post("/Function/DelFunction", { id: id })
            .success(function(data) {
                if (data) {
                    alert("删除成功！");
                    $scope.AddFunctionData = {};
                    $scope.getFuncList();
                } else
                    alert("删除失败！");
            });
        }
    };
});
