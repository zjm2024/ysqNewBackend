using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BusinessCard.GenerateIMG
{
    public partial class AllQRIMG : System.Web.UI.Page
    {
        public string url { set; get; }
        public string text { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            url = string.IsNullOrEmpty(Request.QueryString["url"]) ? "0" : Request.QueryString["url"];
            text = string.IsNullOrEmpty(Request.QueryString["text"]) ? "" : Server.UrlDecode(Request.QueryString["text"]);
        }
    }
}