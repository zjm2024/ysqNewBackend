using SPLibrary.CoreFramework;
using SPLibrary.UserManagement.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUI.Common;

namespace WebUI
{
    public partial class Rules : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int typeId = Convert.ToInt32(string.IsNullOrEmpty(Request.QueryString["HelpDocTypeId"]) ? "0" : Request.QueryString["HelpDocTypeId"]);
            if (typeId > 0)
            {
                HelpDocViewVO vo = SiteCommon.GetHelpDocByTypeId(typeId);
                if (vo != null) { 
                ruleTitle.InnerHtml = vo.Title;
                divNote.InnerHtml = vo.Description;
            }
            }
        }
    }
}