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
    public partial class AgencyReviewEdit : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是销售";
                (this.Master as Shared.MasterPage).PageNameText = "评价详情";
            }
            int businessReviewId = string.IsNullOrEmpty(Request.QueryString["businessReviewId"]) ? 0 : Convert.ToInt32(Request.QueryString["businessReviewId"]);
            Utilities.RegisterJs(Page, "JSCommonVar_AgencyCreated", " var _BusinessReviewId = " + businessReviewId + ";");
        }
    }
}