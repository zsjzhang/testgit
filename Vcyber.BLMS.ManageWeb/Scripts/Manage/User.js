var selectItem;

webix.ready(function () {

    var formTable = {
        width: 930,
        height:600,
        rows: [
                {
                    cols: [
                        {
                            view: "datatable",
                            id: "list1",
                            columns: [{ id: "Id", header: { text: "编号", css: { 'text-align': 'center' } }, width: 100, css: { 'text-align': 'center' } },
                                { id: "UserName", header: { text: "账号名称", css: { 'text-align': 'center' } }, width: 120, css: { 'text-align': 'center' } },

                                 { id: "Email", header: { text: "邮箱地址", css: { 'text-align': 'center' } }, width: 120, css: { 'text-align': 'center' } },

                                 { id: "Phone", header: { text: "手机号", css: { 'text-align': 'center' } }, width: 120, css: { 'text-align': 'center' } },
                                {
                                    id: "CreateTime",
                                    header: { text: "创建时间", css: { 'text-align': 'center' } }, width: 120, css: { 'text-align': 'center' }
                                },
                                 { id: "LastLoginTime", header: { text: "最后登录时间", css: { 'text-align': 'center' } }, width: 120, css: { 'text-align': 'center' } },
                                 { id: "RoleName", header: { text: "角色类型", css: { 'text-align': 'center' } }, width: 120, css: { 'text-align': 'center' } },
                                 { id: "Department", header: { text: "行政部门", css: { 'text-align': 'center' } }, width: 120, css: { 'text-align': 'center' } },
                                 { id: "StatusName", header: { text: "启用状态", css: { 'text-align': 'center' } }, width: 120, css: { 'text-align': 'center' } },
                                 { id: "action", header: [{ text: "操作", colspan: 4, css: { 'text-align': 'center' } }], template: "<a class='editbtn' type='button'>编辑", css: { 'text-align': 'center' }, width: 100 },
                                 { id: "action", template: "<a type='button' href='/UserRoles/AddToRole?userId=#Id#'>角色分配", css: { 'text-align': 'center' }, width: 100 },
                            { id: "action", template: "<a class='resetPWbtn' type='button'>重置密码", css: { 'text-align': 'center' }, width: 100 }, { id: "action", template: "<a class='delbtn' type='button'>删除", css: { 'text-align': 'center' }, width: 100 }
                            ],
                            pager: "pagerA",
                            url: "/Users/UserList",
                            blockselect: false,
                            datafetch: 20,
                            select: "row",
                            on: {
                                "onItemClick": function (id) {
                                    //selectItem = id;
                                }
                            },
                            onClick:
                            {
                                "delbtn": function (e, id, trg) {
                                    var obj = this.getItem(id);
                                    webix.confirm({
                                        ok: "确认",
                                        cancel: "取消",
                                        type: "confirm-error",
                                        text: "确认删除此条目？",
                                        callback: function (result) { //setting callback

                                            if (result == true) {
                                                $.ajax({
                                                    data:
                                                    {
                                                        Id: obj.Id,
                                                    },
                                                    type: "POST",
                                                    url: "/Users/DelUser",
                                                    success: function (message) {
                                                        if (message) {
                                                            $$("list1").remove(id);
                                                        } else {
                                                            webix.alert("删除失败!");
                                                        }
                                                    },
                                                    error: function (message) {
                                                        webix.alert("删除失败!");
                                                    }
                                                });
                                            }
                                        }
                                    });
                                    return false; //here it blocks default behavior


                                },
                                "editbtn": function (e, id, trg) {
                                    var obj = this.getItem(id);
                                    selectItem = obj.Id;
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
                                                          url: "/Users/UserJsonResult/" + selectItem,
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
                                    webix.ui({
                                        view: "window",
                                        id: "editWin",
                                        height: 600,
                                        width: 650,
                                        move: true,
                                        modal: true,
                                        position: "center",
                                        head: editHeader,
                                        body: editPage
                                    }).show();

                                    // return false; //here it blocks default behavior

                                },
                                "rolebtn": function (e, id, trg) {
                                    var obj = this.getItem(id);

                                    var rolePage = {
                                        rows: [
                                        {
                                            cols: [
                                            {
                                                id: "roleTable",
                                                view: "datatable",
                                                columns: [
                                                    { id: "ch1", header: { content: "masterCheckbox" }, checkValue: 'on', uncheckValue: 'off', template: "{common.checkbox()}", width: 40 },
                                                    { id: "id", hidden: true },
                                                    { id: "value", header: "角色名称", width: 280 }
                                                ],

                                                height: 400,
                                                autowidth: true,
                                                url: "/roles/RoleTypeList"
                                            }]
                                        }, {
                                            margin: 5,
                                            align: "center",
                                            cols: [
                                                {
                                                    view: "button", value: "保存", type: "form", click: function () {
                                                        var test = $$('roleTable');
                                                        $$('roleTable').data.each(function (obj) {
                                                            var test = obj;
                                                        });
                                                    }, width: 150
                                                },
                                                { view: "button", value: "返回", click: "$$('roleWin').close();", width: 150 }
                                            ]
                                        }
                                        ]
                                    };
                                    webix.ui({
                                        view: "window",
                                        id: "roleWin",
                                        height: 600,
                                        width: 650,
                                        move: true,
                                        modal: true,
                                        position: "center",
                                        head: editHeader,
                                        body: rolePage
                                    }).show();


                                },


                                "resetPWbtn": function (e, id, trg) {
                                    var obj = this.getItem(id);

                                    var pwPage = {
                                        id: "pwPage", view: "form",
                                        rows: [
                                        {
                                            cols: [{
                                                view: "text", type: "password", label: "旧密码：", id: "OldPassword", width: 400, name: "OldPassword", labelAlign: "right",labelWidth:120, validate: webix.rules.isNotEmpty
                                            }, { view: "label", label: "如果修改密码，请填写旧密码", id: "oldPwdMess" }]
                                        },
                                            {
                                                cols: [{
                                                    view: "text", type: "password", label: "密码：", id: "Password", width: 400, labelAlign: "right", name: "Password", labelWidth: 120, validate: webix.rules.isNotEmpty
                                                }, { view: "label", label: "请输入密码！", id: "pwdMess", hidden: true, css: { "color": "red" } }]
                                            },
                                            {
                                                cols: [{
                                                    view: "text", type: "password", label: "确认密码：", id: "RepeatPassword", width: 400, labelAlign: "right", labelWidth: 120, name: "RepeatPassword",
                                                    on: {
                                                        "onChange": function () {
                                                            if ($$("Password").data.value != this.data.value) {
                                                                $$("rePwdMess").show();
                                                                //set the repwd to red background????

                                                                this.validate = false;
                                                            }

                                                        }
                                                    }
                                                }, { view: "label", label: "两次密码输入不一致！", id: "rePwdMess", hidden: true, css: { "color": "red" } }]
                                            }
                                        , {
                                            margin: 5,
                                            align: "center",
                                            cols: [
                                                {
                                                    view: "button",
                                                    value: "保存",
                                                    type: "form",
                                                    click: function() {

                                                        if (($$("pwPage").validate())) {
                                                            var values = $$('pwPage').getValues();

                                                            $.ajax({
                                                                data: {
                                                                    userId: obj.Id,
                                                                    Password: values.Password,
                                                                    OldPassword: values.OldPassword,
                                                                    ConfirmPassword: values.RepeatPassword
                                                                },
                                                                type: "POST",
                                                                url: "/Users/ResetPw",
                                                                success: function(message) {
                                                                    webix.alert(message.message);
                                                                    if (message.id != 1) {

                                                                        $$("pwPage").clear();
                                                                    } else {
                                                                        $$('pwWin').close();
                                                                        //window.location = "/Manage/Role";
                                                                    }
                                                                },
                                                                error: function(data) {
                                                                    if (data.status == 401) {
                                                                        webix.alert("操作受限");
                                                                    } else {
                                                                        webix.alert("修改账号失败！");
                                                                    }
                                                                }
                                                            });

                                                        }
                                                    }
                                                    ,
                                                        width: 150
                                                    
                                                },
                                                { view: "button", value: "返回", click: "$$('pwWin').close();", width: 150 }
                                            ]
                                        }],
                                        rules: {
                                            RepeatPassword: function (value) {

                                                if (Password.value != value) {
                                                    $$("rePwdMess").show();

                                                    return false;
                                                }
                                                return true;
                                            }
                                        }
                                    };
                                    webix.ui({
                                        view: "window",
                                        id: "pwWin",
                                        height: 600,
                                        width: 750,
                                        move: true,
                                        modal: true,
                                        position: "center",
                                        head: editHeader,
                                        body: pwPage
                                    }).show();

                                }
                            }
                        }
                    ]
                },
            {
                paddingY: 7,
                rows: [
                    {
                        view: "pager", id: "pagerA",
                        template: "{common.first()} {common.prev()} {common.pages()} {common.next()} {common.last()}",
                        size: 20,
                        group: 5
                    }
                ]
            }
        ]
    };

    webix.ui({
        container: "listA",
        type: "head", rows: [
            {
                view: "button", value: "+添加账号", width: 150, type: "form", align: "left", click: "add_new"


            }, { height: 5 },
            formTable

        ]
    }).show();

    $$("list1").hideColumn("Id");
});


//add user page

function closeWin(window) {
    $$(window).close();
}

function roleBinding() {
    var test = $$('roleTable');
}