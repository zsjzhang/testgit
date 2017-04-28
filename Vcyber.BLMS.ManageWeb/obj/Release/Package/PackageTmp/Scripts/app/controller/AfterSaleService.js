var afterSaleService = angular.module('afterSaleService', ['ngGrid']);
afterSaleService.controller('AfterSaleServiceCtrl', function ($scope, $http, $routeParams) {

    //记录验证状态，只有卡劵号和VIN都验证成功，才能核销
    $scope.isCardTrue = false;
    $scope.isVINTrue = false;
    //YC活动
   
    $scope.data = {

    };

    $scope.cardTypes = new Array();// 卡劵类型

    var cardTypes38 = [{ activity: "三八活动", cardid: "pmXbSjvHWuRNfjXKz9PHnGSN4H6k", cardtitle: "4L机油兑换券" }
        , { activity: "三八活动", cardid: "pmXbSjl4yW3dzOJ6PlrsZx_ZOYFo", cardtitle: "4L机油8.5折兑换券" }
        , { activity: "三八活动", cardid: "pmXbSjlxGUgln7h2Fu0y4xBlK9kw", cardtitle: "基础保养体验券" }];

    var cardTypesSpring = [{ activity: "春季免检活动", cardid: "pmXbSjlRlUKd7XfMN2xgq-DcUdKU", cardtitle: "4L机油8.5折代金券" }
        , { activity: "春季免检活动", cardid: "pmXbSjv9vwzLD8eFXStW0BbHEk5w", cardtitle: "1L机油兑换券" }
        , { activity: "春季免检活动", cardid: "pmXbSjvzf3nS_BHJvXd1N_4ngJVI", cardtitle: "2瓶玻璃水兑换券" }
        , { activity: "春季免检活动", cardid: "pmXbSjh59fOta8gbh-nYBZB1RXSk", cardtitle: "基础保养兑换券" }];

    var carTypeLingDong = [
         { activity: "1瓶玻璃水兑换券", cardid: "pmXbSjrPnm-nqaOM7WUs9WNKRhTM", cardtitle: "1瓶玻璃水兑换券" }
    ];

    var carshow = [
         { activity: "北京车展", cardid: "bolishui", cardtitle: "玻璃水兑换券" },
           { activity: "北京车展", cardid: "jichubaoyang", cardtitle: "基础保养兑换券" }
    ];

    $scope.load = function () {
        var activityType = $("#selActivityType");
        var url = "/CustomCard/GetSCServiceCardTypeList";
        $.ajax({
            url: url,
            type: 'Get',
            data: { type: 1, source: 0, iswx: 1 },
            success: function (data) {
                activityType.empty();
                var option = $("<option>").text("请选择").val("");
                activityType.append(option);
                if (data != null) {
                    $.each(data, function (i, n) {
                        var actType = "";
                        var actName = n;
                        if (n == "北京车展") {
                            actType = "carshow";
                        }
                        else if (n == "三八活动") {
                            actType = "Y";
                        }
                        else if (n == "春季免检活动") {
                            actType = "spring";
                        }
                        else if (n == "1瓶玻璃水兑换券") {
                            actName = "领动新车上市";
                            actType = "lingdong";
                        } else {
                            actType = n;
                        }
                        var option = $("<option>").text(actName).val(actType);
                        activityType.append(option);
                    });
                }
            },
            error: function (err) {
                alert("error...");
            }
        });

    }

    $scope.toDou = function (iNum) {
        if (iNum < 10) {
            return '0' + iNum;
        } else {
            return '' + iNum;
        }
    }
    $scope.activityChangetype = function () {
        $("#Recommend1").hide();
        $("#Recommend2").hide();
        $(".col-md-12").find("input[type='text']").val("");
        var Noarray = ["bac397c7-f04b-43e2-b46a-25ae61466152", "a624d929-a6fc-4b4e-a91d-ffa0f4f05291", "619a8427-d7bc-401d-9d2a-72b3148fd52a"];//这几个券不显示(2瓶玻璃水兑换券,50元保养券,机油滤芯兑换券) 
        var flag = $.inArray($scope.formData.SelectCardType, Noarray);
        if ($scope.formData.SelectActivityType == "悦己纳新-售后服务活动" && flag < 0 && $scope.formData.SelectCardType != null) {
                $("#Recommend1").show();
                $("#Recommend2").show();
        }
        console.log($.inArray($scope.formData.SelectCardType, Noarray));
        if ($("#ii option:selected").index()==0) {
            $("#hddm").val("");
        }else{
            $("#hddm").val($scope.cardTypes[$("#ii option:selected").index() - 1].code);
        }
    }
    $scope.activityChange = function () {
        $("#Recommend1").hide();
        $("#Recommend2").hide();
        $(".col-md-12").find("input[type='text']").val("");
        var oDate = new Date();
        $scope.formData.ConsumeDate = $scope.toDou(oDate.getFullYear()) + "-" + $scope.toDou(oDate.getMonth() + 1) + "-" + $scope.toDou(oDate.getDate());

        switch ($scope.formData.SelectActivityType) {
            case "Y": {
                $scope.cardTypes = cardTypes38;
                break;
            }
            case "spring": {
                $scope.cardTypes = cardTypesSpring;
                break;
            }
            case "lingdong": {
                $scope.cardTypes = carTypeLingDong;
                break;
            }
            case "carshow": {
                $scope.cardTypes = carshow;
                break;
            }
           
            default:
                {
                    var cards = new Array();
                    var url = "/CustomCard/GetCustomCardListByAct";
                    $.ajax({
                        url: url,
                        type: 'Get',
                        async: false,
                        data: { actName: $scope.formData.SelectActivityType },
                        success: function (data) {
                            if (data != null) {
                                $.each(data, function (i, n) {
                                    var cardinfo = { activity: n.ActivityType, cardid: n.CardType, cardtitle: n.CardTypeName,code:n.code }
                                    cards.push(cardinfo);
                                });
                                $scope.cardTypes = cards;
                            }
                        },
                        error: function (err) {
                            alert("error...");
                        }
                    });

                }
        }
    }

   // $scope.cardTypes = cardTypes38;

    $scope.CurentTime = function () {
        var now = new Date();

        var year = now.getFullYear();       //年
        var month = now.getMonth() + 1;     //月
        var day = now.getDate();            //日

        var clock = year + "-";

        if (month < 10)
            clock += "0";

        clock += month + "-";

        if (day < 10)
            clock += "0";

        clock += day;

        return (clock);
    }


    $scope.formData = {
        ConsumeDate: $scope.CurentTime(),
        SelectActivityType: "Y"
    };
    $scope.PhoneNumberReadOnly = function () {
        var type = $scope.formData.SelectActivityType;
        var isRes = false;
        if (type == "Y" || type == "spring" || type == "lingdong") {
            isRes = true;
        }
        return isRes;
    }
    $scope.checkMileage = function () {
        if (isNaN($scope.formData.Mileage) || $scope.formData.Mileage < 1 || $scope.formData.Mileage > 1000000) {
            alert("数值不能超出区间，不能含有英文字母等不是数字的文字等");
            $scope.formData.Mileage = null;
            return false;
        }
    }

    //核销
    $scope.submit = function () {
        if (!(/^1[3|4|5|7|8]\d{9}$/.test($scope.formData.PhoneNumber)) && $scope.formData.SelectActivityType == 'carshow') {
            alert("请输入正确的手机号");
            return false;
        }
        if ($.trim($("#txtphonenumber")) == "" || !(/^1[3|4|5|7|8]\d{9}$/.test($scope.formData.PhoneNumber))) {
            alert("请输入正确的手机号");
            return false;
        }
        if (!$scope.isCardTrue) {
            alert("请先验证卡劵，再执行核销");
            return false;
        }

        if (!$scope.isVINTrue) {
            alert("请先验证VIN码，再执行核销");
            return false;
        }
        if ($scope.formData.Mileage < 1 || $scope.formData.Mileage > 1000000) {
            alert("行程里程请输入1-1000000的有效数字");
            return false;
        }

        if ($scope.formData.VIN.length != 17) {
            alert("请准确输入17位车架号，如果输入的车架号有错误，将无法提交相关索赔，得不到相应补偿。");
            return false;
        }
        var Noarray = ["bac397c7-f04b-43e2-b46a-25ae61466152", "a624d929-a6fc-4b4e-a91d-ffa0f4f05291", "619a8427-d7bc-401d-9d2a-72b3148fd52a"];//这几个券不显示(2瓶玻璃水兑换券,50元保养券,机油滤芯兑换券) 
        var flag = $.inArray($scope.formData.SelectCardType, Noarray);
        if ($scope.formData.SelectActivityType == "悦己纳新-售后服务活动" && flag < 0 && $scope.formData.SelectCardType != null) {
            var rname = $scope.formData.RecommendCustName;
            var rphone = $scope.formData.RecommendPhone;
            if ($.trim(rname)=="") {
                alert("试驾人姓名不能为空");
                return false;
            }
            if ($.trim(rphone) == "") {
                alert("试驾人手机号不能为空");
                return false;
            }
            if (!(/^1[3|4|5|7|8]\d{9}$/.test(rphone))) {
                alert("请输入正确的手机号");
                return false;
            }
        }

        var url = "/AfterSaleService/ConfirmUseCard";
        var postData = {
            OpenId: $scope.formData.OpenId,
            PhoneNumber: $scope.formData.PhoneNumber,
            CardType: $scope.formData.SelectCardType,
            CardNo: $scope.formData.CardNo,
            CustName: $scope.formData.CustName,
            VIN: $scope.formData.VIN,
            Mileage: $scope.formData.Mileage,
            ConsumeDate: $scope.formData.ConsumeDate,
            UserId: $scope.formData.UserId,
            CardInfo: $scope.formData.CardInfo,
            Id: $scope.formData.Id,
            ActivityTag: $scope.formData.SelectActivityType,
            RecommendName: $scope.formData.RecommendCustName,
            RecommendPhone: $scope.formData.RecommendPhone
        };

        $http.post(url, postData).success(function (result) {
            if (result.IsSuccess) {
                $scope.isCardTrue = false;
                $scope.isVINTrue = false;
                var msg = "核销成功";
                var Noarray = ["bac397c7-f04b-43e2-b46a-25ae61466152", "a624d929-a6fc-4b4e-a91d-ffa0f4f05291", "619a8427-d7bc-401d-9d2a-72b3148fd52a"];//这几个券不显示(2瓶玻璃水兑换券,50元保养券,机油滤芯兑换券) 
                var flag = $.inArray($scope.formData.SelectCardType, Noarray);
                if ($scope.formData.SelectActivityType == "悦己纳新-售后服务活动" && flag < 0) { //活动
                    msg = "该客户已经领取过维新组合礼品";
                    if ($scope.formData.SelectCardType == "b0c9d368-9450-4de2-a6e2-ff3c140d5649") { //试乘试驾礼品券
                        msg = "该客户可以获得维新组合礼品";
                    }
                }
                alert(msg);
                $(".col-md-12").find("input[type='text']").val("");
                $scope.formData = {};
                window.open('/AfterSaleService/ReportViewer?PhoneNumber=' + $scope.formData.PhoneNumber + '&CustName=' + $scope.formData.CustName + '&CarCategory=' + $scope.formData.CarCategory + '&VIN=' + $scope.formData.VIN + '&Mileage=' + $scope.formData.Mileage + '&CardType=' + selCardType.options[selCardType.options.selectedIndex].text + '&CardInfo=' + $scope.formData.CardInfo + '&CreateTime=' + $scope.formData.ConsumeDate + '&DealerId=' + result.Data.DealerId + '&CardNo=' + $scope.formData.CardNo + '&Type=' + $scope.formData.CardType);

            } else {
                alert(result.Message);
            }
        });
    }

    //输入改变后，重置状态
    $scope.cardNoChange = function () {
        $scope.isCardTrue = false;
    }

    $scope.VINChange = function () {
        $scope.isVINTrue = false;
    }
    //YC活动通过手机号取用户信息
    $scope.GetRecommendNameByPhone = function () {
        if ($scope.formData.SelectCardType == null) {
            alert("请选择兑换劵类型");
            return false;
        }
        if ($scope.formData.CardNo == null || $("#kqhm").val() == "") {
            alert("请输入卡劵号码");
            return false;
        }
        var url = "/AfterSaleService/GetRecommendNameByPhone";
        var postData = {
            phone: $scope.formData.RecommendPhone,
            UserID:$scope.formData.UserId
        };

        $http.post(url, postData).success(function (result) {
            $scope.formData.RecommendCustName = "";
            if (result.IsSuccess) {
                $scope.formData.RecommendCustName = result.Data.CustName;
              
            } else {
              
                alert(result.Message);
            }
        });
    }

    //验证并获取卡劵信息
    $scope.getCardInfo = function () {
        if ($scope.formData.SelectCardType == null) {
            alert("请选择兑换劵类型");
            return false;
        }
        if ($scope.formData.CardNo == null || $("#kqhm").val() == "") {
            alert("请输入卡劵号码");
            return false;
        }
        var url = "/AfterSaleService/GetCardInfo";
        var postData = {
            cardType: $scope.formData.SelectCardType,
            cardNo: $scope.formData.CardNo,
            activity: $scope.formData.SelectActivityType
        };

        $http.post(url, postData).success(function (result) {
            if (result.IsSuccess) {
                console.log("--------------");
                console.log(result.Data.RecommendInfo);
                if (result.Data.RecommendInfo != "" && result.Data.RecommendInfo!=null) {//存在被推荐人信息
                    $scope.formData.RecommendCustName = result.Data.RecommendInfo.Name;
                    $scope.formData.RecommendPhone = result.Data.RecommendInfo.PhoneNumber;
                }
                $scope.formData.PhoneNumber = result.Data.PhoneNumber;
                console.log($scope.formData.PhoneNumber+"~~~");
                $scope.formData.OpenId = result.Data.OpenId;
                $scope.formData.CardInfo = result.Data.CardInfo;
                $scope.formData.Id = result.Data.id;
                $scope.isCardTrue = true;
                $("#txtphonenumber").val(result.Data.PhoneNumber);
            } else {
                $scope.isCardTrue = false;
                alert(result.Message);
            }
        });
    }

    //验证并获取车辆信息
    $scope.GetCustAndCarInfo = function () {
        var url = "/AfterSaleService/GetCustAndCarInfo";
        var postData = {
            vin: $scope.formData.VIN,
            activity: $scope.formData.SelectCardType
        };

        $http.post(url, postData).success(function (result) {
            if (result.IsSuccess) {
                $scope.formData.CustName = result.Data.CustName;
                $scope.formData.CarCategory = result.Data.CarCategory;
                $("#txtcarcategory").val(result.Data.CarCategory);
                $("#txtcustname").val(result.Data.CustName);
                $scope.formData.UserId = result.Data.UserId;
                $scope.isVINTrue = true;
            } else {
                $scope.isVINTrue = false;
                alert(result.Message);
            }
        });
    }

});

afterSaleService.controller('AfterSaleServiceListCtrl', function ($scope, $http, $routeParams) {

    $scope.formData = {};

    $scope.editPlanData = {};

    $scope.colDefs = [];

    $scope.filterOptions = {
        useExternalFilter: true
    };
    $scope.cardTypes = new Array();// 卡劵类型

    var cardTypes38 = [{ activity: "三八活动", cardid: "pmXbSjvHWuRNfjXKz9PHnGSN4H6k", cardtitle: "4L机油兑换券" }
        , { activity: "三八活动", cardid: "pmXbSjl4yW3dzOJ6PlrsZx_ZOYFo", cardtitle: "4L机油8.5折兑换券" }
        , { activity: "三八活动", cardid: "pmXbSjlxGUgln7h2Fu0y4xBlK9kw", cardtitle: "基础保养体验券" }];

    var cardTypesSpring = [{ activity: "春季免检活动", cardid: "pmXbSjlRlUKd7XfMN2xgq-DcUdKU", cardtitle: "4L机油8.5折代金券" }
        , { activity: "春季免检活动", cardid: "pmXbSjv9vwzLD8eFXStW0BbHEk5w", cardtitle: "1L机油兑换券" }
        , { activity: "春季免检活动", cardid: "pmXbSjvzf3nS_BHJvXd1N_4ngJVI", cardtitle: "2瓶玻璃水兑换券" }
        , { activity: "春季免检活动", cardid: "pmXbSjh59fOta8gbh-nYBZB1RXSk", cardtitle: "基础保养兑换券" }];

    var carTypeLingDong = [
         { activity: "1瓶玻璃水兑换券", cardid: "pmXbSjrPnm-nqaOM7WUs9WNKRhTM", cardtitle: "1瓶玻璃水兑换券" }
    ];

    var carshow = [
         { activity: "北京车展", cardid: "bolishui", cardtitle: "玻璃水兑换券" },
           { activity: "北京车展", cardid: "jichubaoyang", cardtitle: "基础保养兑换券" }
    ];

    $scope.load = function () {
        var activityType = $("#selActivityType");
        var url = "/CustomCard/GetSCServiceCardTypeList";
        $.ajax({
            url: url,
            type: 'Get',
            data: { type: 1, source: 0, iswx: 1 },
            success: function (data) {
                activityType.empty();
                var option = $("<option>").text("请选择").val("");
                activityType.append(option);
                if (data != null) {
                    $.each(data, function (i, n) {
                        var actType = "";
                        var actName = n;
                        if (n == "北京车展") {
                            actType = "carshow";
                        }
                        else if (n == "三八活动") {
                            actType = "Y";
                        }
                        else if (n == "春季免检活动") {
                            actType = "spring";
                        }
                        else if (n == "1瓶玻璃水兑换券") {
                            actName = "领动新车上市";
                            actType = "lingdong";
                        }
                        else if (n == "推荐有理") {
                            actType = "TuiJian";
                        }
                        else {
                            actType = n;
                        }
                        var option = $("<option>").text(actName).val(actType);
                        activityType.append(option);
                    });
                }
            },
            error: function (err) {
                alert("error...");
            }
        });

    }
    $scope.toDou = function (iNum) {
        if (iNum < 10) {
            return '0' + iNum;
        } else {
            return '' + iNum;
        }
    }
    $scope.activityChangetype = function () {
        //  $(".col-md-12").find("input[type='text']").val("");
    }
    $scope.activityChange = function () {
        //  $(".col-md-12").find("input[type='text']").val("");
        var oDate = new Date();
        $scope.formData.ConsumeDate = $scope.toDou(oDate.getFullYear()) + "-" + $scope.toDou(oDate.getMonth() + 1) + "-" + $scope.toDou(oDate.getDate());

        switch ($scope.formData.isactivity) {
            case "Y": {
                $scope.cardTypes = cardTypes38;
                break;
            }
            case "spring": {
                $scope.cardTypes = cardTypesSpring;
                break;
            }
            case "lingdong": {
                $scope.cardTypes = carTypeLingDong;
                break;
            }
            case "carshow": {
                $scope.cardTypes = carshow;
                break;
            }

            default:
                {
                    var cards = new Array();
                    var url = "/CustomCard/GetCustomCardListByAct";
                    $.ajax({
                        url: url,
                        type: 'Get',
                        async: false,
                        data: { actName: $scope.formData.isactivity },
                        success: function (data) {
                            if (data != null) {
                                $.each(data, function (i, n) {
                                    var cardinfo = { activity: n.ActivityType, cardid: n.CardType, cardtitle: n.CardTypeName }
                                    cards.push(cardinfo);
                                });
                                $scope.cardTypes = cards;
                            }
                        },
                        error: function (err) {
                            alert("error...");
                        }
                    });

                }
        }
    }

    $scope.cardTypes = cardTypes38;

    $scope.CurentTime = function () {
        var now = new Date();

        var year = now.getFullYear();       //年
        var month = now.getMonth() + 1;     //月
        var day = now.getDate();            //日

        var clock = year + "-";

        if (month < 10)
            clock += "0";

        clock += month + "-";

        if (day < 10)
            clock += "0";

        clock += day;

        return (clock);
    }


    $scope.totalServerItems = 0;

    $scope.pagingOptions = {
        pageSizes: [10],
        pageSize: 10,
        PhoneNumber: null,
        CardType: null,
        CardNo: null,
        CustName: null,
        VIN: null,
        DealerId: null,
        StarCreateTime: null,
        EndCreateTime: null,
        isactivity: null,
        currentPage: 1
    };

    $scope.search = function () {
        $scope.filterOptions.PhoneNumber = $scope.formData.PhoneNumber;
        $scope.filterOptions.CardType = $scope.formData.SelectCardType;
        $scope.filterOptions.CardNo = $scope.formData.CardNo;
        $scope.filterOptions.CustName = $scope.formData.CustName;
        $scope.filterOptions.VIN = $scope.formData.VIN;
        $scope.filterOptions.DealerId = $scope.formData.DealerId;
        $scope.filterOptions.StarCreateTime = $scope.formData.StarCreateTime;
        $scope.filterOptions.EndCreateTime = $scope.formData.EndCreateTime;
        $scope.filterOptions.isactivity = $scope.formData.isactivity;
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
            + currentDate;
        //+ " "
        //+ date.getHours()
        //+ ":"
        //+ date.getMinutes();
    }

    $scope.setPagingData = function (data, page, pageSize, total_count, count) {
        for (i = 0 ; i < data.length ; i++) {
            data[i].CreateTime = $scope.ChangeDateFormat(data[i].CreateTime);
            //data[i].SendTime = $scope.ChangeDateFormat(data[i].SendTime);
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
            var url = "/AfterSaleService/SelectRecordList?";
            if (filter && filter.PhoneNumber != undefined) {
                url += "&PhoneNumber=" + filter.PhoneNumber;
            }
           if (filter && filter.CardType != undefined)
            { 
                url += "&CardType=" + filter.CardType;
            }

            if (filter && filter.CardNo != undefined) {
                url += "&CardNo=" + filter.CardNo;
            }

            if (filter && filter.CustName != undefined) {
                url += "&CustName=" + filter.CustName;
            }

            if (filter && filter.VIN != undefined) {
                url += "&VIN=" + filter.VIN;
            }

            if (filter && filter.DealerId != undefined) {
                url += "&DealerId=" + filter.DealerId;
            }

            if (filter && filter.StarCreateTime != undefined) {
                url += "&StarCreateTime=" + filter.StarCreateTime;
            }

            if (filter && filter.EndCreateTime != undefined) {
                url += "&EndCreateTime=" + filter.EndCreateTime;
            }
            //console.log(filter.isactivity);
            if (filter && filter.isactivity != undefined) {
                url += "&isactivity="+filter.isactivity;
            }

            $http.get(url + '&pageindex=' + page + '&pagesize=' + pageSize).success(function (largeLoad) {
                $scope.setPagingData(largeLoad.data, page, pageSize, largeLoad.total_count, largeLoad.count);
            });

        }, 100);
    };

    var flag = ($("#ce").val() != "" ? false : true);
    if (!flag) {

        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
    }

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
        data: 'myData',
        i18n: 'zh-cn',
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
            field: 'PhoneNumber ',
            displayName: '手机号',
            sortable: false,
            width: 110
        }, {
            field: 'CardTypeName',
            displayName: '卡劵类型',
            sortable: false,
            width: 150
        }, {
            field: 'CardNo',
            displayName: '卡劵号码',
            sortable: false,
            width: 120
        }, {
            field: 'CustName',
            displayName: '姓名',
            sortable: false,
            width: 80
        }, {
            field: '',
            displayName: '操作',
            sortable: false,
            width: 140,
            cellTemplate: '<span>&nbsp&nbsp</span><a href="javascript:void()" data-toggle="modal" data-target="#editPlanModal" ng-click="edit(row)">修改</a><span>&nbsp&nbsp</span><span>&nbsp&nbsp</span><a href="javascript:void()" ng-click="print(row)">打印凭证</a>'
        }, {
            field: 'VIN',
            displayName: '车架号',
            sortable: false,
            width: 150
        }, {
            field: 'CarCategory',
            displayName: '车型',
            sortable: false,
            width: 100
        }, {
            field: 'DealerId',
            displayName: '经销商',
            sortable: false,
            width: 100
        }, {
            field: 'Mileage',
            displayName: '行驶里程',
            sortable: false,
            width: 80
        }, {
            field: 'CreateTime',
            displayName: '创建时间',
            sortable: false,
            width: 140
        }],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions
    };

    $scope.print = function (row) {
        window.open('/AfterSaleService/ReportViewer?PhoneNumber=' + row.entity.PhoneNumber + '&CustName=' + row.entity.CustName + '&CarCategory=' + row.entity.CarCategory + '&VIN=' + row.entity.VIN + '&Mileage=' + row.entity.Mileage + '&CardType=' + row.entity.CardTypeName + '&CardInfo=' + row.entity.CardInfo + '&CreateTime=' + $scope.ChangeDateFormat(row.entity.ConsumeDate) + '&DealerId=' + row.entity.DealerId + '&CardNo=' + row.entity.CardNo + '&Type=' + row.entity.CardType + '&RecommendName=' + row.entity.RecommendName + '&RecommendPhone='+row.entity.RecommendPhone);
    }

    $scope.$watch('myData', function () {

    });

    $scope.editPlanSave = function () {
        if ($scope.editPlanData.CustName == "" || $scope.editPlanData.CustName == undefined) {
            alert("请输入姓名");
            return false;
        } else if ($scope.editPlanData.Mileage == "" || $scope.editPlanData.Mileage == undefined) {
            alert("请输入行驶里程");
            return false;
        } else {
            $http.post('/AfterSaleService/UpdateRecord',
                {
                    Id: $scope.editPlanData.Id,
                    CustName: $scope.editPlanData.CustName,
                    Mileage: $scope.editPlanData.Mileage
                }
            ).success(function (data) {
                if (data.IsSuccess) {
                    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions);
                    alert("操作成功！ \n");
                    this.btnClose.click();
                } else {
                    alert("操作失败！ \n" + data.Message);
                }
            }).error(function (data) {
                alert("操作失败！ \n" + data.Message);
            });

            $scope.editPlanData = {

            };
        }
    }

    $scope.edit = function (row) {
        $scope.editPlanData = {};

        $scope.editPlanData.Id = row.entity.Id;
        $scope.editPlanData.CustName = row.entity.CustName;
        $scope.editPlanData.Mileage = row.entity.Mileage;
    }

    $scope.checkMileage = function () {
        if (isNaN($scope.editPlanData.Mileage) || $scope.editPlanData.Mileage < 1 || $scope.editPlanData.Mileage > 1000000) {
            alert("数值不能超出区间，不能含有英文字母等不是数字的文字等");
            $scope.editPlanData.Mileage = null;
            return false;
        }
    }
});