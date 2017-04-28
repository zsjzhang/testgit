
var addHeader = {
    view: "toolbar",
    cols: [
        { view: "label", label: "角色管理 - 添加账户", align: "left" }
    ]
};
var addForm = [
        {
            cols: [
                {
                    view: "text", label: "账号名：", labelAlign: "right", id: "UserName", name: "UserName", placeholder: "建议使用名字的全拼", validate: webix.rules.isNotEmpty, width: 400
                }, { view: "label", label: "该账号已存在，请重新输入！", id: "nameMess", name: "nameMess", hidden: true, css: { "color": "red" } },
            { view: "label", label: "请输入账号！", id: "nameEmpty", name: "nameEmpty", hidden: true, css: { "color": "red" } }]

        },
        {
            cols: [{
                view: "text", label: "邮箱地址：", id: "Mail", labelAlign: "right", name: "Email", placeholder: "建议使用公司内部邮箱", validate: webix.rules.isEmail, width: 400,
                on: {
                    "onChange": function () {
                        this.validate();
                    }
                }
            }, { view: "label", label: "请输入有效的邮箱地址！", id: "mailMess", hidden: true, css: { "color": "red" } }]
        },
        {
            cols: [{
                view: "text", type: "password", label: "密码：", id: "Password", placeholder: "6-20个字母/数字/符号", labelAlign: "right", name: "Password", validate: webix.rules.isNotEmpty, width: 400
            }, { view: "label", label: "请输入密码！", id: "pwdMess", hidden: true, css: { "color": "red" } }]
        },
        {
            cols: [{
                view: "text", type: "password", label: "确认密码：", id: "RepeatPassword", width: 400, labelAlign: "right", name: "RepeatPassword"

            }, { view: "label", label: "两次密码输入不一致！", id: "rePwdMess", hidden: true, css: { "color": "red" } }]
        },
        //{
        //    cols: [{
        //        view: "select", label: "角色类型：", options: "/Roles/RoleTypeList", id: "RoleId", width: 400, labelAlign: "right", name: "RoleId", validate: webix.rules.isNotEmpty
        //    }, { view: "label", label: "请选择角色类型！", id: "roleTypeMess", hidden: true, css: { "color": "red" } }]
        //},
        //{
        //    cols: [{
        //        view: "text", label: "真实姓名：", id: "Surname", placeholder: "不能为空，建议使用真实姓名", labelAlign: "right", width: 400, name: "Surname", validate: webix.rules.isNotEmpty
        //    }, { view: "label", label: "请输入真实姓名！", id: "realNameMess", hidden: true, css: { "color": "red" } }]
        //},
        {
            cols: [{
                view: "text", label: "手机号：", id: "Phone", placeholder: "建议使用真实手机号", labelAlign: "right", width: 400, name: "Phone", validate: webix.rules.isNotEmpty
            }, { view: "label", label: "请输入有效的手机号！", id: "phoneMess", hidden: true, css: { "color": "red" } }]
        },
        {
            cols: [{
                view: "text", label: "行政部门：", id: "Department", placeholder: "请输入所在部门", labelAlign: "right", width: 400, name: "Department", validate: webix.rules.isNotEmpty
            }, { view: "label", label: "请输入行政部门！", id: "departMess", hidden: true, css: { "color": "red" } }]
        },
        {
            id: "Status", view: "radio", name: "Status", label: "是否启用：", width: 400, labelAlign: "right",
            value: 0, options: [
                { value: "是", id: 0 },
				{ value: "否", id: 1 }            ]
        },
        {
            margin: 5,
            align: "center",
            cols: [
                { view: "button", value: "保存", type: "form", click: "getForm", width: 150 },
                { view: "button", value: "返回", click: "$$('addWin').close();", width: 150 }
            ]
        }
];

var addPage = {
    // container: "listA",
    rows: [
    {
        cols: [
        {
            view: "form",
            id: "add_form",
            width: 700,
            align: "center",
            elements: addForm,
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
                       },

                RepeatPassword: function (value) {

                    if (Password.value != value) {
                        $$("rePwdMess").show();

                        return false;
                    }
                    return true;
                }
            }



        }
        ]
    }
    ]
};

function add_new() {
    webix.ui({
        view: "window",
        id: "addWin",
        height: 600,
        move: true,
        modal: true,
        width: 600,
        position: "center",
        head: addHeader,
        body: addPage
    }).show();

}

function getForm() {
    $$("mailMess").hide();
    $$("nameEmpty").hide();
    $$("pwdMess").hide();
    $$("departMess").hide();
    $$('phoneMess').hide();
    $$("rePwdMess").hide();
    
    if (!($$("add_form").validate())) {
        if (!($$("UserName").validate())) {
            $$("nameEmpty").show();
        }
        if (!($$("Mail").validate())) {
            $$("mailMess").show();
        }
        if (!($$("Password").validate())) {
            $$("pwdMess").show();
        }

        if (!($$("Department").validate())) {
            $$("departMess").show();
        }

    } else {

        var values = $$('add_form').getValues();

        $.ajax({
            data: {
                UserName: values.UserName,
                Email: values.Email,
                Password: values.Password,
                Phone: values.Phone,
                Department: values.Department,
                Status: values.Status,
                RepeatPassword: values.RepeatPassword
            },
            type: "POST",
            url: "/Users/AddUser",
            success: function (message) {
                if (message.id != 1) {
                    webix.alert(message.message);
                    $$("add_form").clear();
                } else {
                    window.location = "/Users/User";
                }

            }
            ,
            error: function (data) {
                if (data.status == 401) {
                    webix.alert("操作受限");
                } else {
                    webix.alert("添加账号失败！");
                }
            }
        });
    }
}

function returnUser() {
    window.location = "/Users/User";
}
