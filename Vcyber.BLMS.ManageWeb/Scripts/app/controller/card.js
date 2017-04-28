var card = angular.module('card', ['ngGrid', 'angularFileUpload']);
card.controller('CardCtrl', function ($scope, $http, $routeParams, $location, $upload) {
    $scope.formData = {};
    $scope.selectItem = {};
    $scope.status = new Array();// 状态
    $scope.select = 0;
    $scope.selectCallCenter = "";

    $scope.airports = new Array();//机场
    $scope.airport = "";//机场编号

    $scope.airportRooms = new Array();//机场候机室
    $scope.airportRoom = 0;//机场候机室编号

    $scope.colDefs = [];

    $scope.filterOptions = {
        useExternalFilter: true
    };

    //发送手机验证码
    $scope.clickSendValidateCode = function () {
        $scope.mySwitch = true;
        setTimeout(function () {

            var url = "/ValidateCode/Send";

            $http.post(url, $scope.formData).success(function (largeLoad) {
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

    //加载状态列表
    $scope.load = function () {
        setTimeout(function () {
            var url = "/Card/CardStateList";

            $http.post(url).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.status = largeLoad.Data.status;
                    $scope.airports = largeLoad.Data.airports;
                }
            });
        }, 100);
    }

    $scope.airportChange = function () {
        setTimeout(function () {
            var url = "/Card/AirportRoomList";
            var postData = { airportName: $scope.airport };

            $http.post(url, postData).success(function (largeLoad) {
                if (largeLoad.IsSuccess) {
                    $scope.airportRooms = largeLoad.Data;
                }
            });
        }, 100);
    }

    $scope.totalServerItems = 0;

    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        phoneNumber: null,
        state: 1,
        iscallcenter: "",
        start: null,
        end: null,
        currentPage: 1
    };

    $scope.search = function () {
        $scope.filterOptions.phoneNumber = $scope.formData.PhoneNumber;
        $scope.filterOptions.state = $scope.select;
        $scope.filterOptions.iscallcenter = $scope.selectCallCenter;
        $scope.filterOptions.start = $scope.formData.Start;
        $scope.filterOptions.end = $scope.formData.End;
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

    $scope.setPagingData = function (data, page, pageSize, total_count, count) {
        //var pagedData = data.slice((page - 1) * pageSize, page * pageSize);

        for (i = 0 ; i < data.length ; i++) {
            data[i].CreateTime = $scope.ChangeDateFormat(data[i].CreateTime);
            data[i].SendTime = $scope.ChangeDateFormat(data[i].SendTime);
            data[i].UseTime = $scope.ChangeDateFormat(data[i].UseTime);
        }




        $scope.myData = data;

        $scope.totalServerItems = total_count;
        $scope.Total = count.Total;
        $scope.NoSend = count.NoSend;
        if (!$scope.$$phase) {
            $scope.$apply();
        }

    };

    $scope.getPagedDataAsync = function (pageSize, page, filter) {
        setTimeout(function () {
            var data;
            var url = "/Card/CardList?";
            var postData = {};
            if (filter && filter.phoneNumber != undefined) {
                url += "&phoneNumber=" + filter.phoneNumber;
                postData.phoneNumber = filter.phoneNumber;
            }

            if (filter && filter.state != undefined) {
                url += "&state=" + filter.state;
                postData.state = filter.state;
            }

            if (filter && filter.iscallcenter != undefined) {
                url += "&iscallcenter=" + filter.iscallcenter;
                postData.iscallcenter = filter.iscallcenter;
            }

            if (filter && filter.start != undefined) {
                url += "&start=" + filter.start;
                postData.start = filter.start;
            }

            if (filter && filter.end != undefined) {
                url += "&end=" + filter.end;
                postData.end = filter.end;
            }
            if (page != null && page != undefined) {
                postData.pageindex = page;
            }
            if (pageSize != null && pageSize != undefined) {
                postData.pageSize = pageSize;
            }

            //$http.post(url, postData).success(function (largeLoad) {
            //    $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count, largeLoad.count);
            //});

            $http.get(url + '&pageindex=' + page + '&pagesize=' + pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count, largeLoad.count);
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
        columnDefs: [
        {
            field: 'Id ',
            displayName: '编号',
            sortable: false,
            width: 50
        }//,{
        //    field: 'IsCallCenter',
        //    displayName: '是否4',
        //    sortable: false,
        //    width: 120,
        //    cellTemplate: '<span></span>'
        //}
        , {
            field: 'PhoneNumber ',
            displayName: '领取用户',
            sortable: false,
            width: 120
        }, {
            field: 'IdentityNumber',
            displayName: '身份证',
            sortable: false,
            width: 180
        }, {
            field: 'SNCode',
            displayName: '卡号',
            sortable: false,
            width: 100
        }, {
            field: 'StatusName',
            displayName: '状态',
            sortable: false,
            width: 80
        }, {
            field: 'CreateTime',
            displayName: '创建时间',
            sortable: false,
            width: 140
        }, {
            field: 'SendTime',
            displayName: '发放时间',
            sortable: false,
            width: 140
        }, {
            field: 'UseTime',
            displayName: '使用时间',
            sortable: false,
            width: 140
        }, {
            field: 'UseAdd',
            displayName: '使用机场',
            sortable: false,
            width: 400
        }, {
            field: 'Status',
            displayName: '操作',
            sortable: false,
            width: 120,
            cellTemplate: '<span>&nbsp&nbsp</span><a href="javascript:void()" ng-show="row.entity.Status==1" data-toggle="modal" data-target="#cardSendModal" ng-click="sendPage(row)">发放</a><a href="javascript:void()" ng-show="row.entity.Status==2" ng-click="sendPageAgin(row)">重发短信</a>'
        }, {
            field: 'AirportName',
            displayName: '预约机场',
            sortable: false,
            width: 160
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.sendPage = function (row) {
        $scope.selectItem.Id = row.entity.Id;
        $scope.selectItem.Code = row.entity.SNCode;
    }

    $scope.sendPageAgin = function (row) {
        $http.post("/Card/SendCardSMS",
                       {
                           code: row.entity.SNCode
                       })
                       .success(function (data) {
                           if (data.IsSuccess) {
                               alert("重新发送短信成功!");
                           }
                           else
                               alert(data.Message);
                       });
    }

    $scope.SendCard = function () {
        if ($scope.airportRoom == 0) {
            alert("请选择机场候机室");
        } else {
            $http.post("/Card/Send",
                       {
                           id: $scope.selectItem.Id,
                           code: $scope.selectItem.Code,
                           phonenumber: $scope.selectItem.PhoneNumber,
                           truephonenumber: $scope.selectItem.TruePhoneNumber,
                           airportId: $scope.airportRoom
                       })
                       .success(function (data) {
                           if (data.IsSuccess) {
                               alert("发放成功!");
                               $('#cardSendModal').modal('hide');
                               $scope.selectItem.PhoneNumber = "";
                           }
                           else
                               alert(data.Message);
                           $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);

                       });
        }
    }

    $scope.upload = function (files) {
        if (files && files.length) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                $upload.upload({
                    url: '/Scripts/ueditor/net/controller.ashx?action=uploadfile',
                    file: file,
                    fileFormDataName: 'upfile'
                }).progress(function (evt) {
                    var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                    console.log('progress: ' + progressPercentage + '% ' +
                                evt.config.file.name);
                }).success(function (data, status, headers, config) {

                    $scope.selectItem.file = data.url;
                    //console.log('file ' + config.file.name + 'uploaded. Response: ' +
                    //            JSON.stringify(data));
                }).error(function (err) {
                    alert('上传失败');
                });
            }
        }
    };

    $scope.ImportCard = function (phone) {
        debugger;
        var i = "";
        var url = "/Card/Import";
        if (phone != undefined && phone == "phone") {
            url = "/Card/ImportPhone";
            i = 1;
        }
        $http.post(url,
                   {
                       path: $scope.selectItem.file
                   })
                   .success(function (data) {
                       if (data.IsSuccess) {
                           alert("导入成功!");
                           $('#cardImportModal'+i).modal('hide');
                           $scope.selectItem.file = "";
                       }
                       else
                           alert(data.Message);
                       $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                   });
    }

    $scope.Export = function () {
        setTimeout(function () {
            $scope.filterOptions.phoneNumber = $scope.formData.PhoneNumber;
            $scope.filterOptions.state = $scope.select;

            var data;
            var url = "/Card/Export?";
            if ($scope.filterOptions && $scope.filterOptions.phoneNumber != undefined) {
                url += "&phoneNumber=" + $scope.filterOptions.phoneNumber;
            }

            if ($scope.filterOptions && $scope.filterOptions.state != undefined) {
                url += "&state=" + $scope.filterOptions.state;
            }

            $http.get(url).success(function (largeLoad) {
                alert("导出完成!");
            });

        }, 100);
    }


    $scope.$watch('myData', function () {

    });

});

card.controller('AirportCtrl', function ($scope, $http, $routeParams, $location, $upload) {
    //查询参数
    $scope.formData = {
        Province: "",
        City: "",
        Airport: ""
    };

    $scope.addPlanData = {};

    $scope.editPlanData = {};

    $scope.provinces = new Array();// 省份

    //加载省份列表
    $scope.load = function () {
        setTimeout(function () {
            var url = "/Card/AirportProvinceList";

            $http.post(url).success(function (largeLoad) {
                $scope.provinces = largeLoad;
            });
        }, 100);
    }

    $scope.provinceChange = function () {
        setTimeout(function () {
            var url = "/Card/AirportCityList";
            var postData = { province: $scope.formData.Province };

            $http.post(url, postData).success(function (largeLoad) {
                $scope.citys = largeLoad;
                $scope.formData.City = null;
                $scope.formData.Airport = null;
                $scope.airports = null;
            });
        }, 100);
    }

    $scope.cityChange = function () {
        setTimeout(function () {
            var url = "/Card/SelectAirportList";
            var postData = { province: $scope.formData.Province, city: $scope.formData.City };

            $http.post(url, postData).success(function (largeLoad) {
                $scope.airports = largeLoad;
                $scope.formData.Airport = null;
            });
        }, 100);
    }

    //定义字段列
    $scope.colDefs = [];

    //定义筛选条件
    $scope.filterOptions = {
        useExternalFilter: true
    };

    $scope.totalServerItems = 0;

    $scope.pagingOptions = {
        pageSizes: [10, 50, 100],
        pageSize: 10,
        province: null,
        city: null,
        airport: null,
        currentPage: 1
    };

    $scope.search = function () {
        $scope.filterOptions.province = $scope.formData.Province;
        $scope.filterOptions.city = $scope.formData.City;
        $scope.filterOptions.airport = $scope.formData.Airport;
        $scope.pagingOptions.currentPage = 1;
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
    };

    $scope.setPagingData = function (data, page, pageSize, total_count, count) {
        $scope.myData = data;
        $scope.totalServerItems = total_count;

        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };

    $scope.getPagedDataAsync = function (pageSize, page, filter) {
        setTimeout(function () {
            var data;
            var url = "/Card/SelectAirportRoomList?";
            if (filter && filter.province != undefined) {
                url += "&province=" + filter.province;
            }

            if (filter && filter.city != undefined) {
                url += "&city=" + filter.city;
            }

            if (filter && filter.airport != undefined) {
                url += "&airport=" + filter.airport;
            }

            $http.get(url + '&pageindex=' + page + '&pagesize=' + pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count, largeLoad.count);
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
        columnDefs: [
        {
            field: 'Id ',
            displayName: '编号',
            sortable: false,
            width: 50
        }, {
            field: 'Id',
            displayName: '操作',
            sortable: false,
            width: 80,
            cellTemplate: '<span>&nbsp&nbsp</span><a href="javascript:void()" data-toggle="modal" data-target="#editPlanModal" ng-click="edit(row)">编辑</a><span>&nbsp&nbsp</span><a href="javascript:void()" ng-click="Delete(row)">删除</a>'
        }, {
            field: 'Province ',
            displayName: '省份',
            sortable: false,
            width: 100
        }, {
            field: 'City',
            displayName: '城市',
            sortable: false,
            width: 80
        }, {
            field: 'AirportName',
            displayName: '机场名称',
            sortable: false,
            width: 180
        }, {
            field: 'AirportRoomType',
            displayName: '候机室类型',
            sortable: false,
            width: 150
        }, {
            field: 'AirportRoomName',
            displayName: '候机室名称',
            sortable: false,
            width: 220
        }, {
            field: 'AirportAddress',
            displayName: '候机室地址',
            sortable: false,
            width: 1000
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.Delete = function (row) {
        if (confirm("确定要删除吗？")) {
            $http.post('/Card/DeleteAirport',
                    {
                        Id: row.entity.Id
                    }
                ).success(function (data) {
                    if (data.IsSuccess) {
                        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                        alert("操作成功！ \n");
                    } else {
                        alert("操作失败！ \n" + data.msg);
                    }
                }).error(function (data) {
                    alert("操作失败！ \n" + data.msg);
                });
        }
    }

    $scope.addPlanSave = function () {
        if ($scope.addPlanData.Province == "" || $scope.addPlanData.Province == undefined) {
            alert("请输入省份");
            return false;
        } else if ($scope.addPlanData.City == "" || $scope.addPlanData.City == undefined) {
            alert("请输入城市");
            return false;
        } else if ($scope.addPlanData.AirportName == "" || $scope.addPlanData.AirportName == undefined) {
            alert("请输入机场名称");
            return false;
        } else if ($scope.addPlanData.AirportRoomType == "" || $scope.addPlanData.AirportRoomType == undefined) {
            alert("请输入候机室类型");
            return false;
        } else if ($scope.addPlanData.AirportRoomName == "" || $scope.addPlanData.AirportRoomName == undefined) {
            alert("请输入候机室名称");
            return false;
        } else if ($scope.addPlanData.AirportAddress == "" || $scope.addPlanData.AirportAddress == undefined) {
            alert("请输入候机室地址");
            return false;
        } else {
            $http.post('/Card/AddAirport',
                {
                    Province: $scope.addPlanData.Province,
                    City: $scope.addPlanData.City,
                    AirportName: $scope.addPlanData.AirportName,
                    AirportRoomType: $scope.addPlanData.AirportRoomType,
                    AirportRoomName: $scope.addPlanData.AirportRoomName,
                    AirportAddress: $scope.addPlanData.AirportAddress
                }
            ).success(function (data) {
                if (data.IsSuccess) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    alert("操作成功！ \n");
                } else {
                    alert("操作失败！ \n" + data.msg);
                }
            }).error(function (data) {
                alert("操作失败！ \n" + data.msg);
            });

            $scope.addPlanData = {

            };
        }
    }

    $scope.editPlanSave = function () {
        if ($scope.editPlanData.Province == "" || $scope.editPlanData.Province == undefined) {
            alert("请输入省份");
            return false;
        } else if ($scope.editPlanData.City == "" || $scope.editPlanData.City == undefined) {
            alert("请输入城市");
            return false;
        } else if ($scope.editPlanData.AirportName == "" || $scope.editPlanData.AirportName == undefined) {
            alert("请输入机场名称");
            return false;
        } else if ($scope.editPlanData.AirportRoomType == "" || $scope.editPlanData.AirportRoomType == undefined) {
            alert("请输入候机室类型");
            return false;
        } else if ($scope.editPlanData.AirportRoomName == "" || $scope.editPlanData.AirportRoomName == undefined) {
            alert("请输入候机室名称");
            return false;
        } else if ($scope.editPlanData.AirportAddress == "" || $scope.editPlanData.AirportAddress == undefined) {
            alert("请输入候机室地址");
            return false;
        } else {
            $http.post('/Card/UpdateAirport',
                {
                    Id: $scope.editPlanData.Id,
                    Province: $scope.editPlanData.Province,
                    City: $scope.editPlanData.City,
                    AirportName: $scope.editPlanData.AirportName,
                    AirportRoomType: $scope.editPlanData.AirportRoomType,
                    AirportRoomName: $scope.editPlanData.AirportRoomName,
                    AirportAddress: $scope.editPlanData.AirportAddress
                }
            ).success(function (data) {
                if (data.IsSuccess) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    alert("操作成功！ \n");
                } else {
                    alert("操作失败！ \n" + data.msg);
                }
            }).error(function (data) {
                alert("操作失败！ \n" + data.msg);
            });

            $scope.editPlanData = {

            };
        }
    }

    $scope.edit = function (row) {
        $scope.editPlanData = {

        };

        $scope.editPlanData.Id = row.entity.Id;
        $scope.editPlanData.Province = row.entity.Province;
        $scope.editPlanData.City = row.entity.City;
        $scope.editPlanData.AirportName = row.entity.AirportName;
        $scope.editPlanData.AirportRoomType = row.entity.AirportRoomType;
        $scope.editPlanData.AirportRoomName = row.entity.AirportRoomName;
        $scope.editPlanData.AirportAddress = row.entity.AirportAddress;
    }

});