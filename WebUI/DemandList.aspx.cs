using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI
{
    public partial class DemandList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string searchTxt = string.IsNullOrEmpty(Request.QueryString["SearchValue"]) ? "" : Request.QueryString["SearchValue"];
            if (!string.IsNullOrEmpty(searchTxt))
            {
                txtSearcha.Value = searchTxt;
            }
        }
    }
}