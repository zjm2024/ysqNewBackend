using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService.CustomerManagement
{
    public partial class BusinessCardPayOutHandle : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "会员提现";
                (this.Master as Shared.MasterPage).PageNameText = "会员提现";
            }

            base.ValidPageRight("会员提现", "Read");
            int PayOutHistoryId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PayOutHistoryId"]) ? "0" : Request.QueryString["PayOutHistoryId"]);
            this.hidPayOutHistoryId.Value = PayOutHistoryId.ToString();


            StringBuilder sb = new StringBuilder();
            sb.Append("var hidPayOutHistoryId='").Append(hidPayOutHistoryId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CustomerPayOutHandle", sb.ToString());
        }
    }
}