var QuestionsIndexScripts = function () {
    function initValidator() {
        $.validator.setDefaults({
            submitHandler: submitQuestion
        });
        $("#mainForm").validate({
            rules: {
                QuestionsType: { required: true },
                QuestionsContent: { required: true },
                Email: { required: true, email: true }
            },
            messages: {
                QuestionsType: { required: "请选择类型" },
                QuestionsContent: { required: "请输入问题详情" },
                Email: { required: "请选择经销商", email: "输入正确格式的电子邮件" }
            },
            onfocusout: false,
            onkeyup: false,
            onclick: false,
            showErrors: function (errorMap, errorList) {
                var errMsg = "";
                $.each(errorList, function (i, v) {
                    errMsg += (v.message + "<br />");
                });
                if (errMsg != "") {
                    $("#errorContainer").html(errMsg);
                }
            }
        });
    }

    $(function () {
        $("input[name='QuestionsType']").change(function () {

            $("input[name='QuestionsType']:checked").removeAttr("checked", "checked");
            $(this).attr("checked", "checked");

        });
        initValidator();

        
        $(".close").click(function () {
            $("#overlay").removeClass("active").children().removeClass("modal-in");
        });
    });

    function submitQuestion() {
        var requestData = common.serializeJsonByFrom("mainForm");
        common.post(common.api("Question/AddQuestion?wxUserInfoId=" + wxUserId), requestData, function (result) {
            $("#overlay").addClass("active").children().eq(0).addClass("modal-in");
            //windows.location.href = common.api("/usercenter/index");
            //mobileAlert("预约成功");
        }, function (error) {
            $("#errorContainer").html(error);
        });
    }

    function closetip() {
        windows.location.href = common.api("/usercenter/index");
    }

}();