using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using CoreFramework.VO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework;
using WebUI.Common;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.ProjectManagement.BO;

namespace WebUI.CustomerManagement
{
    public partial class BusinessCreateEdit : CustomerBasePage
    {
        public int ProjectCount { get; set; }
        public string page { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是雇主";
                (this.Master as Shared.MasterPage).PageNameText ="雇主认证";
            }
            page = string.IsNullOrEmpty(Request.QueryString["page"]) ? "BusinessInfo" : Request.QueryString["page"];
        }
    }
}