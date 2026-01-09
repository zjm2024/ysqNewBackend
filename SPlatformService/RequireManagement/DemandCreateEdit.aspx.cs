using System;
using System.Text;
using SPLibrary.CoreFramework.BO;
using SPLibrary.UserManagement.BO;
using CoreFramework.VO;
using SPLibrary.UserManagement.VO;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SPLibrary.RequireManagement.VO;
using SPLibrary.RequireManagement.BO;

namespace SPlatformService.RequireManagement
{
    public partial class DemandCreateEdit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                (this.Master as Shared.MasterPage).MenuText = "商机管理";
                (this.Master as Shared.MasterPage).PageNameText = "商机修改";
            }

            base.ValidPageRight("商机管理", "Read");

            BindData();
        }

        public void BindData()
        {
            int DemandId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["DemandId"]) ? "0" : Request.QueryString["DemandId"]);

            StringBuilder sb = new StringBuilder();
            sb.Append("var _DemandId=").Append(DemandId.ToString()).Append(";\n");
            Utilities.RegisterJs(Page, "JSCommonVar_RequirementCreateEdit", sb.ToString());

            DemandBO rBO = new DemandBO(new CustomerProfile());
            //getRequirement
            DemandViewVO demandviewVO = rBO.FindDemandById(DemandId);

            txtPhone.Text = demandviewVO.Phone;
            txtPhone.Enabled = false;
            txtDescription.Text = demandviewVO.Description.Replace("<br/>", "\r\n");
            txtEffectiveEndDateCreateEdit.Text = demandviewVO.EffectiveEndDate.ToString("yyyy-MM-dd");
            BindData(demandviewVO.CategoryId);
        }
        private void BindData(int categoryId)
        {
            drpdemand_class.Items.Clear();
            DemandBO rBO = new DemandBO(new CustomerProfile());
            List<DemandCategoryVO> voList = rBO.FindCategory();
            for (int i = 0; i < voList.Count; i++)
            {
                DemandCategoryVO pVO = voList[i];
                drpdemand_class.Items.Add(new ListItem(pVO.CategoryName, pVO.CategoryId.ToString()));
                if (pVO.CategoryId == categoryId)
                    drpdemand_class.SelectedIndex = i;
            }
        }
    }
}