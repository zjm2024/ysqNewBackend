using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using SPLibrary.CoreFramework.BO;
using CoreFramework.VO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CoreFramework;
using Newtonsoft.Json;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using WebUI.Common;
using SPlatformService.Controllers;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.WebConfigInfo;

namespace WebUI.Shared
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public string SiteName
        {
            get
            {
                if (string.IsNullOrEmpty(CacheSystemConfig.GetSystemConfig().SiteDescription))
                    return CacheSystemConfig.GetSystemConfig().SiteName;
                else
                    return CacheSystemConfig.GetSystemConfig().SiteName + "-" + CacheSystemConfig.GetSystemConfig().SiteDescription;
            }
        }
        public string SiteShortName
        {
            get
            {
                return CacheSystemConfig.GetSystemConfig().SiteName;
            }
        }
        public string ServicePhone
        {
            get
            {
                return CacheSystemConfig.GetSystemConfig().ServicePhone;
            }
        }

        public string ServiceNote
        {
            get
            {
                return CacheSystemConfig.GetSystemConfig().ServiceNote;
            }
        }
        public string APPImage
        {
            get
            {
                if (string.IsNullOrEmpty(CacheSystemConfig.GetSystemConfig().APPImage))
                    return ResolveUrl("~/Style/images/zhongxiaoleapp.jpg");
                else
                    return CacheSystemConfig.GetSystemConfig().APPImage;
            }
        }
        public string GZImage
        {
            get
            {
                if (string.IsNullOrEmpty(CacheSystemConfig.GetSystemConfig().GZImage))
                    return ResolveUrl("~/Style/images/zhongxiaolegzh.jpg");
                else
                    return CacheSystemConfig.GetSystemConfig().GZImage;
            }
        }
        public bool IsShowPageMeunTitle
        {
            set { breadcrumbs.Visible = value; }
        }
        public string MenuText
        {
            get { return lblMenu.Text; }
            set { lblMenu.Text = value; }
        }
        public string PageNameText
        {
            get { return lblPageName.Text; }
            set { lblPageName.Text = value; }
        }

        public CustomerProfile CustomerProfile
        {
            get { return new CustomerPrincipal().CustomerProfile; }
        }

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

        public string APIURL
        {
            get
            {
                return ConfigInfo.Instance.APIURL;
            }
        }
        
        public string IMUserID { get; set; }
        public string IMUserPD { get; set; }

        public int CustomerId
        {
            get
            {
                return CustomerProfile.CustomerId;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CustomerId > 0)
            {
                divLogin.Visible = false;
                divUser.Visible = true;
                index_zxt.Visible = true;
                lnkUser.InnerText = (string.IsNullOrEmpty(CustomerProfile.CustomerName) ? CustomerProfile.CustomerAccount : CustomerProfile.CustomerName);


                SPIMController spIMCon = new SPIMController();
                ResultObject result = spIMCon.GetIMuser(CustomerId, Token);
                if(result.Flag == 1)
                {
                    CustomerIMVO imVO = result.Result as CustomerIMVO;

                    IMUserID = imVO.IMId;
                    IMUserPD = imVO.IMPWD;
                }else
                {
                    IMUserID = "";
                    IMUserPD = "";
                }

                ZXTBO uBO = new ZXTBO(new CustomerProfile());
                string conditionStr = "MessageTo = " + CustomerId + " and Status=0";
                int MessageCount = uBO.FindZXTMessageCount(conditionStr);
                if (MessageCount > 0)
                {
                    string str1 = "";
                    if (MessageCount < 100)
                    {
                        str1 = MessageCount.ToString();
                    }
                    else {
                        str1 = "99+";
                    }
                    index_zxt_text.InnerHtml = "未读聊天消息<font>" + str1 + "</font>条";
                }
                else
                {
                    index_zxt_text.InnerHtml = "没有未读聊天消息";
                }
            }
            else
            {
                divLogin.Visible = true;
                divUser.Visible = false;
                index_zxt.Visible = false;
                lnkUser.InnerText = "";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("var ImgPath='").Append((Request.ApplicationPath == "/" ? "" : Request.ApplicationPath) + "/Style/images/").Append("';\n");

            Utilities.RegisterJs(Page, "JSCommonVar", sb.ToString());

            //CustomerName.Text = CustomerProfile.CustomerName;

            GetMenu();
            BindCity();
            //bind message count 
            if (!string.IsNullOrEmpty(Token))
            {
                CustomerController customerCon = new CustomerController();
                int count = Convert.ToInt32(customerCon.GetUnRedMessageCount(Token).Result);
                if (count > 0)
                    liaMessage.InnerHtml = "我的消息(<span style=\"color: red; \">" + count + "</span>)";
            }

            string[] strArr = ServicePhone.Split(';');
            string strPhone = "";
            foreach (string str in strArr)
            {
                if (strPhone != "")
                    strPhone += "<br />";
                strPhone += "<span>" + str + "</span>";

            }
            //divServicePhone.InnerHtml = strPhone;
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

        private void BindCity()
        {

            List<CityVO> cityList = SiteCommon.GetCityListAll();

            string cityStr = "<ul class=\"local-city-panel-bd clearfix\" id=\"divCitys\">";
            for (var i = 0; i < cityList.Count; i++)
            {
                cityStr += " <li class=\"city-bd\"> \r\n";
                cityStr += "      <a href=\"javascript:changeCity('" + cityList[i].CityId + "','" + cityList[i].CityName + "')\" title=\"" + cityList[i].CityName + "\" class=\"j-localize-city\" >" + cityList[i].CityName + "</a> \r\n";
                cityStr += "  </li> \r\n";
                if (i == 8)
                    break;
            }
            cityStr += "</ul>";
            divCityList.InnerHtml = cityStr;
        }

        private void GetMenu()
        {
            string _url = HttpContext.Current.Request.Url.ToString();
            DataTable MenuData = new DataTable();
            MenuData.Columns.Add("PageId");
            MenuData.Columns.Add("ParentId", typeof(int));
            MenuData.Columns.Add("PageURL");
            MenuData.Columns.Add("ChildURL");
            MenuData.Columns.Add("Name");
            DataRow row;
                        
            row = MenuData.NewRow();
            row["PageId"] = "1";
            row["ParentId"] = "0";
            row["Name"] = "我的关注";
            row["PageURL"] = "";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "2";
            row["ParentId"] = "0";
            row["Name"] = "我是雇主";
            row["PageURL"] = "";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "3";
            row["ParentId"] = "0";
            row["Name"] = "我是销售";
            row["PageURL"] = "";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "39";
            row["ParentId"] = "0";
            row["Name"] = "商机需求";
            row["PageURL"] = "~/RequireManagement/DemandBrowse.aspx";
            row["ChildURL"] = "~/RequireManagement/DemandBrowse.aspx";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "4";
            row["ParentId"] = "0";
            row["Name"] = "我的消息";
            row["PageURL"] = "~/CustomerManagement/MessageBrowse.aspx";
            row["ChildURL"] = "~/CustomerManagement/MessageCreateEdit.aspx";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "5";
            row["ParentId"] = "0";
            row["Name"] = "我的钱包";
            row["PageURL"] = "~/CustomerManagement/MyWallet.aspx";
            row["ChildURL"] = "~/CustomerManagement/MyWallet.aspx";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "7";
            row["ParentId"] = "0";
            row["Name"] = "我的奖励";
            row["PageURL"] = "~/CustomerManagement/zxbRequire.aspx";
            row["ChildURL"] = "~/CustomerManagement/zxbRequire.aspx";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "27";
            row["ParentId"] = "0";
            row["Name"] = "意见反馈";
            row["PageURL"] = "~/CustomerManagement/SuggestionCreate.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "6";
            row["ParentId"] = "0";
            row["Name"] = "个人中心";
            row["PageURL"] = "";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "9";
            row["ParentId"] = "2";
            row["Name"] = "雇主认证";
            row["PageURL"] = "~/CustomerManagement/BusinessCreateEdit.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "7";
            row["ParentId"] = "2";
            row["Name"] = "任务发布";
            row["PageURL"] = "~/RequireManagement/RequirementCreateEdit_New.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);           

            row = MenuData.NewRow();
            row["PageId"] = "8";
            row["ParentId"] = "2";
            row["Name"] = "我的任务";
            row["PageURL"] = "~/RequireManagement/RequirementBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);            

            row = MenuData.NewRow();
            row["PageId"] = "10";
            row["ParentId"] = "2";
            row["Name"] = "项目工作台";
            row["PageURL"] = "~/ProjectManagement/BusinessProjectBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "36";
            row["ParentId"] = "2";
            row["Name"] = "雇佣合同";
            row["PageURL"] = "~/ProjectManagement/BusinessContractBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);


            //row = MenuData.NewRow();
            //row["PageId"] = "11";
            //row["ParentId"] = "2";
            //row["Name"] = "我的工具包";
            //row["PageURL"] = "";
            //row["ChildURL"] = "";
            //MenuData.Rows.Add(row);

            //row = MenuData.NewRow();
            //row["PageId"] = "12";
            //row["ParentId"] = "2";
            //row["Name"] = "销售匹配";
            //row["PageURL"] = "";
            //row["ChildURL"] = "";
            //MenuData.Rows.Add(row);

            //row = MenuData.NewRow();
            //row["PageId"] = "13";
            //row["ParentId"] = "3";
            //row["Name"] = "发布服务";
            //row["PageURL"] = "~/RequireManagement/ServicesCreateEdit.aspx";
            //row["ChildURL"] = "";
            //MenuData.Rows.Add(row);

            //row = MenuData.NewRow();
            //row["PageId"] = "14";
            //row["ParentId"] = "3";
            //row["Name"] = "我的服务";
            //row["PageURL"] = "~/RequireManagement/ServicesBrowse.aspx";
            //row["ChildURL"] = "";
            //MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "15";
            row["ParentId"] = "3";
            row["Name"] = "销售认证";
            row["PageURL"] = "~/CustomerManagement/AgencyCreateEdit.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "16";
            row["ParentId"] = "3";
            row["Name"] = "面试邀请";
            row["PageURL"] = "~/CustomerManagement/TenderBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "17";
            row["ParentId"] = "3";
            row["Name"] = "我的投标";
            row["PageURL"] = "~/CustomerManagement/TenderInviteBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "18";
            row["ParentId"] = "3";
            row["Name"] = "项目工作台";
            row["PageURL"] = "~/ProjectManagement/AgencyProjectBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "37";
            row["ParentId"] = "3";
            row["Name"] = "雇佣合同";
            row["PageURL"] = "~/ProjectManagement/AgencyContractBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "19";
            row["ParentId"] = "3";
            row["Name"] = "我的销售案例";
            row["PageURL"] = "~/CustomerManagement/AgencyExperienceBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            //row = MenuData.NewRow();
            //row["PageId"] = "20";
            //row["ParentId"] = "3";
            //row["Name"] = "任务匹配";
            //row["PageURL"] = "";
            //row["ChildURL"] = "";
            //MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "21";
            row["ParentId"] = "6";
            row["Name"] = "账号信息";
            row["PageURL"] = "~/CustomerManagement/CustomerEdit.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "22";
            row["ParentId"] = "6";
            row["Name"] = "密码修改";
            row["PageURL"] = "~/CustomerManagement/PasswordChange.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "23";
            row["ParentId"] = "1";
            row["Name"] = "销售";
            row["PageURL"] = "~/CustomerManagement/MarkAgencyBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);           

            row = MenuData.NewRow();
            row["PageId"] = "24";
            row["ParentId"] = "1";
            row["Name"] = "案例";
            row["PageURL"] = "~/CustomerManagement/MarkProjectBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "25";
            row["ParentId"] = "1";
            row["Name"] = "任务";
            row["PageURL"] = "~/CustomerManagement/MarkRequireBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "26";
            row["ParentId"] = "1";
            row["Name"] = "雇主";
            row["PageURL"] = "~/CustomerManagement/MarkBusinessBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            //row = MenuData.NewRow();
            //row["PageId"] = "28";
            //row["ParentId"] = "3";
            //row["Name"] = "我的客户";
            //row["PageURL"] = "~/CustomerManagement/MyBusinessList.aspx";
            //row["ChildURL"] = "";
            //MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "29";
            row["ParentId"] = "2";
            row["Name"] = "我的销售";
            row["PageURL"] = "~/CustomerManagement/MyAgencyList.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "30";
            row["ParentId"] = "2";
            row["Name"] = "匹配销售";
            row["PageURL"] = "~/CustomerManagement/MatchAgency.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "31";
            row["ParentId"] = "3";
            row["Name"] = "匹配任务";
            row["PageURL"] = "~/CustomerManagement/MatchRequire.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "32";
            row["ParentId"] = "3";
            row["Name"] = "合同柜";
            row["PageURL"] = "~/CustomerManagement/MyContractBox.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "33";
            row["ParentId"] = "3";
            row["Name"] = "工具包";
            row["PageURL"] = "~/CustomerManagement/MyToolkit.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "34";
            row["ParentId"] = "2";
            row["Name"] = "合同柜";
            row["PageURL"] = "~/CustomerManagement/MyContractBox.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "35";
            row["ParentId"] = "2";
            row["Name"] = "工具包";
            row["PageURL"] = "~/CustomerManagement/MyToolkit.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "37";
            row["ParentId"] = "2";
            row["Name"] = "我的评价";
            row["PageURL"] = "~/CustomerManagement/BusinessReviewBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            row = MenuData.NewRow();
            row["PageId"] = "38";
            row["ParentId"] = "3";
            row["Name"] = "我的评价";
            row["PageURL"] = "~/CustomerManagement/AgencyReviewBrowse.aspx";
            row["ChildURL"] = "";
            MenuData.Rows.Add(row);

            string result = "";
            bool ishaveopen = false;

            DataRow[] drs = MenuData.Select("ParentId = 0");

            foreach (DataRow dr in drs)
            {
                bool isopen = false;
                string ul = "";
                if (dr["PageURL"].ToString().Trim().Equals(""))
                {
                    ul = "<li class=\"\"><a href=\"#\"><i class=\"menu-icon fa {2}\"></i><span class=\"menu-text\"> {1} </span><b class=\"arrow fa fa-angle-down\"></b></a><b class=\"arrow\"></b><ul class=\"submenu\">{0}</ul></li>";
                }
                else
                {
                    string url = dr["PageURL"].ToString();
                    string urlChild = dr["ChildURL"].ToString();

                    url = this.ResolveUrl(url);

                    if (dr["Name"].ToString() == "我的关注" || dr["Name"].ToString() == "我的消息" || dr["Name"].ToString() == "我的钱包" || dr["Name"].ToString() == "意见反馈" || dr["Name"].ToString() == "我的奖励" || dr["Name"].ToString() == "商机需求")
                    {

                        if (_url.Contains(url.Substring(1)) || (urlChild != "" && _url.Contains(urlChild.Substring(1))))
                            ul = "<li class=\"active\"><a class=\"menu-first\" data-name=\"" + dr["Name"].ToString() + "\" href=\"" + url + "\"><i class=\"menu-icon fa {2}  \"></i><span class=\"menu-text\"> {1} </span></a><b class=\"arrow\"></b><ul class=\"submenu\">{0}</ul></li>";
                        else
                            ul = "<li class=\"\"><a class=\"menu-first\" data-name=\"" + dr["Name"].ToString() + "\" href=\"" + url + "\"><i class=\"menu-icon fa {2}  \"></i><span class=\"menu-text\"> {1} </span></a><b class=\"arrow\"></b><ul class=\"submenu\">{0}</ul></li>";
                    }
                    else
                        ul = "<li class=\"\"><a href=\"" + url + "\"><i class=\"menu-icon fa {2}  \"></i><span class=\"menu-text\"> {1} </span><b class=\"arrow fa fa-angle-down\"></b></a><b class=\"arrow\"></b><ul class=\"submenu\">{0}</ul></li>";
                }
                string li = "";
                string parentName = dr["Name"].ToString();
                DataRow[] tdrs = MenuData.Select("ParentId=" + dr["PageId"].ToString());
                if (tdrs.Length > 0)
                {
                    ul = ul.Replace("<a href=\"#\">", string.Format("<a href=\"#\" class=\"dropdown-toggle menu-first\" data-name=\"{0}\">", parentName));
                }

                foreach (DataRow tdr in tdrs)
                {
                    string url = tdr["PageURL"].ToString();
                    string urlChild = tdr["ChildURL"].ToString();

                    if (url != "" && (_url.Contains(url.Substring(1)) || (urlChild != "" && _url.Contains(urlChild.Substring(1)))))
                    {
                        li += "<li class=\"active\" ><a href=\"{0}\" data-name=\"{2}\">{1}</a><b class=\"arrow\" ></b></li>";
                    }
                    else
                    {
                        li += "<li class=\"\" ><a href=\"{0}\" data-name=\"{2}\" >{1}</a><b class=\"arrow\"></b></li>";
                    }

                    if (!url.Trim().Equals(""))
                    {
                        if (_url.Contains(url.Substring(1)) || (urlChild != "" && _url.Contains(urlChild.Substring(1))))
                        {
                            isopen = true;
                        }
                        url = this.ResolveUrl(url);

                    }
                    string title = tdr["Name"].ToString();
                    li = string.Format(li, url, title, parentName + "-" + title);
                }
                if (isopen && !ishaveopen)
                {
                    ul = ul.Replace("<li class=\"\">", "<li class=\"active hsub open\">").Replace("<ul class=\"submenu\">", "<ul style=\"display: block;\" class=\"submenu nav-show\">");
                    ishaveopen = true;
                }
                ul = string.Format(ul, li, dr["Name"].ToString(), GetStyle(dr["Name"].ToString()));
                result += ul;
            }
            result += "<style>.wapfooter li.li5 a div{ background-image:url(../Style/images/wap/center_active.png)}.wapfooter li.li5 a p{color:#2ea7e0}</style>";
            menu.InnerHtml = result;
        }

        private string GetStyle(string title)
        {
            if (title.Equals("个人中心"))
            {
                return "fa fa-cog";
            }
            //else if (title.Equals("系统管理"))
            //{
            //    return "fa fa-users";
            //}
            else if (title.Equals("我的消息"))
            {
                return "fa-building-o";
            }
            else if (title.Equals("我的关注"))
            {
                return "fa-star";
            }
            else if (title.Equals("我是雇主"))
            {
                return "fa fa-user";
            }
            //else if (title.Equals("财务管理"))
            //{
            //    return "fa fa-pencil-square-o";
            //}
            else if (title.Equals("我的钱包"))
            {
                return "fa fa-bar-chart-o";
            }
            else if (title.Equals("我的奖励"))
            {
                return "fa fa-bar-chart-zxb";
            }
            else if (title.Equals("商机需求"))
            {
                return "fa fa-bar-chart-cgxq";
            }
            else if (title.Equals("我是销售"))
            {
                return "fa-calendar";
            }
            else if (title.Equals("意见反馈"))
            {
                return "fa fa-list";
            }
            return "";
        }
    }
}