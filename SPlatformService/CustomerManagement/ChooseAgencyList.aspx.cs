using SPLibrary.CoreFramework;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CoreFramework.WebConfigInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPlatformService.CustomerManagement
{
    public partial class ChooseAgencyList : System.Web.UI.Page
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
        }
    }
}