var membershipModule = angular.module('membershipModule', ['ngGrid', 'webix']);
membershipModule.controller('MembershipCtrl', function ($scope, $http, $routeParams) {
    $scope.mLevelDw = { select: -1 };
    $scope.statusDw = { select: -1 };
    $scope.isComDw = { select: -1 };
    $scope.selectItem = {};
    //$scope.isSonata9 = false;
    $scope.IsTmallselect = "";
    $scope.CreatedPerson = "";
    $scope.AuthenticationSource = "";
    $scope.PaperWork = "";
    $scope.ApproveType = {};
    $scope.selectUserType = "";//银卡会员类型

    $scope.isComDw.data = [{ "id": "-1", "value": "请选择" }, { "id": "公司客户", "value": "公司客户" }, { "id": "个人客户", "value": "个人客户" }];

    $http.get('/Common/GetMembershipLevelJsonResult').success(function (data) {
        //data.Delete("索九会员");
        $scope.mLevelDw.data = data;
    });
    $http.get('/Common/GetMembershipStatusJsonResult').success(function (data) {
        $scope.statusDw.data = data;
    });
    //获取会员审批状态
    $http.get('/Common/GetMembershipApproveType').success(function (data) {
        $scope.ApproveType.data = data;
    });
    $scope.filterOptions = {
        useExternalFilter: true,
        identity: $scope.identity
    };
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };
    //发送手机验证码
    $scope.clickSendValidateCode = function () {
        $scope.mySwitch = true;

        setTimeout(function () {

            var url = "/ValidateCode/Send";
            $http.post(url, { "PhoneNumber": $scope.selectItem.PhoneNumber }).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.mySwitch = false;
                    alert("验证码已发送");
                } else {
                    $scope.mySwitch = false;
                    alert(largeLoad.Message);
                }
            });

        }, 100);
    }
    $scope.search = function () {
        $scope.filterOptions.mLevelValue = $scope.mLevelDw.select;
        $scope.filterOptions.statusValue = $scope.statusDw.select;
        $scope.filterOptions.isComValue = $scope.isComDw.select;//是否是企业用户  gjp
        $scope.filterOptions.AuthenticationTimeStart = $scope.AuthenticationTimeStart;
        $scope.filterOptions.AuthenticationTimeEnd = $scope.AuthenticationTimeEnd;
        $scope.filterOptions.BuyTimeStart = $scope.BuyTimeStart;
        $scope.filterOptions.BuyTimeEnd = $scope.BuyTimeEnd;
        $scope.filterOptions.identity = $scope.identity;
        $scope.filterOptions.realName = $scope.realName;
        $scope.filterOptions.nickName = $scope.nickName;
        $scope.filterOptions.VIN = $scope.VIN;
        $scope.filterOptions.beginTime = $scope.beginTime;
        $scope.filterOptions.endTime = $scope.endTime;
        $scope.filterOptions.IsTmall = $scope.IsTmallselect;
        $scope.filterOptions.PayNumber = $scope.PayNumber;
        $scope.filterOptions.ApproveType = $scope.ApproveType;
        $scope.filterOptions.UserType = $scope.selectUserType;

        $scope.filterOptions.PaperWork = $scope.PaperWork;
        $scope.pagingOptions.currentPage = 1;
        //4S店信息（只有Admin登录的时候能看见的查询条件）
        var dealerSelected = eval("(" + $scope.myOptions + ")");
        if (dealerSelected != undefined && dealerSelected != "undefined") {
            $scope.filterOptions.DealerId = dealerSelected.id;
            $scope.filterOptions.DealerName = dealerSelected.value;
        } else {
            $scope.filterOptions.DealerId = null;
            $scope.filterOptions.DealerName = null;
        }
        //alert($scope.DealerID);

        if ($scope.DealerID != "") {
            $scope.filterOptions.DealerId = $scope.DealerID;

        }

        if (($scope.provinceDw.select != "-1" || $scope.cityDw.select != "-1") && $scope.filterOptions.DealerId == undefined) {
            alert("请选择特约店");
            return false;
        }
        //身份证号
        $scope.filterOptions.IDCard = $scope.IDCard;

        $scope.filterOptions.CreatedPerson = $scope.CreatedPerson;
        $scope.filterOptions.AuthenticationSource = $scope.AuthenticationSource;
        console.log($scope.AuthenticationSource);

        //$scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    };

    $scope.setPagingData = function (data, page, pageSize, total_count) {
        //var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
        $scope.myData = data;
        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };
    var flag = ($("#ce").val() != "" ? false : true);
    $scope.getPagedDataAsync = function (pageSize, page, filter) {
        setTimeout(function () {
            var data;

            //var S9 = $scope.isSonata9;

            var tempStr = "?timtspan=" + Date.parse(new Date());
            if ($scope.exactSearch) {
                var eurl = "/Membership/MembershipExtraList" + tempStr;
                $http.get(eurl, { params: { UserType: filter.UserType, PayNumber: filter.PayNumber, IsTmall: filter.IsTmall, Identity: filter.identity, ApproveType: filter.ApproveType, NickName: filter.nickName, MLevel: filter.mLevelValue, VIN: filter.VIN, ExtraSearch: $scope.exactSearch, Status: filter.statusValue, Skip: (page - 1) * pageSize, Count: pageSize, BeginTime: filter.beginTime, EndTime: filter.endTime, IDCard: filter.IDCard, DealerId: filter.DealerId, VIN: filter.VIN } }).success(function (largeLoad) {
                    if (largeLoad.success) {
                        $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
                    } else {
                        alert(largeLoad.msg);
                    }
                });
            } else {
                var url = "/Membership/MembershipList" + tempStr;
                $http.get(url, { params: { UserType: filter.UserType, PayNumber: filter.PayNumber, IsTmall: filter.IsTmall, Identity: filter.identity, NickName: filter.nickName, MLevel: filter.mLevelValue, Status: filter.statusValue, Skip: (page - 1) * pageSize, Count: pageSize, BeginTime: filter.beginTime, EndTime: filter.endTime, IDCard: filter.IDCard, DealerId: filter.DealerId, VIN: filter.VIN, PaperWork: filter.PaperWork, isComValue: filter.isComValue, AuthenticationTimeStart: filter.AuthenticationTimeStart, AuthenticationTimeEnd: filter.AuthenticationTimeEnd, BuyTimeStart: filter.BuyTimeStart, BuyTimeEnd: filter.BuyTimeEnd, AuthenticationSource: filter.AuthenticationSource, CreatedPerson: filter.CreatedPerson } }).success(function (largeLoad) {
                    //if (largeLoad.data[0].Isadmin != "none") {
                    //    flag = true;
                    //}

                    $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
                });
            }

        }, 100);
    };
    if (!flag) {
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    }
    

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
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
            field: 'action',
            displayName: '操作',
            enableCellEdit: false,
            sortable: false,
            width: 340,
            cellTemplate: '<a href="#/Membership/Detail/{{row.getProperty(\'Id\')}}">详情</a>  <a style="display:{{row.getProperty(\'Isadmin\')}}"  href="javascript:return void()" ng-show="row.entity.Status==1" style="color:red" ng-click="Frozen(row)">冻结</a> ' +
                ' <a style="display:{{row.getProperty(\'Isadmin\')}}" href="javascript:return void()" style="color:green" ng-show="row.entity.Status==2" ng-click="UserStatusActive(row)">激活</a>' +
                ' <a style="display:{{row.getProperty(\'Isadmin\')}}" href="javascript:return void()" data-toggle="modal" data-target="#IdentityNumberModal" ng-click="updateINPage(row)">修改证件号</a>' +
                ' <a style="display:{{row.getProperty(\'Isadmin\')}}" href="javascript:return void()" data-toggle="modal" data-target="#PhoneNumberModal" ng-click="updateINPage(row)">修改手机号</a>' +
                 ' <a style="display:{{row.getProperty(\'Isadmin\')}}" href="javascript:return void()" data-toggle="modal" data-target="#UserPasswordModal" ng-click="updateINPage(row)">重置密码</a>' +
                 ' <a style="display:{{row.getProperty(\'Isadmin\')}}" href="javascript:return void()" data-toggle="modal" data-target="#SetWorthModal" ng-click="setInWorth(row)">设置财富</a>'
        },
        {
            field: 'action',
            displayName: '删除',
            enableCellEdit: false,
            sortable: false,
            visible: flag,
            width: 50,
            cellTemplate: '<a  style="display:{{row.getProperty(\'Isadmin\')}}" href="javascript:return void()"  ng-click="Delete(row)">删除</a>'
        },
        {
            field: 'PhoneNumber',
            displayName: '手机号',
            sortable: false,
            width: 130
        },
        {
            field: 'jf',
            displayName: '当前积分',
            sortable: false,
            width: 130
        }, {
            field: 'NickName',
            displayName: '用户名',
            sortable: false,
            width: 130
        }, {
            field: 'IdentityNumber',
            displayName: '证件号',
            sortable: false,
            width: 200
        },
        //{
        //    field: 'IsMembership',
        //    displayName: '是否为银卡会员',
        //    sortable: false,
        //    width: 130
        //}, {
        //    field: 'UserType',
        //    displayName: '银卡会员类型',
        //    sortable: false,
        //    width: 130
        //}
       {
           field: 'No',
           displayName: '会员卡号',
           sortable: false,
           width: 130
       }, {
           field: 'PayNumber',
           displayName: '付款码',
           sortable: false,
           width: 130
       }, {
           field: 'CreateTime',
           displayName: '创建时间',
           width: 150
       }, {
           field: 'Status',
           displayName: '用户状态',
           visible: false
       }, {
           field: 'StatusName',
           displayName: '用户状态',
           sortable: false,
           width: 120
       }
       //, {
       //    field: 'MLevel',
       //    displayName: '用户等级值',
       //    sortable: false,
       //    width: 100
       //}
       , {
           field: 'MLevelName',
           displayName: '会员等级',
           sortable: false,
           width: 120
       }, {
           field: 'CreatedPerson',
           displayName: '创建人',
           sortable: false,
           width: 130
       }
       , {
           field: 'AccntType',
           displayName: '车主类型',
           sortable: false,
           width: 130
       }, {
           field: 'Job',
           displayName: '认证时间',
           sortable: false,
           width: 130
       }
        , {
            field: 'Age',
            displayName: '年龄',
            sortable: false,
            width: 130
        }
        , {
            field: 'Gender',
            displayName: '性别',
            sortable: false,
            width: 80
        }, {
            field: 'City',
            displayName: '城市',
            sortable: false,
            width: 100

        }
        , {
            field: 'Area',
            displayName: '地区',
            sortable: false,
            width: 100
        }, {
            field: 'AuthenticationSource',
            displayName: '认证渠道',
            sortable: false,
            width: 100
        }
        //, {
        //    field: 'VIN',
        //    displayName: '车架号',
        //    sortable: false,
        //    width: 100
        //}
        ],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.Frozen = function (row) {
        if (confirm("确定要冻结此用户？")) {
            $http.post('/Membership/FrozenMembership', { id: row.entity.Id }).success(function (data) {
                if (data.success) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    alert("操作成功！ \n");
                } else {
                    alert("操作失败！ \n" + data.msg);
                }
            }).error(function (data) {
                alert("操作失败！ \n" + data.msg);
            });
        }
    };

    $scope.UserStatusActive = function (row) {
        if (confirm("确定要激活此用户？")) {
            $http.post('/Membership/UserStatusActive', { id: row.entity.Id }).success(function (data) {
                if (data.success) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    alert("操作成功！ \n");
                } else {
                    alert("操作失败！ \n" + data.msg);
                }
            }).error(function (data) {
                alert("操作失败！ \n" + data.msg);
            });
        }
    };

    $scope.Delete = function (row) {
        if (confirm("确定要删除此用户？（用户相应积分会同时删除）")) {
            $http.post('/Membership/Delete', { id: row.entity.Id }).success(function (data) {
                if (data.success) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    alert("操作成功！ \n");
                } else {
                    alert("操作失败！ \n" + data.msg);
                }
            }).error(function (data) {
                alert("操作失败！ \n" + data.msg);
            });
        }
    };

    $scope.updateINPage = function (row) {
        $scope.selectItem.Id = row.entity.Id;
        $scope.selectItem.IdentityNumber = row.entity.IdentityNumber;
        $scope.selectItem.PhoneNumber = row.entity.PhoneNumber;
    }

    $scope.setInWorth = function (row) {
        $scope.selectItem.Id = row.entity.Id;
        $scope.selectItem.mlevel = row.entity.MLevel;

        $http.post("/Membership/FindWorth",
        {
            id: $scope.selectItem.Id,
        })
        .success(function (data) {
            $scope.selectItem.mintegral = data.Integral;
            // $scope.selectItem.mbluebean = data.BlueBean;
            //$scope.selectItem.mempiric = data.Empiric;
        })
        .error(function (data) {
            alert(data);
        });
    }

    $scope.saveWorth = function () {
        var mlevel = $scope.selectItem.mlevel;
        //var mbluebean = $scope.selectItem.mbluebean;
        //var mempiric = $scope.selectItem.mempiric;
        var mintegral = $scope.selectItem.mintegral;

        //if (mlevel == "-1" && mbluebean == "" && mempiric == "" && mintegral == "") {
        //    return;
        //}
        if (mlevel == "-1" && mintegral == "") {
            return;
        }

        $http.post("/Membership/SaveWorth",
        {
            id: $scope.selectItem.Id,
            mlevel: $scope.selectItem.mlevel,
            integral: $scope.selectItem.mintegral,
            //blueBean: $scope.selectItem.mbluebean,
            //empiric: $scope.selectItem.mempiric,
        })
        .success(function (data) {
            if (data.status == "success") {
                $scope.status = 3;
                alert("修改成功!");
                $('#SetWorthModal').modal('hide');

            }
            else
                alert(data.message);
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

        })
        .error(function (data) {
            alert(data);
        });
    };
    //修改手机号和用户名
    $scope.updatePhoneNumberModal = function () {
        $http.post("/Membership/updatePhoneNumberModal",
            {
                id: $scope.selectItem.Id,
                valideCode: $scope.PhoneValidateCode,
                phoneNumber: $scope.selectItem.PhoneNumber
            })
            .success(function (data) {
                if (data.success) {
                    if (data.success.Result) {
                        $scope.status = 3;
                        alert("修改成功!");
                    } else {
                        alert(data.msg);
                    }
                    $('#PhoneNumberModal').modal('hide');

                } else
                    alert(data.msg);
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize,
                    $scope.pagingOptions.currentPage,
                    $scope.filterOptions);

            })
            .error(function (data) {
                // alert(data);
            });
        $scope.PhoneValidateCode = "";
    };

    $scope.updateIdentityNumberBy4S = function () {
        $http.post("/Membership/UpdateIdentityNumberBy4S",
        {
            id: $scope.selectItem.Id,
            valideCode: $scope.PhoneValidateCode,
            phoneNumber: $scope.selectItem.PhoneNumber,
            identityNumber: $scope.selectItem.IdentityNumber
        })
        .success(function (data) {
            if (data.success) {
                $scope.status = 3;
                //alert("修改成功!");
                alert(data.msg);
                $('#IdentityNumberModal').modal('hide');

            }
            else
                alert(data.msg);
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

        })
        .error(function (data) {
            alert(data);
        });
        $scope.PhoneValidateCode = "";
    };

    $scope.updatePassword = function () {

        var pw = $scope.selectItem.password;

        if (pw == "" || pw.length < 8 || pw.length > 20) {
            alert('新密码长度为8至20位'); return;
        }

        $http.post("/Membership/UpdatePassword",
       {
           id: $scope.selectItem.Id,
           valideCode: $scope.PhoneValidateCode,
           phoneNumber: $scope.selectItem.PhoneNumber,
           password: $scope.selectItem.password
       })
       .success(function (data) {
           if (data.code == "200") {
               $scope.status = 3;
               alert("修改成功!");
               $scope.selectItem.password = "";
               $('#UserPasswordModal').modal('hide');

           }
           else
               alert(data.msg);
           $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

       })
       .error(function (data) {
           alert(data);
       });
        $scope.PhoneValidateCode = "";
    };

    //****************4S店下拉列表********************
    $scope.provinceDw = { select: -1 };
    $scope.cityDw = { select: -1 };
    $scope.dealerDw = { select: -1 };//4s店
    $http.get('/Common/GetDealerProvinceListJsonResult').success(function (data) {
        $scope.provinceDw.data = data;
        $scope.provinceChange();
        $scope.cityChange();
    });

    $scope.provinceChange = function () {
        $scope.myOptions = undefined;
        $http.get('/Common/GetDealerCityListJsonResult', { params: { province: $scope.provinceDw.select } }).success(function (data) {
            $scope.cityDw.select = -1;
            $scope.cityDw.data = data;
            $scope.cityChange();
        });
    };
    $scope.myOptions = $scope.dealerDw[0];
    $scope.cityChange = function () {
        $scope.myOptions = undefined;
        $http.get('/Common/GetDealerListJsonResult', { params: { province: $scope.provinceDw.select, city: $scope.cityDw.select } }).success(function (data) {
            $scope.dealerDw.select = -1;
            $scope.dealerDw.data = data;
        });
    };
    //****************4S店下拉列表********************

});

membershipModule.controller('MembershipCreateCtrl', function ($scope, $http, $routeParams) {
    $scope.newMembership = {
        IsCanClick: false,
        Agree: true
    };

    $scope.isGenerateNickName = false;
    //$scope.nickName = {};
    //$scope.generateNick = function () {

    //    var url = "/Membership/GetNickName";

    //    $http.get(url).success(function (data) {
    //        if (data.success) {
    //            $scope.nickName = data.Name;
    //            $scope.newMembership.NickName = data.Name.NickName;
    //        } else {

    //        }
    //    }).error(function (msg) {
    //        alert(msg);
    //    });
    //}
    $scope.test = function () {

        $scope.mySwitch = true;

        setTimeout(function () {

            var url = "/ValidateCode/Send";

            $http.post(url, $scope.newMembership).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.mySwitch = false;
                    alert("验证码已发送");
                } else {
                    $scope.mySwitch = false;
                    alert(largeLoad.Message);
                }
            });

        }, 100);
    }

   

    $scope.sendValideCode = function () {
        $scope.mySwitch = true;
        var url = "/ValidateCode/Send";

        $http.post(url, { PhoneNumber: $scope.newMembership.PhoneNumber }).success(function (largeLoad) {
            if (largeLoad.IsSuccess) {
                $scope.mySwitch = false;
                alert("验证码已发送");
            } else {
                $scope.mySwitch = false;
                alert(largeLoad.Message);
            }
        });
    };

    //验证即将入会人员手机号和身份证号是否已经注册过
    $scope.checkNumber = function () {
        $scope.newMembership.IsCanClick = true;
        $http.post('/ValidateCode/checkUserIsOf', $scope.newMembership).success(function (data) {
            if (data.success) {
                //$scope.newMembership = {};
                //$scope.nickName = {};
                $scope.newMembership.IsCanClick =false;
                $scope.newMembership.Agree = true;
                //alert(data.msg);
            } else {

                $scope.newMembership.IsCanClick = false;
                alert(data.msg);
            }
        }).error(function (data) {
            $scope.newMembership.IsCanClick = false;
            alert("未知错误");
        });

    }

    $scope.create = function () {
        $scope.newMembership.IsCanClick = true;
        // $scope.newMembership.NickName = $scope.nickName.NickName;
        $http.post('/Membership/CreateMembership', $scope.newMembership).success(function (data) {
            if (data.success) {
                $scope.newMembership = {};
                //$scope.nickName = {};
                $scope.newMembership.IsCanClick = false;
                $scope.newMembership.Agree = true;
                alert(data.msg);
            } else {
                $scope.newMembership.IsCanClick = false;
                alert("添加会员失败！ \n" + data.msg);
            }
        }).error(function (data) {
            $scope.newMembership.IsCanClick = false;
            alert("添加会员失败！ \n" + data.msg);
        });
    };   
});

membershipModule.controller('MembershipActiveCtrl', function ($scope, $http, $routeParams) {
    $scope.newMembership = {};

    $scope.sendValideCode = function () {
        $scope.mySwitch = true;
        setTimeout(function () {
            var url = "/ValidateCode/Send";

            $http.post(url, { PhoneNumber: $scope.newMembership.PhoneNumber }).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.mySwitch = false;
                    alert("验证码已发送");
                } else {
                    $scope.mySwitch = false;
                    alert(largeLoad.Message);
                }
            });

        }, 5000);
    };

    $scope.active = function () {
        $http.post('/Membership/UserWithoutCarActive', $scope.newMembership).success(function (data) {
            if (data.success) {
                $scope.newMembership = {};
                alert(data.msg);
            } else {
                alert("激活会员失败！ \n" + data.msg);
            }
        }).error(function (data) {
            alert("激活会员失败！ \n" + data.msg);
        });
    };
});

membershipModule.controller('MembershipDetailCtrl', function ($scope, $http, $routeParams) {
    $scope.isNotMembership = true;

    $scope.tabSelection = "user";
    //基本信息
    $scope.membershipDetail = {};

    $http.get('/Membership/GetMembershipDetail', { params: { id: $routeParams.id } }).success(function (data) {
        $scope.membershipDetail = data;
        if (data.IsPay == 1 ||
            (data.No != null && data.No != ""))
            $scope.isNotMembership = false;
    });
    
    $http.get('/Membership/GetMembershipCars', { params: { id: $routeParams.id } }).success(function (data) {
        $scope.cars = data;
    }).error(function (data) {
        alert("操作失败！ \n" + data.msg);
    });

    $scope.tabChange = function (sel) {
        $scope.tabSelection = sel;
    };

    $scope.active = function () {
        if (confirm("确定此用户已缴纳100元入会费, 并激活为索九会员?")) {
            $http.post('/Membership/MembershipActive', { id: $routeParams.id }).success(function (data) {
                if (data.success) {
                    $scope.isNotMembership = false;
                    alert("操作成功");
                } else {
                    alert("操作失败！ \n" + data.msg);
                }
            }).error(function (data) {
                alert("操作失败！ \n" + data.msg);
            });
        }
    };
});

membershipModule.controller('MembershipServiceListCtrl', function ($scope, $http, $routeParams) {
    //服务记录
    $scope.serviceTypeDw = { select: -1 };

    $http.get('/Common/GetServiceTypeJsonResult').success(function (data) {
        $scope.serviceTypeDw.data = data;
    });

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
        $scope.filterOptions.serviceType = $scope.serviceTypeDw.select;
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
            var eurl = "/Membership/GetServiceList";
            $http.get(eurl, { params: { userid: $routeParams.id, serviceType: filter.serviceType, Skip: (page - 1) * pageSize, Count: pageSize } }).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            });

        }, 100);
    };

    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
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
            field: 'ServiceTypeValue',
            displayName: '服务类型',
            sortable: false,
            width: 100
        }, {
            field: 'ReserveType',
            displayName: '预约方式',
            sortable: false,
            width: 200
        }, {
            field: 'RepairTime',
            displayName: '服务开始时间',
            sortable: false,
            width: 120
        }, {
            field: 'FinishTime',
            displayName: '服务结束时间',
            sortable: false,
            width: 120
        }, {
            field: 'Status',
            displayName: '服务状态',
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
//凭证审核
//membershipModule.controller('MembershipApproveProofCtrl', function ($scope, $http, $routeParams) {


//    //$http.get('/Membership/GetProofInfo', { params: { id: $routeParams.id } }).success(function (data) {
//    //    $scope.ProofInfo = data;
//    //});
//    //var eurl = "/Membership/GetProofInfo";
//    //$scope.getPagedDataAsync = function () {
//    //    setTimeout(function () {
//    $scope.ProofData = {};
//    var data;
//    var eurl = "/Membership/GetProofInfo";
//    $http.get(eurl, { params: { userid: $routeParams.id } }).success(function (data) {
//        $scope.ProofData.ImageProofFront = data.ImageProofFront;
//        $scope.ProofData.ImageProofVerso = data.ImageProofVerso;
//        $scope.ProofData.ImageProofByHand = data.ImageProofByHand;
//        $scope.ProofData.ApproveStatus = data.ApproveStatus;
//    });

//    //    }, 100);
//    //};
//    //$scope.getPagedDataAsync();

//    //更改凭证状态
//    $scope.UpdateStuatus = function () {

//        if (window.confirm("是否确定更改凭证审核状态?")) {
//            $http.post("/Membership/UpdateProofStatus",
//                    { id: $routeParams.id })
//                    .success(function (data) {
//                        if (data.success) {
//                            alert("更改成功!");
//                            $('#UpdateStuatusbtn').hide();

//                        }
//                        else
//                            alert(data.msg);

//                    });
//        }
//    };
    //列表绑定数据
    //$scope.gridOptions = {
    //    i18n: 'zh-cn',
    //    data: 'myData',
    //    rowTemplate: '<div style="height: 100%"><div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell ">' +
    //        '<div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }"> </div>' +
    //        '<div ng-cell></div>' +
    //        '</div></div>',
    //    multiSelect: false,
    //    enableCellSelection: false,
    //    enableRowSelection: true,
    //    enableCellEdit: false,
    //    enablePinning: false,
    //    columnDefs: [{
    //        field: 'action',
    //        displayName: '缩略图',
    //        enableCellEdit: false,
    //        sortable: false,
    //        width: 120,
    //        heigth: 80,
    //        cellTemplate: '<div><img ng-src="{{row.getProperty(\'ImageProofFront\')}}" style="width:100px;height:50px" ></div>'

    //    },
    //    {
    //        field: 'action',
    //        displayName: '缩略图',
    //        enableCellEdit: false,
    //        sortable: false,
    //        width: 120,
    //        heigth: 80,
    //        cellTemplate: '<div><img ng-src="{{row.getProperty(\'ImageProofVerso\')}}" style="width:100px;height:50px" ></div>'

    //    },
    //    {
    //        field: 'action',
    //        displayName: '缩略图',
    //        enableCellEdit: false,
    //        sortable: false,
    //        width: 120,
    //        heigth: 80,
    //        cellTemplate: '<div><img ng-src="{{row.getProperty(\'ImageProofByHand\')}}" style="width:100px;height:50px" ></div>'

    //    },
    //    {
    //        field: 'action',
    //        displayName: '操作',
    //        enableCellEdit: false,
    //        sortable: false,
    //        width: 180,
    //        cellTemplate: '<div>' +
    //            '<a href="javascript:return void()" ng-click="UpdateStuatus(row.getProperty(\'MembershipID\'))" ng-show="row.getProperty(\'ApproveStatus\')==0 || row.getProperty(\'ApproveStatus\')==1">确认</a> ' +
    //            '</div>'
    //    }, ],
    //}
//});

membershipModule.controller('MembershipScoreListCtrl', function ($scope, $http, $routeParams) {
    //积分明细
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
            var eurl = "/Membership/GetScoreList";
            $http.get(eurl, { params: { userid: $routeParams.id, Skip: (page - 1) * pageSize, Count: pageSize } }).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            });

        }, 100);
    };

    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
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
            field: 'CreateTimeStr',
            displayName: '积分变动时间',
            sortable: false,
            width: 130
        }, {
            field: 'value',
            displayName: '积分值',
            sortable: false,
            width: 200
        }, {
            field: 'remark',
            displayName: '积分变动原因',
            sortable: false,
            width: 300
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

membershipModule.controller('MembershipCardListCtrl', function ($scope, $http, $routeParams) {
    //卡券列表
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
            var eurl = "/Membership/GetCardList";
            $http.get(eurl, { params: { userid: $routeParams.id, Skip: (page - 1) * pageSize, Count: pageSize } }).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            });

        }, 100);
    };

    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
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
            field: 'SNCode',
            displayName: '卡券串码',
            sortable: false,
            width: 100
        }, {
            field: 'SendTime_Str',
            displayName: '获取时间',
            sortable: false,
            width: 200
        }, {
            field: 'StatusName',
            displayName: '状态',
            sortable: false,
            width: 100
        }, {
            field: 'UseTime',
            displayName: '使用时间',
            width: 150
        }, {
            field: '',
            displayName: '有效期',
            width: 150
        }, {
            field: 'SendTypeName',
            displayName: '获取方式',
            width: 150
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

membershipModule.controller('MembershipScheduleListCtrl', function ($scope, $http, $routeParams) {
    //预约记录
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
        //var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
        for (i = 0 ; i < data.length ; i++) {
            data[i].ScheduleDate = $scope.ChangeDateFormat(data[i].ScheduleDate);
        }

        $scope.myData = data;
        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };

    $scope.getPagedDataAsync = function (pageSize, page, filter) {
        setTimeout(function () {
            var data;
            var eurl = "/Membership/GetScheduleList";
            $http.get(eurl, { params: { userid: $routeParams.id, page: page, count: pageSize } }).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            });

        }, 100);
    };

    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
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
            field: 'ScheduleType',
            displayName: '预约类型',
            sortable: false,
            width: 150
        }, {
            field: 'ScheduleDate',
            displayName: '预约时间',
            sortable: false,
            width: 200
        }, {
            field: 'State',
            displayName: '受理状态',
            sortable: false,
            width: 100
        }, {
            field: 'UpdateTime',
            displayName: '受理时间',
            width: 150
        }, {
            field: 'UpdateName',
            displayName: '受理人',
            width: 150
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

membershipModule.controller('MembershipConsumeListCtrl', function ($scope, $http, $routeParams) {
    //消费记录
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
        //var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
        for (i = 0 ; i < data.length ; i++) {
            data[i].ConsumeDate = $scope.ChangeDateFormat(data[i].ConsumeDate);
        }


        $scope.myData = data;
        $scope.totalServerItems = total_count;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };

    $scope.getPagedDataAsync = function (pageSize, page, filter) {
        setTimeout(function () {
            var data;
            var eurl = "/Membership/GetConsumeList";
            $http.get(eurl, { params: { userid: $routeParams.id, page: page, Count: pageSize } }).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            });

        }, 100);
    };

    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
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
            displayName: '经销商',
            visible: false,
            width: 100
        }, {
            field: 'DealerName',
            displayName: '经销商',
            sortable: false,
            width: 100
        }, {
            field: 'ConsumeDate',
            displayName: '消费时间',
            sortable: false,
            width: 200
        }, {
            field: 'ScheduleOrderNo',
            displayName: '预约编号',
            sortable: false,
            width: 100
        }, {
            field: 'ConsumeTypeString',
            displayName: '消费类型',
            width: 150
        }, {
            field: 'PartCost',
            displayName: '配件费',
            width: 150
        }, {
            field: 'MaterialCost',
            displayName: '材料费',
            width: 150
        }, {
            field: 'LaborCost',
            displayName: '工时费',
            width: 150
        }, {
            field: 'PurchaseCost',
            displayName: '购车费',
            width: 150
        }, {
            field: 'PointCost',
            displayName: '积分抵扣',
            width: 150
        }, {
            field: 'TotalCost',
            displayName: '总费用',
            width: 150
        }, {
            field: 'ConsumePoints',
            displayName: '消耗积分',
            width: 150
        }, {
            field: 'RewardPoints',
            displayName: '产生积分',
            width: 150
        }, {
            field: 'ApproveStatus',
            displayName: '审查状态',
            width: 150
        }, {
            field: 'PointStatus',
            displayName: '积分发放状态',
            width: 150
        }, {
            field: 'Comment',
            displayName: '备注',
            width: 150
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };
});

membershipModule.controller('MembershipApprovingListCtrl', function ($scope, $http, $routeParams) {
    //消费记录
    $scope.filterOptions = {
        useExternalFilter: true,
        phoneNumber: $scope.phoneNumber,
        ApproveType: $scope.ApproveType,
        Status: $scope.Status,
        PayNumber: $scope.PayNumber
    };
    //$scope.ApproveType = 1;
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
            var eurl = "/Membership/GetMembershipApprovingList";
            $http.get(eurl, { params: { PayNumber: $scope.PayNumber, phoneNumber: $scope.phoneNumber, IdentityNumber: $scope.IdentityNumber, ApproveType: $scope.ApproveType, Status: $scope.Status, skip: (page - 1) * pageSize, count: pageSize, IsPay: $scope.IsPay, Amount: $scope.Amount, DealerId: $scope.DealerId, PaperWork: $scope.PaperWork, VINNumber: $scope.VINNumber } }).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count);
            });

        }, 100);
    };

    var flag = ($("#ce").val() != "" ? false : true);

    if (!flag) {
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    }

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
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
            field: 'MembershipId',
            displayName: 'MembershipId',
            sortable: false,
            visible: false,
            width: 100
        }, {
            field: 'action',
            displayName: '操作',
            width: 150,
            cellTemplate: '<a href="javascript:return void()" ng-show="row.getProperty(\'Status\')==1" style="color:green" ng-click="approval(row.getProperty(\'ApprovalId\'),row.getProperty(\'PhoneNumber\'),row.getProperty(\'SubmitTime\'))">通过认证</a>'
        }, {
            field: 'ApprovalId',
            displayName: 'ApprovalId',
            sortable: false,
            visible: false,
            width: 100
        }, {
            field: 'DealerId',
            displayName: '店代码',
            sortable: false,
            width: 150
        }, {
            field: 'PhoneNumber',
            displayName: '手机号',
            sortable: false,
            width: 150
        },
        {
            field: 'IdentityNumber',
            displayName: '证件号',
            sortable: false,
            width: 150
        },
        {
            field: 'Amount',
            displayName: '应付金额',
            sortable: false,
            width: 150
        },
        {
            field: 'RealName',
            displayName: '车主姓名',
            sortable: false,
            width: 150
        }
        //, {
        //    field: 'VIN',
        //    displayName: 'VIN码',
        //    sortable: false,
        //    width: 200
        //}
        , {
            field: 'IsPay',
            displayName: '是否已付费',
            sortable: false,
            width: 100
        }, {
            field: 'PayNumber',
            displayName: '付款码',
            sortable: false,
            width: 200
        }, {
            field: 'Status',
            displayName: '状态',
            visible: false,
            width: 100
        }, {
            field: 'StatusName',
            displayName: '状态',
            width: 150
        }
        , {
            field: 'SubmitTime',
            displayName: '申请提交时间',
            width: 150
        }
        ],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.approval = function (id, phone,SubmitTime) {
        if (window.confirm("确定此用户已缴纳入会费用并通过认证？")) {
            $http.post("/membership/ApprovalMembershipRequest", { id: id, phone: phone, SubmitTime: SubmitTime })
            .success(function (data) {
                if (data.success)
                    alert("操作成功!");
                else
                    alert("操作失败!" + data.msg);
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
            });
        }
    };

    $scope.reject = function (id) {
        if (window.confirm("确定要拒绝认证?")) {
            $http.post("/membership/RejectMembershipRequest", { id: id })
            .success(function (data) {
                if (data.success)
                    alert("操作成功!");
                else
                    alert("操作失败!" + data.msg);
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
            });
        }
    };
});
membershipModule.controller('MembershipRequestFailedCtrl', function ($scope, $http, $routeParams) {
    //呼叫中心会员认证失败列表
    $scope.status = "";
    $scope.selectItem = {};
    $scope.statuslist =
       [{
           id: 1,
           Name: '未处理'
       }, {
           id: 2,
           Name: '已处理'
       }];

    $scope.selected = 0;

    $scope.filterOptions = {
        useExternalFilter: true,
        status: $scope.selected,
        phone: $scope.phoneNumber
    };
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        currentPage: 1
    };

    $scope.search = function () {
        $scope.filterOptions.Status = $scope.selected;
        $scope.filterOptions.phone = $scope.phoneNumber;
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

        var url = "/Membership/GetApprovedFailedList?";

        if (filter && filter.Status != undefined) {
            url += "&status=" + filter.Status;
        }

        if (filter && filter.phone != undefined) {
            url += "&phonenumber=" + filter.phone;
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

    //修改处理状态
    $scope.updateStatus = function (id) {

        if (window.confirm("是否确定修改为已处理?")) {
            $http.post("/Membership/UpdateRequestStauts", { id: id })
                    .success(function (data) {
                        if (data.success) {
                            $scope.status = 2;
                            alert("修改成功!");
                        }
                        else {
                            alert(data.msg);
                        }
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    });
        }
    };
    $scope.updateINPage = function (row) {
        $scope.selectItem.UserId = row.entity.UserId;
        $scope.selectItem.IdentityNumber = row.entity.IdentityNumber;
    };
    $scope.updateIdentityNumber = function () {
        $http.post("/Membership/UpdateIdentityNumber",
                   {
                       id: $scope.selectItem.UserId,
                       identityNumber: $scope.selectItem.IdentityNumber
                   })
                   .success(function (data) {
                       if (data.success) {
                           $scope.status = 2;
                           alert("修改成功!");
                           $('#IdentityNumberModal').modal('hide');

                       }
                       else
                           alert(data.msg);
                       $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                   });
    };

    //$scope.updateIdentityNumber = function (id) {

    //    if (window.confirm("是否确定重新激活?")) {
    //        $http.post("/Membership/Activate",
    //                { id: id })
    //                .success(function (data) {
    //                    if (data.success) {
    //                        alert("修改成功!");
    //                    }
    //                    else
    //                        alert(data.msg);
    //                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

    //                });
    //    }
    //}

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
            displayName: 'ID',
            sortable: false,
            visible: false,
            width: 100
        }, {
            field: 'action',
            displayName: '操作',
            width: 200,
            cellTemplate: '<div><a ng-show="row.entity.Status==1" href="javascript:return void()" ng-click="updateStatus(row.getProperty(\'UserId\'))">标记为已处理</a>  '
                + '<label  ng-show="row.entity.Status==2">已处理</label>'
                + '<a  ng-show="row.entity.Status==1" href="javascript:return void()" data-toggle="modal" data-target="#IdentityNumberModal" ng-click="updateINPage(row)">修改身份证</a> '
                + '</div>'
        }, {
            field: 'UserId',
            displayName: '申请人ID',
            visible: false,
            width: 100
        }, {
            field: 'UserName',
            displayName: '申请人',
            sortable: false,
            width: 150
        }, {
            field: 'RequestTime',
            displayName: '申请时间',
            sortable: false,
            width: 150
        }, {
            field: 'VIN',
            displayName: 'VIN',
            visible: false,
            width: 200
        }, {
            field: 'IdentityNumber',
            displayName: '身份证号',
            sortable: false,
            width: 200
        }, {
            field: 'IsPay',
            displayName: '是否已付费',
            sortable: false,
            width: 100
        }, {
            field: 'PayNumber',
            displayName: '付款码',
            sortable: false,
            width: 200
        }, {
            field: 'CarCategory',
            displayName: '车型',
            sortable: false,
            width: 200
        }, {
            field: 'Status',
            displayName: '处理状态',
            sortable: false, visible: false,
            width: 50
        }, {
            field: 'StatusValue',
            displayName: '处理状态',
            sortable: false,
            width: 100
        }, {
            field: 'OperationTime',
            displayName: '处理时间',
            sortable: false,
            width: 140
        }, {
            field: 'Operator',
            displayName: '处理人',
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

membershipModule.controller('MembershipCreateNormalCtrl', function ($scope, $http, $routeParams) {
    $scope.newMembership = {
        IsCanClick: false
    };

    //发送手机验证码
    $scope.clickSendValidateCode = function () {
        $scope.mySwitch = true;

        setTimeout(function () {

            var url = "/ValidateCode/Send";

            $http.post(url, $scope.newMembership).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.mySwitch = false;
                    alert("验证码已发送");
                } else {
                    $scope.mySwitch = false;
                    alert(largeLoad.Message);
                }
            });

        }, 100);
    }

    $scope.isGenerateNickName = false;
    $scope.nickName = {};
    $scope.generateNick = function () {
        var url = "/Membership/GetNickName";

        $http.get(url).success(function (data) {
            if (data.success) {
                $scope.nickName = data.Name;
                $scope.newMembership.NickName = data.Name.NickName;
            } else {

            }
        }).error(function (msg) {
            alert(msg);
        });
    }
    //test


    $scope.create = function () {
        $scope.newMembership.IsCanClick = true;
        $scope.newMembership.NickName = $scope.nickName.NickName;
        $http.post('/Membership/CreateMembershipNormal', $scope.newMembership).success(function (data) {
            if (data.success) {
                $scope.newMembership = {};
                $scope.nickName = {};
                $scope.newMembership.IsCanClick = false;
                alert(data.msg);
            } else {
                $scope.newMembership.IsCanClick = false;
                alert("添加会员失败！ \n" + data.msg);
            }
        }).error(function (data) {
            $scope.newMembership.IsCanClick = false;
            alert("添加会员失败！ \n" + data.msg);
        });
    };
});