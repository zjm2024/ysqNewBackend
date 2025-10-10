using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;

namespace SPlatformService.CustomerManagement
{
    public partial class CardPersonalBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "企业名片";
                (this.Master as Shared.MasterPage).PageNameText = "企业管理";
            }

            base.ValidPageRight("企业管理", "Read");

            int systemBusinessID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["BusinessID"]) ? "0" : Request.QueryString["BusinessID"]);
            this.BusinessID.Value = systemBusinessID.ToString();

            int systemCustomerId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);
            this.CustomerId.Value = systemCustomerId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var BusinessID='").Append(BusinessID.ClientID).Append("';\n");
            sb.Append("var CustomerId='").Append(CustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardMessageCreateEdit", sb.ToString());
        }
    }
}