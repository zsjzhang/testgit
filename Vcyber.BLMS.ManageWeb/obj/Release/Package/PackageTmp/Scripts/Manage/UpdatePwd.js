////定期更换密码可以让你的账户更加安全。

webix.ready(function () {
    var pwdForm = [
    {
        cols: [
            {
                view: "text",
                label: "当前密码：",
                type: "password",
                labelAlign: "right",
                labelWidth:150,
                id: "OldPassword",
                name: "OldPassword", placeholder: "请输入当前密码", width: 500,
                validate: webix.rules.isNotEmpty
            }, 
            { view: "label", label: "请输入当前密码！", id: "oldPwdEmpty", name: "oldPwdEmpty", hidden: true, css: { "color": "red" } }]
    },
    {
        cols: [{
            view: "text",
            type: "password",
            label: "新密码：",
            id: "Password",
            labelWidth: 150,
            name:"Password",
            labelAlign: "right", width: 500,
            validate: webix.rules.isNotEmpty
        },  { view: "label", label: "请输入有效密码！", id: "pwdValidMess", hidden: true, css: { "color": "red" } }
        ]
    },
    {
        cols: [{
            view: "text",
            type: "password",
            label: "重复新密码：",
            id: "ConfirmPassword",
            labelWidth: 150,
            labelAlign: "right", width: 500,
            name: "ConfirmPassword",
            on: {
                "onChange": function () {
                    if ($$("Password").data.value != this.data.value) {
                        $$("rePwdMess").show();
                        //set the repwd to red background????

                        this.validate = false;
                        return false;
                    }
                    return true;
                }
            }
        }, { view: "label", label: "两次密码输入不一致！", id: "rePwdMess", hidden: true, css: { "color": "red" } }]
    },
    {
        margin: 5,
        align: "center",
        cols: [
            { view: "button", value: "保存", type: "form", click: "getPwdForm", width: 150 }
        ]
    }];

    //var pwdHeader = {
    //    view: "toolbar",
    //    cols: [
    //        { view: "label", label: "修改密码", align: "left" }
    //    ]
    //};

    var pwdPage = {
        width: 930,

        height: 600,
        rows: [
         //  pwdHeader,
            {
                cols: [
                    {
                        view: "form",
                        id: "my_form",
                        align: "center",
                        elements: pwdForm,
                        elementsConfig: {
                            labelAlign: "left",
                            labelWidth: 100
                        }
                    }]
            }
        ]
    };

    webix.ui({
        container: "listA",
        type: "head",
        id: "a1",
        rows: [
            {view: "label", label: "   定期更换密码可以让你的账户更加安全", height:50 },
            pwdPage, { height: 100 }
            
        
        ]
    }).show();
//    webix.ui({
//        container: "listA",
//        type: "space",
//        id: "a1",
//        cols: [
//            leftMenu,
//            pwdPage
//        ]
//    }).show();
});

function getPwdForm() {
    $$("oldPwdEmpty").hidden = true;
    $$("rePwdMess").hidden = true;
    $$("pwdValidMess").hidden = true;
    if (!$$("my_form").validate()) {
        if (!$$("OldPassword").validate()) {
            $$("oldPwdEmpty").show();
        }
        if (!$$("Password").validate()) {
            $$("pwdValidMess").show();
        }
        
    } else {
        if ($$("Password").data.value != $$("ConfirmPassword").data.value) {
            $$("rePwdMess").show();
            $$("ConfirmPassword").validate = false;
            return false;
        }

        var values = $$('my_form').getValues();
        $.ajax({
            data: {
                OldPassword: values.OldPassword,
                Password: values.Password,
                ConfirmPassword: values.ConfirmPassword
            },
            type: "POST",
            url: "/Account/UpdatePwd",
            success: function (message) {

                webix.alert(message.message);
                if (message != 1) {
                    $$("my_form").clear();
                }
                
                if (message == 1) {

                    webix.ui({
                                            view: "window",
                                            id: "succssWin",
                                            width: 500,
                                            position: "center",
                                            head: {
                                                view: "toolbar",
                                                cols: [
                                                    {
                                                        view: "label",
                                                        label: '修改密码',
                                                        width: 100,
                                                        align: 'left'
                                                    },
                                                    {
                                                        view: "button",
                                                        label: '关闭',
                                                        width: 100,
                                                        align: 'right',
                                                        click: "$$('succssWin').close();"
                                                    }
                                                ]
                                            },
                                            body: {
                                                align: 'center',
                                                rows: [{},
                                                    {
                                                        view: "label",
                                                        label: "√  操作成功，密码已修改！",
                                                        align: 'center',
                                                        width: 300
                                                    }, {},
                                                    {
                                                        cols: [
                                                            { view: "button", value: "返回首页", click: "returnRole", width: 100 }
                                                        ]
                                                    }
                                                ]
                                            }
                                        }).show();
                }
            },
            error: function (data) {
                if (data.status == 401) {
                    webix.alert("操作受限");
                } else {

                    webix.message("添加失败！");
                }
            }
        });
    }
}