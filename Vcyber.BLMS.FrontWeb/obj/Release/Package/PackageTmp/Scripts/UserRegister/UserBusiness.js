function userBusiness() { }

userBusiness.Search = function () {
    var startTime = $("#startTime").val().trim();
    var endTime = $("#endTime").val().trim();
    var startMoney = $("#startMoney").val().trim();
    var endMoney = $("#endMoney").val().trim();
    var tradeType = $("#tradeType").val();

    $.post("../UserBusiness/Search", { "StartTime": startTime, "EndTime": endTime, "StartMoney": startMoney, "EndMoney": endMoney, "TradeType": tradeType, "Index": 1, "Size": 10 }, function (data) {
        $(".RowDiv").remove();

        if (data.TotalCount > 0) {
            $(data.Datas).each(function () {
                var html = "<div class='RowDiv'><div>" + this.CreateTimeString + "</div><div>" + this.TradeTypeString + "</div>";
                html += " <div>" + this.TradeMoney + "</div><div>" + this.FlowNumber + "</div><div>" + this.Remark + "</div></div>";
                $("#TableDiv").append(html);
            });
        }

        var length = $(".RowDiv").length;
        $("#TableDiv").height(length * 35+45);
    });
}
