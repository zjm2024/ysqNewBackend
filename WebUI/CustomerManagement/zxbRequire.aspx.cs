using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.WebConfigInfo;

namespace WebUI.CustomerManagement
{
    public partial class zxbRequire : CustomerBasePage
    {
        public string str;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我的奖励";
                (this.Master as Shared.MasterPage).PageNameText = "奖励列表";
            }
            str = CacheSystemConfig.GetSystemConfig().zxbNote;
        }
    }
}