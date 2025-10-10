using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Web.UI.WebControls;
using SPLibrary.CustomerManagement.VO;
using WebUI.Common;
using SPLibrary.RequireManagement.BO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.ProjectManagement.VO;
using SPLibrary.WebConfigInfo;
using SPLibrary.BussinessManagement.BO;

namespace WebUI.ProjectManagement
{
    public partial class GenerateProject : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "我是雇主";
                (this.Master as Shared.MasterPage).PageNameText = "雇佣合同";
            }
            int requireId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["RequireId"]) ? "0" : Request.QueryString["RequireId"]);

            int customerId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CustomerId"]) ? "0" : Request.QueryString["CustomerId"]);

            int contractId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["ContractId"]) ? "0" : Request.QueryString["ContractId"]);
            this.hidContractId.Value = contractId.ToString();

            RequireBO rBO = new RequireBO(new CustomerProfile());
            ProjectBO pBO = new ProjectBO(new CustomerProfile());
            BusinessBO bBO = new BusinessBO(new CustomerProfile());
            AgencyBO aBO = new AgencyBO(new CustomerProfile());

            StringBuilder sb = new StringBuilder();
            if (contractId > 0)
            {
                ContractViewVO cVO = pBO.FindContractById(contractId);

                txtProjectName.Text = cVO.ProjectName;
                txtCost.Text = cVO.Cost.ToString();
                txtCommission.Text = cVO.Commission.ToString();
                txtStartDate.Text = cVO.StartDate.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : cVO.StartDate.ToString("yyyy-MM-dd");
                txtEndDate.Text = cVO.EndDate.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : cVO.EndDate.ToString("yyyy-MM-dd");

                if (cVO.BusinessStatus == 1)
                { huzhutonghi.Style.Remove("display"); }
                else
                { huzhutonghi.Style.Add("display", "none"); }
                if (cVO.AgencyStatus == 1)
                { xiaoshoutonghi.Style.Remove("display"); }
                else
                { xiaoshoutonghi.Style.Add("display", "none"); }

                //绑定附件
                List<ContractFileVO> fileList = pBO.FindContractFileList(contractId);
                string oTR = "";
                for (int i = 0; i < fileList.Count; i++)
                {
                    ContractFileVO tcVO = fileList[i];
                    
                    oTR += "   <div class=\"jqgrow\"  onclick=\"DeleteFile(this)\"> \r\n";
                    var fileName = tcVO.FileName;
                    var suffixIndex = fileName.LastIndexOf(".");
                    var suffix = fileName.Substring(suffixIndex + 1).ToUpper();
                    oTR += "<a href=\"" + tcVO.FilePath + "\" title=\"" + tcVO.FileName + "\">";
                    if (suffix != "BMP" && suffix != "JPG" && suffix != "JPEG" && suffix != "PNG" && suffix != "GIF")
                    {
                        oTR += "  <div class=\"File\"></div>\r\n";
                    }
                    else
                    {
                        oTR += "  <div class=\"img\" style=\"background-image:url(" + tcVO.FilePath + ")\"></div>\r\n";
                    }
                    oTR += "</a>";
                    oTR += "      <p class=\"center\" title=\"" + tcVO.FileName + "\">" + tcVO.FileName + "</p> \r\n";
                    oTR += "      <p class=\"JqgrowDel\" title=\"删除\">删除</p> \r\n";
                    oTR += "      <input type=\"hidden\" name=\"File_FileName\" value=\"" + tcVO.FileName + "\" /> \r\n";
                    oTR += "      <input type=\"hidden\" name=\"File_FilePath\" value=\"" + tcVO.FilePath + "\" /> \r\n";
                    oTR += "      <input type=\"hidden\" name=\"File_CreatedAt\" value=\"" + tcVO.CreatedAt + "\" /> \r\n";
                    oTR += "      <input type=\"hidden\" name=\"File_ContractId\" value=\"" + tcVO.ContractId + "\" /> \r\n";
                    oTR += "   </div> \r\n";
                }
                oTR += "<div class=\"G_JieDuan_file_add\">";
                oTR += "<input id = \"fileAdd\" name = \"id-input-file\" type = \"file\" onchange = \"changefile('fileAdd')\" />";
                oTR += "</div> ";
                divFileList.InnerHtml = oTR;

                if (cVO.BusinessCustomerId == CustomerProfile.CustomerId)
                {
                    //当前是雇主

                    //根据状态显示按钮
                    if (cVO.AgencyStatus == 0 && cVO.BusinessStatus == 0)
                    {
                        btn_save.Style["display"] = "";
                    }
                    else
                    {
                        btn_save.Style["display"] = "none";
                    }

                    if (cVO.BusinessStatus == 0)
                    {
                        btn_businesscancel.Style["display"] = "none";
                        btn_businessapprove.Style["display"] = "";
                    }
                    else
                    {
                        bool isHasPayed = false;
                        ProjectVO pVO = pBO.FindProjectByContract(contractId);
                        if (pVO != null)
                        {
                            isHasPayed = pBO.IsHasPayed(pVO.ProjectId);
                        }
                        if (isHasPayed)
                            btn_businesscancel.Style["display"] = "none";
                        else
                            btn_businesscancel.Style["display"] = "";
                        btn_businessapprove.Style["display"] = "none";
                    }

                    btn_agencycancel.Style["display"] = "none";
                    btn_agencyapprove.Style["display"] = "none";
                }
                else if (cVO.AgencyCustomerId == CustomerProfile.CustomerId)
                {
                    //当前是销售
                    //根据状态显示按钮
                    if (cVO.AgencyStatus == 0 && cVO.BusinessStatus == 0)
                    {
                        btn_save.Style["display"] = "";
                    }
                    else
                    {
                        btn_save.Style["display"] = "none";
                    }

                    btn_businesscancel.Style["display"] = "none";
                    btn_businessapprove.Style["display"] = "none";

                    if (cVO.AgencyStatus == 0)
                    {
                        btn_agencycancel.Style["display"] = "none";
                        btn_agencyapprove.Style["display"] = "";
                    }
                    else
                    {
                        bool isHasPayed = false;
                        ProjectVO pVO = pBO.FindProjectByContract(contractId);
                        if (pVO != null)
                        {
                            isHasPayed = pBO.IsHasPayed(pVO.ProjectId);
                        }
                        if (isHasPayed)
                            btn_agencycancel.Style["display"] = "none";
                        else
                            btn_agencycancel.Style["display"] = "";
                        btn_agencyapprove.Style["display"] = "none";
                    }
                }

                BusinessViewVO bVO = bBO.FindBusinessById(cVO.BusinessId);
                AgencyViewVO aVO = aBO.FindAgencyById(cVO.AgencyId);
                RequirementViewVO rVO = rBO.FindRequireById(cVO.RequirementId);

                lblCompanyName.InnerHtml = bVO.CompanyName;
                lblAgencyName.InnerHtml = aVO.AgencyName;
                lblCompanyName2.InnerHtml = bVO.CompanyName;
                lblAgencyName2.InnerHtml = aVO.AgencyName;
                lblBusinessLicense.InnerHtml = bVO.BusinessLicense;
                lblBusinessTel.InnerHtml = rVO.Phone;
                lblCode.InnerHtml = rVO.RequirementCode;
                lblrequirement.InnerHtml = rVO.Title;
                lblAgencyTel.InnerHtml = aVO.Phone;
                lblIDCard.InnerHtml = aVO.IDCard;
                if(cVO.BusinessSignDate.Year != 1900)
                    lblBusinessTime.InnerHtml = cVO.BusinessSignDate.ToString();
                if (cVO.AgencySignDate.Year != 1900)
                    lblAgencyTime.InnerHtml = cVO.AgencySignDate.ToString();

                sb.Append("var _RequireId='").Append(cVO.RequirementId).Append("';\n");
                sb.Append("var _AgencyCustomerId='").Append(cVO.AgencyCustomerId).Append("';\n");
                sb.Append("var _BusinessCustomerId='").Append(cVO.BusinessCustomerId).Append("';\n");
                sb.Append("var _BusinessStatus=").Append(cVO.BusinessStatus).Append(";\n");
                sb.Append("var _AgencyStatus=").Append(cVO.AgencyStatus).Append(";\n");
            }
            else
            {
                CustomerViewVO customerBusinessVO = SiteCommon.GetCustomerById(CustomerProfile.CustomerId);
                CustomerViewVO customerAgencyVO = SiteCommon.GetCustomerById(customerId);

                sb.Append("var _RequireId='").Append(requireId).Append("';\n");
                sb.Append("var _AgencyCustomerId='").Append(customerId).Append("';\n");
                sb.Append("var _BusinessCustomerId='").Append(CustomerProfile.CustomerId).Append("';\n");
                sb.Append("var _AgencyId='").Append(customerAgencyVO.AgencyId).Append("';\n");
                sb.Append("var _BusinessId='").Append(customerBusinessVO.BusinessId).Append("';\n");
                sb.Append("var _BusinessStatus=0;\n");
                sb.Append("var _AgencyStatus=0;\n");

                //默认开始结束时间
                txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtEndDate.Text = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");

                // 默认 ProjectName,Cost,Commission
                RequirementViewVO rVO = rBO.FindRequireById(requireId);

                txtProjectName.Text = rVO.Title;
                txtCost.Text = rVO.Cost.ToString();
                txtCommission.Text = rVO.Commission.ToString();

                BusinessViewVO bVO = bBO.FindBusinessById(customerBusinessVO.BusinessId);
                AgencyViewVO aVO = aBO.FindAgencyById(customerAgencyVO.AgencyId);

                lblCompanyName.InnerHtml = bVO.CompanyName;
                lblAgencyName.InnerHtml = aVO.AgencyName;
                lblCompanyName2.InnerHtml = bVO.CompanyName;
                lblAgencyName2.InnerHtml = aVO.AgencyName;
                lblBusinessLicense.InnerHtml = bVO.BusinessLicense;
                lblBusinessTel.InnerHtml = rVO.Phone;
                lblCode.InnerHtml = rVO.RequirementCode;
                lblrequirement.InnerHtml = rVO.Title;
                lblAgencyTel.InnerHtml = aVO.Phone;
                lblIDCard.InnerHtml = aVO.IDCard;

                btn_save.Style["display"] = "";
                btn_businesscancel.Style["display"] = "none";
                btn_businessapprove.Style["display"] = "none";
                btn_agencycancel.Style["display"] = "none";
                btn_agencyapprove.Style["display"] = "none";
            }


            sb.Append("var hidContractId='").Append(hidContractId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_GenerateProject", sb.ToString());
        }
    }
}