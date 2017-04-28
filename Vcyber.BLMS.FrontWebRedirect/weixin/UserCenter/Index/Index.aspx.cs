using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vcyber.BLMS.FrontWebRedirect.weixin.UserCenter
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["weixinPage"]); 
        }
    }
}