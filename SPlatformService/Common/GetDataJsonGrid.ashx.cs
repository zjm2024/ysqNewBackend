using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using CoreFramework.VO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using CoreFramework;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.ProjectManagement.VO;
using SPLibrary.WebConfigInfo;
using SPLibrary.BusinessCardManagement.BO;
using SPLibrary.BusinessCardManagement.VO;
using Newtonsoft.Json;
using SPLibrary.CoreFramework.Logging.BO;

namespace WebUI.Sample
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary> 
    public class GetDataJsonGrid : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string sEcho = context.Request.Form.Get("oper");
            int page = Convert.ToInt32(context.Request["page"]);
            int iDisplayLength = Convert.ToInt32(context.Request["rows"]);
            int iDisplayStart = (page - 1) * iDisplayLength;
            //int iSortCol;

            //string iSortColName = "[" + context.Request["sidx"] + "]";  //SQL Server
            string iSortColName = context.Request["sidx"];//MySQL
            if (context.Request["sidx"].Contains(","))
                iSortColName = context.Request["sidx"];

            string ase = context.Request["sord"];
            string condition = "";
            if (context.Request["Param"] != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Filter filter = serializer.Deserialize<Filter>(context.Request["Param"].ToString());
                condition += filter.Result();
            }
            if (context.Request["filters"] != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Filter filter = serializer.Deserialize<Filter>(context.Request["filters"].ToString());
                if (!string.IsNullOrEmpty(filter.Result()))
                {
                    if (!string.IsNullOrEmpty(condition))
                    {
                        condition += " and ";
                    }
                    condition += filter.Result();
                }
            }
            if (string.IsNullOrEmpty(condition))
            {
                condition = "1=1";
            }
            string[] param = context.Request.QueryString.AllKeys;
            int pageindex = iDisplayStart / iDisplayLength + 1;
            string finalCondition = null;

            //兼容DataTable
            string browseModel = "list";
            if (context.Request["BrowseModel"] != null)
            {
                //datatable
                browseModel = context.Request["BrowseModel"].ToLower();
            }
            if (browseModel == "datatable")
            {
                string json = DataTableToJson(page, iDisplayLength, 0, getDataTable(condition, pageindex, iDisplayLength, iSortColName, ase, param, context, finalCondition), false);
                context.Response.Write(json);
            }
            else
            {
                int count = getcount(condition, param, context, ref finalCondition);
                string json = ListToJson(page, iDisplayLength, count, getJson(condition, pageindex, iDisplayLength, iSortColName, ase, param, context, finalCondition), false);
                context.Response.Write(json);
            }

            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static DataTable getDataTable(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc, string[] param, HttpContext context, string finalCondition)
        {

            return new DataTable();
        }

        public static List<CommonVO> getJson(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc, string[] param, HttpContext context, string finalCondition)   //执行存储过程，提出数据
        {
            if (context.Request[param[0]].Equals("DepartmentList"))
            {
                try
                {
                    return GetDepartmentList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("UserList"))
            {
                try
                {
                    return GetUserList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("RoleList"))
            {
                try
                {
                    return GetRoleList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CustomerLoginList"))
            {
                try
                {
                    return GetCustomerLoginList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("ViolationList"))
            {
                try
                {
                    return GetViolationList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("AgentApplyList"))
            {
                try
                {
                    return GetAgentApplyList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }

            else if (context.Request[param[0]].Equals("VipApplyList"))
            {
                try
                {
                    return GetVipApplyList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }

            else if (context.Request[param[0]].Equals("CustomerList"))
            {
                try
                {
                    return GetCustomerList("(originType<>'C_Service' or originType IS NULL) and AppType=" + new UserPrincipal().UserProfile.AppType + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }

            else if (context.Request[param[0]].Equals("CustomerVipList"))
            {
                try
                {
                    return GetCustomerVipList("Agent=1 and (originType<>'C_Service' or originType IS NULL) and AppType=" + new UserPrincipal().UserProfile.AppType + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("ZxbConfigList"))
            {
                try
                {
                    return GetZxbConfigList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("InvitationCustomerList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " InvitationCustomerID = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    return GetCustomerList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("BusinessList"))
            {
                try
                {
                    return GetBusinessList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("BusinessApproveList"))
            {
                try
                {
                    return GetBusinessApproveList("RealNameStatus = 3" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("ProvinceList"))
            {
                try
                {
                    return GetProvinceList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CategoryList"))
            {
                try
                {
                    return GetCategoryList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("SuggestionList"))
            {
                try
                {
                    return GetSuggestionList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CarouselList"))
            {
                try
                {
                    return GetCarouselList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CityList"))
            {
                try
                {
                    return GetCityList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CategoryChooseList"))
            {
                try
                {
                    return GetCategoryChooseList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("AgencyList"))
            {
                try
                {
                    if (context.Request["provinceId"] != null)
                    {
                        if (Convert.ToInt32(context.Request["ProvinceId"]) > 0)
                            condition = " ProvinceId = " + context.Request["provinceId"] + " and " + condition;
                    }
                    if (context.Request["cityId"] != null)
                    {
                        if (Convert.ToInt32(context.Request["cityId"]) > 0)
                            condition = " CityId = " + context.Request["cityId"] + " and " + condition;
                    }
                    if (context.Request["parentCategoryId"] != null)
                    {
                        if (Convert.ToInt32(context.Request["ParentCategoryIds"]) > 0)
                            condition = " ParentCategoryIds like '%," + context.Request["parentCategoryId"] + ",%' and " + condition;
                    }
                    if (context.Request["categoryId"] != null)
                    {
                        if (Convert.ToInt32(context.Request["categoryId"]) > 0)
                            condition = " CategoryIds like '%," + context.Request["categoryId"] + ",%' and " + condition;
                    }
                    return GetAgencyList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardRedPacketList"))
            {
                try
                {
                    return GetCardRedPacketList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardRedPacketDetaillistList"))
            {
                try
                {
                    if (context.Request["RedPacketId"] != null)
                    {
                        return GetCardRedPacketDetaillistList("1=1" + " and RedPacketId="+ context.Request["RedPacketId"] + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }
                        return GetCardRedPacketDetaillistList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("PersonalList"))
            {
                try       
                {      
                    if (context.Request["BusinessID"] != null)
                    {
                        return GetPersonalList("1=1" + " and BusinessID=" + context.Request["BusinessID"] + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }
                    if (context.Request["CustomerId"] != null)
                    {
                        return GetPersonalList("1=1" + " and CustomerId=" + context.Request["CustomerId"] + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }            
                    return GetPersonalList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("PartSignList"))
            {
                try
                {
                    if (context.Request["PartID"] != null)
                    {
                        return GetCardPartSignInNumList("1=1" + " and PartyID=" + context.Request["PartID"] + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }
                    return GetCardPartSignInNumList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("PartyInviterList"))
            {
                try
                {
                    if (context.Request["PartID"] != null)
                    {
                        return GetCardPartyInviterList(Convert.ToInt32(context.Request["PartID"]), pageindex, iDisplayLength, iSortColName, asc);
                    }
                    return GetCardPartyInviterList(0, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardAccessRecordsViewList"))
            {
                try
                {
                    if (context.Request["PartID"] != null)
                    {
                        return GetCardAccessRecordsViewList(Convert.ToInt32(context.Request["PartID"]), pageindex, iDisplayLength, iSortColName, asc);
                    }
                    return GetCardAccessRecordsViewList(0, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("GroupJoinList"))
            {
                try
                {
                    if (context.Request["GroupID"] != null)
                    {
                        return GetGroupJoinNumList("1=1" + " and GroupID=" + context.Request["GroupID"] + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }
                    return GetGroupJoinNumList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {
                    

                }
            }
            else if (context.Request[param[0]].Equals("CardPayOutList"))
            {
                
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        return GetCardPayOutList("1=1" + " and status=1 and HostCustomerId=" + context.Request["CustomerId"] + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }
                    return GetCardPayOutList("1=1" + " and status=1 and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {


                }
            }
            else if (context.Request[param[0]].Equals("CardBalanceList"))
            {

                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        return CardBalanceList(Convert.ToInt32(context.Request["CustomerId"]), pageindex, iDisplayLength, iSortColName, asc);
                    }
                }
                catch (Exception ex)
                {


                }
            }

            else if (context.Request[param[0]].Equals("CardOrderIncomeList"))
            {

                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        return GetCardOrderIncomeList("1=1" + " and status=1 and (OneRebateCustomerId=" + context.Request["CustomerId"] + " or TwoRebateCustomerId=" + context.Request["CustomerId"] + ") and " + condition, pageindex, iDisplayLength, iSortColName, asc, Int32.Parse(context.Request["CustomerId"]));
                    }
                    return GetCardOrderIncomeList("1=1" + " and status=1 and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {


                }
            }

            else if (context.Request[param[0]].Equals("CardRedPacketDetailList"))
            {
                try
                {
                    return GetCardRedPacketDetailList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }

            else if (context.Request[param[0]].Equals("CardLaunchList"))
            {
                try
                {
                    return GetCardLaunchList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }



            else if (context.Request[param[0]].Equals("CardList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        return GetCardList("CustomerId=" + context.Request["CustomerId"] + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }
                    return GetCardList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }

            else if (context.Request[param[0]].Equals("CardExchangeCodeList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        return GetCardExchangeCodeList("CustomerId=" + context.Request["CustomerId"] + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }
                    return GetCardExchangeCodeList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }

            else if (context.Request[param[0]].Equals("PartyContactsList"))
            {
                try
                {
                    int PartID = context.Request["PartID"] != null ? Int32.Parse(context.Request["PartID"]) : 0;
                    return GetPartyContactsList(PartID);
                    
                }
                catch (Exception ex)
                {

                }
            }

            else if (context.Request[param[0]].Equals("AgentFinanceList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        return GetAgentFinanceList("1=1" + " and CustomerId=" + context.Request["CustomerId"] + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }
                    return GetAgentFinanceList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardGroupList"))
            {
                try
                {
                    return GetCardGroupList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardRebateList"))
            {
                try
                {
                    return GetCardRebateList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("QuestionnaireList"))
            {
                try
                {
                    return GetQuestionnaireList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("QuestionnaireSignupList"))
            {
                try
                {
                    if (context.Request["QuestionnaireID"] != null)
                    {
                        return GetQuestionnaireSignupList("1=1" + " and QuestionnaireID=" + context.Request["QuestionnaireID"] + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }
                    return GetQuestionnaireSignupList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("PartyList"))
            {
                try
                {
                    if (context.Request["isLuckDraw"] == "true")
                    {
                        return GetPartyList("Type=3" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }
                    return GetPartyList("Type<>3" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);  
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("DiscountCodeList"))
            {
                try
                {
                    return GetDiscountCodeList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardAgentList"))
            {
                try
                {
                    return GetCardAgentList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("BusinessCardList"))
            {
                try
                {
                    return GetBusinessCardList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("BusinessCardPosterList"))
            {
                try
                {
                    return GetBusinessCardPosterList(pageindex, iDisplayLength);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("BusinessCardOrderList"))
            {
                try
                {
                    return GetBusinessCardOrderList("Status=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("AchievemenList"))
            {
                try
                {
                    return GetAchievemenList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CustomerCardList"))
            {
                try
                {
                    if (context.Request["originCustomerId"] != null&& context.Request["originCustomerId"] != null)
                    {
                        condition = " originCustomerId = " + context.Request["originCustomerId"] + " and " + "CreatedAt like '" + context.Request["MONTH"] + "%'" + " and " + condition;
                    }
                    return GetCustomerCardList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("AgencyApproveList"))
            {
                try
                {
                    return GetAgencyApproveList("RealNameStatus = 3" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("AgencyExperienceList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    return GetAgencyExperienceList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("AgencyExperienceApproveList"))
            {
                try
                {
                    return GetAgencyExperienceList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("RequirementList"))
            {
                try
                {
                    if(context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    if (context.Request["provinceId"] != null)
                    {
                        if (Convert.ToInt32(context.Request["ProvinceId"]) > 0)
                            condition = " ProvinceId = " + context.Request["provinceId"] + " and " + condition;
                    }
                    if (context.Request["cityId"] != null)
                    {
                        if (Convert.ToInt32(context.Request["cityId"]) > 0)
                            condition = " CityId = " + context.Request["cityId"] + " and " + condition;
                    }
                    if (context.Request["parentCategoryId"] != null)
                    {
                        if (Convert.ToInt32(context.Request["ParentCategoryId"]) > 0)
                            condition = " ParentCategoryId = " + context.Request["parentCategoryId"] + " and " + condition;
                    }
                    if (context.Request["categoryId"] != null)
                    {
                        if (Convert.ToInt32(context.Request["categoryId"]) > 0)
                            condition = " CategoryId = " + context.Request["categoryId"] + " and " + condition;
                    }
                    return GetRequirementList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("DemandList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    if (context.Request["Status"] != null)
                    {
                        condition = " Status = " + context.Request["Status"] + " and " + condition;
                    }

                    return GetDemandList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CategoryRequireList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and Status in (1,3) and " + condition;
                    }
                    if (context.Request["provinceId"] != null)
                    {
                        condition = " ProvinceId = " + context.Request["provinceId"] + " and " + condition;
                    }
                    if (context.Request["cityId"] != null)
                    {
                        if (Convert.ToInt32(context.Request["cityId"]) > 0)
                            condition = " CityId = " + context.Request["cityId"] + " and " + condition;
                    }
                    if (context.Request["parentCategoryId"] != null)
                    {
                        condition = " ParentCategoryId = " + context.Request["parentCategoryId"] + " and " + condition;
                    }
                    if (context.Request["categoryId"] != null)
                    {
                        if (Convert.ToInt32(context.Request["categoryId"]) > 0)
                            condition = " CategoryId = " + context.Request["categoryId"] + " and " + condition;
                    }
                    return GetRequirementList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("ServicesList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    return GetServicesList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("ProjectList"))
            {
                try
                {
                    if (context.Request["ExcludeStatus"] != null)
                    {
                        condition = " Status <> " + context.Request["ExcludeStatus"] + " and " + condition;
                    }
                    return GetProjectList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("BusinessProjectList"))
            {
                try
                {
                    if (context.Request["BusinessCustomerId"] != null)
                    {
                        condition = " BusinessCustomerId = " + context.Request["BusinessCustomerId"] + " and " + condition;
                    }
                    else
                    {
                        condition = " BusinessCustomerId = 0 and " + condition;
                    }
                    return GetProjectList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("AgencyProjectList"))
            {
                try
                {
                    if (context.Request["AgencyCustomerId"] != null)
                    {
                        condition = " AgencyCustomerId = " + context.Request["AgencyCustomerId"] + " and " + condition;
                    }
                    else
                    {
                        condition = " AgencyCustomerId = 0 and " + condition;
                    }
                    return GetProjectList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("AgencyContractList"))
            {
                try
                {
                    if (context.Request["AgencyCustomerId"] != null)
                    {
                        condition = "Status = 1 and AgencyCustomerId = " + context.Request["AgencyCustomerId"] + " and " + condition;
                    }
                    else
                    {
                        condition = " AgencyCustomerId = 0 and " + condition;
                    }
                    return GetContractList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("zxbRequireList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    return GetzxbRequireList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("BusinessContractList"))
            {
                try
                {
                    if (context.Request["BusinessCustomerId"] != null)
                    {
                        condition = "Status = 1 and BusinessCustomerId = " + context.Request["BusinessCustomerId"] + " and " + condition;
                    }
                    else
                    {
                        condition = " BusinessCustomerId = 0 and " + condition;
                    }
                    return GetContractList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("ContractList"))
            {
                try
                {

                    condition = "Status = 1 and " + condition;

                    return GetContractList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CompleteProjectList"))
            {
                try
                {
                    return GetProjectList(" Status = 2 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("ComplaintsList"))
            {
                try
                {
                    return GetComplaintsList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("ComplaintsUnResloveList"))
            {
                try
                {
                    return GetComplaintsList(" Status = 0 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("MarkAgencyList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " MarkCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    return GetMarkAgencyList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("MarkBusinessList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " MarkCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    return GetMarkBusinessList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("MarkProjectList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " MarkCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    return GetMarkProjectList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("MarkRequireList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " MarkCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    return GetMarkRequireList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("MessageList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " SendTo = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    else
                    {
                        condition = " SendTo = 0 and " + condition;
                    }
                    if (context.Request["MessageTypeId"] != null)
                    {
                        condition = " MessageTypeId = " + context.Request["MessageTypeId"] + " and " + condition;
                    }
                    return GetMessageList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("SystemMessageList"))
            {
                try
                {                  
                    return GetSystemMessageList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardMessageList"))
            {
                try
                {
                    return GetCardMessageList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardOrderList"))
            {
                try
                {
                    return GetCardOrderList(condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("SoftarticleList"))
            {
                try
                {
                    string sql = " Status>0 and OriginalSoftArticleID = 0" + " and " + condition;
                    if(context.Request["SoftArticleID"]!="0")
                    {
                        sql = " Status>0  and OriginalSoftArticleID = " + context.Request["SoftArticleID"] + " and " + condition;
                    }

                    return GetSoftarticleList(sql, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("ComplaintList"))
            {
                try
                {
                    return GetComplaintList(condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardNewsList"))
            {
                try
                {
                    return GetCardNewsList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardPoterList"))
            {
                try
                {
                    return CardPoterList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("FarmGameViewList"))
            {
                try
                {
                    return GetFarmGameViewList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("FarmgamePrizeList"))
            {
                try
                {
                    return GetFarmgamePrizeList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)   
                {

                }
            }
            else if (context.Request[param[0]].Equals("FarmgamePrizeOrderViewList"))
            {
                try
                {
                    return GetFarmgamePrizeOrderViewList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardHelpList"))
            {
                try
                {
                    return GetCardHelpList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("MiniprogramsList"))
            {
                try
                {
                    return GetMiniprogramsList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("BusinessCardHelpList"))
            {
                try
                {
                    return GetBusinessCardHelpList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CompanyList"))
            {
                try
                {
                    return GetCompanyList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("TenderList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    else
                    {
                        condition = " 1<> 1 and " + condition;
                    }
                    return GetTenderList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("TenderInviteList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    else
                    {
                        condition = " 1<> 1 and " + condition;
                    }
                    return GetTenderInviteList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CustomerHostingList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    else
                    {
                        condition = " 1<> 1 and " + condition;
                    }
                    return GetCommissionDelegationViewList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("RequireCommissionList"))
            {
                try
                {
                    string CustomerId = context.Request["CustomerId"];
                    return GetRequireCommissionList(Convert.ToInt32(CustomerId));
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CustomerPayOutList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    else
                    {
                        condition = " 1<> 1 and " + condition;
                    }
                    return GetCustomerPayOutList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CustomerInComeList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                    }
                    else
                    {
                        condition = " 1<> 1 and   " + condition;
                    }
                    return GetCustomerInComeList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CustomerPayInList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and PayInStatus = 1 and " + condition;
                    }
                    else
                    {
                        condition = " 1<> 1 and  PayInStatus = 1  " + condition;
                    }
                    return GetCustomerPayInList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }

            else if (context.Request[param[0]].Equals("CustomerPayInViewList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and PayInStatus = 1 and " + condition;
                    }
                    else
                    {
                        condition = "  PayInStatus = 1  and " + condition;
                    }
                    return GetCustomerPayInHistoryViewList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CustomerPayOutViewList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + " and PayOutStatus = 1 and " + condition;
                    }
                    else
                    {
                        condition = "   PayOutStatus = 1  and " + condition;
                    }
                    return GetCustomerPayOutHistoryViewList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CustomerPayOutHandleList"))
            {
                try
                {
                    condition = "   PayOutStatus = 0  and " + condition;                  
                    return GetCustomerPayOutHistoryViewList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardPayOutHandleList"))
            {
                try
                {
                    condition = " PayOutStatus=0 and " + condition;
                    return GetCardPayOutHistoryList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("CardPayOutHandleList2"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        return GetCardPayOutHistoryList("1=1" + " and CustomerId=" + context.Request["CustomerId"] + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                    }
                    condition = "  PayOutStatus!=0 and " + condition;
                    return GetCardPayOutHistoryList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("BcPayOutHistoryList"))
            {
                try
                {
                    condition = " PayOutStatus=0 and " + condition;
                    return GetBcPayOutHistoryList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("PlatformCommissionList"))
            {
                try
                {
                    
                    return GetPlatformCommissionList(" 1=1 and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
       

            return new List<CommonVO>();
        }
       
        public static List<CommonVO> GetDepartmentList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            DepartmentBO dBO = new DepartmentBO(new UserPrincipal().UserProfile);
            List<DepartmentViewVO> list = dBO.FindAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (DepartmentViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetUserList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            UserBO uBO = new UserBO(new UserPrincipal().UserProfile);
            List<UserViewVO> list = uBO.FindAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (UserViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetRoleList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            RoleBO rBO = new RoleBO(new UserPrincipal().UserProfile);
            List<RoleViewVO> list = rBO.FindAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (RoleViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCustomerList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CustomerViewVO> list = uBO.FindAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CustomerViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCustomerVipList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CustomerVO> list = uBO.FindAllByPageIndex2(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CustomerVO vo in list)
            {
                vo.ExchangeCodeCount = cBO.FindExchangeCodeTotalCount("CustomerId = "+ vo.CustomerId+ " and ExpirationAt>now()");
                vo.U_ExchangeCodeCount = cBO.FindExchangeCodeTotalCount("CustomerId = " + vo.CustomerId+ " and Status=0 and ExpirationAt>now()");
                volist.Add(vo);
            }
            return volist;
        }
        

        public static List<CommonVO> GetCustomerLoginList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CustomerLoginHistoryVO> list = uBO.FindAllLoginHistoryByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CustomerLoginHistoryVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetViolationList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<ViolationVO> list = uBO.FindAllViolationByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (ViolationVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetAgentApplyList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardAgentApplyVO> list = uBO.FindAllByPageIndexByCardAgentApply(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardAgentApplyVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetVipApplyList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardVipApplyVO> list = uBO.FindAllByPageIndexByCardVipApply(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardVipApplyVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetZxbConfigList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<ZxbConfigVO> list = uBO.FindZxbConfigByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (ZxbConfigVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetBusinessList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            List<BusinessViewVO> list = uBO.FindAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (BusinessViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetBusinessApproveList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            List<BusinessViewVO> list = uBO.FindAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (BusinessViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetProvinceList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CityBO uBO = new CityBO(new UserProfile());
            List<ProvinceVO> list = uBO.FindProvinceAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (ProvinceVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCategoryList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CategoryBO uBO = new CategoryBO(new UserProfile());
            List<CategoryVO> list = uBO.FindCategoryAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CategoryVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetSuggestionList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            SystemBO uBO = new SystemBO(new UserProfile());
            List<SuggestionVO> list = uBO.FindSuggestionAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (SuggestionVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCarouselList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            SystemBO uBO = new SystemBO(new UserProfile());
            List<CarouselVO> list = uBO.FindCarouselAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CarouselVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetCityList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CityBO uBO = new CityBO(new UserProfile());
            condition = string.IsNullOrEmpty(condition) ? "Status = true and ProvinceStatus = true" : (condition + " and Status = true and ProvinceStatus = true");
            List<CityViewVO> list = uBO.FindCityAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CityViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCategoryChooseList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CategoryBO uBO = new CategoryBO(new UserProfile());
            condition = string.IsNullOrEmpty(condition) ? "Status = true and ParentCategoryStatus = true" : (condition + " and Status = true and ParentCategoryStatus = true");
            List<CategoryViewVO> list = uBO.FindCategoryViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CategoryViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetAgencyList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencyViewVO> list = uBO.FindAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (AgencyViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetCardRedPacketList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardRedPacketViewVO> list = uBO.FindCardRedPacketViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardRedPacketViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCardRedPacketDetaillistList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardRedPacketListViewVO> list = uBO.FindCardRedPacketListViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardRedPacketListViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetPersonalList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            BusinessCardBO uBO = new BusinessCardBO(new CustomerProfile());
            
            List<PersonalViewVO> list = uBO.FindAllByPageIndexByPersonal(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (PersonalViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        
        public static List<CommonVO> GetCardPartSignInNumList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardPartySignUpViewVO> list = uBO.FindCardPartSignUpViewByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardPartySignUpViewVO vo in list)
            {
                List<CardDataVO> CardDataVO = uBO.FindCardByCustomerId(vo.InviterCID);
                if (CardDataVO.Count > 0)
                {
                    vo.PromotionAwardName = CardDataVO[0].Name;
                }
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCardPartyInviterList(int PartyID, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardPartySignUpViewVO> list = uBO.FindAllByPageIndexByInviter(PartyID, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardPartySignUpViewVO vo in list)
            {
                List<CardDataVO> CardDataVO = uBO.FindCardByCustomerId(vo.CustomerId);
                if (CardDataVO.Count > 0)
                {
                    vo.Name = CardDataVO[0].Name;
                }
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCardAccessRecordsViewList(int PartyID, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardAccessRecordsViewVO> list = uBO.FindCardAccessRecordsViewAllByPageIndex("ById="+ PartyID+ " and (Type='ReadParty' or Type='ForwardParty' or Type='SignUpParty')", (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardAccessRecordsViewVO vo in list)
            {
                vo.Nation= vo.AccessAt.ToString("yyyy-MM-dd HH:mm:ss");

                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetGroupJoinNumList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardGroupCardViewVO> list = uBO.FindCardGroupJoinAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardGroupCardViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCardPayOutList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardPartyOrderViewVO> list = uBO.FindCardPayOutAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardPartyOrderViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> CardBalanceList(int CustomerId, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardBalanceList> list = uBO.FindBalanceListByCustomerId(CustomerId);
            IEnumerable<CardBalanceList> newlist = list.Skip((pageindex - 1) * iDisplayLength).Take(iDisplayLength);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardBalanceList vo in newlist)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCardOrderIncomeList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc,int CustomerId=0)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardOrderViewVO> list = cBO.FindCardOrderViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardOrderViewVO vo in list)
            {
                if (CustomerId > 0)
                {
                    if(vo.OneRebateCustomerId== CustomerId)
                    {
                        vo.Cost = vo.OneRebateCost;
                        vo.Position = "直推奖";
                    }
                    if (vo.TwoRebateCustomerId == CustomerId)
                    {
                        vo.Cost = vo.TwoRebateCost;
                        vo.Position = "间推奖";
                    }
                }
                volist.Add(vo);
            }
            return volist;
        }


        public static List<CommonVO> GetCardRedPacketDetailList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardRedPacketDetailVO> list = uBO.FindCardRedPacketDetailAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardRedPacketDetailVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCardLaunchList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardLaunchVO> list = uBO.FindCardLaunchAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardLaunchVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        

        public static List<CommonVO> GetCardList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CardDataViewVO> list= cBO.FindCardDataViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            for(int i=0;i< list.Count;i++)
            {
                List<CardDataVO> cVO = cBO.FindCardByCustomerId(list[i].originCustomerId);
                if (cVO.Count > 0)
                {
                    list[i].originName = cVO[0].Name;
                }else
                {
                    if(list[i].originCustomerId>0)
                        list[i].originName = uBO.FindCustomenById(list[i].originCustomerId).CustomerName;
                }

                List<CustomerLoginHistoryVO> hVO = uBO.FindLoginHistoryByParams("CustomerId="+ list[i].CustomerId+ " order by LoginAt desc limit 1");
                if (hVO.Count > 0)
                {
                    list[i].LoginAt = hVO[0].LoginAt;
                }
            }

            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardDataViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCardExchangeCodeList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CardExchangeCodeVO> list = cBO.FindAllByPageIndexByExchangeCode(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardExchangeCodeVO vo in list)
            {
                if (vo.ToCustomerId > 0)
                {
                    CustomerVO cVO = uBO.FindCustomenById(vo.ToCustomerId);
                    if (cVO != null){
                        vo.ToCustomerName = cVO.CustomerName;
                    }
                }
                volist.Add(vo);
            }
            return volist;
        }


        public static List<CommonVO> GetPartyContactsList(int PartID)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CardPartyContactsVO> list = cBO.FindPartyContacts(PartID);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardPartyContactsVO vo in list)
            {
                CardDataVO cVO = cBO.FindCardById(vo.CardID);
                volist.Add(cVO);
            }
            return volist;
        }

        

        public static List<CommonVO> GetAgentFinanceList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CardAgentFinanceVO> list = cBO.FindAllByPageIndexByCardAgentFinance(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);

            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardAgentFinanceVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCardGroupList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardGroupViewViewVO> list = cBO.FindAllByPageIndexByGroup(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardGroupViewViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetQuestionnaireList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardQuestionnaireViewVO> list = cBO.FindCardQuestionnaireViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardQuestionnaireViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCardRebateList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardRebateViewVO> list = cBO.FindAllByPageIndexByCardRebate(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardRebateViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        
        public static List<CommonVO> GetQuestionnaireSignupList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardQuestionnaireSignupVO> list = cBO.FindCardQuestionnaireSignupAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardQuestionnaireSignupVO vo in list)
            {
                string SigupForm = "";


                var model = JsonConvert.DeserializeObject<List<QuestionnaireSigupForm>>(vo.SigupForm);

                for (int i = 0; i < model.Count; i++)
                {
                    if (model[i].Type != 4)
                    {
                        SigupForm += model[i].Name + "：" + model[i].value + "；\r\n";
                    }else
                    {
                        SigupForm += model[i].Name + "：";
                        for (int j = 0; j < model[i].UrlList.Count; j++){
                            SigupForm += model[i].UrlList[j].url;
                            if(j!= model[i].UrlList.Count - 1)
                            {
                                SigupForm += ",";
                            }
                        }
                        SigupForm+= "；\r\n";
                    }
                    
                }

                vo.SigupForm = SigupForm;

                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetPartyList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardPartyVO> list = cBO.newFindAllByPageIndexByParty(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            SystemBO sBO = new SystemBO(new UserProfile());
            ConfigVO ConfigVO = sBO.FindConfig();
            foreach (CardPartyVO vo in list)
            {
                /*
                try
                {
                    vo.counts = cBO.FindCardPartSignInNumTotalCount("PartyID = " + vo.PartyID);
                    vo.Cost = cBO.FindPartyOrderSumCost("PartyID = " + vo.PartyID + " and Status=1");
                    List<CardDataVO> cvo = cBO.FindCardByCustomerId(vo.CustomerId);
                    if (cvo.Count > 0)
                    {
                        vo.Name = cvo[0].Name;
                        vo.Phone = cvo[0].Phone;
                        vo.Headimg = cvo[0].Headimg;
                    }
                }
                catch
                {

                }
                */
                vo.Details = "";
                if (ConfigVO.CompanyPartyID == vo.PartyID)
                {
                    vo.Details = "头条活动";
                }
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetDiscountCodeList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardDiscountCodeVO> list = cBO.FindAllByPageIndexByDiscountCode(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardDiscountCodeVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCardAgentList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardAgentVO> list = cBO.FindAllByPageIndexByCardAgent(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardAgentVO vo in list)
            {
                CardAgentVO CardAgentVO = cBO.FindCardAgentByVO(vo);
                CardAgentVO.UserCount = cBO.getCardAgentByUserCount(CardAgentVO.City,"1=1");
                CardAgentVO.VipCount = cBO.getCardAgentByUserCount(CardAgentVO.City, "isVip=1 and DATE_FORMAT(ExpirationAt,'%y-%m-%d') > DATE_FORMAT(now(),'%y-%m-%d')");
                volist.Add(CardAgentVO);
            }
            return volist;
        }

        public static List<CommonVO> GetBusinessCardList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<BusinessCardViewVO> list = cBO.FindAllByPageIndexByBusinessCard(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (BusinessCardViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetBusinessCardPosterList(int pageindex, int iDisplayLength)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<FileVO> list = cBO.GetPosterback(iDisplayLength, pageindex);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (FileVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetBusinessCardOrderList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
            List<OrderViewVO> list = cBO.FindOrderViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (OrderViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetAchievemenList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardAchievemenViewVO> list = cBO.FindAllByPageIndexByAchievemen(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardAchievemenViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCustomerCardList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CustomerCardViewVO> list = cBO.FindAllByPageIndexByCustomerCard(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CustomerCardViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetAgencyApproveList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencyViewVO> list = uBO.FindAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (AgencyViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetAgencyExperienceList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencyExperienceViewVO> list = uBO.FindAllExperienceByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (AgencyExperienceViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetRequirementList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<RequirementViewVO> list = uBO.FindRequireAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (RequirementViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetServicesList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            ServicesBO uBO = new ServicesBO(new CustomerProfile());
            List<ServicesViewVO> list = uBO.FindServicesAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (ServicesViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetProjectList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ProjectViewVO> list = uBO.FindProjectAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (ProjectViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetContractList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ContractViewVO> list = uBO.FindContractAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (ContractViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetComplaintsList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<ComplaintsViewVO> list = uBO.FindComplaintsAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (ComplaintsViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetMarkAgencyList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<MarkAgencyViewVO> list = uBO.FindMarkAgencyAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (MarkAgencyViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetzxbRequireList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO _bo = new CustomerBO(new CustomerProfile());
            List<zxbRequireVO> list = _bo.ZXBFindRequireAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (zxbRequireVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetMarkBusinessList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<MarkBusinessViewVO> list = uBO.FindMarkBusinessAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (MarkBusinessViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetMarkProjectList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<MarkProjectViewVO> list = uBO.FindMarkProjectAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (MarkProjectViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetMarkRequireList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<MarkRequireViewVO> list = uBO.FindMarkRequireAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (MarkRequireViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetMessageList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            MessageBO uBO = new MessageBO(new CustomerProfile());
            List<MessageViewVO> list = uBO.FindAllMessageByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (MessageViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetSystemMessageList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            SystemBO uBO = new SystemBO(new CustomerProfile());
            List<SystemMessageViewVO> list = uBO.FindSystemMessageAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (SystemMessageViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetCardMessageList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);

            List<CardNoticeVO> list = cBO.FindCardNoticeAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardNoticeVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetCardOrderList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);

            List<CardOrderViewVO> list = cBO.FindCardOrderViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardOrderViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        
        public static List<CommonVO> GetSoftarticleList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);

            List<CardSoftArticleVO> list = cBO.FindSoftArticleAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    if (list[i].OriginalSoftArticleID > 0)
                    {
                        CardSoftArticleVO ocVO = cBO.FindSoftArticleById(list[i].OriginalSoftArticleID);

                        list[i].ReadCount = ocVO.ReadCount;
                        list[i].ReprintCount = ocVO.ReprintCount;
                        list[i].GoodCount = ocVO.GoodCount;
                    }

                    if (list[i].CardID > 0)
                    {
                        CardDataVO dVO = cBO.FindCardById(list[i].CardID);
                        if (dVO != null)
                        {
                            list[i].Card = dVO;
                            list[i].Headimg = list[i].Card.Headimg;
                            list[i].Name = list[i].Card.Name;
                        }else
                        {
                            List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(list[i].CustomerId);
                            if (CardDataVO.Count > 0)
                            {
                                list[i].Headimg = CardDataVO[0].Headimg;
                                list[i].Name = CardDataVO[0].Name;
                            }
                        }
                    }
                    else
                    {
                        List<CardDataVO> CardDataVO = cBO.FindCardByCustomerId(list[i].CustomerId);
                        if (CardDataVO.Count > 0)
                        {
                            list[i].Headimg = CardDataVO[0].Headimg;
                            list[i].Name = CardDataVO[0].Name;
                        }
                    }
                }
                catch(Exception ex)
                {
                    LogBO _log = new LogBO(typeof(GetDataJsonGrid));
                    string strErrorMsg = "Message:" + ex.Message.ToString() + "\r\n  Stack :" + ex.StackTrace + " \r\n Source :" + ex.Source;
                    _log.Error(strErrorMsg);
                }
                
            }
            
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardSoftArticleVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetComplaintList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);

            List<CardSoftArticleComplaintVO> list = cBO.FindSoftArticleComplaintAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardSoftArticleComplaintVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetCardNewsList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);

            List<CardNewsVO> list = cBO.FindCardNewsAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardNewsVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> CardPoterList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);

            List<CardPoterVO> list = cBO.FindAllByPageIndexByCardPoter(condition+ " and CustomerId=0", (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardPoterVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        
        public static List<CommonVO> GetFarmGameViewList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            FarmGameBO cBO = new FarmGameBO(new CustomerProfile());

            List<FarmGameViewVO> list = cBO.FindFarmGameViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (FarmGameViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetFarmgamePrizeList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            FarmGameBO cBO = new FarmGameBO(new CustomerProfile());

            List<FarmgamePrizeVO> list = cBO.FindFarmgamePrizeAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (FarmgamePrizeVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetFarmgamePrizeOrderViewList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            FarmGameBO cBO = new FarmGameBO(new CustomerProfile());

            List<FarmgamePrizeOrderViewVO> list = cBO.FindFarmgamePrizeOrderViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (FarmgamePrizeOrderViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        
        public static List<CommonVO> GetCardHelpList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);

            List<CardHelpVO> list = cBO.FindCardHelpAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardHelpVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetMiniprogramsList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            MiniprogramsBO cBO = new MiniprogramsBO();

            List<wxMiniprogramsVO> list = cBO.FindMiniprogramsAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (wxMiniprogramsVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetBusinessCardHelpList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());

            List<HelpVO> list = cBO.FindHelpAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (HelpVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        
        public static List<CommonVO> GetCompanyList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CompanyBO cBO = new CompanyBO(new CustomerProfile());

            List<SPLibrary.CustomerManagement.VO.CompanyVO> list = cBO.FindCompanyAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (SPLibrary.CustomerManagement.VO.CompanyVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetTenderList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<TenderInfoViewVO> list = uBO.FindTenderInfoAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (TenderInfoViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetTenderInviteList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<TenderInviteViewVO> list = uBO.FindTenderInviteAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (TenderInviteViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCommissionDelegationViewList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<CommissionDelegationViewVO> list = uBO.FindCommissionDelegationViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CommissionDelegationViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetRequireCommissionList(int CustomerId)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<RequireCommissionDelegationviewVO> list = uBO.FindRequireDelegateCommisiondelegationView(CustomerId);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (RequireCommissionDelegationviewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        

        public static List<CommonVO> GetCustomerPayOutList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<PayoutHistoryVO> list = uBO.FindPayoutHistoryAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (PayoutHistoryVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetCustomerInComeList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CommissionIncomeViewVO> list = uBO.FindCommissionIncomeAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CommissionIncomeViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetCustomerPayInList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<PayinHistoryVO> list = uBO.FindPayinHistoryAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (PayinHistoryVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetCardPayOutHistoryList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
            List<CardPayOutVO> list = uBO.FindCardPayoutHistoryAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CardPayOutVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        
        public static List<CommonVO> GetBcPayOutHistoryList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            BusinessCardBO uBO = new BusinessCardBO(new CustomerProfile());
            List<BcPayOutHistoryVO> list = uBO.FindBcPayOutHistoryAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (BcPayOutHistoryVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        
        public static List<CommonVO> GetCustomerPayOutHistoryViewList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CustomerPayOutHistoryViewVO> list = uBO.FindCustomerPayoutHistoryViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CustomerPayOutHistoryViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        public static List<CommonVO> GetCustomerPayInHistoryViewList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            CustomerBO uBO = new CustomerBO(new CustomerProfile());
            List<CustomerPayInHistoryViewVO> list = uBO.FindCustomerPayInHistoryViewAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (CustomerPayInHistoryViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }




        public static List<CommonVO> GetPlatformCommissionList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            SystemBO uBO = new SystemBO(new UserProfile());
            List<PlatformCommissionViewVO> list = uBO.FindPlatformCommissionAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (PlatformCommissionViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }
        
        public int getcount(string condition, string[] param, HttpContext context, ref string finalCondition)     //获取数据总行数，iTotalRecords参数需要
        {

            if (context.Request[param[0]].Equals("DeaprtmentList"))
            {
                DepartmentBO dBO = new DepartmentBO(new UserPrincipal().UserProfile);
                return dBO.FindTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("UserList"))
            {
                UserBO uBO = new UserBO(new UserPrincipal().UserProfile);
                return uBO.FindTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("RoleList"))
            {
                RoleBO rBO = new RoleBO(new UserPrincipal().UserProfile);
                return rBO.FindTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CustomerList"))
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindTotalCount(condition + " and " + " (originType<>'C_Service' or originType IS NULL) and  AppType=" + new UserPrincipal().UserProfile.AppType);
            }
            else if (context.Request[param[0]].Equals("CustomerVipList"))
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.GetCustomerCount(condition + " and Agent=1 and " + " (originType<>'C_Service' or originType IS NULL) and  AppType=" + new UserPrincipal().UserProfile.AppType);
            }
            else if (context.Request[param[0]].Equals("CustomerLoginList"))
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindLoginHistoryTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("ViolationList"))
            {
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindViolationTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("AgentApplyList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindCardAgentApplyTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("VipApplyList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindCardVipApplyTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("BusinessList"))
            {
                BusinessBO uBO = new BusinessBO(new CustomerProfile());
                return uBO.FindTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("BusinessApproveList"))
            {
                BusinessBO uBO = new BusinessBO(new CustomerProfile());
                condition = " Status = 0 " + " And " + condition;
                return uBO.FindTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("ProvinceList"))
            {
                CityBO uBO = new CityBO(new UserProfile());
                return uBO.FindProvinceTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CategoryList"))
            {
                CategoryBO uBO = new CategoryBO(new UserProfile());
                return uBO.FindCategoryTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("SuggestionList"))
            {
                SystemBO uBO = new SystemBO(new UserProfile());
                return uBO.FindSuggestionTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CityList"))
            {
                CityBO uBO = new CityBO(new UserProfile());
                condition = string.IsNullOrEmpty(condition) ? "Status = true and ProvinceStatus = true" : (condition + " and Status = true and ProvinceStatus = true");
                return uBO.FindCityTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CategoryChooseList"))
            {
                CategoryBO uBO = new CategoryBO(new UserProfile());
                condition = string.IsNullOrEmpty(condition) ? "Status = true and ParentCategoryStatus = true" : (condition + " and Status = true and ParentCategoryStatus = true");
                return uBO.FindCategoryViewTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("AgencyList"))
            {
                AgencyBO uBO = new AgencyBO(new CustomerProfile());
                if (context.Request["provinceId"] != null)
                {
                    if (Convert.ToInt32(context.Request["ProvinceId"]) > 0)
                        condition = " ProvinceId = " + context.Request["provinceId"] + " and " + condition;
                }
                if (context.Request["cityId"] != null)
                {
                    if (Convert.ToInt32(context.Request["cityId"]) > 0)
                        condition = " CityId = " + context.Request["cityId"] + " and " + condition;
                }
                if (context.Request["parentCategoryId"] != null)
                {
                    if (Convert.ToInt32(context.Request["ParentCategoryIds"]) > 0)
                        condition = " ParentCategoryIds like '%," + context.Request["parentCategoryId"] + ",%' and " + condition;
                }
                if (context.Request["categoryId"] != null)
                {
                    if (Convert.ToInt32(context.Request["categoryId"]) > 0)
                        condition = " CategoryIds like '%," + context.Request["categoryId"] + ",%' and " + condition;
                }
                return uBO.FindTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardRedPacketList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
               
                return uBO.FindCardRedPacketViewTotalCount(condition);
            }

            else if (context.Request[param[0]].Equals("CardRedPacketDetaillistList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                if (context.Request["RedPacketId"] != null) {
                    condition = "RedPacketId="+ context.Request["RedPacketId"];
                }
                return uBO.FindCardRedPacketListViewTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardLaunchList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindCardLaunchCount(condition);
            }
            else if (context.Request[param[0]].Equals("PersonalList"))
            {
                BusinessCardBO uBO = new BusinessCardBO(new CustomerProfile());
                if (context.Request["BusinessID"] != null)
                {
                    condition = "BusinessID=" + context.Request["BusinessID"];
                }
                if (context.Request["CustomerId"] != null)
                {
                    condition = "CustomerId=" + context.Request["CustomerId"];
                }
                return uBO.FindPersonalTotalCount(condition);
            }
            
            else if (context.Request[param[0]].Equals("PartSignList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                if (context.Request["PartID"] != null)
                {
                    condition = "PartyID=" + context.Request["PartID"];
                }
                return uBO.FindCardPartSignUpViewListTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("PartyInviterList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindInviterNumTotalCount(Convert.ToInt32(context.Request["PartID"]));  
            }
            else if (context.Request[param[0]].Equals("CardAccessRecordsViewList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindCardAccessRecordsViewCount("ById=" + context.Request["PartID"]);
            }
            else if (context.Request[param[0]].Equals("GroupJoinList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                if (context.Request["GroupID"] != null)
                {
                    condition = "GroupID=" + context.Request["GroupID"];
                }
                return uBO.FindCardGroupJoinTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardPayOutList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                if (context.Request["CustomerId"] != null)
                {
                    condition = "Status=1 and HostCustomerId=" + context.Request["CustomerId"];
                }
                return uBO.FindCardPayOutTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardBalanceList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                int CustomerId = 0;
                if (context.Request["CustomerId"] != null)
                {
                    CustomerId = Convert.ToInt32(context.Request["CustomerId"]);
                }
                return uBO.FindBalanceListByCustomerId(CustomerId).Count;
            }

            else if (context.Request[param[0]].Equals("CardOrderIncomeList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                if (context.Request["CustomerId"] != null)
                {
                    condition = "Status=1 and (OneRebateCustomerId = " + context.Request["CustomerId"] + " or TwoRebateCustomerId = " + context.Request["CustomerId"] + ")";
                }
                return uBO.FindCardPayOutTotalCount(condition);
            }

            
            else if (context.Request[param[0]].Equals("CardRedPacketDetailList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);

                return uBO.FindCardRedPacketDetailTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                if (context.Request["CustomerId"] != null)
                {
                    condition = "CustomerId=" + context.Request["CustomerId"];
                }
                return uBO.FindCardDataViewTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardExchangeCodeList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                if (context.Request["CustomerId"] != null)
                {
                    condition = "CustomerId=" + context.Request["CustomerId"];
                }
                return uBO.FindExchangeCodeTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("PartyContactsList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                int PartID = context.Request["PartID"] != null ? Int32.Parse(context.Request["PartID"]) : 0;
                return uBO.FindPartyContacts(PartID).Count;
            }
            else if (context.Request[param[0]].Equals("AgentFinanceList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                if (context.Request["CustomerId"] != null)
                {
                    condition += "and CustomerId=" + context.Request["CustomerId"];
                }
                return uBO.FindCardAgentFinanceTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardGroupList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindGroupTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("QuestionnaireList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindCardQuestionnaireView(condition);
            }
            else if (context.Request[param[0]].Equals("CardRebateList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindCardRebateTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("QuestionnaireSignupList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                if (context.Request["QuestionnaireID"] != null)
                {
                    condition = "QuestionnaireID=" + context.Request["QuestionnaireID"];
                }
                return uBO.FindCardQuestionnaireSignup(condition);
            }
            else if (context.Request[param[0]].Equals("PartyList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                if (context.Request["isLuckDraw"] == "true")
                {
                    condition = "Type=3";
                }else
                {
                    condition = "Type<>3";
                }
                return uBO.newFindPartyTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("DiscountCodeList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindDiscountCodeTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardAgentList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindCardAgentTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("BusinessCardList"))
            {
                BusinessCardBO uBO = new BusinessCardBO(new CustomerProfile());
                return uBO.FindBusinessCardTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("BusinessCardPosterList"))
            {
                BusinessCardBO uBO = new BusinessCardBO(new CustomerProfile());
                return uBO.GetPosterbackNum();
            }
            else if (context.Request[param[0]].Equals("BusinessCardOrderList"))
            {
                BusinessCardBO uBO = new BusinessCardBO(new CustomerProfile());
                return uBO.FindOrderViewCount(condition);
            }
            else if (context.Request[param[0]].Equals("AchievemenList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindAchievemenTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CustomerCardList"))
            {
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                if (context.Request["originCustomerId"] != null && context.Request["MONTH"] != null)
                {
                    condition = " originCustomerId = " + context.Request["originCustomerId"] + " and " + "CreatedAt like '" + context.Request["MONTH"] + "%'" + " and "+ condition;
                }
                return uBO.FindCustomerCardTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("AgencyApproveList"))
            {
                AgencyBO uBO = new AgencyBO(new CustomerProfile());
                condition = " Status = 0 " + " And " + condition;
                return uBO.FindTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("AgencyExperienceList"))
            {
                AgencyBO uBO = new AgencyBO(new CustomerProfile());
                if (context.Request["CustomerId"] != null)
                {
                    condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                return uBO.FindExperienceTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("AgencyExperienceApproveList"))
            {
                AgencyBO uBO = new AgencyBO(new CustomerProfile());
                return uBO.FindExperienceTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("RequirementList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                if (context.Request["provinceId"] != null)
                {
                    if (Convert.ToInt32(context.Request["ProvinceId"]) > 0)
                        condition = " ProvinceId = " + context.Request["provinceId"] + " and " + condition;
                }
                if (context.Request["cityId"] != null)
                {
                    if (Convert.ToInt32(context.Request["cityId"]) > 0)
                        condition = " CityId = " + context.Request["cityId"] + " and " + condition;
                }
                if (context.Request["parentCategoryId"] != null)
                {
                    if (Convert.ToInt32(context.Request["ParentCategoryId"]) > 0)
                        condition = " ParentCategoryId = " + context.Request["parentCategoryId"] + " and " + condition;
                }
                if (context.Request["categoryId"] != null)
                {
                    if (Convert.ToInt32(context.Request["categoryId"]) > 0)
                        condition = " CategoryId = " + context.Request["categoryId"] + " and " + condition;
                }
                RequireBO uBO = new RequireBO(new CustomerProfile());
                return uBO.FindRequireTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("ServicesList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                ServicesBO uBO = new ServicesBO(new CustomerProfile());
                return uBO.FindServicesTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("ProjectList"))
            {
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                return uBO.FindProjectTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("BusinessProjectList"))
            {
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                if (context.Request["BusinessCustomerId"] != null)
                {
                    condition = "Status <> 6 and BusinessCustomerId = " + context.Request["BusinessCustomerId"] + " and " + condition;
                }

                return uBO.FindProjectTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("AgencyProjectList"))
            {
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                if (context.Request["AgencyCustomerId"] != null)
                {
                    condition = "Status <> 6 and AgencyCustomerId = " + context.Request["AgencyCustomerId"] + " and " + condition;
                }

                return uBO.FindProjectTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("AgencyContractList"))
            {
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                if (context.Request["AgencyCustomerId"] != null)
                {
                    condition = "Status = 1 and AgencyCustomerId = " + context.Request["AgencyCustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " AgencyCustomerId = 0 and " + condition;
                }

                return uBO.FindContractTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("BusinessContractList"))
            {
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                if (context.Request["BusinessCustomerId"] != null)
                {
                    condition = "Status = 1 and BusinessCustomerId = " + context.Request["BusinessCustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " BusinessCustomerId = 0 and " + condition;
                }

                return uBO.FindContractTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("ContractList"))
            {
                ProjectBO uBO = new ProjectBO(new CustomerProfile());

                condition = "Status = 1 and " + condition;

                return uBO.FindContractTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CompleteProjectList"))
            {
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                condition = " Status = 3 " + " And " + condition;
                return uBO.FindProjectTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("ComplaintsList"))
            {
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                return uBO.FindComplaintsTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("ComplaintsUnResloveList"))
            {
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                condition = " Status = 0 " + " And " + condition;
                return uBO.FindComplaintsTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("MarkAgencyList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " MarkCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindMarkAgencyTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("MarkBusinessList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " MarkCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindMarkBusinessTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("MarkProjectList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " MarkCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindMarkProjectTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("MarkRequireList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " MarkCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindMarkRequireTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("MessageList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " SendTo = " + context.Request["CustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " SendTo = 0 and " + condition;
                }
                if (context.Request["MessageTypeId"] != null)
                {
                    condition = " MessageTypeId = " + context.Request["MessageTypeId"] + " and " + condition;
                }
                MessageBO uBO = new MessageBO(new CustomerProfile());
                return uBO.FindMessageTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("SystemMessageList"))
            {
                SystemBO uBO = new SystemBO(new CustomerProfile());
                return uBO.FindSystemMessageTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardMessageList"))
            {
                CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return cBO.FindCardMessageTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardOrderList"))
            {
                CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return cBO.FindCardOrderViewTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("SoftarticleList"))
            {
                CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                string sql = " Status>0 and OriginalSoftArticleID = 0 and " + condition;

                if(context.Request["SoftArticleID"] != "0")
                {
                    sql = " Status>0  and OriginalSoftArticleID = " + context.Request["SoftArticleID"] + " and " + condition;
                }
                return cBO.FindSoftArticleTotalCount(sql);

            }
            else if (context.Request[param[0]].Equals("ComplaintList"))
            {
                CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return cBO.FindSoftArticleComplaintTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardNewsList"))
            {  
                CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return cBO.FindCardNewsTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardPoterList"))
            {
                CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return cBO.FindCardPoterTotalCount(condition+ " and CustomerId = 0");
            }
            else if (context.Request[param[0]].Equals("FarmGameViewList"))
            {
                FarmGameBO cBO = new FarmGameBO(new CustomerProfile());
                return cBO.FindFarmGameViewCount(condition);
            }
            else if (context.Request[param[0]].Equals("FarmgamePrizeList"))
            {
                FarmGameBO cBO = new FarmGameBO(new CustomerProfile());
                return cBO.FindFarmgamePrizeCount(condition);      
            }
            else if (context.Request[param[0]].Equals("FarmgamePrizeOrderViewList"))
            {
                FarmGameBO cBO = new FarmGameBO(new CustomerProfile());
                return cBO.FindFarmgamePrizeOrderViewCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardHelpList"))
            {
                CardBO cBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return cBO.FindCardHelpTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("MiniprogramsList"))
            {
                MiniprogramsBO cBO = new MiniprogramsBO();
                return cBO.FindMiniprogramsTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("BusinessCardHelpList"))
            {
                BusinessCardBO cBO = new BusinessCardBO(new CustomerProfile());
                return cBO.FindHelpTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CompanyList"))
            {
                CompanyBO cBO = new CompanyBO(new CustomerProfile());
                return cBO.FindCompanyCount(condition);
            }
            else if (context.Request[param[0]].Equals("TenderList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }
                RequireBO uBO = new RequireBO(new CustomerProfile());
                return uBO.FindTenderInfoTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("TenderInviteList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }
                RequireBO uBO = new RequireBO(new CustomerProfile());
                return uBO.FindTenderInviteTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CustomerHostingList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                return uBO.FindCommissionDelegationViewTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CustomerPayOutList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindPayoutHistoryTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CustomerPayInList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " CustomerId = " + context.Request["CustomerId"] + " and PayInStatus = 1  and " + condition;
                }
                else
                {
                    condition = " 1<>1 and and PayInStatus = 1 and  " + condition;
                }
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindPayinHistoryTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CustomerPayInViewList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " CustomerId = " + context.Request["CustomerId"] + " and PayInStatus = 1  and " + condition;
                }
                else
                {
                    condition = " PayInStatus = 1 and  " + condition;
                }
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindCustomerPayInHistoryViewTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CustomerPayOutViewList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " CustomerId = " + context.Request["CustomerId"] + " and PayInStatus = 1  and " + condition;
                }
                else
                {
                    condition = "  PayOutStatus = 1 and  " + condition;
                }
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindCustomerPayoutHistoryViewTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CustomerPayOutHandleList"))
            {
                condition = "   PayOutStatus = 0  and " + condition;
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindCustomerPayoutHistoryViewTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardPayOutHandleList"))
            {
                condition = " PayOutStatus=0 and " + condition;
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindCardPayoutHistoryTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CardPayOutHandleList2"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " 1=1  and " + condition+ " CustomerId=" + context.Request["CustomerId"];
                }else
                {
                    condition = " PayOutStatus!=0  and " + condition;
                }
                CardBO uBO = new CardBO(new CustomerProfile(), new UserPrincipal().UserProfile.AppType);
                return uBO.FindCardPayoutHistoryTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("BcPayOutHistoryList"))
            {
                condition = " PayOutStatus=0 and " + condition;
                BusinessCardBO uBO = new BusinessCardBO(new CustomerProfile());
                return uBO.FindBcPayoutHistoryTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("PlatformCommissionList"))
            {
                SystemBO uBO = new SystemBO(new UserProfile());
                return uBO.FindPlatformCommissionTotalCount(condition);
            }

            return 0;
        }

        public static List<CommonVO> GetDemandList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            DemandBO uBO = new DemandBO(new CustomerProfile());
            List<DemandViewVO> list = uBO.FindDemandAllByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (DemandViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static string ListToJson(int page, int iDisplayLength, int totalRow, List<CommonVO> volist, bool dt_dispose)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{\"page\":\"" + page.ToString() + "\",");
            json.Append("\"records\":\"" + totalRow.ToString() + "\",");
            if (volist.Count <= 0)
            {
                json.Append("\"total\":\"0\",");
                json.Append("\"invdata\":[]}");
            }
            else
            {
                json.Append("\"total\":\"" + (totalRow / iDisplayLength + (((totalRow % iDisplayLength) >= 1) ? 1 : 0)).ToString() + "\",");
                json.Append("\"invdata\":[");
                //json.AppendFormat("{\"sEcho\":{0},\n \"iTotalRecords\":{1},\n \"iTotalDisplayRecords\": {2},\n \"aaData\":[", sEcho, totalRow, totalRow);

                for (int i = 0; i < volist.Count; i++)
                {
                    json.Append("{");

                    List<string> columnname = VOHelper.GetVOPropertyList(volist[i].GetType());
                    for (int j = 0; j < columnname.Count; j++)
                    {
                        json.Append("\"");
                        json.Append(columnname[j]);
                        json.Append("\":\"");
                        if (volist[i][columnname[j]].GetType().Name.Equals("DateTime"))
                        {
                            if (columnname[j] == "CreatedAt" || columnname[j] == "UpdatedAt" || columnname[j] == "SendAt")
                                json.Append(((DateTime)volist[i].GetValue(typeof(object), columnname[j])).ToString("yyyy-MM-dd HH:mm:ss"));
                            else
                                json.Append(((DateTime)volist[i].GetValue(typeof(object), columnname[j])).ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            json.Append(volist[i].GetValue(typeof(object), columnname[j]).ToString().Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("\v", " ").Replace(@"\", " ").Replace("\"", "'"));
                        }
                        json.Append("\",");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("},");
                }
                json.Remove(json.Length - 1, 1);
                json.Append("]}");
            }
            return json.ToString();
        }

        public static string DataTableToJson(int page, int iDisplayLength, int totalRow, DataTable dtSource, bool dt_dispose)
        {
            totalRow = dtSource.Rows.Count;
            StringBuilder json = new StringBuilder();
            json.Append("{\"page\":\"" + page.ToString() + "\",");
            json.Append("\"records\":\"" + totalRow.ToString() + "\",");
            if (dtSource.Rows.Count <= 0)
            {
                json.Append("\"total\":\"0\",");
                json.Append("\"invdata\":[]}");
            }
            else
            {
                json.Append("\"total\":\"" + (totalRow / iDisplayLength + (((totalRow % iDisplayLength) >= 1) ? 1 : 0)).ToString() + "\",");
                json.Append("\"invdata\":[");
                //json.AppendFormat("{\"sEcho\":{0},\n \"iTotalRecords\":{1},\n \"iTotalDisplayRecords\": {2},\n \"aaData\":[", sEcho, totalRow, totalRow);

                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    json.Append("{");

                    //List<string> columnname = new List<string>();
                    foreach (DataColumn col in dtSource.Columns)
                    {
                        //    columnname.Add(col.ColumnName);
                        //}
                        //for (int j = 0; j < columnname.Count; j++)
                        //{
                        string columnName = col.ColumnName;
                        json.Append("\"");
                        json.Append(columnName);
                        json.Append("\":\"");
                        if (col.DataType.Name.Equals("DateTime"))
                        {
                            if (columnName == "CreatedAt" || columnName == "UpdatedAt")
                                json.Append(((DateTime)dtSource.Rows[i][columnName]).ToString("yyyy-MM-dd HH:mm:ss"));
                            else
                                json.Append(((DateTime)dtSource.Rows[i][columnName]).ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            json.Append(dtSource.Rows[i][columnName].ToString().Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("\v", " ").Replace(@"\", " ").Replace("\"", "'"));
                        }
                        json.Append("\",");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("},");
                }
                json.Remove(json.Length - 1, 1);
                json.Append("]}");
            }
            return json.ToString();
        }


    }
}

