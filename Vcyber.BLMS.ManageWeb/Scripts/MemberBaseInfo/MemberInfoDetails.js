webix.ready(function () {
        var baseForm = 
      [{
                cols: [
           ,{
                view: "text",
                label: "会员账户名：",
                id: "Name",
                name: "Name",
                labelAlign: "right",
                inputWidth: 500,
                labelWidth: 100
            },
            {
                view: "text",
                label: "创建来源：",
                id: "MallName",
                name: "MallName",
                labelAlign: "right",
                inputWidth: 500,
                labelWidth: 100
            }, {
                view: "text",
                label: "创建时间：",
                id: "CreateTime",
                name: "CreateTime",
                labelAlign: "right",
                inputWidth: 500,
                labelWidth: 100
            }]
            },
            {
                cols: [
            {
                view: "text",
                label: "绑定邮箱：",
                id: "Email",
                name: "Email",
                labelAlign: "right",
                inputWidth: 500,
                labelWidth: 100,
                disabled:true
            }, {
                view: "text",
                label: "绑定手机：",
                id: "Mobile",
                name: "Mobile",
                labelAlign: "right",
                inputWidth: 500,
                labelWidth: 100
            }]
            }, {
                cols: [
           {
                view: "text",
                label: "状态：",
                id: "StateName",
                name: "StateName",
                labelAlign: "right",
                inputWidth: 500,
                labelWidth: 100
            }, {}]


        }];
        var form = 
          {
                view: "form",
                id: "base_form",
                width: 800,
                align: "center",
                elements: baseForm,
                url: "/Member/GetMemberBaseInfo/?mallAccountId=" + mallAccountId + "&mallCode="+mallCode,
                elementsConfig: {
                    labelAlign: "left",
                    labelWidth: 100
                }


        };



    webix.ui({
        container: "mylayout1",
        rows: [{
            type: "space", padding: 0, border: 0, responsive: "a1", cols: [

                form
            ]
        }
        ]
    }).show();
    $$("base_form").disable();
});