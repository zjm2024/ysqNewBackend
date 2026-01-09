using SPLibrary.CoreFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUI.Common;

namespace WebUI
{
    public partial class ComissionDelegation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divNote.InnerHtml = SiteCommon.GetHelpDocByTypeName("酬金托管制");
        }
    }
}