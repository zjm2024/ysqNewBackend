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
    public partial class qymp : System.Web.UI.Page
    {
        public string url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), 0);
            url = cBO.GetUrlScheme("pages/Welcome/Welcome", "");
        }
    }
}