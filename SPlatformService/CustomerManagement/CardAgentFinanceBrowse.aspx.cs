using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.CustomerManagement.BO;
using CoreFramework.VO;

namespace SPlatformService.CustomerManagement
{
    public partial class CardAgentFinanceBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "月结清单";
                //this.Master.PageNameText = "会员管理";
            }
            base.ValidPageRight("代理商", "Read");

            int systemCustomerId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);
            this.CustomerId.Value = systemCustomerId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var CustomerId='").Append(CustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardMessageCreateEdit", sb.ToString());

            int AppType = UserProfile.AppType;
            CardBO cBO = new CardBO(new CustomerProfile(), AppType);
            cBO.setAgentFinance(systemCustomerId);
        }
    }
}