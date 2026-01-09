using CoreFramework.VO;
using SPLibrary.BussinessManagement.BO;
using SPLibrary.CoreFramework.BO;
using SPLibrary.CustomerManagement.BO;
using SPLibrary.CustomerManagement.VO;
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
    public partial class MatchRequire : CustomerBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Master != null)
            {
                (base.Master as Shared.MasterPage).MenuText = "匹配任务";
            }
            
            CustomerViewVO customerVO = SiteCommon.GetCustomerById(CustomerProfile.CustomerId);

            AgencyBO aBO = new AgencyBO(new CustomerProfile());

            bool isAgency = false;
            if (customerVO.AgencyId > 0 && customerVO.AgencyStatus == 1)
                isAgency = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("var isAgency= ").Append(isAgency.ToString().ToLower()).Append(";\n");
            sb.Append("var _AgencyId= '").Append(customerVO.AgencyId).Append("';\n");
            Utilities.RegisterJs(Page, "JSCommonVar_MatchRequire", sb.ToString());

            CustomerBO _bo = new CustomerBO(new CoreFramework.VO.CustomerProfile());
            List<ZxbConfigVO> zVO = _bo.ZXBAddrequirebyCode("任务匹配");
            if (_bo.ZXBFindRequireCount("CustomerId = " + customerVO.CustomerId + " and type=" + zVO[0].ZxbConfigID) == 0)
            {
                //发放乐币奖励
                _bo.ZXBAddrequire(customerVO.CustomerId, zVO[0].Cost, zVO[0].Purpose, zVO[0].ZxbConfigID);
            }
        }
    }
}