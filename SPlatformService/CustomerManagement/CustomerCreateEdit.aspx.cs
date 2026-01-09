using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.CustomerManagement.VO;
using WebUI.Common;

namespace SPlatformService.CustomerManagement
{
    public partial class CustomerCreateEdit : BasePage
    {
        public int mid;
        protected int BusinessId;
        protected int AgencyId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "会员管理";
                //this.Master.PageNameText = "部门管理";
            }

            base.ValidPageRight("会员管理", "Read");

            int customerId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);
            this.hidCustomerId.Value = customerId.ToString();
            mid = customerId;

            if (customerId > 0)
            {
                CustomerViewVO customerVO = SiteCommon.GetCustomerById(customerId);

                if (customerVO.BusinessId > 0)
                {
                    if (customerVO.BusinessStatus == 0)
                    {
                        lblBusinessInfo.Text = "审核中";
                    }
                    else if (customerVO.BusinessStatus == 1)
                    {
                        lblBusinessInfo.Text = "已认证";
                    }
                    else if (customerVO.BusinessStatus == 2)
                    {
                        lblBusinessInfo.Text = "已拒绝";
                    }
                    btn_ViewBusiness.Visible = true;
                    BusinessId = customerVO.BusinessId;
                    btn_ViewBusiness.ServerClick += new System.EventHandler(this.BusinessClick);
                }
                else
                {
                    lblBusinessInfo.Text = "未认证";
                    btn_ViewBusiness.Visible = false;
                }

                if (customerVO.AgencyId > 0)
                {
                    if (customerVO.AgencyStatus == 0)
                    {
                        lblAgencyInfo.Text = "审核中";
                    }
                    else if (customerVO.AgencyStatus == 1)
                    {
                        lblAgencyInfo.Text = "已认证";
                    }
                    else if (customerVO.AgencyStatus == 2)
                    {
                        lblAgencyInfo.Text = "已拒绝";
                    }
                    btn_ViewAgency.Visible = true;
                    AgencyId = customerVO.AgencyId;
                    btn_ViewAgency.ServerClick += new System.EventHandler(this.AgencyClick);
                }
                else
                {
                    lblAgencyInfo.Text = "未认证";
                    btn_ViewAgency.Visible = false;
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidCustomerId='").Append(hidCustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CustomerCreateEdit", sb.ToString());
        }
        public void BusinessClick(object sender, EventArgs e) {
            Response.Redirect("BusinessCreateEdit.aspx?BusinessId="+BusinessId);
        }
        public void AgencyClick(object sender, EventArgs e)
        {
            Response.Redirect("AgencyCreateEdit.aspx?AgencyId=" + AgencyId);
        }
    }
}