using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService.Controllers;
using SPlatformService.Models;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.ProjectManagement.VO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.UserManagement.VO;
using SPLibrary.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUI.Common;

namespace WebUI
{
    public partial class ProjectList : System.Web.UI.Page
    {
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

            int cityId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CityId"]) ? "0" : Request.QueryString["CityId"]);
            int categoryId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CategoryId"]) ? "0" : Request.QueryString["CategoryId"]);
            string searchTxt = string.IsNullOrEmpty(Request.QueryString["SearchValue"]) ? "" : Request.QueryString["SearchValue"];

            if (cityId < 1)
            {
                try
                {
                    cityId = Convert.ToInt32(Request.Cookies["local_city_id"].Value);
                }
                catch
                {
                    cityId = 0;
                }
            }

            if (!string.IsNullOrEmpty(searchTxt))
            {
                txtSearcha.Value = searchTxt;
            }

            BindData(cityId, categoryId);

            //获取条件和排序，生成filterModel
            ConditionModel filterModel = GetFilterModel();

            //获取数据    

            GetBusinessList(filterModel);

            SetPaging(filterModel);
        }

        public ConditionModel GetFilterModel()
        {
            ConditionModel filterModel = new ConditionModel();
            Filter filterObj = new Filter();
            Paging pageInfoObj = new Paging();

            filterModel.Filter = filterObj;
            filterModel.PageInfo = pageInfoObj;

            pageInfoObj.PageIndex = 1;
            pageInfoObj.PageCount = 20;
            pageInfoObj.SortName = "CreatedAt";
            pageInfoObj.SortType = "asc";

            filterObj.groupOp = "AND";

            List<SPLibrary.WebConfigInfo.Rules> ruleList = new List<SPLibrary.WebConfigInfo.Rules>();

            if (drpProvince.Items[drpProvince.SelectedIndex].Value == "-1" || drpProvince.Items[drpProvince.SelectedIndex].Value == "-2")
            {
                SPLibrary.WebConfigInfo.Rules ruleObj = new SPLibrary.WebConfigInfo.Rules();
                ruleObj.field = "1";
                ruleObj.op = "eq";
                ruleObj.data = "1";
                ruleList.Add(ruleObj);
            }
            else if (drpCity.Items[drpCity.SelectedIndex].Value != "-1" && drpCity.Items[drpCity.SelectedIndex].Value != "-2")
            {

                SPLibrary.WebConfigInfo.Rules ruleObj = new SPLibrary.WebConfigInfo.Rules();
                ruleObj.field = "CityId";
                ruleObj.op = "ieq";
                ruleObj.data = drpCity.Items[drpCity.SelectedIndex].Value;
                ruleList.Add(ruleObj);
            }
            else
            {
                List<string> cityIdList = new List<string>();
                for (int i = 0; i < drpCity.Items.Count; i++)
                {
                    if (drpCity.Items[i].Value != "-1" && drpCity.Items[i].Value != "-2")
                        cityIdList.Add(drpCity.Items[i].Value);
                }
                if (cityIdList.Count > 0)
                {
                    SPLibrary.WebConfigInfo.Rules ruleObj = new SPLibrary.WebConfigInfo.Rules();
                    ruleObj.field = "CityId";
                    ruleObj.op = "in";
                    ruleObj.data = string.Join(",", cityIdList.ToArray());
                    ruleList.Add(ruleObj);
                }
                else
                {
                    //没有城市，也就没有数据
                    SPLibrary.WebConfigInfo.Rules ruleObj = new SPLibrary.WebConfigInfo.Rules();
                    ruleObj.field = "1";
                    ruleObj.op = "ne";
                    ruleObj.data = "1";
                    ruleList.Add(ruleObj);
                }
            }

            List<string> categoryIdList = new List<string>();
            for (int i = 0; i < drpCategory.Items.Count; i++)
            {
                if (drpCategory.Items[i].Value != "-1" && drpCategory.Items[i].Value != "-2")
                    categoryIdList.Add(drpCategory.Items[i].Value);
            }

            if (categoryIdList.Count > 0)
            {
                SPLibrary.WebConfigInfo.Rules ruleObj = new SPLibrary.WebConfigInfo.Rules();
                ruleObj.field = "CategoryId";
                ruleObj.op = "in";
                ruleObj.data = string.Join(",", categoryIdList.ToArray());
                ruleList.Add(ruleObj);
            }
            else if (drpParentCategory.Items[drpParentCategory.SelectedIndex].Value == "-1" || drpParentCategory.Items[drpParentCategory.SelectedIndex].Value == "-2")
            {
                SPLibrary.WebConfigInfo.Rules ruleObj = new SPLibrary.WebConfigInfo.Rules();
                ruleObj.field = "1";
                ruleObj.op = "eq";
                ruleObj.data = "1";
                ruleList.Add(ruleObj);
            }
            else
            {
                //没有城市，也就没有数据
                SPLibrary.WebConfigInfo.Rules ruleObj = new SPLibrary.WebConfigInfo.Rules();
                ruleObj.field = "1";
                ruleObj.op = "ne";
                ruleObj.data = "1";
                ruleList.Add(ruleObj);
            }

            filterObj.rules = ruleList.ToArray();

            return filterModel;
        }

        public void GetBusinessList(ConditionModel filterModel)
        {
            RequireController requireCon = new RequireController();
            CustomerController customerCon = new CustomerController();
            
            ResultObject result = requireCon.GetProjectList(filterModel);
            if (result == null)
            {
                return;
            }
            List<ProjectViewVO> projectList = result.Result as List<ProjectViewVO>;

            var str = "\r\n";
            if (result.Flag == 0 || projectList.Count == 0)
            {
                divList.InnerHtml = "<span style=\"margin-left: 50px;\">暂未找到相关数据</span>";
                return;
            }
            for (var i = 0; i < projectList.Count; i++)
            {
                ProjectViewVO project = projectList[i];
                str += "<div class=\"sign-data project-list-height\"> \r\n";
                str += "    <div class=\"fl\" style=\"width: 100%;\"> \r\n";
                str += "        <div style=\"float:left;width: 100%;\">";
                str += "        <div class=\"price\"  style=\"display: none\">￥" + project.Commission + "元</div> \r\n";
                str += "        <div class=\"title\" style=\"width: 345px;margin-left: 0px;\"> \r\n";
                str += "            <a target=\"_blank\" title=\"" + project.Title + "\" href=\"Project.aspx?projectId=" + project.ProjectId + "\">" + RemoveLongString(project.Title, 20) + "</a> \r\n";
                str += "        </div> \r\n";
                str += "        <div class=\"title\" style=\"width: 30%;\"> \r\n";
                //如果已经登录,判断是否已经关注了
                if (CustomerId < 1 || Token == "")
                {
                    str += "            <img width=\"26\" height=\"26\" title=\"关注\" src=\"Style/images/marked_non.png\" style=\"cursor:pointer;\" onclick=\"markObject('" + project.ProjectId + "', this)\" /> \r\n";
                }
                else
                {
                    ResultObject result1 = customerCon.IsMarked(project.ProjectId, 3, Token);

                    if (result1.Flag == 1)
                    {
                        str += "            <img width=\"26\" height=\"26\" title=\"取消关注\" src=\"Style/images/marked.png\" style=\"cursor:pointer;\" onclick=\"deleteMark('" + result1.Result.ToString() + "','" + project.ProjectId + "', this)\" /> \r\n";
                    }
                    else
                    {
                        str += "            <img width=\"26\" height=\"26\" title=\"关注\" src=\"Style/images/marked_non.png\" style=\"cursor:pointer;\" onclick=\"markObject('" + project.ProjectId + "', this)\" /> \r\n";
                    }
                }
                str += "        </div> \r\n";
                str += "      </div> \r\n";
                str += "      <a target=\"_blank\" title=\"" + project.Title + "\" href=\"Project.aspx?projectId=" + project.ProjectId + "\" style='display:block;'> \r\n";
                str += "         <div class=\"content\"><strong>客户名称</strong>： " + project.BusinessName + "</div> \r\n";
                str += "         <div class=\"content\"><strong>销售</strong>：" + project.AgencyName + "</div> \r\n";
                str += "         <div class=\"content\"><strong>开始时间</strong>：" + project.StartDate.ToString("yyyy-MM-dd") + "</div> \r\n";

                str += "         <div class=\"content\"><strong>完成时间</strong>：" + project.EndDate.ToString("yyyy-MM-dd") + "</div> \r\n";
                str += "         <div class=\"content\"><strong>成交时间</strong>：" + project.CreatedAt.ToString("yyyy-MM-dd") + "</div> \r\n";
                ResultObject result2 = requireCon.GetRequireSite(project.RequirementId);
                RequirementViewVO requireVO = result2.Result as RequirementViewVO;
                str += "         <div class=\"content\" ><strong>任务编号</strong>：" + project.RequirementCode + "</div> \r\n";
                str += "         <div class=\"content2\" ><strong>任务详情</strong>：" + requireVO.Description + "</div> \r\n";
                str += "    </a> \r\n";
                str += "    </div> \r\n";
                str += "</div> \r\n";
            }

            divList.InnerHtml = str;
        }

        public void SetPaging(ConditionModel filterModel)
        {
            RequireController requireCon = new RequireController();
            
            ResultObject result = requireCon.GetProjectListCount(filterModel);

            int totalCount = Convert.ToInt32(result.Result.ToString());

            int dataCount = totalCount / 20 + 1;

            hidDataCount.Value = dataCount.ToString();

            //设置分页控件
            var pageStr = "";
            pageStr += "<ul> \r\n";
            pageStr += "    <li class=\"div-up\"><a href=\"#\" onclick=\"return onPageUp();\">上一页</a></li> \r\n";
            pageStr += "    <li class=\"selected\"><a href=\"#\" onclick=\"return onPageGoTo(this);\">1</a></li> \r\n";

            for (var i = 2; i <= dataCount; i++)
            {
                pageStr += "    <li><a href=\"#\" onclick=\"return onPageGoTo(this);\">" + i + "</a></li> \r\n";
            }

            pageStr += "    <li class=\"div-up\"><a href=\"#\" onclick=\"return onPageDown();\">下一页</a></li> \r\n";
            pageStr += "</ul> \r\n";

            pageList.InnerHtml = pageStr;
        }
        private void BindData(int cityId, int categoryId)
        {
            int provinceId = 0;
            int parentCategoryId = 0;
            //根据cityID和Cagegoryid  查找ProvinceID和ParentCategoryid
            if (cityId > 0)
            {
                CityVO cityVO = SiteCommon.GetCity(cityId);
                provinceId = cityVO.ProvinceId;
            }
            if (categoryId > 0)
            {
                CategoryVO categoryVO = SiteCommon.GetCategory(categoryId);
                parentCategoryId = categoryVO.ParentCategoryId;
            }

            drpProvince.Items.Clear();
            drpProvince.Items.Add(new ListItem("全部", "-1"));
            List<ProvinceVO> voList = SiteCommon.GetProvinceList();

            for (int i = 0; i < voList.Count; i++)
            {
                ProvinceVO pVO = voList[i];
                drpProvince.Items.Add(new ListItem(pVO.ProvinceName, pVO.ProvinceId.ToString()));
                if (pVO.ProvinceId == provinceId)
                    drpProvince.SelectedIndex = i + 1;
            }
            drpProvince.Items.Add(new ListItem("不限", "-2"));
            if (provinceId <= 0)
                drpProvince.SelectedIndex = 0;

            drpCity.Items.Clear();
            drpCity.Items.Add(new ListItem("全部", "-1"));
            List<CityVO> voChildList = SiteCommon.GetCityList(Convert.ToInt32(drpProvince.Items[drpProvince.SelectedIndex].Value));

            for (int i = 0; i < voChildList.Count; i++)
            {
                CityVO cVO = voChildList[i];
                drpCity.Items.Add(new ListItem(cVO.CityName, cVO.CityId.ToString()));
                if (cVO.CityId == cityId)
                    drpCity.SelectedIndex = i + 1;
            }
            drpCity.Items.Add(new ListItem("不限", "-2"));
            if (cityId <= 0)
                drpCity.SelectedIndex = 0;

            drpParentCategory.Items.Clear();
            drpParentCategory.Items.Add(new ListItem("全部", "-1"));
            List<CategoryVO> voCategoryList = SiteCommon.GetParentCategoryList();
            for (int i = 0; i < voCategoryList.Count; i++)
            {
                CategoryVO pVO = voCategoryList[i];
                drpParentCategory.Items.Add(new ListItem(pVO.CategoryName, pVO.CategoryId.ToString()));
                if (pVO.CategoryId == parentCategoryId)
                    drpParentCategory.SelectedIndex = i + 1;
            }
            drpParentCategory.Items.Add(new ListItem("不限", "-2"));
            if (parentCategoryId <= 0)
                drpParentCategory.SelectedIndex = 0;

            drpCategory.Items.Clear();
            drpCategory.Items.Add(new ListItem("全部", "-1"));
            List<CategoryVO> voCategoryChildList = SiteCommon.GetCategoryList(Convert.ToInt32(drpParentCategory.Items[drpParentCategory.SelectedIndex].Value));
            for (int i = 0; i < voCategoryChildList.Count; i++)
            {
                CategoryVO cVO = voCategoryChildList[i];
                drpCategory.Items.Add(new ListItem(cVO.CategoryName, cVO.CategoryId.ToString()));
                if (cVO.CategoryId == categoryId)
                    drpCategory.SelectedIndex = i + 1;
            }
            drpCategory.Items.Add(new ListItem("不限", "-2"));
            if (categoryId <= 0)
                drpCategory.SelectedIndex = 0;
        }
        private string RemoveLongString(string input, int len)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            return ((input.Length > len) ? (input.Substring(0, len) + "...") : input);
        }
    }
}