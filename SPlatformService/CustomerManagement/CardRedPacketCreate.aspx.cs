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
    public partial class CardRedPacketCreate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                (this.Master as Shared.MasterPage).PageNameText = "红包发放";
            }
            base.ValidPageRight("乐聊名片", "Read");

            int systemMessageId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["RedPacketId"]) ? "0" : Request.QueryString["RedPacketId"]);
            this.RedPacketId.Value = systemMessageId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var RedPacketId='").Append(RedPacketId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardMessageCreateEdit", sb.ToString());
        }
    }
}