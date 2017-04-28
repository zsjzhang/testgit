webix.ready(function () {
    
    webix.ui({
        container: "MembersContainer",
        type: "head",
        rows: [{ height: 5 },
        {

            cols: [{
                view: "text",
                label: "会员名称：",
                id: "userName",
                labelAlign: "right",
                labelWidth: 100
            }, {
                view: "select",
                label: "商城平台：",
                options: "/Common/GetAllMallJsonResult",
                id: "mallCode",
                value: "-1",
                labelAlign: "right", labelWidth: 120

            },
                {
                view: "button", value: "查询", type: "form", width: 80, align: "center",
                click: function () {
                    var memberNumber = $$('userName').getValue();
                    var mallCode = $$('mallCode').getValue();

                    var url = "/MemberBaseInfo/MemberJsonList?userName="
                        + memberNumber + "&&systemId=" + mallCode;
                    $$("kbUserList").clearAll();
                    $$("kbUserList").refresh();
                    $$("pagerA").select(0);
                    $$("kbUserList").load(url);
                }
            }, { width: 20 }]

        },
            {
                view: "datatable",
                id: "kbUserList",
                columns: [{ id: "id",hidden:true },
                    { id: "userName", header: { text: "会员名称", css: { 'text-align': 'center' } }, width: 200, css: { 'text-align': 'center' } },
                    { id: "systemName", header: { text: "商城平台", css: { 'text-align': 'center' } }, width: 200, css: { 'text-align': 'center' } },
                    { id: "systemId", hidden: true},
                   { id: "State", header: { text: "状态", css: { 'text-align': 'center' } }, width: 200, css: { 'text-align': 'center' } },
                   { id: "action", header: [{ text: "操作", css: { 'text-align': 'center' } }], template: "<a class='detailbtn' href='/MemberBaseInfo/MemberDetails?id=#id#' type='button'>查看", css: { 'text-align': 'center' }, width: 200 }
                   
                ],
                pager: "pagerA",
                blockselect: false,
                datafetch: 10,
                url: "/MemberBaseInfo/MemberJsonList",
                select: "row"
            },
            {
                paddingY: 7,
                rows: [
                    {
                        view: "pager", id: "pagerA",
                        template: "{common.first()} {common.prev()} {common.pages()} {common.next()} {common.last()}",
                        size: 10,
                        group: 5
                    }
                ]
            }
        ]

    }).show();
});


function returnInfo() {
    window.location = "/Member/Index";
}