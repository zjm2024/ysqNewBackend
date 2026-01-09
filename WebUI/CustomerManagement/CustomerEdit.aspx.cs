using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CustomerManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUI.Common;

namespace WebUI.CustomerManagement
{
    public partial class CustomerEdit : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Master != null)
            {
                (base.Master as Shared.MasterPage).MenuText = "账号信息";
            }            

            int customerId = CustomerProfile.CustomerId;
            this.hidCustomerId.Value = customerId.ToString();


            StringBuilder sb = new StringBuilder();
            sb.Append("var hidCustomerId='").Append(hidCustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CustomerCreateEdit", sb.ToString());

            if(customerId > 0)
            {
                CustomerViewVO customerVO = SiteCommon.GetCustomerById(customerId);                

                imgHeaderLogoPic.Src = customerVO.HeaderLogo;
                txtCustomerCode.Text = customerVO.CustomerCode;
                txtCustomerAccount.Text = customerVO.CustomerAccount;
                txtPhone.Text = customerVO.Phone;
                txtCustomerName.Text = customerVO.CustomerName;
                txtBirthday.Text = customerVO.Birthday.ToString("yyyy-MM-dd");                
                txtEmail.Text = customerVO.Email;
                txtDescription.Text = customerVO.Description;

                if (customerVO.Sex)
                    radSexMale.Checked = true;
                else
                    radSexFeMale.Checked = true;
                
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
                    btn_ApplicantBusiness.Visible = false;
                }
                else
                {
                    lblBusinessInfo.Text = "未认证";
                    btn_ViewBusiness.Visible = false;
                    btn_ApplicantBusiness.Visible = true;
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
                    btn_ApplicantAgency.Visible = false;
                }
                else
                {
                    lblAgencyInfo.Text = "未认证";
                    btn_ViewAgency.Visible = false;
                    btn_ApplicantAgency.Visible = true;
                }
            }
        }
        protected void UserLogout(object sender, EventArgs e)
        {
            HttpContext.Current.Session["#Session#TOKEN"] = null;
            IUserPrincipal iup = new UserPrincipal();
            FormsAuthentication.SignOut();
            UserPrincipal.ClearProfileSession();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}