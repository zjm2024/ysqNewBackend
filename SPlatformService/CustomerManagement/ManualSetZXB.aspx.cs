using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;

namespace SPlatformService.CustomerManagement
{
    public partial class ManualSetZXB : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "会员管理";
                (this.Master as Shared.MasterPage).PageNameText = "发放奖励";
            }

            base.ValidPageRight("会员管理", "Read");

            int customerId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);
            this.hidCustomerId.Value = customerId.ToString();            
            

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidCustomerId='").Append(hidCustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CustomerCreateEdit", sb.ToString());
        }
    }
}