using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUI.Common;

namespace WebUI.CustomerManagement
{
    public partial class SuggestionCreate : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "意见反馈";
                //(this.Master as Shared.MasterPage).PageNameText = "任务发布";
            }
            int customerId = CustomerProfile.CustomerId;
            if (customerId > 0)
            {
                CustomerViewVO customerVO = SiteCommon.GetCustomerById(customerId);
                txtContactPerson.Text = customerVO.CustomerName;
                txtPhone.Text = customerVO.Phone;
            }
        }
    }
}