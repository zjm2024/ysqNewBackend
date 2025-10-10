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
    public partial class CardRedPacketListBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            (this.Master as Shared.MasterPage).MenuText = "红包列表";
            (this.Master as Shared.MasterPage).PageNameText = "领取记录";

            int systemRedPacketId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["RedPacketId"]) ? "0" : Request.QueryString["RedPacketId"]);
            this.RedPacketId.Value = systemRedPacketId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var RedPacketId='").Append(RedPacketId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardMessageCreateEdit", sb.ToString());
        }
    }
}