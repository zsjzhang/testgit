var addForm = [
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
    name: "Description",
    inputWidth: 500,
    labelAlign: "right",
    validate: webix.rules.isNotEmpty,
    labelWidth: 100
},
{
    rows: [
    {
        cols: [
                    { view: "button", label: "添加URL", click: "add_url", width: 100, align: "center" },
            { view: "label", label: "URL里边有重复！", width: 200, align: "right", hidden: true, id: "warnLabel", css: { "color": "red" } },
            { view: "label", label: "URL里边有空URL！", width: 200, align: "right", hidden: true, id: "warnLabel1", css: { "color": "red" } }]
            }, {
                view: "datatable",
                align: "center",
                minHeight: 200,
                maxWidth: 200,
                scroll:true,
                id: "list1",
                columns: [
                    { id: "Url", editor: "text", validate: webix.rules.isNotEmpty, header: { text: "URL", css: { 'text-align': 'center' } }, width: 200, css: { 'text-align': 'center' } },
                    { id: "Describe", editor: "text", header: { text: "描述", css: { 'text-align': 'center' } }, width: 200, css: { 'text-align': 'center' } },
                    { id: "action", header: [{ text: "操作", css: { 'text-align': 'center' } }], template: "<a class='delbtn' type='button'>删除", css: { 'text-align': 'center' }, width: 100 }
                ],
                editable: true,
                editaction: "custom",
                //autoheight: true,
                autowidth: true,
                select: "row",
                on: {
                    "onItemClick": function (id) {
                        this.editRow(id);
                    }

                },
                onClick: {
                    "delbtn": function (e, id, trg) {
                        $$("list1").remove(id);
                        return false; //here it blocks default behavior

                    }
                }
            }]
    },

      {
          align: "bottom",
          cols: [
              { view: "button", value: "保存", type: "form", click: "getForm", width: 100, align: "center" },
              { view: "button", value: "关闭", click: "window.location='/Function/Function'", width: 100, align: "center" }
          ]
      }
];

var addHeader = {
    view: "toolbar",
    cols: [
        { view: "label", label: "角色管理 - 添加权限", align: "left" }
    ]
};

var addPage = {
    rows: [
        {
            cols: [
                {
                    view: "form",
                    id: "my_form",
                    width: 700,
                    minHeight: 400,
                    align: "right",
                    elements: addForm,
                    elementsConfig: {
                        labelAlign: "left",
                        labelWidth: 100
                    }
                }]
        }
    ]
};

function add_function() {
    webix.ui({
        view: "window",
        id: "addWin",
        move: true,
        modal: true,
        position: "center",
        head: addHeader,
        body: addPage,
    }).show();


}

function add_url() {
 $$("list1").add({
        IsDel: 1,
        Url: "",
        Describe: ""
    }, 0);
}


function getForm() {
    if ($$("my_form").validate()) {

        var listOfObjects = [];
        var values = $$('my_form').getValues();
        var dtable = $$('list1');
        var test=0 ;
        dtable.eachRow(
           function(row) {
                for (var i = 0; i < listOfObjects.length; i++) {
                    if (listOfObjects[i].Url==null ||listOfObjects[i].Url == "")
                    {
                        $$("warnLabel1").show();
                        test += 1;
                        return false;
                    }
                    if (listOfObjects[i].Url == dtable.getItem(row).Url) {
                        $$("warnLabel").show();
                        test += 1;
                        return false;
                    }
                }
                var singleObj = {};
                singleObj["Url"] = dtable.getItem(row).Url;
                //singleObj["IsDel"] = dtable.getItem(row).IsDel;
                singleObj["Describe"] = dtable.getItem(row).Describe;

                listOfObjects.push(singleObj);
                return true;
            }
        );
        if (test==0) {
            //-3: add url failed, -2: add function failed,-1: the function name had been existed, 1: successful
        var things = JSON.stringify({ name: values.Name, things: listOfObjects, description: values.Description, parentId: selectItem });
        $.ajax({
            data: things,
            contentType: "application/json",
            type: "POST",
            url: "/Function/AddFunction",
            success: function(message) {
                if (message == -1) {
                    webix.alert("同等级的权限名已经存在！");
                }
                
                if (message == -2) {
                    webix.alert("添加权限失败！");
                }
                
                if (message == -3) {
                    webix.alert("添加权限成功，URL添加失败！");
                   // window.location = "/Manage/Function";
                }
                else { window.location = "/Function/Function"; }
                

            },
            error: function(data) {
                if (data.status == 401) {
                    webix.alert("操作受限");
                }
                else {
                webix.message("添加失败！");
            }
        }
        });
    }
}

}
