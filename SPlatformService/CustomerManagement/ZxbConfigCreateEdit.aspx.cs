using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.CustomerManagement.VO;
using WebUI.Common;

namespace SPlatformService.CustomerManagement
{
    public partial class ZxbConfigCreateEdit : BasePage
    {
        public int mid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "乐币奖励";
                //this.Master.PageNameText = "乐币奖励";
            }

            base.ValidPageRight("乐币奖励", "Read");

            int ZxbConfigID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["ZxbConfigID"]) ? "0" : Request.QueryString["ZxbConfigID"]);
            this.hidCustomerId.Value = ZxbConfigID.ToString();
            mid = ZxbConfigID;

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidZxbConfigID='").Append(hidCustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_ZxbConfigCreateEdit", sb.ToString());
        }
    }
}