var selectItem;
webix.ready(function () {
    var formTable = {
        width: 930,
        htight:600,
        rows: [

            {
                view: "treetable",
                id: "treeID",
                columns: [
                    { id: "Id", header: "", css: { "text-align": "right" } },
                    {
                        id: "Name",
                        header: "功能名称",
                        width: 500,
                        template: "{common.treetable()} #Name#"
                    },
                      { id: "action", header: [{ text: "操作", colspan: 3, css: { 'text-align': 'center' } }], template: "<a class='subbtn' type='button'>添加子功能", css: { 'text-align': 'center' }, width: 150 },
                        { id: "action", template: "<a class='editbtn' type='button'>编辑", css: { 'text-align': 'center' }, width: 150 },
                        { id: "action", template: "<a class='delbtn' type='button'>删除", css: { 'text-align': 'center' }, width: 150 }

                ],
                autoheight: true,
                maxHeight: 500,
                minHeight: 400,
                editable: true,
                editaction: "custom",
                select: "row",
                url: "/Function/FunctionListJsonResult",
                on: {
                    "onItemClick": function (id) {
                        selectItem = id;
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
                            text: "确认删除此条目？(所有子节点会级联删除)",
                            callback: function (result) { //setting callback

                                if (result == true) {
                                    $.ajax({
                                        data:
                                        {
                                            id: obj.id,
                                        },
                                        type: "POST",
                                        url: "/Function/DelFunction",
                                        success: function (message) {
                                            if (message) {
                                                $$("treeID").remove(id);
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
                        selectItem = obj.id;
                        var editForm = [{
                            view: "text",
                            id: "Id",
                            name: "Id",
                            hidden: true
                        },
                            {
                                view: "text",
                                label: "权限名：",
                                id: "Name",
                                name: "Name",
                                labelAlign: "right",
                                validate: webix.rules.isNotEmpty,
                                inputWidth: 500,
                                labelWidth: 100
                            },
                            {
                                view: "text",
                                label: "权限描述：",
                                id: "Description",
                                name: "Describe",
                                inputWidth: 500,
                                labelAlign: "right",
                                validate: webix.rules.isNotEmpty,
                                labelWidth: 100
                            },
                            {
                                rows: [
                                        { view: "button", label: "添加URL", click: "edit_add_url", width: 100, align: "center" }, {
                                            view: "datatable",
                                            align: "center",
                                            minHeight: 200,
                                            maxWidth: 200,
                                            scroll: true,
                                            id: "urlList",
                                            columns: [
                                            { id: "Id", name: "Id" },
                                                {
                                                    id: "Url", name: "Url",
                                                    validate: webix.rules.isNotEmpty, editor: "text", header: { text: "URL", css: { 'text-align': 'center' } }, width: 200, css: { 'text-align': 'center' }
                                                },
                                                                 { id: "Describe", name: "Describe", editor: "text", header: { text: "描述", css: { 'text-align': 'center' } }, width: 200, css: { 'text-align': 'center' } },
                                                { id: "action", header: [{ text: "操作", css: { 'text-align': 'center' } }], template: "<a class='delbtn' type='button'>删除", css: { 'text-align': 'center' }, width: 100 }
                                            ],
                                            editable: true,
                                            editaction: "custom",
                                            autoheight: true,
                                            autowidth: true,
                                            select: "row",
                                            url: "/Function/FunctionUrlJsonResult/" + obj.id,
                                            on: {
                                                "onItemClick": function (selectid) {
                                                    this.editRow(selectid);
                                                }

                                            },
                                            onClick: {
                                                "delbtn": function (selecte, selectid, event) {
                                                    webix.confirm({
                                                        ok: "确认",
                                                        cancel: "取消",
                                                        type: "confirm-error",
                                                        text: "确认删除此条目？",
                                                        callback: function (result) { //setting callback

                                                            if (result == true) {

                                                                $$("urlList").remove(selectid);
                                                            }
                                                        }
                                                    });

                                                    return false;

                                                }
                                            }
                                        }]
                            },
                            {
                                align: "bottom",
                                cols: [
                                    { view: "button", value: "保存", type: "form", click: "getEditForm", width: 100, align: "center" },
                                    { view: "button", value: "关闭", click: "$$('editWin').close();", width: 100, align: "center" }
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
                                            minHeight: 400,
                                            align: "center",
                                            elements: editForm,
                                            url: "/Function/FunctionJsonResult/" + obj.id,
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
                            move: true,
                            modal: true,
                            position: "center",
                            head: editHeader,
                            body: editPage
                        }).show();
                        $$("urlList").hideColumn("Id");
                        return false; //here it blocks default behavior

                    },
                    "subbtn": function (e, id, trg) {
                        var obj = this.getItem(id);
                        selectItem = obj.id;
                        webix.ui({
                            view: "window",
                            id: "addWin",
                            height: 600,
                            width: 600,
                            move: true,
                            modal: true,
                            position: "center",
                            head: addHeader,
                            body: addPage
                        }).show();

                        return false;
                    }
                }

            }
        ]
    };


    webix.ui({
        container: "listA",
        type: "head", rows: [{
            view: "button", value: "添加功能", width: 100, align: "left", type: "form", click: "add_function" 
            
        }, {height:5},


                formTable


        ]       
    }).show();

    $$("treeID").hideColumn("Id");
});

function closeWin(window) {
    $$(window).close();
}
