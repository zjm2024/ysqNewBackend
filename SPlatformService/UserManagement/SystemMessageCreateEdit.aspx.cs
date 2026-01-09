using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using CoreFramework.VO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SPlatformService.UserManagement
{
    public partial class SystemMessageCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "通告";
            }
            BindData();
            base.ValidPageRight("通告", "Read");

            int systemMessageId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["SystemMessageId"]) ? "0" : Request.QueryString["SystemMessageId"]);
            this.hidSystemMessageId.Value = systemMessageId.ToString();
            
            

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidSystemMessageId='").Append(hidSystemMessageId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_SystemMessageCreateEdit", sb.ToString());
        }
        public void BindData()
        {
            drpMessageType.Items.Clear();
            MessageBO sBO = new MessageBO(new CustomerProfile());
            List<MessageTypeVO> voList = sBO.FindAllMessageType();
            foreach (MessageTypeVO pVO in voList)
            {
                drpMessageType.Items.Add(new ListItem(pVO.MessageTypeName, pVO.MessageTypeId.ToString()));
            }
            drpMessageType.SelectedIndex = 0;

           
        }
    }
}