//var url = "/Manage/GetOperationLogJsonResult";
webix.ready(function () {

    webix.ui({
        container: "OperationLogContainer",
        type: "head",
        id: "a1",
        width: 930,
        height:600,
        rows: [
            //=========
            {
                cols: [
                    {
                        id: "name",
                        view: "text",
                        label: '会员名称：',
                        labelAlign: "right",
                        labelWidth: 120,
                        width: 300
                    },{
                        id: "startDate",
                        view: "datepicker",
                        label: '日期：',
                        labelAlign: "right",
                        labelWidth: 120,
                        width: 300
                    },
                    {
                        id: "endDate",
                        view: "datepicker",
                        label: '-',
                        labelAlign: "right",
                        labelWidth: 10,
                        width: 190
                    }, {width:30},{
                        view: "button",
                        label: "查询",
                        type: "form",
                        click: "getSearchForm",
                        width: 100,
                        align: "center"
                    }
                  
                ]
            } 
            ,
            ////===========
            //{
            //    // need to get the data from controller or use webix math operation
            //    id: "lblMessage",
            //    view: "label",
            //    label: "***",
            //},
            //======================
            //datagrid
            {
                cols: [
                    {
                        view: "datatable",
                        id: "list1",
                        columns: [{ id: "Id", header: { text: "日志编号", css: { 'text-align': 'center' } }, width: 130, css: { 'text-align': 'center' } },
                            { id: "OperateUserName", header: { text: "账户名称", css: { 'text-align': 'center' } }, width: 200, css: { 'text-align': 'center' } },
                             { id: "OperateTime", header: { text: "操作日期", css: { 'text-align': 'center' } }, width: 200, css: { 'text-align': 'center' } },
                            { id: "IpAddress", header: { text: "IP地址", css: { 'text-align': 'center' } }, width: 180, css: { 'text-align': 'center' } },
                            { id: "OperateItem", header: { text: "操作记录", css: { 'text-align': 'center' } }, width: 200, css: { 'text-align': 'center' } }
                            ],
                        pager: "pagerA",
                        blockselect: false,
                        datafetch: 10,
                        url: "/OperationLog/GetOperationLogJsonResult",
                        select: "row"
                    }
                ]
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


   // loadDatatable(url, {});
});

function getSearchForm() {

    var startDate = $$('startDate').getValue();
    if (startDate) {
        startDate = startDate.toLocaleDateString();
    }

    var endDate = $$('endDate').getValue();
    if (endDate) {
        endDate = endDate.toLocaleDateString();
    }

    var name = $$('name').getValue();
   
    var data = {
        StartTime: startDate,
        EndTime: endDate,
        Name:name
    };

    var url = "/OperationLog/GetOperationLogJsonResult/?";

    url += "&&StartTime=" + startDate;
    url += "&&EndTime=" + endDate;
    url += "&&Name=" + name;
    
    $$("list1").clearAll();
    $$("list1").refresh();
    $$("pagerA").select(0);

    $$("list1").load(url);
    
    //loadDatatable(url, data);
}
//加载Datatable和统计信息的数据
function loadDatatable(url, data) {
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        success: function (msg) {

            var data1 = new webix.DataCollection({
                data: msg.data
            });

            $$('lblMessage').setValue(msg.sub_message);
            $$('list1').data.sync(data1);
        },
        error: function (msg) {
            $$('lblMessage').setValue("没有记录");
            $$('list1').data.sync({});
        }
    });
}