using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SPlatformService.UserManagement
{
    public partial class CardBusinessCardCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "企业名片";
                (this.Master as Shared.MasterPage).PageNameText = "企业管理";
            }
            base.ValidPageRight("企业管理", "Read");

            int systemMessageId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["BusinessID"]) ? "0" : Request.QueryString["BusinessID"]);
            this.BusinessID.Value = systemMessageId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidBusinessID='").Append(BusinessID.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardBusinessCardCreateEdit", sb.ToString());
        }
    }
}