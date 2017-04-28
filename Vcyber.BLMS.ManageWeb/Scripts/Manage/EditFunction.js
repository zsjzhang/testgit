
var editHeader = {
    view: "toolbar",
    cols: [
        { view: "label", label: "角色管理 - 编辑权限", align: "left" }
    ]
};

function edit_add_url() {
    $$("urlList").add({
        IsDel: 1,
        Url: "",
        Describe: "",
        Id:-1
    }, 0);
}

function getEditForm() {
    if ($$("edit_form").validate()) {
        var listOfObjects = [];
        var values = $$('edit_form').getValues();
        var dtable = $$('urlList');
        dtable.eachRow(
            function(row) {
                var singleObj = {};
                singleObj["Url"] = dtable.getItem(row).Url;
                //singleObj["IsDel"] = dtable.getItem(row).IsDel;
                singleObj["Describe"] = dtable.getItem(row).Describe;
                singleObj["Id"] = dtable.getItem(row).Id;
                listOfObjects.push(singleObj);
            }
        );

        var things = JSON.stringify({ name: values.Name, things: listOfObjects, description: values.Describe, id: values.Id });
        $.ajax({
            data: things,
            contentType: "application/json",
            type: "POST",
            url: "/Function/EditFunction",
            success: function(message) {

                window.location = "/Function/Function";

            },
            error: function (data) {
                if (data.status == 401) {
                    webix.alert("操作受限");
                } else {
                    webix.message("修改失败！");
                }
            }
        });
    }
}
