using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using CoreFramework.VO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SPlatformService.UserManagement
{
    public partial class HelpDocCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "基础资料";
            }

            base.ValidPageRight("基础资料", "Read");

            BindData();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidHelpDocId='").Append(hidHelpDocId.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_HelpDocCreateEdit", sb.ToString());
        }

        public void BindData()
        {
            drpHelpDocType.Items.Clear();
            SystemBO sBO = new SystemBO(new UserProfile());
            List<HelpDocTypeVO> voList = sBO.FindHelpDocTypeList(0, false);
            foreach (HelpDocTypeVO typeVO in voList)
            {
                drpHelpDocType.Items.Add(new ListItem(typeVO.HelpDocTypeName, typeVO.HelpDocTypeId.ToString()));
            }
            drpHelpDocType.SelectedIndex = 0;

            drpHelpDocType2.Items.Clear();
            List<HelpDocTypeVO> voChildList = sBO.FindHelpDocTypeList(Convert.ToInt32(drpHelpDocType.SelectedValue), false);
            foreach (HelpDocTypeVO typeVO in voChildList)
            {
                drpHelpDocType2.Items.Add(new ListItem(typeVO.HelpDocTypeName, typeVO.HelpDocTypeId.ToString()));
            }
            drpHelpDocType2.SelectedIndex = 0;


        }
    }
}