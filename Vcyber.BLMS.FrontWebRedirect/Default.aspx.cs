using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vcyber.BLMS.FrontWebRedirect
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var host = Request.Url.Host.ToLower();
            if (host.Equals(ConfigurationManager.AppSettings["Level2Domain80Host"]))
            {
                Response.Redirect(ConfigurationManager.AppSettings["Level2Domain"]);

            }
            else if (host.Equals(ConfigurationManager.AppSettings["Level3Domain80Host"]))
            {
                Response.Redirect(ConfigurationManager.AppSettings["Level3Domain"]);
            }

            Response.Redirect(ConfigurationManager.AppSettings["Level1Domain"]); 
        }
    }
}