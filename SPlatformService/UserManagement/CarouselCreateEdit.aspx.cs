using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;

namespace SPlatformService.UserManagement
{
    public partial class CarouselCreateEdit : BasePage
    {
        public int mid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "轮播消息";
            }

            base.ValidPageRight("轮播消息", "Read");

            int CarouselID = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["CarouselID"]) ? "0" : Request.QueryString["CarouselID"]);
            this.hidCustomerId.Value = CarouselID.ToString();
            mid = CarouselID;

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidCarouselID='").Append(hidCustomerId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_CarouselCreateEdit", sb.ToString());
        }
    }
}