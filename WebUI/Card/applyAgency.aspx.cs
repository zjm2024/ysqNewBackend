using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class applyAgency : System.Web.UI.Page
    {
        public string priceclass = "";
        public string Token = "";
        public int CustomerId = 0;
        public int Type = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            wxlogin.login();
            Token = wxlogin.Token;
            CustomerId = wxlogin.CustomerId;

            Type = Convert.ToInt32(string.IsNullOrEmpty(Request["Type"]) ? "0" : Request["Type"]);
            if (Type == 6)
            {
                priceclass = "合伙人";
            }
            if (Type == 7)
            {
                priceclass = "分公司";
            }
        }
    }
}