var editHeader = {
    view: "toolbar",
    cols: [
        { view: "label", label: "账号管理 - 编辑账号", align: "left" }
    ]
};

var editForm = [
        {
            cols: [
                {
                    view: "text", label: "账号名：", labelAlign: "right", id: "UserName", name: "UserName", validate: webix.rules.isNotEmpty, width: 400
                }, { view: "label", label: "该账号已存在，请重新输入！", id: "nameMess", name: "nameMess", hidden: true, css: { "color": "red" } },
            { view: "label", label: "请输入账号！", id: "nameEmpty", name: "nameEmpty", hidden: true, css: { "color": "red" } }]

        },
        {
            cols: [{
                view: "text", label: "邮箱地址：", id: "Email", labelAlign: "right", width: 400, name: "Email", validate: webix.rules.isEmail,
                on: {
                    "onChange": function () {
                        this.validate();
                    }
                }
            }, { view: "label", label: "请输入有效的邮箱地址！", id: "mailMess", hidden: true, css: { "color": "red" } }]
        },
        
        {
            cols: [{
                view: "text", label: "手机号：", id: "Phone", labelAlign: "right", name: "Phone", width: 400, validate: webix.rules.isNotEmpty
            }, { view: "label", label: "请输入有效的手机号！", id: "phoneMess", hidden: true, css: { "color": "red" } }]
        },
        {
            cols: [{
                view: "text", label: "行政部门：", id: "Department", labelAlign: "right", name: "Department", width: 400, validate: webix.rules.isNotEmpty
            }, { view: "label", label: "请输入行政部门！", id: "departMess", hidden: true, css: { "color": "red" } }]
        },
        {
            view: "radio", name: "Status", label: "是否启用：", width: 400, labelAlign: "right"
            , options: [
                   { value: "是", id: 0 },
				{ value: "否", id: 1}            ]
        },
        {
            margin: 5,
            align: "center",
            cols: [
                { view: "button", value: "保存", type: "form", click: "getEditForm", width: 150 },
                { view: "button", value: "返回", click: "returnUser", width: 150 }
            ]
        }
];

var editPage = {
    rows: [
        {
            cols: [
                {
                    view: "form",
                    id: "edit_form",
                    width: 700,
                    align: "center",
                    elements: editForm,
                    elementsConfig: {
                        labelAlign: "left",
                        labelWidth: 100
                    },
                    rules: {
                        Phone:
                           function (value) {
                               var length = value.length;
                               var mobile = /((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)/;
                               if (!(length == 11 && mobile.test(value))) {
                                   this.validate = false;
                                   $$('phoneMess').show();
                                   return false;
                               }
                               return true;
                           }
                    }
                }]
        }
    ]
};


function getEditForm() {
    if (!$$('edit_form').validate()) {
        if (!$$("UserName").validate()) {
            $$("nameEmpty").show();
        }
        if (!$$("Mail").validate()) {
            $$("mailMess").show();
        }
      
        if (!$$("Phone").validate()) {
            $$("phoneMess").show();
        }
        if (!$$("Department").validate()) {
            $$("departMess").show();
        }
    } else {

        var values = $$('edit_form').getValues();

        $.ajax({
            data: {
                Id: selectItem,
                UserName: values.UserName,
                Email: values.Email,
              
                Phone: values.Phone,
                Department: values.Department,
                Status: values.Status
            },
            type: "POST",
            url: "/Users/EditUser",
            success: function (message) {
                if (message.id != 1) {
                    webix.alert(message.message);
                    $$("edit_form").clear();
                } else {

                    var parList = $$("list1");
                    var pageNumber = parList.getPage();
                    parList.clearAll();
                    parList.loadNext(20, 20 * pageNumber, null, "/Users/UserList");
                    $$('editWin').close();
                    //window.location = "/Manage/Role";
                }

            },
            error: function (data) {
                if (data.status == 401) {
                    webix.alert("操作受限");
                } else {
                    webix.alert("修改账号失败！");
                }
            }
        });
    }
}

function returnUser() {
    window.location = "/Users/User";
}
