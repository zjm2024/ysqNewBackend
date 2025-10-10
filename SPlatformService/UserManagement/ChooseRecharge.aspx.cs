using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;

namespace SPlatformService.UserManagement
{
    public partial class ChooseRecharge : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtCost.ToolTip = "请输入金额";
        }
    }
}