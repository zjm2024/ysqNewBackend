using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using CoreFramework.VO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SPLibrary.RequireManagement.VO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.CustomerManagement.BO;
using SPlatformService.Controllers;

namespace SPlatformService.RequireManagement
{
    public partial class CardAdd : BasePage
    {
        public string Headimg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐聊名片";
                (this.Master as Shared.MasterPage).PageNameText = "新建名片";
            }

            base.ValidPageRight("名片信息", "Read");

            Headimg = new CardController().GetRandomHeadimg().Result.ToString();
        }
    }
}