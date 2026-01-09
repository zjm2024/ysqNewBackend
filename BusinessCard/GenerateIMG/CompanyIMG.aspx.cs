using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BusinessCard.GenerateIMG
{
    public partial class CompanyIMG : System.Web.UI.Page
    {
        public string Headimg { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Headimg = string.IsNullOrEmpty(Request.QueryString["Headimg"]) ? "" : Request.QueryString["Headimg"];
        }
    }
}