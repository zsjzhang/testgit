//webix.ready(function () {
    //var bindTable = {
    //    rows: [
    //    {
    //        view: "toolbar",
    //        cols: [
    //            { view: "label", label: "权限 - 权限设置", inputWidth: 200, align: "left" },
    //            { view: "button", value: "确认绑定", width: 100, align: "left", click: "bind_function" }
    //        ]
    //    },
    //    {
    //        view: "treetable",
    //        id: "treeID",
    //        columns: [
    //    {
    //        id: "IsChecked", view:"checkbox",    //        header: { content: "masterCheckbox" }, checkValue: 1, uncheckValue: 0,  width: 40
    //    },
	//			 { id: "Id", header: "", css: { "text-align": "right" } },	
    //            {
    //                id: "Name",
    //                header: "功能名称",
    //                template: "{common.icon()}{common.folder()}{common.checkbox()} #Name#",
    //                width: 300,
    //            }                 
    //        ],
    //        autoheight: true,
    //        maxHeight: 500,
    //        editable: true,
    //        editaction: "custom",
    //        select: "row",
    //        url: "/Manage/FunctionListJsonResult"
    //    }]
                
    //};
    
    //var leftMenu = {
    //    rows: [
    //        {
    //            cols: [
    //                {
    //                    header: "权限设置",
    //                    body:
    //                    {
    //                        view: "menu",
    //                        labelalign: "right",
    //                        type: {
    //                            height: 40,
    //                            width: 200
    //                        },
    //                        data: [
    //                            { id: "1", value: ".角色管理", href: "/Manage/Role" },
    //                            { id: "2", value: ".账号管理", href: "/Manage/User" },
    //                            { id: "3", value: ".操作日志", href: "/Manage/Log" },
    //                            { id: "4", value: ".系统设置", href: "/Manage/Function" },
    //                            { id: "5", value: ".修改密码", href: "/Manage/UpdatePwd" }
    //                        ],
    //                        layout: "y"
    //                    }
    //                }
    //            ]
    //        }
    //    ]
    //};

//    webix.ui({
//        container: "listA",
//        type: "space", id: "a1", rows: [{
//            type: "space", padding: 0, responsive: "a1", cols: [

//                leftMenu,
//                formTable
//            ]
//        }
//        ]
//    }).show();

//});
    var bindHeader = {
        view: "toolbar",
        cols: [
            { view: "label", label: "账号管理 - 权限设置", align: "left" }
        ]
    };
    
    var bindTable = {
        rows: [
        {
            view: "toolbar",
            cols: [
                { view: "label", label: "权限 - 权限设置", inputWidth: 200, align: "left" },
                { view: "button", value: "确认绑定", width: 100, align: "left", click: "bind_function" }
            ]
        },
        {
            view: "treetable",
            id: "treeID",
            columns: [
        {
            id: "IsChecked", view: "checkbox",            header: { content: "masterCheckbox" }, checkValue: 1, uncheckValue: 0, width: 40
        },                                                                                                                                                                                                                                                                                                                                                                                      
				 { id: "Id", header: "", css: { "text-align": "right" } },
                {
                    id: "Name",
                    header: "功能名称",
                    template: "{common.icon()} {common.checkbox()} {common.folder()} #Name#",
                    width: 300,
                }
            ],
            autoheight: true,
            maxHeight: 500,
            editable: true,
            editaction: "custom",
            select: "row",
            url: "/Role/FunctionListJsonResult"
        }]

    };
    