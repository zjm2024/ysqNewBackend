using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using SPLibrary.UserManagement.VO;

namespace SPlatformService.UserManagement
{
    public partial class PersonalEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "个人中心";
                (this.Master as Shared.MasterPage).PageNameText = "资料修改";
            }
            if (!IsPostBack)
            {
                BindList();
            }
            this.hidUserId.Value = UserProfile.UserId.ToString();

            StringBuilder sb = new StringBuilder(); 
            sb.Append("var hidUserId='").Append(hidUserId.ClientID).Append("';\n");
            sb.Append("var roleListId='").Append(listRoleSelected.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_UserEdit", sb.ToString());
        }

        private void BindList()
        {
            UserBO uBO = new UserBO(UserProfile);
            RoleBO rBO = new RoleBO(UserProfile);
            List<UserRoleVO> urList = uBO.FindUserRoleByUserId(UserProfile.UserId);
            foreach (UserRoleVO urVO in urList)
            {
                RoleVO roleVO = rBO.FindById(urVO.RoleId);
                listRoleSelected.Items.Add(new ListItem(roleVO.RoleName, roleVO.RoleId.ToString()));

            }     

        }
    }
}