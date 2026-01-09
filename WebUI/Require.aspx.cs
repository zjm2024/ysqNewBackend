using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService.Controllers;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUI.Common;

namespace WebUI
{
    public partial class Require : System.Web.UI.Page
    {
        public string str = "";
        public RequirementViewVO requireVO;
        public string divtargetCategoryStr = "";
        public string divtargetCityStr = "";
        public string oTR = "";
        public string fTR = "";
        public string Token
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["#Session#TOKEN"] == null)
                {
                    return null;

                }
                else
                {
                    return HttpContext.Current.Session["#Session#TOKEN"].ToString();
                }
            }
        }
        public int CustomerId
        {
            get
            {
                return new CustomerPrincipal().CustomerProfile.CustomerId;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = string.IsNullOrEmpty(Request.QueryString["requireId"]) ? "0" : Request.QueryString["requireId"];
            if (id != "0")
            {
                RequireBO uBO = new RequireBO(new CustomerProfile());
                
                RequireController requireCon = new RequireController();
                ResultObject result = requireCon.GetRequireSite(Convert.ToInt32(id));
                if (result == null)
                {
                    return;
                }
                requireVO = result.Result as RequirementViewVO;
                requireVO.MainImg = uBO.getRequireIMG(requireVO.RequirementId);

                if (string.IsNullOrEmpty(CacheSystemConfig.GetSystemConfig().SiteDescription))
                    (this.Master as Shared.MasterPageSite).SiteName = requireVO.Title + "_" + CacheSystemConfig.GetSystemConfig().SiteName;
                else
                    (this.Master as Shared.MasterPageSite).SiteName = requireVO.Title + "_" + CacheSystemConfig.GetSystemConfig().SiteName + "_" + CacheSystemConfig.GetSystemConfig().SiteDescription;
                /*
                lblTitle.InnerHtml = requireVO.Title;
                //lblBusinessName.InnerHtml = requireVO.CustomerName;
                lblBusinessName.InnerText = requireVO.CustomerName;
                lblBusinessName.HRef = "Business.aspx?businessId=" + requireVO.BusinessId;
                lblBusinessName.Title = requireVO.CustomerName;
                lblCommission.InnerHtml = requireVO.Commission.ToString("N2");
                
                lblCreatedAt.InnerHtml = requireVO.CreatedAt.ToString("yyyy-MM-dd");
                lblEffectiveEnd.InnerHtml = requireVO.EffectiveEndDate.ToString("yyyy-MM-dd");
                lblRequireCode.InnerHtml = requireVO.RequirementCode;
                divDescription.InnerHtml = requireVO.Description;

                lblCommissionDescription.InnerHtml = requireVO.CommissionDescription;
                lblTargetAgency.InnerHtml = requireVO.TargetAgency;
                lblDelegationStatus.InnerHtml = ((requireVO.DelegationCommission > 0) ? "是" : "否");
                */
                List<RequirementTargetCategoryViewVO> targetCategoryList = uBO.FindTargetCategoryByRequire(Convert.ToInt32(id));
                List<RequirementTargetCityViewVO> targetCityList = uBO.FindTargetCityByRequire(Convert.ToInt32(id));
                List<RequirementFileVO> fileList = uBO.FindRequireFileByRequire(Convert.ToInt32(id));                
                
                string targetCategoryStr = "";
                foreach (RequirementTargetCategoryViewVO tcVO in targetCategoryList)
                {
                    targetCategoryStr += tcVO.CategoryName + ",";
                }
                if (targetCategoryStr.Length > 0)
                    targetCategoryStr = targetCategoryStr.Substring(0, targetCategoryStr.Length - 1);
                divtargetCategoryStr = targetCategoryStr;

                string targetCityStr = "";
                foreach (RequirementTargetCityViewVO tcVO in targetCityList)
                {
                    targetCityStr += tcVO.CityName + ",";
                }
                if (targetCityStr.Length > 0)
                    targetCityStr = targetCityStr.Substring(0, targetCityStr.Length - 1);

                divtargetCityStr = targetCityStr;
                
                /*
                string fileStr = "";
                for (int i = 0; i < fileList.Count; i++)
                {
                    RequirementFileVO tcVO = fileList[i];
                    fileStr += "<a href=\"" + tcVO.FilePath.Replace("~/www.zhongxiaole.net/", "") + "\">" + tcVO.FileName + "</a>\r\n";                    
                }
                if (string.IsNullOrEmpty(fileStr))
                    divFile.InnerHtml = "无";
                else
                    divFile.InnerHtml = fileStr;

                //divTargetAgency.InnerHtml = requireVO.TargetAgency;
                divAgencyCondition.InnerHtml = requireVO.AgencyCondition;
                divCompanyDescription.InnerHtml = requireVO.CompanyDescription;
                divProductDescription.InnerHtml = requireVO.ProductDescription;
                */
                
                List<RequirementTargetClientVO> bclientVOList = SiteCommon.GetRequireClientByBusiness(requireVO.RequirementId);
                for (int i = 0; i < bclientVOList.Count; i++)
                {
                    RequirementTargetClientVO tiVO = bclientVOList[i];
                    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tiVO.ClientName + "\">" + tiVO.ClientName + "</td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tiVO.CompanyName + "\">" + tiVO.CompanyName + "</td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tiVO.Description + "\">" + tiVO.Description + "</td> \r\n";
                    oTR += "</tr> \r\n";
                }

                List<RequirementFileVO> FclientVOList = SiteCommon.GetRequireFileByRequire(requireVO.RequirementId);
                for (int i = 0; i < FclientVOList.Count; i++)
                {
                    RequirementFileVO tiVO = FclientVOList[i];
                    fTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
                    fTR += "  <td class=\"center\" title=\"" + tiVO.FileName + "\">" + tiVO.FileName + "</td> \r\n";

                    tiVO.FilePath = tiVO.FilePath.Replace("~","/");
                    fTR += "  <td class=\"center\"><a href=\""+ tiVO.FilePath + "\"  title=\"下载\" target=\"_blank\">下载</a></td> \r\n";
                    fTR += "</tr> \r\n";
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("var _CustomerId='").Append(CustomerId).Append("';\n");
                sb.Append("var _BusinessCustomerId='").Append(requireVO.CustomerId).Append("';\n");
                sb.Append("var _Token='").Append(Token).Append("';\n");
                Utilities.RegisterJs(Page, "JSCommonVar_RequireDetail", sb.ToString());

                CustomerController customerCon = new CustomerController();

                int TenderInviteCount = uBO.FindTenderInviteTotalCount("RequirementId=" + requireVO.RequirementId);//已投标数量
                ProjectBO pBO = new ProjectBO(new CustomerProfile());
                int ProjectCount = pBO.FindProjectTotalCount("RequirementId=" + requireVO.RequirementId);//已创建项目数量

                str += "<div class=\"ProjectCount\">已有<font>" + TenderInviteCount + "</font>人投简历，已创建了<font>" + ProjectCount + "</font>个项目</div>";

                if (CustomerId < 1)
                {
                    str += "<div class=\"Collection\" title=\"关注\" onclick=\"markObject('" + requireVO.RequirementId + "', this)\"></div>";
                }
                else
                {
                    ResultObject result1 = customerCon.IsMarked(requireVO.RequirementId, 4, Token);

                    if (result1.Flag == 1)
                    {
                        str += "<div class=\"Collection on\" title=\"取消关注\" onclick=\"deleteMark('" + result1.Result.ToString() + "','" + requireVO.RequirementId + "', this)\"></div>\r\n";
                    }
                    else
                    {
                        str += "<div class=\"Collection\"  title=\"关注\" onclick=\"markObject('" + requireVO.RequirementId + "', this)\"></div>\r\n";
                    }
                }
                str += "<a class=\"line\" title=\"咨询\" href=\"ZXTIM.aspx?MessageTo=" + requireVO.CustomerId + "\" target=\"_blank\"></a>\r\n";

               

                if (requireVO.Status == 1)
                {
                    //发布
                    str += "<button type=\"button\" class=\"lshow_btn\" onclick=\"onTenderInvite('" + requireVO.RequirementId + "')\">投简历</button>";
                }
                else if (requireVO.Status == 3)
                {
                    //暂停投简历
                    str += "<button type=\"button\" class=\"btn\">雇主已暂停投简历</button>";
                }
                else if (requireVO.Status == 4)
                {
                    //已选定销售
                    str += "<button type=\"button\" class=\"btn\">已选定销售</button>";
                }
            }
        }        
    }
}