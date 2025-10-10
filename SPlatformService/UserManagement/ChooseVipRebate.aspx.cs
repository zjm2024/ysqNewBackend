using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;

namespace SPlatformService.UserManagement
{
    public partial class ChooseVipRebate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtCustomerId.ToolTip = "请输入会员ID";
        }
    }
}