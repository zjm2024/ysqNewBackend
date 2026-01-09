using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService.RequireManagement
{
    public partial class DemandBrowse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "商机管理";
                (this.Master as Shared.MasterPage).PageNameText = "商机列表";
            }
        }
    }
}