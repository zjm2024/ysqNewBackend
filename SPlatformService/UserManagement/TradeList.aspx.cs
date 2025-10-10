using SPLibrary.CoreFramework.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService.UserManagement
{
    public partial class TradeList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "财务管理";
                (this.Master as Shared.MasterPage).PageNameText = "交易列表";
            }
            base.ValidPageRight("交易列表", "Read");
        }
    }
}