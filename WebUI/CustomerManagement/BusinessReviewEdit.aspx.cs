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
    public partial class BusinessReviewEdit : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是雇主";
                (this.Master as Shared.MasterPage).PageNameText = "评价详情";
            }
            int agencyReviewId = string.IsNullOrEmpty(Request.QueryString["agencyReviewId"]) ? 0 : Convert.ToInt32(Request.QueryString["agencyReviewId"]);
            Utilities.RegisterJs(Page, "JSCommonVar_BusinessCreated", " var _AgencyReviewId = " + agencyReviewId + ";");
        }
    }
}