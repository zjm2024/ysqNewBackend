using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using WebUI.Common;
using SPLibrary.CustomerManagement.VO;

namespace WebUI.RequireManagement
{
    public partial class DemandBrowse : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "商机管理";
                (this.Master as Shared.MasterPage).PageNameText = "需求列表";
            }

            CustomerViewVO customerVO = SiteCommon.GetCustomerById(CustomerProfile.CustomerId);
            bool isBusiness = false;
            if (customerVO.BusinessId > 0 && customerVO.BusinessStatus == 1)
                isBusiness = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("var isBusiness= ").Append(isBusiness.ToString().ToLower()).Append(";\n");
            Utilities.RegisterJs(Page, "JSCommonVar_RequirementBrowse", sb.ToString());
        }
    }
}