using CoreFramework.VO;
using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService.RequireManagement
{
    public partial class ChooseRequireList : System.Web.UI.Page
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
        protected void Page_Load(object sender, EventArgs e)
        {
            //provinceId, cityId, parentCategoryId, categoryId
            int provinceId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["provinceId"]) ? "0" : Request.QueryString["provinceId"]);
            int cityId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["cityId"]) ? "0" : Request.QueryString["cityId"]);
            int parentCategoryId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["parentCategoryId"]) ? "0" : Request.QueryString["parentCategoryId"]);
            int categoryId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["categoryId"]) ? "0" : Request.QueryString["categoryId"]);

            StringBuilder sb = new StringBuilder();
            sb.Append("var provinceId='").Append(provinceId).Append("';\n");
            sb.Append("var cityId='").Append(cityId).Append("';\n");
            sb.Append("var parentCategoryId='").Append(parentCategoryId).Append("';\n");
            sb.Append("var categoryId='").Append(categoryId).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_AgencyCreateEdit", sb.ToString());

            //BindData();
        }

        //private void BindData()
        //{
        //    UserProfile uProfile = (this.Master as Shared.MasterPage).UserProfile;
        //    CityBO uBO = new CityBO(uProfile);
        //    CategoryBO cBO = new CategoryBO(uProfile);
        //    drpProvince.Items.Clear();
        //    List<ProvinceVO> voList = uBO.FindProvinceList(true);

        //    for (int i = 0; i < voList.Count; i++)
        //    {
        //        ProvinceVO pVO = voList[i];
        //        drpProvince.Items.Add(new ListItem(pVO.ProvinceName, pVO.ProvinceId.ToString()));
        //    }

        //    drpCity.Items.Clear();
        //    drpCity.Items.Add(new ListItem("全部", "-1"));
        //    List<CityVO> voChildList = uBO.FindCityByProvince(Convert.ToInt32(drpProvince.Items[drpProvince.SelectedIndex].Value), true);

        //    for (int i = 0; i < voChildList.Count; i++)
        //    {
        //        CityVO cVO = voChildList[i];
        //        drpCity.Items.Add(new ListItem(cVO.CityName, cVO.CityId.ToString()));
        //    }

        //    drpParentCategory.Items.Clear();
        //    List<CategoryVO> voCategoryList = cBO.FindParentCategoryList(true);
        //    for (int i = 0; i < voCategoryList.Count; i++)
        //    {
        //        CategoryVO pVO = voCategoryList[i];
        //        drpParentCategory.Items.Add(new ListItem(pVO.CategoryName, pVO.CategoryId.ToString()));
        //    }

        //    drpCategory.Items.Clear();
        //    drpCategory.Items.Add(new ListItem("全部", "-1"));
        //    List<CategoryVO> voCategoryChildList = cBO.FindCategoryByParent(Convert.ToInt32(drpParentCategory.Items[drpParentCategory.SelectedIndex].Value), true);
        //    for (int i = 0; i < voCategoryChildList.Count; i++)
        //    {
        //        CategoryVO cVO = voCategoryChildList[i];
        //        drpCategory.Items.Add(new ListItem(cVO.CategoryName, cVO.CategoryId.ToString()));

        //    }
        //}
    }
}