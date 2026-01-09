using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;

namespace SPlatformService.UserManagement
{
    public partial class ProvinceCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "行政区域";
            }

            base.ValidPageRight("行政区域", "Read");

            int provinceId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["ProvinceId"]) ? "0" : Request.QueryString["ProvinceId"]);
            this.hidProvinceId.Value = provinceId.ToString();
          

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidProvinceId='").Append(hidProvinceId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_ProvinceCreateEdit", sb.ToString());
        }
    }
}