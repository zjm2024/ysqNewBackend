using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService.CustomerManagement
{
    public partial class CustomerPayOutBrowse : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            (this.Master as Shared.MasterPage).MenuText = "会员提现";
            (this.Master as Shared.MasterPage).PageNameText = "提现申请列表";
        }
    }
}