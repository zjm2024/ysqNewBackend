using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;

namespace SPlatformService.UserManagement
{
    public partial class CategoryCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "行业分类";
            }

            base.ValidPageRight("行业分类", "Read");

            int categoryId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CategoryId"]) ? "0" : Request.QueryString["CategoryId"]);
            this.hidCategoryId.Value = categoryId.ToString();
            
            

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidCategoryId='").Append(hidCategoryId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CategoryCreateEdit", sb.ToString());
        }
    }
}