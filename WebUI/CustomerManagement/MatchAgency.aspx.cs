using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
using SPLibrary.RequireManagement.BO;
using SPLibrary.RequireManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebUI.Common;

namespace WebUI.CustomerManagement
{
    public partial class MatchAgency : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Master != null)
            {
                (base.Master as Shared.MasterPage).MenuText = "匹配销售";
            }

            CustomerViewVO customerVO = SiteCommon.GetCustomerById(CustomerProfile.CustomerId);
            bool isBusiness = false;
            if (customerVO.BusinessId > 0 && customerVO.BusinessStatus == 1)
            {
                //bind require
                RequireBO rBO = new RequireBO(new CoreFramework.VO.CustomerProfile());
                List<RequirementViewVO> requireList = rBO.FindRequireByCustomerId(customerVO.CustomerId, 1);
                drpRequire.Items.Clear();
                for (int i = 0; i < requireList.Count; i++)
                {
                    RequirementViewVO cVO = requireList[i];
                    drpRequire.Items.Add(new ListItem(cVO.Title, cVO.CategoryId.ToString()));
                }


                isBusiness = true;
                StringBuilder sb = new StringBuilder();
                sb.Append("var isBusiness= ").Append(isBusiness.ToString().ToLower()).Append(";\n");
                Utilities.RegisterJs(Page, "JSCommonVar_MatchAgency", sb.ToString());
            }
            CustomerBO _bo = new CustomerBO(new CoreFramework.VO.CustomerProfile());
            List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("销售匹配");
            if (_bo.ZXBFindRequireCount("CustomerId = " + customerVO.CustomerId + " and type=" + zVO[0].ZxbConfigID) == 0)
            {
                //发放乐币奖励
                _bo.ZXBAddrequire(customerVO.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
            }
        }
    }
}