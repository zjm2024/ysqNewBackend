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
    public partial class MiniprogramsCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "企业名片";
                (this.Master as Shared.MasterPage).PageNameText = "小程序管理";
            }
            base.ValidPageRight("小程序管理", "Read");

            int systemMessageId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["AppType"]) ? "0" : Request.QueryString["AppType"]);
            this.AppType.Value = systemMessageId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidAppType='").Append(AppType.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardMessageCreateEdit", sb.ToString());
        }
    }
}