using CoreFramework.VO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.UserManagement.BO;
using SPLibrary.CoreFramework;
using SPLibrary.WebConfigInfo;

namespace SPlatformService.Shared
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public string SiteName
        {
            get
            {
                if (UserProfile.AppType == 3)
                {
                    return "微云智推后台管理系统";
                }
                if (string.IsNullOrEmpty(CacheSystemConfig.GetSystemConfig().SiteDescription))
                    return "华顺青为后台管理系统";
                else
                    return "华顺青为后台管理系统";
            }
        }
        public string logo
        {
            get {
                if (UserProfile.AppType == 3)
                {
                    return ResolveUrl("~/Style/images/logo-zheng.png");
                }
                return ResolveUrl("~/Style/images/logo.png");
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

        public UserProfile UserProfile
        {
            get { return new UserPrincipal().UserProfile; }
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

        protected void Page_Load(object sender, EventArgs e)
        {   
            StringBuilder sb = new StringBuilder();
            sb.Append("var ImgPath='").Append((Request.ApplicationPath == "/" ? "" : Request.ApplicationPath) + "/Style/images/").Append("';\n");

            Utilities.RegisterJs(Page, "JSCommonVar", sb.ToString());

            UserName.Text = UserProfile.UserName;

            GetMenu();
        }

        protected void UserLogout(object sender, EventArgs e)
        {
            HttpContext.Current.Session["#Session#TOKEN"] = null;
            IUserPrincipal iup = new UserPrincipal();
            FormsAuthentication.SignOut();
            UserPrincipal.ClearProfileSession();
            Session.Abandon();
            Response.Redirect("~/login.aspx");
        }

        private void GetMenu()
        {
            //string[] array = HttpContext.Current.Request.Url.ToString().Split('/');
            string _url = HttpContext.Current.Request.Url.ToString();
            UserBO uBO = new UserBO(UserProfile);
            DataTable dtTemp = uBO.GetMenu(UserProfile.UserId);
            DataTable MenuData = new DataTable();
            MenuData.Columns.Add("PageId");
            MenuData.Columns.Add("ParentId", typeof(int));
            MenuData.Columns.Add("PageURL");
            MenuData.Columns.Add("ChildURL");
            MenuData.Columns.Add("Name");
            DataRow row;
            foreach (DataRow rowTemp in dtTemp.Rows)
            {
                row = MenuData.NewRow();
                row["PageId"] = rowTemp["SecurityTypeId"];
                row["ParentId"] = Convert.IsDBNull(rowTemp["ParentSecurityTypeId"]) ? 0 : rowTemp["ParentSecurityTypeId"];
                row["Name"] = rowTemp["SecurityTypeName"];
                string urlValue = Convert.IsDBNull(rowTemp["SecurityTypeValue"]) ? "" : rowTemp["SecurityTypeValue"].ToString();
                string[] urlArray = urlValue.Split(';');
                row["PageURL"] = urlArray.Length > 0 ? urlArray[0] : "";
                row["ChildURL"] = urlArray.Length > 1 ? urlArray[1] : "";
                MenuData.Rows.Add(row);
            }
            
            string result = "";
            bool ishaveopen = false;

            DataRow[] drs = MenuData.Select("ParentId = 0");

            foreach (DataRow dr in drs)
            {
                bool isopen = false;
                string ul = "";
                if (dr["PageURL"].ToString().Trim().Equals(""))
                {
                    if (dr["Name"].ToString() == "乐聊名片" && UserProfile.AppType == 3)
                    {
                        ul = "<li class=\"\"><a href=\"#\"><i class=\"menu-icon fa {2}\"></i><span class=\"menu-text\"> 微云智推 </span><b class=\"arrow fa fa-angle-down\"></b></a><b class=\"arrow\"></b><ul class=\"submenu\">{0}</ul></li>";
                    }
                    else
                    {
                        ul = "<li class=\"\"><a href=\"#\"><i class=\"menu-icon fa {2}\"></i><span class=\"menu-text\"> {1} </span><b class=\"arrow fa fa-angle-down\"></b></a><b class=\"arrow\"></b><ul class=\"submenu\">{0}</ul></li>";
                    }
                    
                }
                else
                {
                    string url = dr["PageURL"].ToString();
                    string urlChild = dr["ChildURL"].ToString();

                    url = this.ResolveUrl(url);
                    
                    if (dr["Name"].ToString() == "工作面板" || dr["Name"].ToString() == "会员管理" || dr["Name"].ToString() == "用户日志" || dr["Name"].ToString() == "审核测试" || dr["Name"].ToString() == "有害信息" || dr["Name"].ToString() == "来源记录" || dr["Name"].ToString() == "商机管理" || dr["Name"].ToString() == "乐币奖励")
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
                        if (_url.Contains(url.Substring(1)) ||(urlChild != "" && _url.Contains(urlChild.Substring(1))))
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

            menu.InnerHtml = result;
        }

        private string GetStyle(string title)
        {
            if (title.Equals("个人中心"))
            {
                return "fa fa-cog";
            }
            else if (title.Equals("系统管理"))
            {
                return "fa fa-users";
            }
            else if (title.Equals("工作面板"))
            {
                return "fa-building-o";
            }
            else if (title.Equals("会员管理"))
            {
                return "fa-star";
            }
            else if (title.Equals("用户日志"))
            {
                return "fa-calendar";
            }
            else if (title.Equals("有害信息"))
            {
                return "fa-fire";
            }
            else if (title.Equals("审核测试"))
            {
                return "fa fa-pencil-square-o";
            }
            else if (title.Equals("来源记录"))
            {
                return "fa fa-pencil-square-o";
            }
            else if (title.Equals("雇主管理"))
            {
                return "fa fa-user";
            }
            else if (title.Equals("财务管理"))
            {
                return "fa fa-pencil-square-o";
            }
            else if (title.Equals("任务管理"))
            {
                return "fa fa-bar-chart-o";
            }
            else if (title.Equals("销售管理"))
            {
                return "fa-calendar";
            }
            else if (title.Equals("商机管理"))
            {
                return "fa-cart-plus";
            }
            else if (title.Equals("乐币奖励"))
            {
                return "fa-gift";
            }
            else if (title.Equals("乐聊名片"))
            {
                return "fa-fire";
            }
            else if (title.Equals("企业名片"))
            {
                return "fa-pie-chart";
            }
            else if (title.Equals("星选农场"))
            {
                return "fa-map-signs";
            }
            else if (title.Equals("数据分析"))
            {
                return "fa fa-list";
            }
            return "";
        }
    }
}