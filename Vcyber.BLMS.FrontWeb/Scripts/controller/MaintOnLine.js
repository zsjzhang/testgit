var action = {};

$(function () {
    var carType = $("#selCarType").val();
    var KM = $("#selKM").val();

    $.ajax({
        url: "/BlueVIP/MaintOnLinePage",
        type: "post",
        data: { carType: carType, KM: KM, t: (new Date()).getTime() },
        success: function (result) {
            $("#divContent").html(result);
        },
        error: function (err) {

        }
    });
});

action.SearchList = function () {
    var carType = $("#selCarType").val();
    var KM = $("#selKM").val();

    $.ajax({
        url: "/BlueVIP/MaintOnLinePage",
        type: "post",
        data: { carType: carType, KM: KM, t: (new Date()).getTime() },
        success: function (result) {
            $("#divContent").html(result);
        },
        error: function (err) {

        }
    });
};