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
    public partial class SelectPartyCard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                (this.Master as Shared.MasterPage).PageNameText = "活动信息";
            }

            base.ValidPageRight("活动信息", "Read");

            int businessId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PartID"]) ? "0" : Request.QueryString["PartID"]);
            this.hidPartID.Value = businessId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidPartID='").Append(hidPartID.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_SelectPartyCard", sb.ToString());
        }
    }
}