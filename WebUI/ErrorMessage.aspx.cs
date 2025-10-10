using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI
{
    public partial class ErrorMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (this.Master != null)
            //{
            //    this.Master.MenuText = "其它";
            //    this.Master.PageNameText = "错误提示";
            //}

            if (Session["ExceptionHandleTip"] != null)
            {
                hidErrorTip.Value = Session["ExceptionHandleTip"].ToString();
            }
            if (Session["ExceptionHandleMsg"] != null)
            {
                hidErrorMessage.Value = Session["ExceptionHandleMsg"].ToString();
            }
        }
    }
}