using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService.Controllers;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.ProjectManagement.VO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.RequireManagement.VO;
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
    public partial class Business : System.Web.UI.Page
    {
        public BusinessViewVO businessVO;
        public BusinessBO buBO=new BusinessBO(new CustomerProfile());
        public string headImg = ConfigInfo.Instance.NoImg;
        public string oTR="";
        public string rTR = "";
        public string dTR = "";
        public string Description = "";
        public string Token
        {
            get
            {
                if(HttpContext.Current == null || HttpContext.Current.Session["#Session#TOKEN"] == null)
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
            string id = string.IsNullOrEmpty(Request.QueryString["businessId"]) ? "0" : Request.QueryString["businessId"];
            if (id != "0")
            {
                RequireController requireCon = new RequireController();
                ResultObject result = requireCon.GetBusinessSite(Convert.ToInt32(id));
                if (result == null)
                {
                    return;
                }
                businessVO = result.Result as BusinessViewVO;

                if (string.IsNullOrEmpty(CacheSystemConfig.GetSystemConfig().SiteDescription))
                    (this.Master as Shared.MasterPageSite).SiteName = businessVO.CompanyName + "_" + CacheSystemConfig.GetSystemConfig().SiteName;
                else
                    (this.Master as Shared.MasterPageSite).SiteName = businessVO.CompanyName + "_" + CacheSystemConfig.GetSystemConfig().SiteName + "_" + CacheSystemConfig.GetSystemConfig().SiteDescription;

                Description = new HtmlParser(businessVO.Description).Text();

                headImg = buBO.getBusinessIMG(businessVO.BusinessId);
                /*
                lblTitle.InnerHtml = businessVO.CompanyName;
                lblCompanyType.InnerHtml = businessVO.CompanyType;
                divMainProducts.InnerHtml = businessVO.MainProducts;
                lblCityName.InnerHtml = businessVO.CityName;
                lblSetUpDate.InnerHtml = businessVO.SetupDate.ToString("yyyy-MM-dd");
                divDescription.InnerHtml = businessVO.Description;
                imgCompanyLogoPic.Src = businessVO.CompanyLogo;

                lblAddress.InnerHtml = businessVO.Address;
                lblCompanySite.InnerHtml = businessVO.CompanySite;
                //lblCompanyTel.InnerHtml = businessVO.CompanyTel;
                lblLicense.InnerHtml = businessVO.BusinessLicense;
                lblReviewScore.InnerHtml = businessVO.ReviewScore.ToString("N2");
                lblProjectCount.InnerHtml = businessVO.ProjectCount.ToString();

                divProduct.InnerHtml = businessVO.ProductDescription;
                
                lblCategory.InnerHtml = RemoveComma(businessVO.CategoryNames);

                lblTargetCategory.InnerHtml = RemoveComma(businessVO.TargetCategory);

                lblTargetCity.InnerHtml = RemoveComma(businessVO.TargetCity);
                */
                List<BusinessClientVO> bclientVOList = SiteCommon.GetBusinessClientByBusiness(Convert.ToInt32(id));
                
                for (int i = 0; i < bclientVOList.Count; i++)
                {
                    BusinessClientVO tiVO = bclientVOList[i];
                    oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tiVO.ClientName + "\">" + tiVO.ClientName + "</td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tiVO.CompanyName + "\">" + tiVO.CompanyName + "</td> \r\n";
                    oTR += "  <td class=\"center\" title=\"" + tiVO.Description + "\">" + tiVO.Description + "</td> \r\n";
                    oTR += "</tr> \r\n";
                }
                //oTR += "</tbody> \r\n";
                //oTR += "</table> ";
                //divClient.InnerHtml = oTR;


                RequireBO rBO = new RequireBO(new CustomerProfile());
                List<RequirementViewVO> RequirementVOList = rBO.FindRequireByCustomerId(businessVO.CustomerId,1);

                for (int i = 0; i < RequirementVOList.Count; i++)
                {
                    RequirementViewVO tiVO = RequirementVOList[i];
                    rTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr verticalalign\"> \r\n";
                    if (tiVO.MainImg == "")
                    {
                        rTR += "  <td class=\"center\" title=\"" + tiVO.Title + "\"><p>" + tiVO.Title + "</p></td> \r\n";
                    }
                    else {
                        rTR += "  <td class=\"center RequireMainImg\" title=\"" + tiVO.Title + "\"><img src=\"" + tiVO.MainImg + "\"/><p>" + tiVO.Title + "</p></td> \r\n";
                    }
                    
                    rTR += "  <td class=\"center\">" +SiteCommon.NoHTML(tiVO.Description) + "</td> \r\n";
                    rTR += "  <td class=\"center\"><a target=\"_blank\" title=\"查看任务\" href=\"Require.aspx?requireId="+ tiVO.RequirementId + "\">查看任务</a></td> \r\n";
                    rTR += "</tr> \r\n";
                }

                DemandBO dBO=new DemandBO(new CustomerProfile());
                List<DemandViewVO> DemandViewVOList = dBO.FindDemandByCustomerId(businessVO.CustomerId);
                for (int i = 0; i < DemandViewVOList.Count; i++)
                {
                    DemandViewVO tiVO = DemandViewVOList[i];
                    dTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr verticalalign\"> \r\n";
                    dTR += "  <td class=\"center\">" + SiteCommon.NoHTML(tiVO.Description) + "</td> \r\n";
                    dTR += "  <td class=\"center\"><a target=\"_blank\" title=\"查看需求\" href=\"Demand.aspx?DemandId=" + tiVO.DemandId + "\">查看需求</a></td> \r\n";
                    dTR += "</tr> \r\n";
                }

                //divChat.InnerHtml = "<img width=\"21\" height=\"26\" title=\"咨询\" src=\"Style/images/im_chat.png\" style=\"cursor:pointer;margin: 6px 6px 6px 0;line-height: 24px;height: 24px;\" onclick=\"AddNewChat('" + businessVO.CustomerId + "')\" /> \r\n";

                StringBuilder sb = new StringBuilder();
                sb.Append("var _CustomerId='").Append(CustomerId).Append("';\n");
                sb.Append("var _BusinessCustomerId='").Append(businessVO.CustomerId).Append("';\n");
                sb.Append("var _Token='").Append(Token).Append("';\n");
                Utilities.RegisterJs(Page, "JSCommonVar_RequireDetail", sb.ToString());

                //绑定评分
                ProjectBO pBO = new ProjectBO(new CustomerProfile());
                List<BusinessSumReviewVO> businessSumReviewVOList = pBO.FindBusinessSumReviewByBusinessId(businessVO.BusinessId);
                List<AllBusinessAverageScoreViewVO> allAveVOList = pBO.FindAllBusinessAverageScoreView();
                decimal score1 = 0m, score2 = 0m, score3 = 0m, score4 = 0m, score5 = 0m;
                decimal av1 = 0m, av2 = 0m, av3 = 0m, av4 = 0m, av5 = 0m;
                av1 = 5; av2 = 4; av3 = 5; av4 = 4; av5 = 5;
                foreach (BusinessSumReviewVO asVO in businessSumReviewVOList)
                {
                    switch (asVO.ReviewType)
                    {
                        case 1:
                            score1 = asVO.AverageScore;
                            break;
                        case 2:
                            score1 = asVO.AverageScore;
                            break;
                        case 3:
                            score1 = asVO.AverageScore;
                            break;
                        case 4:
                            score1 = asVO.AverageScore;
                            break;
                        case 5:
                            score1 = asVO.AverageScore;
                            break;
                    }
                }
                foreach (AllBusinessAverageScoreViewVO asVO in allAveVOList)
                {
                    switch (asVO.ReviewType)
                    {
                        case 1:
                            av1 = asVO.AverageScore;
                            break;
                        case 2:
                            av2 = asVO.AverageScore;
                            break;
                        case 3:
                            av3 = asVO.AverageScore;
                            break;
                        case 4:
                            av4 = asVO.AverageScore;
                            break;
                        case 5:
                            av5 = asVO.AverageScore;
                            break;
                    }
                }
                
                string reviewStr = "";
                reviewStr += "<div><span> 销售支持：<font> " + score1.ToString("N2") + " </font></span><span> 付款及时性：<font> " + score4.ToString("N2") + " </font></span></div> \r\n";
                reviewStr += "<div><span> 售后服务：<font> " + score2.ToString("N2") + " </font></span><span> 诚信度：<font> " + score5.ToString("N2") + " </font></span></div> \r\n";
                reviewStr += "<div><span> 产品质量：<font> " + score3.ToString("N2") + " </font></span><span> 综合得分：<font> " + ((score1 + score2 + score3 + score4 + score5) / 5).ToString("N2") + " </font></span></div> \r\n";

                divBusinessReview.InnerHtml = reviewStr;
            }
        }

        public string RemoveComma(string input)
        {
            if (input.Length == 0)
                return input;

            char[] charList = input.ToCharArray();
            int first = 0, last = charList.Length - 1;
            if (charList[0] == ',')
                first++;
            if (charList[charList.Length - 1] == ',')
                last--;
            string result = "";
            for (int i = first; i <= last; i++)
            {
                result += charList[i];
            }
            return result;
        }

        private string GetCompareString(decimal score, decimal avg)
        {
            string str = score >= avg ? "<td class=\"score-high\">高于{0}%</td>" : "<td class=\"score-low\">低于{0}%</td>";

            decimal per = 0m;
            if (avg == 0m)
            {
                per = 100;
            }
            else
            {
                per = ((score - avg) / avg) * 100;
            }

            str = string.Format(str, Math.Abs(per).ToString("N2"));

            return str;

        }
    }
}