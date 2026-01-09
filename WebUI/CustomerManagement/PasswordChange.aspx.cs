using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.CustomerManagement
{
    public partial class PasswordChange : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "个人中心";
                (this.Master as Shared.MasterPage).PageNameText = "修改密码";
            }

            //base.ValidPageRight("修改密码", "View");

            this.hidCustomerId.Value = CustomerProfile.CustomerId.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidCustomerId='").Append(hidCustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CustomerEdit", sb.ToString());
        }
    }
}