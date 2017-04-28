//var UserCenterServiceScheduleScripts = function () {
//    $(function () {
//        dataPaging();
//        $("#divMore").click(dataPaging);
//    });
    
//    var pageIndex = 1;
//    function dataPaging() {
//        var itemHtmlTemp="\
//            <div class=\"service-fold-item\">\
//                <div class=\"service-fold-hd\">\
//                    <div class=\"col\">\
//                        <strong>{ServiceType}:</strong>\
//                        <p>{RepairTime}</p>\
//                    </div>\
//                    <span class=\"ico-use\">{Status}</span>\
//                    <span class=\"ico-arrow\"><img src=\"images/ico_arrow.png\" /></span>\
//                </div>\
//                <div class=\"service-fold-ct\">\
//                    <p> 服务名称 <span>{ServiceType}</span><br/>\
//                        预约时间 <span>{RepairTime}</span><br/>\
//                        服务状态 <span>{Status}</span><br/>\
//                        处理时间 <span>{FinishTime}</span>\
//                    </p>\
//                </div>\
//            </div>\//         ";
//        common.post(common.resolveUrl("/UserCenter/APIServiceSchedule"), { pageIndex: pageIndex }, function (result) {
//            var data = result.Items;
//            if (data != null) {
//                $.each(data, function (index, item) {
//                    var itemHtml = itemHtmlTemp;
//                    itemHtml = itemHtml.replace(/{ServiceType}/g, item.ServiceType);
//                    itemHtml = itemHtml.replace(/{RepairTime}/g, item.RepairTime.FormatJsonDate("yyyy年MM月dd日 HH:mm"));
//                    itemHtml = itemHtml.replace(/{ServiceType}/g, item.ServiceType);
//                    itemHtml = itemHtml.replace(/{Status}/g, item.Status);
//                    itemHtml = itemHtml.replace(/{FinishTime}/g, item.FinishTime.FormatJsonDate("yyyy年MM月dd日 HH:mm"));
//                    $("#divDataList").append(productItemHtml);
//                });
//            }
//            if (result.TotalPages == 0 || pageIndex >= result.TotalPages) {
//                //alert("已到最后一页");
//                $("#divMore").hide();
//            }
//            pageIndex++;
//        });
//    }
//}();