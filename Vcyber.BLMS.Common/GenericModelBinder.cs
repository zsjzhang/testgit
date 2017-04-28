using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Vcyber.BLMS.Common
{
    public class GenericModelBinder<T> : IModelBinder
    {
        #region 变量
        #endregion

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var json = controllerContext.HttpContext.Request[bindingContext.ModelName] as string;
            if (json == null) return null;
            //Logger.Info(bindingContext.ModelName + ": " + json);

            JavaScriptSerializer serialize = new JavaScriptSerializer();
            return serialize.Deserialize<T>(json);
        }
    }
}
