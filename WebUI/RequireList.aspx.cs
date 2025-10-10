using CoreFramework.VO;
using Newtonsoft.Json;
using SPlatformService.Controllers;
using SPlatformService.Models;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.Logging.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.RequireManagement.VO;
using SPLibrary.UserManagement.VO;
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
    public partial class RequireList : System.Web.UI.Page
    {
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

            if(cityId < 1)
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

            if (CustomerId > 0)
            {
                CustomerViewVO customerVO = SiteCommon.GetCustomerById(CustomerId);
                bool isAgency = false, isBusiness = false;
                if (customerVO.AgencyId > 0 && customerVO.AgencyStatus == 1)
                    isAgency = true;
                if (customerVO.BusinessId > 0 && customerVO.BusinessStatus == 1)
                    isBusiness = true;
                StringBuilder sb = new StringBuilder();
                sb.Append("var isAgency= ").Append(isAgency.ToString().ToLower()).Append(";\n");
                sb.Append("var isBusiness= ").Append(isBusiness.ToString().ToLower()).Append(";\n");
                Utilities.RegisterJs(Page, "JSCommonVar_AgencyExperienceBrowse", sb.ToString());
            }
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
    }
}