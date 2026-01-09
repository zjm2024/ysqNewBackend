using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using CoreFramework.VO;
using System.Web.UI.WebControls;

namespace SPlatformService.CustomerManagement
{
    public partial class AgencyCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isApprove = string.IsNullOrEmpty(Request.QueryString["IsApprove"]) ? false : Convert.ToBoolean(Request.QueryString["IsApprove"]);
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "销售管理";
                (this.Master as Shared.MasterPage).PageNameText = isApprove ? "销售认证" : "销售认证";
            }
            if (isApprove)
                base.ValidPageRight("销售认证", "Read");
            else
                base.ValidPageRight("销售列表", "Read");

            int agencyId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["AgencyId"]) ? "0" : Request.QueryString["AgencyId"]);
            this.hidAgencyId.Value = agencyId.ToString();

            BindData();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidAgencyId='").Append(hidAgencyId.ClientID).Append("';\n");
            sb.Append("var isApprove='").Append(isApprove.ToString()).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_AgencyCreateEdit", sb.ToString());
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