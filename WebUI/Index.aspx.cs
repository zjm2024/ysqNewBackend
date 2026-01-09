using CoreFramework.VO;
using SPlatformService.Controllers;
using SPlatformService.Models;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.UserManagement.VO;
using SPLibrary.WebConfigInfo;
using SPLibrary.CoreFramework.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using WebUI.Common;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;

namespace WebUI
{
    public partial class Index : System.Web.UI.Page
    {
        private int ProvinceId { get; set; }
        private int CityId { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CityId = Convert.ToInt32(Request.Cookies["local_city_id"].Value);
                if (CityId > 0)
                {
                    CityVO cityVO = SiteCommon.GetCity(CityId);
                    ProvinceId = cityVO.ProvinceId;
                }
            }
            catch
            {
                CityId = -1;
                ProvinceId = -1;
            }

            BindConfig();
            BindRecommend();
        }
        private void BindConfig()
        {
            SystemController systemCon = new SystemController();
            ResultObject result = systemCon.GetConfig();
            if (result.Flag == 1)
            {
                ConfigVO configVO = (ConfigVO)Cache["configVO"];
                if (configVO == null)
                {
                    configVO = result.Result as ConfigVO;
                    Cache.Insert("configVO", configVO);
                }

                txtProjectCount.InnerHtml = configVO.ProjectTotal.ToString();
                txtTotalCommission.InnerHtml = configVO.CommissionTotal.ToString();
            }
        }
        private void BindRecommend()
        {
            DemandBO rBO = new DemandBO(new CustomerProfile());

            List<DemandViewVO> demandList = (List<DemandViewVO>)Cache["demandList"];
            if(demandList==null)
            {
                demandList = rBO.FindDemandAllByPageIndex("Status <> 0 and Status <> 2 and TO_DAYS(EffectiveEndDate) - TO_DAYS(now()) >=0", 1, 20, "CreatedAt", "desc");
                Cache.Insert("demandList", demandList);
            }
            
            string divDemandList = "";
            foreach (DemandViewVO vo in demandList)
            {
                divDemandList += "<li>\r\n";
                divDemandList += "<div class=\"Demand_info\">"; 
                divDemandList += "<p>"+ NoHTML(vo.Description) + "</p>\r\n";
                divDemandList += "<span> 发布时间："+ DateFormatToString(vo.CreatedAt) + " </span><span class=\"lr\">收到留言：" + vo.OfferCount + "条</span>\r\n";
                divDemandList += "</div>";
                divDemandList += "<a class=\"Demand_btn\"  href=\"Demand.aspx?DemandId=" + vo.DemandId + "\" target=\"_blank\">详情查看</a>";
                divDemandList += "<a class=\"Demand_btn\"  href=\"ZXTIM.aspx?MessageTo=" + vo.CustomerId + "\" target=\"_blank\">线上联系</a>";
                divDemandList += "</li>\r\n";
            }
            lindex_demand_list.InnerHtml = divDemandList;

            List<RecommendRequireViewVO> requireList = (List<RecommendRequireViewVO>)Cache["requireList"];
            if (requireList == null)
            {
                requireList = GetRecommendRequireList();
                Cache.Insert("requireList", requireList);
            }

            //绑定到UI
            string divRequireStr = "";
            divRequireStr += "<ul>\r\n";
            RequireBO qBO= new RequireBO(new CustomerProfile());
            foreach (RecommendRequireViewVO vo in requireList)
            {
                string title = vo.Title;
                if (title.Length > 8)
                    title = title.Substring(0, 8) + "...";

                string headimg = qBO.getRequireIMG(vo.RequirementId);
                divRequireStr += "<li>\r\n";
                divRequireStr += "<a href = \"Require.aspx?requireId=" + vo.RequirementId + "\" >\r\n";
                divRequireStr += "<div class=\"img\" style=\"background-image:url(" + headimg + ")\"></div>\r\n";
                divRequireStr += "<div class=\"laddess\">" + vo.CityName + "</div>\r\n";
                divRequireStr += "<div class=\"ltitle\" title=\""+ vo.Title + "\">\r\n";
                divRequireStr += title;
                divRequireStr += "</div>\r\n";
                divRequireStr += "<div class=\"ltitle_right\">\r\n";
                divRequireStr += (vo.DelegationCommission > 0 ? "已托管" : "未托管");
                divRequireStr += "</div>";
                divRequireStr += "<div class=\"lcost\">酬金：&yen;" + vo.Commission.ToString("N0") + "</div>\r\n";
                divRequireStr += "<div class=\"lbtn\">免费咨询</div>\r\n";
                divRequireStr += "</a>\r\n";
                divRequireStr += "</li>\r\n";
            }
            divRequireStr += "</ul>\r\n";

            divRequireList.InnerHtml = divRequireStr;

            List<RecommendAgencyViewVO> agencyList = (List<RecommendAgencyViewVO>)Cache["agencyList"];
            if (agencyList == null)
            {
                agencyList = GetRecommendAgencyList();
                Cache.Insert("agencyList", agencyList);
            }

            //绑定UI
            string divAgencyStr = "";
            divAgencyStr += "<div class=\"search-list-wrap\"> \r\n";
            divAgencyStr += "    <ul class=\"clearfix search-agency-wrap\"> \r\n";
            AgencyBO aBO=new AgencyBO(new CustomerProfile());
            foreach (RecommendAgencyViewVO vo in agencyList)
            {
                divAgencyStr += "<li>\r\n";
                divAgencyStr += "<a href = \"Agency.aspx?agencyId=" + vo.AgencyId + "\">\r\n";


                string headimg = aBO.getAgencyIMG(vo.AgencyId);
                divAgencyStr += "<div class=\"img\" style=\"background-image:url(" + headimg + ")\"></div>\r\n";
                divAgencyStr += "<div class=\"info\">\r\n";

                string AgencyName = "";
                if (vo.AgencyName.Length >= 2)
                {
                    AgencyName = vo.AgencyName.Substring(0, 2) + "***";
                }
                else {
                    AgencyName = vo.AgencyName;
                }
                divAgencyStr += "<p class=\"p1\"><span class=\"s1\">" + AgencyName + (vo.Sex ? "先生" : "女士") + "</span><span class=\"s2\">" + vo.CityName + "</span><span class=\"s3\">评分：" + vo.ReviewScore.ToString("N2") + "</span></p>\r\n";
                divAgencyStr += "<p>优势行业：" + RemoveLongString(RemoveComma(vo.TargetCategory), 16) + "</p>\r\n";
                divAgencyStr += "<p>优势客户：" + RemoveLongString(RemoveComma(vo.TargetClient), 16) + "</p>\r\n";
                divAgencyStr += "<p>擅长产品：" + RemoveLongString(vo.FamiliarProduct, 16) + "</p>\r\n";
                divAgencyStr += "<p>已成交：" + vo.ProjectCount + "</p>\r\n";
                divAgencyStr += "<p class=\"p3\">酬金收入&nbsp;&yen;" + vo.ProjectCommission + "<span class=\"lbtn\">查看详情</span></p>\r\n";
                divAgencyStr += "</div>\r\n";
                divAgencyStr += "</a>\r\n";
                divAgencyStr += "</li>\r\n";
            }
            divAgencyStr += "      </ul> \r\n";
            divAgencyStr += "    </div> \r\n";
            divAgencyStr += "</div> \r\n";

            divAgencyList.InnerHtml = divAgencyStr;

        }

        private List<RecommendRequireViewVO> GetRecommendRequireList()
        {
            UserController uc = new UserController();
            ResultObject requireResult = uc.GetRecommendRequireList(ProvinceId, CityId, -1, -1);
            List<RecommendRequireViewVO> requireList = requireResult.Result as List<RecommendRequireViewVO>;
            if (requireList == null)
            {
                requireList = new List<RecommendRequireViewVO>();
                //从列表获取
                ConditionModel filterModel = new ConditionModel();
                Filter filterObj = new Filter();
                Paging pageInfoObj = new Paging();

                filterModel.Filter = filterObj;
                filterModel.PageInfo = pageInfoObj;

                pageInfoObj.PageIndex = 1;
                pageInfoObj.PageCount = 8;
                pageInfoObj.SortName = "CreatedAt";
                pageInfoObj.SortType = "desc";
                filterObj.groupOp = "AND";
                List<SPLibrary.WebConfigInfo.Rules> ruleList = new List<SPLibrary.WebConfigInfo.Rules>();

                SPLibrary.WebConfigInfo.Rules ruleObj = new SPLibrary.WebConfigInfo.Rules();
                ruleObj.field = "Status";
                ruleObj.op = "eq";
                ruleObj.data = "1";
                ruleList.Add(ruleObj);
                filterObj.rules = ruleList.ToArray();

                RequireController requireCon = new RequireController();

                ResultObject resultTemp = requireCon.GetRequireList(filterModel);
                if (resultTemp != null)
                {
                    List<RequirementViewVO> requireTempList = resultTemp.Result as List<RequirementViewVO>;

                    if (requireTempList != null)
                    {
                        foreach (RequirementViewVO requireViewVO in requireTempList)
                        {
                            RecommendRequireViewVO rrViewVO = new RecommendRequireViewVO();
                            rrViewVO.RequirementId = requireViewVO.RequirementId;
                            rrViewVO.RequirementCode = requireViewVO.RequirementCode;
                            rrViewVO.Title = requireViewVO.Title;
                            rrViewVO.Commission = requireViewVO.Commission;
                            rrViewVO.CityName = requireViewVO.CityName;
                            rrViewVO.CustomerId = requireViewVO.CustomerId;
                            rrViewVO.MainImg = requireViewVO.MainImg;
                            rrViewVO.DelegationCommission = requireViewVO.DelegationCommission;

                            requireList.Add(rrViewVO);
                        }
                    }
                }
            }
            else if (requireList.Count < 8)
            {
                //不足10个从列表补齐
                ConditionModel filterModel = new ConditionModel();
                Filter filterObj = new Filter();
                Paging pageInfoObj = new Paging();

                filterModel.Filter = filterObj;
                filterModel.PageInfo = pageInfoObj;

                pageInfoObj.PageIndex = 1;
                pageInfoObj.PageCount = 8;
                pageInfoObj.SortName = "CreatedAt";
                pageInfoObj.SortType = "desc";
                filterObj.groupOp = "AND";

                List<SPLibrary.WebConfigInfo.Rules> ruleList = new List<SPLibrary.WebConfigInfo.Rules>();

                SPLibrary.WebConfigInfo.Rules ruleObj = new SPLibrary.WebConfigInfo.Rules();
                ruleObj.field = "Status";
                ruleObj.op = "eq";
                ruleObj.data = "1";
                ruleList.Add(ruleObj);
                filterObj.rules = ruleList.ToArray();

                RequireController requireCon = new RequireController();

                ResultObject resultTemp = requireCon.GetRequireList(filterModel);
                if (resultTemp != null)
                {


                    List<RequirementViewVO> requireTempList = resultTemp.Result as List<RequirementViewVO>;

                    if (requireTempList != null)
                    {
                        //判断requireList是否够8个

                        foreach (RequirementViewVO requireViewVO in requireTempList)
                        {
                            if (requireList.Count >= 8)
                                break;

                            //判断是否存在
                            bool isContain = false;
                            foreach (RecommendRequireViewVO voTemp in requireList)
                            {
                                if (voTemp.RequirementId == requireViewVO.RequirementId)
                                {
                                    isContain = true;
                                    break;
                                }
                            }

                            if (!isContain)
                            {
                                RecommendRequireViewVO rrViewVO = new RecommendRequireViewVO();
                                rrViewVO.RequirementId = requireViewVO.RequirementId;
                                rrViewVO.RequirementCode = requireViewVO.RequirementCode;
                                rrViewVO.Title = requireViewVO.Title;
                                rrViewVO.Commission = requireViewVO.Commission;
                                rrViewVO.CityName = requireViewVO.CityName;
                                rrViewVO.CustomerId = requireViewVO.CustomerId;
                                rrViewVO.MainImg = requireViewVO.MainImg;
                                rrViewVO.DelegationCommission = requireViewVO.DelegationCommission;
                                rrViewVO.TargetCategory = requireViewVO.TargetCategory;
                                rrViewVO.TargetCity = requireViewVO.TargetCity;
                                rrViewVO.TargetClient = requireViewVO.TargetClient;

                                requireList.Add(rrViewVO);
                            }
                        }
                    }
                }
            }
            else if (requireList.Count > 8)
            {
                for (int i = requireList.Count - 1; i >= 8; i--)
                {
                    requireList.RemoveAt(i);
                }
            }
            return requireList;
        }

        private List<RecommendAgencyViewVO> GetRecommendAgencyList()
        {
            UserController uc = new UserController();
            ResultObject agencyResult = uc.GetRecommendAgencyList(ProvinceId, CityId, -1, -1);
            List<RecommendAgencyViewVO> agencyList = agencyResult.Result as List<RecommendAgencyViewVO>;
            if (agencyList == null)
            {
                agencyList = new List<RecommendAgencyViewVO>();
                //从列表获取
                ConditionModel filterModel = new ConditionModel();
                Filter filterObj = new Filter();
                Paging pageInfoObj = new Paging();

                filterModel.Filter = filterObj;
                filterModel.PageInfo = pageInfoObj;

                pageInfoObj.PageIndex = 1;
                pageInfoObj.PageCount = 6;
                pageInfoObj.SortName = "CreatedAt";
                pageInfoObj.SortType = "desc";
                filterObj.groupOp = "AND";

                List<SPLibrary.WebConfigInfo.Rules> ruleList = new List<SPLibrary.WebConfigInfo.Rules>();

                SPLibrary.WebConfigInfo.Rules ruleObj = new SPLibrary.WebConfigInfo.Rules();
                ruleObj.field = "1";
                ruleObj.op = "eq";
                ruleObj.data = "1";
                ruleList.Add(ruleObj);
                filterObj.rules = ruleList.ToArray();

                RequireController requireCon = new RequireController();

                ResultObject resultTemp = requireCon.GetAgencyList(filterModel);
                if (resultTemp != null)
                {

                    List<AgencyViewVO> agencyTempList = resultTemp.Result as List<AgencyViewVO>;

                    if (agencyTempList != null)
                    {
                        foreach (AgencyViewVO agencyViewVO in agencyTempList)
                        {
                            RecommendAgencyViewVO rrViewVO = new RecommendAgencyViewVO();
                            rrViewVO.AgencyId = agencyViewVO.AgencyId;
                            rrViewVO.HeaderLogo = agencyViewVO.HeaderLogo;
                            rrViewVO.AgencyName = agencyViewVO.AgencyName;
                            rrViewVO.CityName = agencyViewVO.CityName;
                            rrViewVO.CustomerId = agencyViewVO.CustomerId;
                            rrViewVO.ProjectCount = agencyViewVO.ProjectCount;
                            rrViewVO.AgencyLevel = agencyViewVO.AgencyLevel;
                            rrViewVO.ShortDescription = agencyViewVO.ShortDescription;
                            rrViewVO.TargetCategory = agencyViewVO.TargetCategory;
                            rrViewVO.TargetClient = agencyViewVO.TargetClient;
                            rrViewVO.FamiliarProduct = agencyViewVO.FamiliarProduct;
                            rrViewVO.ReviewScore = agencyViewVO.ReviewScore;
                            rrViewVO.Sex = agencyViewVO.Sex;
                            rrViewVO.PersonalCard = agencyViewVO.PersonalCard;
                            agencyList.Add(rrViewVO);
                        }
                    }
                }
            }
            else if (agencyList.Count < 6)
            {
                //不足10个从列表补齐
                ConditionModel filterModel = new ConditionModel();
                Filter filterObj = new Filter();
                Paging pageInfoObj = new Paging();

                filterModel.Filter = filterObj;
                filterModel.PageInfo = pageInfoObj;

                pageInfoObj.PageIndex = 1;
                pageInfoObj.PageCount = 6;
                pageInfoObj.SortName = "CreatedAt";
                pageInfoObj.SortType = "desc";
                filterObj.groupOp = "AND";

                List<SPLibrary.WebConfigInfo.Rules> ruleList = new List<SPLibrary.WebConfigInfo.Rules>();

                SPLibrary.WebConfigInfo.Rules ruleObj = new SPLibrary.WebConfigInfo.Rules();
                ruleObj.field = "1";
                ruleObj.op = "eq";
                ruleObj.data = "1";
                ruleList.Add(ruleObj);
                filterObj.rules = ruleList.ToArray();

                RequireController requireCon = new RequireController();

                ResultObject resultTemp = requireCon.GetAgencyList(filterModel);
                if (resultTemp != null)
                {

                    List<AgencyViewVO> agencyTempList = resultTemp.Result as List<AgencyViewVO>;

                    if (agencyTempList != null)
                    {
                        //判断requireList是否够6个

                        foreach (AgencyViewVO agencyViewVO in agencyTempList)
                        {
                            if (agencyList.Count >= 6)
                                break;

                            //判断是否存在
                            bool isContain = false;
                            foreach (RecommendAgencyViewVO voTemp in agencyList)
                            {
                                if (voTemp.AgencyId == agencyViewVO.AgencyId)
                                {
                                    isContain = true;
                                    break;
                                }
                            }
                            if (!isContain)
                            {
                                RecommendAgencyViewVO rrViewVO = new RecommendAgencyViewVO();
                                rrViewVO.AgencyId = agencyViewVO.AgencyId;
                                rrViewVO.HeaderLogo = agencyViewVO.HeaderLogo;
                                rrViewVO.AgencyName = agencyViewVO.AgencyName;
                                rrViewVO.CityName = agencyViewVO.CityName;
                                rrViewVO.CustomerId = agencyViewVO.CustomerId;
                                rrViewVO.ProjectCount = agencyViewVO.ProjectCount;
                                rrViewVO.AgencyLevel = agencyViewVO.AgencyLevel;
                                rrViewVO.ShortDescription = agencyViewVO.ShortDescription;
                                rrViewVO.TargetCategory = agencyViewVO.TargetCategory;
                                rrViewVO.TargetClient = agencyViewVO.TargetClient;
                                rrViewVO.FamiliarProduct = agencyViewVO.FamiliarProduct;
                                rrViewVO.ReviewScore = agencyViewVO.ReviewScore;
                                rrViewVO.Sex = agencyViewVO.Sex;
                                rrViewVO.PersonalCard = agencyViewVO.PersonalCard;
                                agencyList.Add(rrViewVO);
                            }
                        }
                    }
                }
            }
            else if (agencyList.Count > 6)
            {
                for (int i = agencyList.Count - 1; i >= 6; i--)
                {
                    agencyList.RemoveAt(i);
                }
            }

            return agencyList;

        }

        private string RemoveComma(string input)
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
        private static string DateFormatToString(DateTime dt)
        {
            TimeSpan span = (DateTime.Now - dt).Duration();
            if (span.TotalDays > 730)
            {
                return dt.ToString("yyyy-MM-dd");
            }
            else if (span.TotalDays > 365)
            {
                return "一年前";
            }
            else if (span.TotalDays > 180)
            {
                return "半年前";
            }
            else if (span.TotalDays > 60)
            {
                return "2个月前";
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒前";
            }
        }
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }
    }
}