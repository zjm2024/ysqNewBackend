using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class WxLink : System.Web.UI.Page
    {
        public string url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = string.IsNullOrEmpty(Request.QueryString["path"]) ? "" : Request.QueryString["path"];
            string query = string.IsNullOrEmpty(Request.QueryString["query"]) ? "" : Request.QueryString["query"];
            int AppType = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["AppType"]) ? "1" : Request.QueryString["AppType"]);

            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            url = cBO.GetUrlScheme(path, query, 1);
            //url = cBO.GetUrlLink(path, query, 1);
        }
    }
}