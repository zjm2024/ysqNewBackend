using CoreFramework.VO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService.UserManagement
{
    public partial class RecommendRequireCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "任务推荐";
            }

            BindData();
        }
        private void BindData()
        {
            UserProfile uProfile = (this.Master as Shared.MasterPage).UserProfile;
            CityBO uBO = new CityBO(uProfile);
            CategoryBO cBO = new CategoryBO(uProfile);
            drpProvince.Items.Clear();
            drpProvince.Items.Add(new ListItem("全部", "-1"));
            List<ProvinceVO> voList = uBO.FindProvinceList(true);

            for (int i = 0; i < voList.Count; i++)
            {
                ProvinceVO pVO = voList[i];
                drpProvince.Items.Add(new ListItem(pVO.ProvinceName, pVO.ProvinceId.ToString()));
            }

            drpCity.Items.Clear();
            drpCity.Items.Add(new ListItem("全部", "-1"));
            List<CityVO> voChildList = uBO.FindCityByProvince(Convert.ToInt32(drpProvince.Items[drpProvince.SelectedIndex].Value), true);

            for (int i = 0; i < voChildList.Count; i++)
            {
                CityVO cVO = voChildList[i];
                drpCity.Items.Add(new ListItem(cVO.CityName, cVO.CityId.ToString()));
            }

            drpParentCategory.Items.Clear();
            drpParentCategory.Items.Add(new ListItem("全部", "-1"));
            List<CategoryVO> voCategoryList = cBO.FindParentCategoryList(true);
            for (int i = 0; i < voCategoryList.Count; i++)
            {
                CategoryVO pVO = voCategoryList[i];
                drpParentCategory.Items.Add(new ListItem(pVO.CategoryName, pVO.CategoryId.ToString()));
            }

            drpCategory.Items.Clear();
            drpCategory.Items.Add(new ListItem("全部", "-1"));
            List<CategoryVO> voCategoryChildList = cBO.FindCategoryByParent(Convert.ToInt32(drpParentCategory.Items[drpParentCategory.SelectedIndex].Value), true);
            for (int i = 0; i < voCategoryChildList.Count; i++)
            {
                CategoryVO cVO = voCategoryChildList[i];
                drpCategory.Items.Add(new ListItem(cVO.CategoryName, cVO.CategoryId.ToString()));

            }
        }
    }
}