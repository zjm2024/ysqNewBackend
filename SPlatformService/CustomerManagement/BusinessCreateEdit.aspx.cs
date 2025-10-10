using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using CoreFramework.VO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SPlatformService.CustomerManagement
{
    public partial class BusinessCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isApprove = string.IsNullOrEmpty(Request.QueryString["IsApprove"]) ? false : Convert.ToBoolean(Request.QueryString["IsApprove"]);
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "雇主管理";
                (this.Master as Shared.MasterPage).PageNameText = isApprove ? "雇主认证" : "雇主认证";
            }
            if (isApprove)
                base.ValidPageRight("雇主认证", "Read");
            else
                base.ValidPageRight("雇主列表", "Read");

            int businessId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["BusinessId"]) ? "0" : Request.QueryString["BusinessId"]);
            this.hidBusinessId.Value = businessId.ToString();

            BindData();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidBusinessId='").Append(hidBusinessId.ClientID).Append("';\n");
            sb.Append("var isApprove='").Append(isApprove.ToString()).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_BusinessCreateEdit", sb.ToString());
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