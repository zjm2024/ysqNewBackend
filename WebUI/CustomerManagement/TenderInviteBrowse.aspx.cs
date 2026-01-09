using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.CustomerManagement.VO;
using WebUI.Common;

namespace WebUI.CustomerManagement
{
    public partial class TenderInviteBrowse : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是销售";
                (this.Master as Shared.MasterPage).PageNameText = "我的投标";
            }
            CustomerViewVO customerVO = SiteCommon.GetCustomerById(CustomerProfile.CustomerId);
            bool isAgency = false;
            if (customerVO.AgencyId > 0 && customerVO.AgencyStatus == 1)
                isAgency = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("var isAgency= ").Append(isAgency.ToString().ToLower()).Append(";\n");
            Utilities.RegisterJs(Page, "JSCommonVar_TenderInviteBrowse", sb.ToString());
        }
    }
}