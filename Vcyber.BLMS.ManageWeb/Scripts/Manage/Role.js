var selectItem;
webix.ready(function () {

    //Role 首页内容
    var formTable = {
        width: 930, height: 600,
        rows: [

            {
                cols: [
                    {
                        view: "datatable",
                        id: "list1",
                        columns: [{ id: "Id" },
                            { id: "Name", header: { text: "角色名称", css: { 'text-align': 'center' } }, width: 120, css: { 'text-align': 'center' } },
                            { id: "Describe", header: { text: "描述", css: { 'text-align': 'center' } }, width: 490, css: { 'text-align': 'center' } },
                            { id: "action", header: [{ text: "操作", colspan: 3, css: { 'text-align': 'center' } }], template: "<a class='editbtn' type='button'>编辑", css: { 'text-align': 'center' }, width: 100 },
                            { id: "action", template: "<a class='rolebtn' type='button'>权限设置", css: { 'text-align': 'center' }, width: 100 },
                            { id: "action", template: "<a class='delbtn' type='button'>删除", css: { 'text-align': 'center' }, width: 100 }
                        ],
                        pager: "pagerA",
                        url: "/Roles/RoleList",
                        datafetch: 20,
                        blockselect: false,
                        select: "row",
                        on: {
                            "onItemClick": function (id) {
                                selectItem = id;
                            }
                        },
                        onClick:
                        {
                            "rolebtn": function (e, id, trg) {
                                var obj = this.getItem(id);
                                $.ajax({
                                    url: "/Roles/BindFunctionJsonResult/" + obj.Id,
                                    type: "GET",
                                    success: function (data) {
                                        var bindPage = {
                                            rows: [
                                            {
                                                cols: [
                                                {
                                                    rows: [

                                                         {
                                                             id: "treeID",
                                                             view: "tree",
                                                             template: "{common.icon()} {common.checkbox()} #Name#",

                                                             data: data.FunctionModels,
                                                             autoheight: true, minHeight: 200,
                                                             threeState: true,
                                                             ready: function () {
                                                                 this.openAll();
                                                                 $.each(data.IdList, function (i, val) {
                                                                     $$("treeID").checkItem(val);
                                                                 });

                                                             }

                                                         }
                                                        , {
                                                            margin: 5,
                                                            cols: [
                                                                { view: "button", value: "保存", align: "center", type: "form", click: "bind_function('" + obj.Id + "')", width: 150 },
                                                                { view: "button", value: "返回", align: "center", click: "closeWin('bindWin')", width: 150 }
                                                            ]
                                                        }]

                                                }]
                                            }
                                            ]
                                        };
                                        webix.ui({
                                            view: "window",
                                            id: "bindWin",
                                            move: true,
                                            modal: true,
                                            position: "center",
                                            head: bindHeader,
                                            body: bindPage
                                        }).show();
                                        //$$('treeID').hideColumn("Id");
                                        return false; //here it blocks default behavior
                                    },
                                    error: function (data) {

                                    }
                                });
                            }
                        ,
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
                                                url: "/Roles/DelRole",
                                                success: function (message) {
                                                    if (message) {
                                                        $$("list1").remove(id);
                                                    } else {
                                                        webix.alert("删除失败!");
                                                    }
                                                },
                                                error: function (data) {
                                                    if (data.status == 401) {
                                                        webix.alert("操作受限");
                                                    } else {

                                                        webix.alert("删除失败!");
                                                    }
                                                }
                                            });
                                        }
                                    }
                                });
                                return false; //here it blocks default behavior

                            },
                            "editbtn": function (e, id, trg) {
                                var obj = this.getItem(id);
                                var editPage = {
                                    rows: [
                                        {
                                            cols: [
                                                {
                                                    view: "form",
                                                    id: "my_form",
                                                    width: 700,
                                                    align: "center",
                                                    elements: editForm,
                                                    url: "/Roles/RoleJsonResult/" + obj.Id,
                                                    elementsConfig: {
                                                        labelAlign: "left",
                                                        labelWidth: 100
                                                    }
                                                }]
                                        }
                                    ]
                                };
                                webix.ui({
                                    view: "window",
                                    id: "editWin",
                                    height: 600,
                                    width: 600,
                                    move: true,
                                    modal: true,
                                    position: "center",
                                    head: editHeader,
                                    body: editPage
                                }).show();

                                return false; //here it blocks default behavior

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
        type: "head",
        rows: [
            {
               view: "button", value: "+添加角色", width: 150, type: "form", align: "left", click: "add_new" 

                
            }, {height:5},

                formTable


        ]
    }).show();

    $$("list1").hideColumn("Id");
});

//AddRole页面内容
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

function closeWin(window) {
    $$(window).close();
}

function bind_function(id) {

    // var listOfObjects = [];
    var dtable = $$("treeID");
    var listOfObjects = dtable.getChecked();
    for (var i = 0; i < listOfObjects.length; i++) {
        var item = dtable.getIndexById(listOfObjects[i]);
    }
   

    var things = JSON.stringify({ roleId: id, things: listOfObjects });
    $.ajax({
        data: things,
        contentType: "application/json",
        type: "POST",
        url: "/Roles/BindFunction",
        success: function (message) {

            window.location = "/Roles/Role";

        },
        error: function (data) {
            webix.message("修改失败！");
        }
    });

}
