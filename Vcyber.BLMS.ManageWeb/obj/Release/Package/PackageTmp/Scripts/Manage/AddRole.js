
  var addForm = [
       {
                view: "text",
                label: "角色名：",
                id: "Name",
                name: "Name",
                labelAlign: "right",
                placeholder: "必填项，且最多50个字",
                validate: webix.rules.isNotEmpty,
                inputWidth: 500,
                labelWidth: 100
          
        },
  {
      view: "textarea",
      label: "角色描述：",
      id: "Description",
      name: "Description",
      inputWidth: 500,
      height: 300,
      labelAlign: "right",
      placeholder: "最多200个字",
      labelWidth: 100
  },
        {
            align: "center",
            cols: [
                { view: "button", value: "保存", type: "form", click: "getForm", width: 100 ,align:"center"},
                { view: "button", value: "关闭", click: "closeWin('addWin')", width: 100,align:"center" }
            ]
        }
  ];

  var addHeader = {
      view: "toolbar",
      cols: [
          { view: "label", label: "角色管理 - 添加角色", align: "left" }
      ]
  };

var addPage = {
      rows: [
          {
              cols: [
                  {
                      view: "form",
                      id: "my_form",
                      width: 800,
                      align: "center",
                      elements: addForm,
                      elementsConfig: {
                          labelAlign: "left",
                          labelWidth: 100
                      },
                      //rules: {
                      //    Name: function (value)
                      //    {
                      //        if (value == null || value.trim() == ""||value.length>50) {
                      //            return false;
                      //        }
                      //        return true;
                      //    },
                      //    Description: function(value) {
                      //        if (value.length > 200) {
                      //            return false;
                      //        }
                      //        return true;
                      //    }
                      //}
                  }]
          }
      ]
  };

function getForm() {

    if ($$("my_form").validate()) {
  
        var values = $$('my_form').getValues();
        $.ajax({
            data: {
                Name: values.Name,
                Describe: values.Description,
            },
            type: "POST",
            url: "/Roles/AddRole",
            success: function (message) {
                if(message.id != 1) {
                    webix.alert(message.message);
                    $$("my_form").clear();
                }else {

                    window.location = "/Roles/Role";
                }
            },
            error: function (data) {
                if (data.status == 401) {
                    webix.alert("操作受限");
                }
                else{
                    webix.message("添加失败！");
                }
            }
        });
    }
}

function returnRole() {
    window.location = "/Roles/Role";
}