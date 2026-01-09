using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Web.UI.WebControls;

namespace WebUI.CustomerManagement
{
    public partial class MessageCreateEdit : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我的消息";
                (this.Master as Shared.MasterPage).PageNameText = "消息查看";
            }            

            int messageId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["messageId"]) ? "0" : Request.QueryString["messageId"]);
            this.hidMessageId.Value = messageId.ToString();
            
            StringBuilder sb = new StringBuilder();
            sb.Append("var hidMessageId='").Append(hidMessageId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_MessageCreateEdit", sb.ToString());
        }
        
    }
}