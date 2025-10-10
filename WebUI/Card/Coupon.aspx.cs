using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.Card
{
    public partial class Coupon : System.Web.UI.Page
    {
        public string CouponText { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            CouponText = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["CouponText"]) ? "" : HttpContext.Current.Request.QueryString["CouponText"];
        }
    }
}