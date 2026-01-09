using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService
{
    public partial class NoSecurity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                this.Master.MenuText = "其它";
                this.Master.PageNameText = "权限提示";
            }

        }
    }
}