using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Web.UI.WebControls;

namespace SPlatformService.RequireManagement
{
    public partial class ServicesCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "任务管理";
                (this.Master as Shared.MasterPage).PageNameText = "服务列表";
            }

            base.ValidPageRight("服务列表", "Read");

            int servicesId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["ServicesId"]) ? "0" : Request.QueryString["ServicesId"]);
            this.hidServicesId.Value = servicesId.ToString();

            BindData();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidServicesId='").Append(hidServicesId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_ServicesCreateEdit", sb.ToString());
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
            
        }
    }
}