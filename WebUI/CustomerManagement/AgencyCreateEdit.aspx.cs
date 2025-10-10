using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework;
using WebUI.Common;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.ProjectManagement.BO;

namespace WebUI.CustomerManagement
{
    public partial class AgencyCreateEdit : CustomerBasePage
    {
        public int ProjectCount { get; set; }
        public string page { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是销售";
                (this.Master as Shared.MasterPage).PageNameText = "销售认证";
            }
            page = string.IsNullOrEmpty(Request.QueryString["page"]) ? "AgencyInfo" : Request.QueryString["page"];
        }
    }
}