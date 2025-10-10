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
    public partial class UserCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "系统管理";
                (this.Master as Shared.MasterPage).PageNameText = "用户管理";
            }            

            base.ValidPageRight("用户管理", "Read");

            int userId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["UserId"]) ? "0" : Request.QueryString["UserId"]);
            this.hidUserId.Value = userId.ToString();

            if (!IsPostBack)
            {
                BindList();
            }
            UserBO uBO = new UserBO(UserProfile);
            hidIsDelete.Value = uBO.IsHasSecurity(UserProfile.UserId, "用户管理", "Delete").ToString().ToLower();
            hidIsEdit.Value = uBO.IsHasSecurity(UserProfile.UserId, "用户管理", "Edit").ToString().ToLower();
            hidIsUpdatePassword.Value = uBO.IsHasSecurity(UserProfile.UserId, "修改员工密码", "Allow").ToString().ToLower();

            StringBuilder sb = new StringBuilder();
            sb.Append("var hidUserId='").Append(hidUserId.ClientID).Append("';\n");
            sb.Append("var hidIsDelete='").Append(hidIsDelete.ClientID).Append("';\n");
            sb.Append("var hidIsEdit='").Append(hidIsEdit.ClientID).Append("';\n");
            sb.Append("var hidIsUpdatePassword='").Append(hidIsUpdatePassword.ClientID).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_UserEdit", sb.ToString());
        }

        private void BindList()
        {
            RoleBO rBO = new RoleBO(UserProfile);
            UserBO uBO = new UserBO(UserProfile);
            List<RoleVO> roleVOList = rBO.FindRoleAll(UserProfile.CompanyId);

            List<UserRoleVO> urList = uBO.FindUserRoleByUserId(Convert.ToInt32(this.hidUserId.Value));
            foreach (UserRoleVO urVO in urList)
            {
                if (!roleVOList.Exists(delegate (RoleVO tURVO)
                {
                    return urVO.RoleId == tURVO.RoleId;
                }))
                {
                    RoleVO roleVO = rBO.FindById(urVO.RoleId);
                    roleVOList.Add(roleVO);
                }
            }

            foreach (RoleVO roleVO in roleVOList)
            {
                listRole.Items.Add(new ListItem(roleVO.RoleName, roleVO.RoleId.ToString()));
            }

        }
    }
}