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
    public partial class PlatformRule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<HelpDocTypeVO> helpTypeList = SiteCommon.GetHelpDocTypeList(4);
            string htmlStr = "";
            for (var i = 0; i < helpTypeList.Count; i++)
            {
                htmlStr += "<p><a href=\"Rules.aspx?HelpDocTypeId=" + helpTypeList[i].HelpDocTypeId + "\" target=\"_blank\">" + helpTypeList[i].HelpDocTypeName + "</a></p> \r\n";
            }
            divNote.InnerHtml = htmlStr;
        }
    }
}