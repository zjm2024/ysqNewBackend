using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService.Controllers;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.ProjectManagement.VO;
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
    public partial class Agency : System.Web.UI.Page
    {
        public AgencyViewVO agencyVO;
        public string headImg = ConfigInfo.Instance.NoImg;
        public string levelName = "普通销售";
        public string oTR = "";
        public string dTR = "";
        public string str = "";
        public string Token
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session["#Session#TOKEN"] == null)
                {
                    return "";

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
        public bool isBusiness
        {
            get
            {
                BusinessBO bBO = new BusinessBO(new CustomerProfile());

                if (CustomerId > 0)
                {
                    return bBO.FindBusinessByCustomerId(CustomerId).Status == 1;
                }
                else
                {
                    return false;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = string.IsNullOrEmpty(Request.QueryString["agencyId"]) ? "0" : Request.QueryString["agencyId"];
            if (id != "0")
            {
                AgencyBO uBO = new AgencyBO(new CustomerProfile());

                RequireController requireCon = new RequireController();
                ResultObject result = requireCon.GetAgencySite(Convert.ToInt32(id));
                if (result == null)
                {
                    return;
                }
                agencyVO = result.Result as AgencyViewVO;

                headImg = uBO.getAgencyIMG(agencyVO.AgencyId);
                string AgencyName = "";
                if (agencyVO.AgencyName.Length >= 2)
                {
                    AgencyName = agencyVO.AgencyName.Substring(0, 2) + "***";
                }
                else
                {
                    AgencyName = agencyVO.AgencyName;
                }
                if (string.IsNullOrEmpty(CacheSystemConfig.GetSystemConfig().SiteDescription))
                    (this.Master as Shared.MasterPageSite).SiteName = AgencyName + (agencyVO.Sex ? "先生" : "女士") + "_" + CacheSystemConfig.GetSystemConfig().SiteName;
                else
                    (this.Master as Shared.MasterPageSite).SiteName = AgencyName + (agencyVO.Sex ? "先生" : "女士") + "_" + CacheSystemConfig.GetSystemConfig().SiteName + "_" + CacheSystemConfig.GetSystemConfig().SiteDescription;

                levelName = "普通销售";
                if (agencyVO.AgencyLevel == "1")
                {
                    levelName = "金牌销售";
                }
                else if (agencyVO.AgencyLevel == "2")
                {
                    levelName = "银牌销售";
                }
                else if (agencyVO.AgencyLevel == "3")
                {
                    levelName = "铜牌销售";
                }
                else if (agencyVO.AgencyLevel == "4")
                {
                    levelName = "普通销售";
                }
                List<AgencySolutionVO> asoluVOList = SiteCommon.GetAgencySolutionByAgency(Convert.ToInt32(id));
                for (int i = 0; i < asoluVOList.Count; i++)
                {
                    AgencySolutionVO tiVO = asoluVOList[i];
                    bool isShow = true;
                    if (CustomerId > 0&& tiVO.PrivacyType == 1) {
                        CustomerBO cBO = new CustomerBO(new CustomerProfile());
                        CustomerViewVO cVO = cBO.FindById(CustomerId);

                        string str = tiVO.Keyword;
                        string str2 = cVO.CustomerName;
                        if (str2.IndexOf(tiVO.ClientName) > -1)
                        {
                            isShow = false;
                        }
                        if (str.IndexOf(str2) > -1)
                        {
                            isShow = false;
                        }


                        BusinessBO bBO = new BusinessBO(new CustomerProfile());
                        if(cVO.BusinessId>0)
                        {
                            BusinessViewVO bVO = bBO.FindBusinessById(cVO.BusinessId);
                            string str3 = bVO.CompanyName;
                            if (str3 != "")
                            {
                                if (str3.IndexOf(tiVO.ClientName) > -1)
                                {
                                    isShow = false;
                                }
                                if (str.IndexOf(str3) > -1)
                                {
                                    isShow = false;
                                }
                            }
                        }
                    }
                    if (tiVO.PrivacyType!=2&& isShow)
                    {
                        oTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr\"> \r\n";
                        oTR += "  <td class=\"center\" title=\"" + tiVO.ClientName + "\">" + tiVO.ClientName + "</td> \r\n";
                        oTR += "  <td class=\"center\" title=\"" + tiVO.ProjectName + "\">" + tiVO.ProjectName + "</td> \r\n";
                        oTR += "  <td class=\"center\" title=\"" + tiVO.Cost + "\">" + tiVO.Cost + "</td> \r\n";
                        oTR += "  <td class=\"center\" title=\"" + ((tiVO.ProjectDate.ToString("yyyy-MM-dd") == "1900-01-01") ? "" : tiVO.ProjectDate.ToString("yyyy-MM-dd")) + "\">" + ((tiVO.ProjectDate.ToString("yyyy-MM-dd") == "1900-01-01") ? "" : tiVO.ProjectDate.ToString("yyyy-MM-dd")) + "</td> \r\n";
                        oTR += "  <td class=\"center\" > \r\n";
                        for (int j = 0; j < tiVO.AgencySolutionFileList.Count; j++)
                        {
                            oTR += "<a target=\"_blank\" href=\"" + tiVO.AgencySolutionFileList[j].FilePath + "\">" + tiVO.AgencySolutionFileList[j].FileName + "</a>";
                        }
                        oTR += "  </td> \r\n";
                        oTR += "</tr> \r\n";
                    }
                }

                DemandBO dBO = new DemandBO(new CustomerProfile());
                List<DemandViewVO> DemandViewVOList = dBO.FindDemandByCustomerId(agencyVO.CustomerId);
                for (int i = 0; i < DemandViewVOList.Count; i++)
                {
                    DemandViewVO tiVO = DemandViewVOList[i];
                    dTR += "<tr class=\"ui-widget-content jqgrow ui-row-ltr verticalalign\"> \r\n";
                    dTR += "  <td class=\"center\">" + SiteCommon.NoHTML(tiVO.Description) + "</td> \r\n";
                    dTR += "  <td class=\"center\"><a target=\"_blank\" title=\"查看需求\" href=\"Demand.aspx?DemandId=" + tiVO.DemandId + "\">查看需求</a></td> \r\n";
                    dTR += "</tr> \r\n";
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("var _CustomerId='").Append(CustomerId).Append("';\n");
                sb.Append("var _AgencyCustomerId='").Append(agencyVO.CustomerId).Append("';\n");
                sb.Append("var _Token='").Append(Token).Append("';\n");
                Utilities.RegisterJs(Page, "JSCommonVar_AgencyDetail", sb.ToString());

                CustomerController customerCon = new CustomerController();
                
                if (CustomerId < 1 || Token == "")
                {
                    str += "<div class=\"Collection\" title=\"关注\" onclick=\"markObject('" + agencyVO.CustomerId + "', this)\"></div>\r\n";
                }
                else
                {
                    ResultObject result1 = customerCon.IsMarked(agencyVO.CustomerId, 1, Token);

                    if (result1.Flag == 1)
                    {
                        str += "<div class=\"Collection on\" title=\"取消关注\" onclick=\"deleteMark('" + result1.Result.ToString() + "','" + agencyVO.CustomerId + "', this)\"></div>\r\n";
                    }
                    else
                    {
                        str += "<div class=\"Collection\"  title=\"关注\" onclick=\"markObject('" + agencyVO.CustomerId + "', this)\"></div>\r\n";
                    }

                }

                //绑定评分
                ProjectBO pBO = new ProjectBO(new CustomerProfile());
                List<AgencySumReviewVO> agencySumReviewVOList = pBO.FindAgencySumReviewByAgencyId(agencyVO.AgencyId);
                List<AllAgencyAverageScoreViewVO> allAveVOList = pBO.FindAllAgencyAverageScoreView();
                decimal score1 = 0m, score2 = 0m, score3 = 0m, score4 = 0m, score5 = 0m;
                decimal av1 = 0m, av2 = 0m, av3 = 0m, av4 = 0m, av5 = 0m;
                foreach (AgencySumReviewVO asVO in agencySumReviewVOList)
                {
                    switch (asVO.ReviewType)
                    {
                        case 1:
                            score1 = asVO.AverageScore;
                            break;
                        case 2:
                            score2 = asVO.AverageScore;
                            break;
                        case 3:
                            score3 = asVO.AverageScore;
                            break;
                        case 4:
                            score4 = asVO.AverageScore;
                            break;
                        case 5:
                            score5 = asVO.AverageScore;
                            break;
                    }
                }
                foreach (AllAgencyAverageScoreViewVO asVO in allAveVOList)
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
                reviewStr += "<div><span> 工作态度：<font> " + score1.ToString("N2") + " </font></span><span> 诚信度：<font> " + score4.ToString("N2") + " </font></span></div> \r\n";
                reviewStr += "<div><span> 沟通技巧：<font> " + score2.ToString("N2") + " </font></span><span> 客户关系：<font> " + score5.ToString("N2") + " </font></span></div> \r\n";
                reviewStr += "<div><span> 打单能力：<font> " + score3.ToString("N2") + " </font></span><span> 综合得分：<font> " + ((score1 + score2 + score3 + score4 + score5) / 5).ToString("N2") + " </font></span></div> \r\n";
                divAgencyReview.InnerHtml = reviewStr;
            }
            
        }

        private string GetCompareString(decimal score,decimal avg)
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
        private string RemoveLongString(string input, int len)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            return ((input.Length > len) ? (input.Substring(0, len) + "...") : input);
        }
    }
}