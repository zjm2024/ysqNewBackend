using System;
using System.Collections.Generic;
using SPLibrary.CoreFramework.WebConfigInfo;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.ProjectManagement
{
    public partial class ProjectChangeCommission : System.Web.UI.Page
    {
        public string Token
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["#Session#TOKEN"] == null)
                {
                    return null;

                }
                else
                {
                    return HttpContext.Current.Session["#Session#TOKEN"].ToString();
                }
            }
        }
        public int CustomerId
        {
            get
            {
                return new CustomerPrincipal().CustomerProfile.CustomerId;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}