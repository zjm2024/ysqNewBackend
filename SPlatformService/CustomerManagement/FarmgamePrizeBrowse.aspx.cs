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
    public partial class FarmgamePrizeBrowse : BasePage
    {
        public int Number { set; get; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "星选农场";
                (this.Master as Shared.MasterPage).PageNameText = "兑换奖品";
            }
            base.ValidPageRight("兑换奖品", "Read");  
        }
    }
}