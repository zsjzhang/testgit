
var editHeader = {
    view: "toolbar",
    cols: [
        { view: "label", label: "角色管理 - 编辑角色", align: "left" }
    ]
};

var editForm = [
        { view: "text", id: "Id", name: "Id", hidden: true },
 { view: "text", label: "角色名：", id: "Name", name: "Name", labelAlign: "right", validate: webix.rules.isNotEmpty },
        { view: "textarea", label: "角色描述：", id: "Describe", name: "Describe", inputWidth: 500, height: 300, labelAlign: "right", validate: webix.rules.isNotEmpty },
        {
            margin: 5,
            cols: [
                { view: "button", value: "保存", type: "form", click: "getEditForm", width: 150 },
                { view: "button", value: "返回", click: "closeWin('editWin')", width: 150 }
            ]
        }
];

//var editPage = {
//    rows: [
//        {
//            cols: [
//                {
//                    view: "form",
//                    id: "my_form",
//                    width: 700,
//                    align: "center",
//                    elements: editForm,
//                    url: "/Manage/RoleJsonResult/" + id,
//                    elementsConfig: {
//                        labelAlign: "left",
//                        labelWidth: 100
//                    }
//                }]
//        }
//    ]
//};


function getEditForm() {
    var values = $$('my_form').getValues();
    $.ajax({
        data: {
            Id: values.Id,
            Name: values.Name,
            Describe: values.Describe
        },
        type: "POST",
        url: "/Roles/EditRole",
        success: function (message) {
            if (message.id != 1) {
                webix.alert(message.message);
                $$("my_form").clear();
            } else {

                var parList = $$("list1");
                var pageNumber = parList.getPage();
                parList.clearAll();
                parList.loadNext(20, 20 * pageNumber, null, "/Roles/RoleList");
                closeWin('editWin');
            }

        },
        error: function (data) {
            if (data.status == 401) {
                webix.alert("操作受限");
            } else {
                webix.message("编辑失败！");
            }
        }
    });
}

function returnRole() {
    window.location = "/Roles/Role";
}
