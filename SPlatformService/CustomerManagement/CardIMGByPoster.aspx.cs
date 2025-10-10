using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService.CustomerManagement
{
    public partial class CardIMGByPoster : System.Web.UI.Page
    {
        public string QRimg { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            QRimg = string.IsNullOrEmpty(Request.QueryString["QRimg"]) ? "" : Server.UrlDecode(Request.QueryString["QRimg"]);
            QRimg = QRimg.Replace("https", "http");
        }
    }
}