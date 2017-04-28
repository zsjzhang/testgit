$(function () {
    //加载机场
    GetAllAirports();
    //机场预约服务
    $("#btnSubscribe").click(function () {
        $.ajax({
            url: "/LifeService/AirportReserve",
            type: "get",
            success: function (result) {
                if (result == null || result == "") {
                    AlertFalse("您是注册用户或未登录，暂时无法预约该服务", function () {
                        window.location.href = "/Account/Login?url=/LifeService/AirportIndex"
                    });
                }
                else {
                    window.location.href = "/LifeService/AirportReserve";
                }
            },
            error: function () {

            }
        });
    });
})
//获取机场
function GetAllAirports() {
    $.ajax({
        url: "/LifeService/GetAllAirports",
        type: "post",
        dataType: "json",
        success: function (result) {
            if (result && $.isArray(result) && result.length > 0) {
                var _airportcount = Math.ceil(result.length / 2);
                var _tableHtml = "<tr><td>编号</td><td>机场名称</td><td>编号</td><td>机场名称</td></tr>";
                for (var i = 0, j = _airportcount; i < _airportcount; i++, j = i + _airportcount) {
                    _tableHtml += "<tr>";
                    _tableHtml += " <td>" + (i + 1) + "</td>";
                    _tableHtml += " <td class='searchAirport'  style=' cursor:pointer;'>" + result[i].AirportName + "</td>";
                    if (j + 1 <= result.length) {
                        _tableHtml += " <td>" + (j + 1) + "</td>";
                        _tableHtml += "<td class='searchAirport'  style=' cursor:pointer;'>" + result[j].AirportName + "</td>";
                    }
                    _tableHtml += "</tr>";
                }
                $("#tbAirports").html(_tableHtml);
            }
        },
        error: function () {

        }
    });
}