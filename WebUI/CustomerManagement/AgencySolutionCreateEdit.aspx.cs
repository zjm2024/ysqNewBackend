using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.CustomerManagement
{
    public partial class AgencySolutionCreateEdit : System.Web.UI.Page
    {
        public string APIURL
        {
            get
            {
                return ConfigInfo.Instance.APIURL;
            }
        }
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
            StringBuilder sb = new StringBuilder();
            sb.Append("var ImgPath='").Append((Request.ApplicationPath == "/" ? "" : Request.ApplicationPath) + "/Style/images/").Append("';\n");

            Utilities.RegisterJs(Page, "JSCommonVar", sb.ToString());

        }
    }
}