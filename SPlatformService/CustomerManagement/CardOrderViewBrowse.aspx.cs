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

namespace SPlatformService.UserManagement
{
    public partial class CardOrderViewBrowse : BasePage
    {
        public int Number { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                (this.Master as Shared.MasterPage).PageNameText = "VIP订单";
            }
            base.ValidPageRight("VIP订单", "Read");

            int systemMessageId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["NoticeID"]) ? "0" : Request.QueryString["NoticeID"]);
            this.NoticeID.Value = systemMessageId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidNoticeID='").Append(NoticeID.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardMessageCreateEdit", sb.ToString());
        }
    }
}