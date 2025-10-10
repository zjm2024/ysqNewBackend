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
    public partial class FarmgamePrizeCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "星选农场";
                (this.Master as Shared.MasterPage).PageNameText = "兑换奖品";
            }
            base.ValidPageRight("兑换奖品", "Read");

            int systemMessageId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["PrizeID"]) ? "0" : Request.QueryString["PrizeID"]);
            this.PrizeID.Value = systemMessageId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidPrizeID='").Append(PrizeID.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_FarmgamePrizeCreateEdit", sb.ToString());
        }
    }
}