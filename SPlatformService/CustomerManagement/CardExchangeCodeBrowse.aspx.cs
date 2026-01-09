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
using SPLibrary.CoreFramework.WebConfigInfo;

namespace SPlatformService.CustomerManagement
{
    public partial class CardExchangeCodeBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                (this.Master as Shared.MasterPage).PageNameText = "名片信息";
            }
            base.ValidPageRight("名片信息", "Read");
            int systemCustomerId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);
            this.CustomerId.Value = systemCustomerId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidCustomerId='").Append(CustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CardBusinessCardCreateEdit", sb.ToString());
        }
    }
}