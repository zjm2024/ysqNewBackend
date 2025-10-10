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
    public partial class Address : System.Web.UI.Page
    {
        public string Headimg { set; get; }
        public string Type { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Headimg = string.IsNullOrEmpty(Request.QueryString["Headimg"]) ? "" : Server.UrlDecode(Request.QueryString["Headimg"]);
            Type = string.IsNullOrEmpty(Request.QueryString["Type"]) ? "" : Server.UrlDecode(Request.QueryString["Type"]);
        }
    }
}