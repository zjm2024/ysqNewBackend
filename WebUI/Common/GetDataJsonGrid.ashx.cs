using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.ProjectManagement.BO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.ProjectManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using SPLibrary.WebConfigInfo;
using System.Data;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using SPLibrary.CoreFramework.WebConfigInfo;

namespace WebUI.Common
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
            else if (context.Request[param[0]].Equals("CustomerList"))
            {
                try
                {
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
                    return GetBusinessApproveList(" Status = 0 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
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
                    if (context.Request["CategoryIds"] != null)
                    {
                        //如果是Search，怎不需要包含
                        if (!condition.Contains("CategoryIds LIKE"))
                        {
                            condition = " Status = 1 And CategoryIds like '%" + context.Request["CategoryIds"] + "%' and " + condition;
                        }
                    }
                    return GetAgencyList("1=1" + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }
            else if (context.Request[param[0]].Equals("MatchAgencyList"))
            {
                try
                {
                    int requireId = 0;
                    if (context.Request["Param"] != null)
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        Filter filter = serializer.Deserialize<Filter>(context.Request["Param"].ToString());
                        foreach (SPLibrary.WebConfigInfo.Rules ru in filter.rules)
                        {
                            if(ru.field == "RequireId")
                            {
                                requireId = Convert.ToInt32(ru.data);
                                break;
                            }
                        }
                    }
                    else if (context.Request["RequireId"] != null)
                    {
                        requireId = Convert.ToInt32(context.Request["RequireId"]);
                    }

                    return GetMatchAgencyList(requireId, " Status=1 ", pageindex, iDisplayLength, iSortColName, asc);
                }
                catch (Exception ex)
                {

                }
            }            
            else if (context.Request[param[0]].Equals("AgencyApproveList"))
            {
                try
                {
                    return GetAgencyApproveList(" Status = 0 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
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
            else if (context.Request[param[0]].Equals("RequirementList"))
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
                        if (context.Request["CategoryIds"] != null)
                        {
                            condition = " CategoryId in (" + context.Request["CategoryIds"] + ") and " + condition;
                        }
                        else
                        {
                            condition = " CategoryId = 0 and " + condition;
                        }
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
            else if (context.Request[param[0]].Equals("MatchRequireList"))
            {
                try
                {
                    int agencyId = 0;
                    if (context.Request["Param"] != null)
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        Filter filter = serializer.Deserialize<Filter>(context.Request["Param"].ToString());
                        foreach (SPLibrary.WebConfigInfo.Rules ru in filter.rules)
                        {
                            if (ru.field == "AgencyId")
                            {
                                agencyId = Convert.ToInt32(ru.data);
                                break;
                            }
                        }
                    }
                    else if (context.Request["AgencyId"] != null)
                    {
                        agencyId = Convert.ToInt32(context.Request["AgencyId"]);
                    }

                    return GetMatchRequirementList(agencyId, " Status=1 ", pageindex, iDisplayLength, iSortColName, asc);
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
                        condition = "Status <> 6 and BusinessCustomerId = " + context.Request["BusinessCustomerId"] + " and " + condition;
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
                        condition = "Status <> 6 and AgencyCustomerId = " + context.Request["AgencyCustomerId"] + " and " + condition;
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
            else if (context.Request[param[0]].Equals("CustomerPayOutList"))
            {
                try
                {
                    if (context.Request["CustomerId"] != null)
                    {
                        condition = " CustomerId = " + context.Request["CustomerId"] + "  and " + condition;
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
            else if (context.Request[param[0]].Equals("CustomerPayOutPendingList"))
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
            else if (context.Request[param[0]].Equals("MyBusinessList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " AgencyCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }
                return GetMyBusinessList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
            }
            else if (context.Request[param[0]].Equals("MyAgencyList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " BusinessCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }
                return GetMyAgencyList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
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
            else if (context.Request[param[0]].Equals("BusinessReviewList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " BusinessCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                else if (context.Request["AgencyCustomerId"] != null)
                {
                    condition = " AgencyCustomerId = " + context.Request["AgencyCustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }                
                return GetBusinessReviewList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
            }
            else if (context.Request[param[0]].Equals("AgencyReviewList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " AgencyCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                else if (context.Request["BusinessCustomerId"] != null)
                {
                    condition = " BusinessCustomerId = " + context.Request["BusinessCustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }
                return GetAgencyReviewList(" 1=1 " + " and " + condition, pageindex, iDisplayLength, iSortColName, asc);
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

        public static List<CommonVO> GetMatchAgencyList(int requireId,string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<AgencyViewVO> list = uBO.FindMatchAgencyByPageIndex(requireId,condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (AgencyViewVO vo in list)
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

        public static List<CommonVO> GetMatchRequirementList(int agencyId,string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            RequireBO uBO = new RequireBO(new CustomerProfile());
            List<RequirementViewVO> list = uBO.FindMatchRequireByPageIndex(agencyId,condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
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

        public static List<CommonVO> GetMyBusinessList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            AgencyBO uBO = new AgencyBO(new CustomerProfile());
            List<BusinessViewVO> list = uBO.FindAllMyBusinessByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (BusinessViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetMyAgencyList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            BusinessBO uBO = new BusinessBO(new CustomerProfile());
            List<AgencyViewVO> list = uBO.FindAllMyAgencyByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (AgencyViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetBusinessReviewList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<BusinessReviewViewVO> list = uBO.FindBusinessReviewByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (BusinessReviewViewVO vo in list)
            {
                volist.Add(vo);
            }
            return volist;
        }

        public static List<CommonVO> GetAgencyReviewList(string condition, int pageindex, int iDisplayLength, string iSortColName, string asc)
        {
            ProjectBO uBO = new ProjectBO(new CustomerProfile());
            List<AgencyReviewViewVO> list = uBO.FindAgencyReviewByPageIndex(condition, (pageindex - 1) * iDisplayLength + 1, pageindex * iDisplayLength, iSortColName, asc);
            List<CommonVO> volist = new List<CommonVO>();
            foreach (AgencyReviewViewVO vo in list)
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
                return uBO.FindTotalCount(condition);
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
                if (context.Request["CategoryIds"] != null)
                {
                    //如果是Search，怎不需要包含
                    if (!condition.Contains("CategoryIds LIKE"))
                    {
                        condition = " Status = 1 And CategoryIds like '%" + context.Request["CategoryIds"] + "%' and " + condition;
                    }
                }
                AgencyBO uBO = new AgencyBO(new CustomerProfile());
                return uBO.FindTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("MatchAgencyList"))
            {
                try
                {
                    int requireId = 0;
                    if (context.Request["Param"] != null)
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        Filter filter = serializer.Deserialize<Filter>(context.Request["Param"].ToString());
                        foreach (SPLibrary.WebConfigInfo.Rules ru in filter.rules)
                        {
                            if (ru.field == "RequireId")
                            {
                                requireId = Convert.ToInt32(ru.data);
                                break;
                            }
                        }
                    }
                    else if (context.Request["RequireId"] != null)
                    {
                        requireId = Convert.ToInt32(context.Request["RequireId"]);
                    }
                    AgencyBO uBO = new AgencyBO(new CustomerProfile());
                    return uBO.FindMatchAgencyTotalCount(requireId, "Status=1");
                }
                catch (Exception ex)
                {

                }
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
            else if (context.Request[param[0]].Equals("RequirementList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " CustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                if (context.Request["Status"] != null)
                {
                    condition = " Status = " + context.Request["Status"] + " and " + condition;
                    if (context.Request["CategoryIds"] != null)
                    {
                        condition = " CategoryId in (" + context.Request["CategoryIds"] + ") and " + condition;
                    }
                    else
                    {
                        condition = " CategoryId = 0 and " + condition;
                    }
                }
                RequireBO uBO = new RequireBO(new CustomerProfile());
                return uBO.FindRequireTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("MatchRequireList"))
            {
                try
                {
                    int agencyId = 0;
                    if (context.Request["Param"] != null)
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        Filter filter = serializer.Deserialize<Filter>(context.Request["Param"].ToString());
                        foreach (SPLibrary.WebConfigInfo.Rules ru in filter.rules)
                        {
                            if (ru.field == "AgencyId")
                            {
                                agencyId = Convert.ToInt32(ru.data);
                                break;
                            }
                        }
                    }
                    else if (context.Request["AgencyId"] != null)
                    {
                        agencyId = Convert.ToInt32(context.Request["AgencyId"]);
                    }
                    RequireBO uBO = new RequireBO(new CustomerProfile());
                    return uBO.FindMatchRequireTotalCount(agencyId, "Status=1");
                }
                catch (Exception ex)
                {

                }
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
                    condition = "Status <> 6 and  BusinessCustomerId = " + context.Request["BusinessCustomerId"] + " and " + condition;
                }

                return uBO.FindProjectTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("AgencyProjectList"))
            {
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                if (context.Request["AgencyCustomerId"] != null)
                {
                    condition = "Status <> 6 and  AgencyCustomerId = " + context.Request["AgencyCustomerId"] + " and " + condition;
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
            else if (context.Request[param[0]].Equals("CustomerPayOutPendingList"))
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
                    condition = " 1<>1 and " + condition;
                }
                CustomerBO uBO = new CustomerBO(new CustomerProfile());
                return uBO.FindPayinHistoryTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("MyBusinessList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " AgencyCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }
                AgencyBO uBO = new AgencyBO(new CustomerProfile());
                return uBO.FindMyBusinessTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("MyAgencyList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " BusinessCustomerId = " + context.Request["CustomerId"] + " and PayInStatus = 1  and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }
                BusinessBO uBO = new BusinessBO(new CustomerProfile());
                return uBO.FindMyAgencyTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("CustomerInComeList"))
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
                return uBO.FindCommissionIncomeTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("BusinessReviewList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " BusinessCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                else if (context.Request["AgencyCustomerId"] != null)
                {
                    condition = " AgencyCustomerId = " + context.Request["AgencyCustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                return uBO.FindBusinessReviewTotalCount(condition);
            }
            else if (context.Request[param[0]].Equals("AgencyReviewList"))
            {
                if (context.Request["CustomerId"] != null)
                {
                    condition = " AgencyCustomerId = " + context.Request["CustomerId"] + " and " + condition;
                }
                else if (context.Request["BusinessCustomerId"] != null)
                {
                    condition = " BusinessCustomerId = " + context.Request["BusinessCustomerId"] + " and " + condition;
                }
                else
                {
                    condition = " 1<>1 and " + condition;
                }
                ProjectBO uBO = new ProjectBO(new CustomerProfile());
                return uBO.FindAgencyReviewTotalCount(condition);
            }

            return 0;
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