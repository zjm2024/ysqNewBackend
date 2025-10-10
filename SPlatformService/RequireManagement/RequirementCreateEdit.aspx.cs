using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using CoreFramework.VO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SPlatformService.RequireManagement
{
    public partial class RequirementCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "任务管理";
                (this.Master as Shared.MasterPage).PageNameText = "任务列表";
            }

            base.ValidPageRight("任务列表", "Read");

            BindData();

            int requirementId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["RequirementId"]) ? "0" : Request.QueryString["RequirementId"]);
            this.hidRequirementId.Value = requirementId.ToString();

            UserBO uBO = new UserBO(UserProfile);

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidRequirementId='").Append(hidRequirementId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_RequirementCreateEdit", sb.ToString());
        }

        public void BindData()
        {
            drpProvince.Items.Clear();
            CityBO sBO = new CityBO(new UserProfile());
            List<ProvinceVO> voList = sBO.FindProvinceList(true);
            foreach (ProvinceVO pVO in voList)
            {
                drpProvince.Items.Add(new ListItem(pVO.ProvinceName, pVO.ProvinceId.ToString()));
            }
            drpProvince.SelectedIndex = 0;

            drpCity.Items.Clear();
            List<CityVO> voChildList = sBO.FindCityByProvince(Convert.ToInt32(drpProvince.SelectedValue), true);
            foreach (CityVO cVO in voChildList)
            {
                drpCity.Items.Add(new ListItem(cVO.CityName, cVO.CityId.ToString()));
            }
            drpCity.SelectedIndex = 0;

            drpCategory1.Items.Clear();
            CategoryBO cBO = new CategoryBO(new UserProfile());
            List<CategoryVO> voCategoryList = cBO.FindParentCategoryList(true);
            foreach (CategoryVO pVO in voCategoryList)
            {
                drpCategory1.Items.Add(new ListItem(pVO.CategoryName, pVO.CategoryId.ToString()));
            }
            drpCategory1.SelectedIndex = 0;

            drpCategory2.Items.Clear();
            List<CategoryVO> voCategoryChildList = cBO.FindCategoryByParent(Convert.ToInt32(drpCategory1.SelectedValue), true);
            foreach (CategoryVO cVO in voCategoryChildList)
            {
                drpCategory2.Items.Add(new ListItem(cVO.CategoryName, cVO.CategoryId.ToString()));
            }
            drpCategory2.SelectedIndex = 0;
        }
    }
}