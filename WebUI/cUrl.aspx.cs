using CoreFramework.VO;
using SPLibrary.BusinessCardManagement.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI
{
    public partial class cUrl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string code = string.IsNullOrEmpty(Request.QueryString["code"]) ? "" : Request.QueryString["code"];
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            string url = cBO.FindShortUrl(code);
            if (url != "")
            {
                Server.Transfer(url.Replace("https://zhongxiaole.net/", "./"), true);
            }else
            {
                Response.Write("链接已失效！");
            }
            
        }
    }
}